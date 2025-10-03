import { useState, useEffect } from 'react';
import { Plus, Edit, Trash2, MessageSquare, Calendar, Flag } from 'lucide-react';
import { tasksApi, projectsApi } from '../services/api';
import { Task, Project, CreateTaskDto, AddTaskCommentDto, TaskStatus } from '../types';

const Tasks = () => {
  const [tasks, setTasks] = useState<Task[]>([]);
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [showCommentModal, setShowCommentModal] = useState(false);
  const [editingTask, setEditingTask] = useState<Task | null>(null);
  const [formData, setFormData] = useState<CreateTaskDto>({
    title: '',
    description: '',
    priority: 'Medium',
    dueDate: '',
    projectId: 0,
    userId: 1
  });
  const [commentData, setCommentData] = useState<AddTaskCommentDto>({
    comment: '',
    userId: 1
  });
  const [showWarning, setShowWarning] = useState(false);
  const [warningMessage, setWarningMessage] = useState('');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    try {
      console.log('Carregando projetos e tarefas...');
      const projectsResponse = await projectsApi.getUserProjects(1);
      console.log('Projetos carregados:', projectsResponse.data);
      
      setProjects(projectsResponse.data);
      
      // Buscar tarefas de todos os projetos
      const allTasks: Task[] = [];
      for (const project of projectsResponse.data) {
        try {
          const tasksResponse = await tasksApi.getProjectTasks(project.id, 1);
          allTasks.push(...tasksResponse.data);
        } catch (error) {
          console.error(`Erro ao carregar tarefas do projeto ${project.id}:`, error);
        }
      }
      
      console.log('Tarefas carregadas:', allTasks);
      setTasks(allTasks);
    } catch (error) {
      console.error('Erro ao carregar dados:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (editingTask) {
        await tasksApi.updateTask(editingTask.id, 1, {
          title: formData.title,
          description: formData.description,
          status: editingTask.status,
          dueDate: formData.dueDate
        });
      } else {
        await tasksApi.createTask(formData);
      }
      
      setShowModal(false);
      setEditingTask(null);
      setFormData({ title: '', description: '', priority: 'Medium', dueDate: '', projectId: 0, userId: 1 });
      fetchData();
    } catch (error: any) {
      console.error('Erro ao salvar tarefa:', error);
      
      // Verificar se é erro de limite de tarefas
      if (error.response?.data?.includes('limite máximo de 20 tarefas')) {
        setWarningMessage('Este projeto já atingiu o limite máximo de 20 tarefas. Não é possível adicionar mais tarefas.');
        setShowWarning(true);
      } else {
        setWarningMessage('Erro ao salvar tarefa. Tente novamente.');
        setShowWarning(true);
      }
    }
  };

  const handleAddComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingTask) return;
    
    try {
      await tasksApi.addTaskComment(editingTask.id, commentData);
      setShowCommentModal(false);
      setCommentData({ comment: '', userId: 1 });
      fetchData();
    } catch (error) {
      console.error('Erro ao adicionar comentário:', error);
    }
  };

  const handleEdit = (task: Task) => {
    setEditingTask(task);
    setFormData({
      title: task.title,
      description: task.description || '',
      priority: task.priority,
      dueDate: task.dueDate ? new Date(task.dueDate).toISOString().split('T')[0] : '',
      projectId: task.projectId,
      userId: task.userId
    });
    setShowModal(true);
  };

  const handleDelete = async (taskId: number) => {
    if (window.confirm('Tem certeza que deseja excluir esta tarefa?')) {
      try {
        await tasksApi.deleteTask(taskId, 1);
        fetchData();
      } catch (error) {
        console.error('Erro ao excluir tarefa:', error);
      }
    }
  };

  const handleStatusChange = async (taskId: number, newStatus: string) => {
    try {
      await tasksApi.updateTask(taskId, 1, {
        title: tasks.find(t => t.id === taskId)?.title || '',
        description: tasks.find(t => t.id === taskId)?.description || '',
        status: newStatus as TaskStatus,
        dueDate: tasks.find(t => t.id === taskId)?.dueDate || ''
      });
      fetchData();
    } catch (error) {
      console.error('Erro ao atualizar status da tarefa:', error);
    }
  };

  const handleAddCommentClick = (task: Task) => {
    setEditingTask(task);
    setShowCommentModal(true);
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'High': return 'bg-red-100 text-red-800';
      case 'Medium': return 'bg-yellow-100 text-yellow-800';
      case 'Low': return 'bg-green-100 text-green-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Completed': return 'bg-green-100 text-green-800';
      case 'InProgress': return 'bg-blue-100 text-blue-800';
      case 'Pending': return 'bg-yellow-100 text-yellow-800';
      case 'Cancelled': return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-primary-600"></div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-2xl font-bold text-gray-900">Tarefas</h1>
          <p className="mt-1 text-sm text-gray-500">
            Gerencie suas tarefas e acompanhe o progresso
          </p>
        </div>
        <button
          onClick={() => setShowModal(true)}
          className="btn btn-primary"
        >
          <Plus className="h-4 w-4 mr-2" />
          Nova Tarefa
        </button>
      </div>

      {/* Tasks List */}
      <div className="space-y-4">
        {tasks.map((task) => (
          <div key={task.id} className="card">
            <div className="flex justify-between items-start">
              <div className="flex-1">
                <div className="flex items-center justify-between mb-2">
                  <h3 className="text-lg font-semibold text-gray-900">{task.title}</h3>
                  <div className="flex items-center space-x-2">
                    <select
                      value={task.status}
                      onChange={(e) => handleStatusChange(task.id, e.target.value)}
                      className="text-sm border border-gray-300 rounded-md px-2 py-1 focus:outline-none focus:ring-2 focus:ring-blue-500"
                    >
                      <option value="Pending">Pendente</option>
                      <option value="InProgress">Em Progresso</option>
                      <option value="Completed">Concluída</option>
                      <option value="Cancelled">Cancelada</option>
                    </select>
                    <button
                      onClick={() => handleAddCommentClick(task)}
                      className="p-1 text-gray-400 hover:text-blue-600"
                      title="Adicionar comentário"
                    >
                      <MessageSquare className="h-4 w-4" />
                    </button>
                    <button
                      onClick={() => handleEdit(task)}
                      className="p-1 text-gray-400 hover:text-gray-600"
                      title="Editar"
                    >
                      <Edit className="h-4 w-4" />
                    </button>
                    <button
                      onClick={() => handleDelete(task.id)}
                      className="p-1 text-gray-400 hover:text-red-600"
                      title="Excluir"
                    >
                      <Trash2 className="h-4 w-4" />
                    </button>
                  </div>
                </div>
                
                {task.description && (
                  <p className="text-gray-600 mb-3">{task.description}</p>
                )}
                
                <div className="flex items-center space-x-4 text-sm text-gray-500">
                  <span className="bg-gray-100 text-gray-800 px-2 py-1 rounded-full">
                    {task.projectName}
                  </span>
                  <span className={`px-2 py-1 rounded-full ${getPriorityColor(task.priority)}`}>
                    <Flag className="h-3 w-3 inline mr-1" />
                    {task.priority}
                  </span>
                  <span className={`px-2 py-1 rounded-full ${getStatusColor(task.status)}`}>
                    {task.status}
                  </span>
                  {task.dueDate && (
                    <span className="flex items-center">
                      <Calendar className="h-3 w-3 mr-1" />
                      {new Date(task.dueDate).toLocaleDateString('pt-BR')}
                    </span>
                  )}
                </div>
              </div>
            </div>
            
            {/* Task History */}
            {task.taskHistories.length > 0 && (
              <div className="mt-4 pt-4 border-t border-gray-200">
                <h4 className="text-sm font-medium text-gray-900 mb-2">Histórico</h4>
                <div className="space-y-2">
                  {task.taskHistories.slice(-3).map((history) => (
                    <div key={history.id} className="text-sm text-gray-600">
                      <p>{history.comment}</p>
                      <p className="text-xs text-gray-400">
                        {history.userName} - {new Date(history.createdAt).toLocaleString('pt-BR')}
                      </p>
                    </div>
                  ))}
                </div>
              </div>
            )}
          </div>
        ))}
      </div>

      {/* Task Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                {editingTask ? 'Editar Tarefa' : 'Nova Tarefa'}
              </h3>
              
              <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                  <label className="label">Título</label>
                  <input
                    type="text"
                    required
                    className="input"
                    value={formData.title}
                    onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                  />
                </div>
                
                <div>
                  <label className="label">Descrição</label>
                  <textarea
                    className="input"
                    rows={3}
                    value={formData.description}
                    onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                  />
                </div>
                
                <div>
                  <label className="label">Projeto</label>
                  <select
                    required
                    className="input"
                    value={formData.projectId}
                    onChange={(e) => setFormData({ ...formData, projectId: parseInt(e.target.value) })}
                  >
                    <option value={0}>Selecione um projeto</option>
                    {projects.map((project) => (
                      <option key={project.id} value={project.id}>
                        {project.name}
                      </option>
                    ))}
                  </select>
                </div>
                
                <div>
                  <label className="label">Prioridade</label>
                  <select
                    className="input"
                    value={formData.priority}
                    onChange={(e) => {
                      if (editingTask && e.target.value !== editingTask.priority) {
                        setWarningMessage('Não é permitido alterar a prioridade de uma tarefa após sua criação.');
                        setShowWarning(true);
                        return;
                      }
                      setFormData({ ...formData, priority: e.target.value as any });
                    }}
                    disabled={editingTask ? true : false}
                  >
                    <option value="Low">Baixa</option>
                    <option value="Medium">Média</option>
                    <option value="High">Alta</option>
                  </select>
                  {editingTask && (
                    <p className="text-xs text-gray-500 mt-1">
                      A prioridade não pode ser alterada após a criação da tarefa
                    </p>
                  )}
                </div>
                
                <div>
                  <label className="label">Data de Vencimento</label>
                  <input
                    type="date"
                    className="input"
                    value={formData.dueDate}
                    onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                  />
                </div>
                
                <div className="flex justify-end space-x-3">
                  <button
                    type="button"
                    onClick={() => setShowModal(false)}
                    className="btn btn-secondary"
                  >
                    Cancelar
                  </button>
                  <button type="submit" className="btn btn-primary">
                    {editingTask ? 'Atualizar' : 'Criar'}
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}

      {/* Comment Modal */}
      {showCommentModal && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <h3 className="text-lg font-medium text-gray-900 mb-4">
                Adicionar Comentário
              </h3>
              
              <form onSubmit={handleAddComment} className="space-y-4">
                <div>
                  <label className="label">Comentário</label>
                  <textarea
                    required
                    className="input"
                    rows={4}
                    value={commentData.comment}
                    onChange={(e) => setCommentData({ ...commentData, comment: e.target.value })}
                    placeholder="Digite seu comentário..."
                  />
                </div>
                
                <div className="flex justify-end space-x-3">
                  <button
                    type="button"
                    onClick={() => setShowCommentModal(false)}
                    className="btn btn-secondary"
                  >
                    Cancelar
                  </button>
                  <button type="submit" className="btn btn-primary">
                    Adicionar
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      )}

      {/* Warning Modal */}
      {showWarning && (
        <div className="fixed inset-0 bg-gray-600 bg-opacity-50 overflow-y-auto h-full w-full z-50">
          <div className="relative top-20 mx-auto p-5 border w-96 shadow-lg rounded-md bg-white">
            <div className="mt-3">
              <div className="mx-auto flex items-center justify-center h-12 w-12 rounded-full bg-yellow-100">
                <svg className="h-6 w-6 text-yellow-600" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-2.5L13.732 4c-.77-.833-1.732-.833-2.5 0L4.268 16.5c-.77.833.192 2.5 1.732 2.5z" />
                </svg>
              </div>
              <div className="mt-2 text-center">
                <h3 className="text-lg font-medium text-gray-900">Aviso</h3>
                <div className="mt-2 px-7 py-3">
                  <p className="text-sm text-gray-500">{warningMessage}</p>
                </div>
                <div className="flex justify-center mt-4">
                  <button
                    onClick={() => setShowWarning(false)}
                    className="btn btn-primary"
                  >
                    Entendi
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default Tasks;

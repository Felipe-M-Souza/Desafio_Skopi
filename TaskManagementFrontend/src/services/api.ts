import axios from 'axios';
import { 
  Project, 
  Task, 
  CreateProjectDto, 
  UpdateProjectDto, 
  CreateTaskDto, 
  UpdateTaskDto, 
  AddTaskCommentDto,
  UserTaskReport 
} from '../types';

const API_BASE_URL = (import.meta as any).env?.VITE_API_URL || 'http://localhost:5000';

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Projects API
export const projectsApi = {
  getUserProjects: (userId: number) => 
    api.get<Project[]>(`/api/projects/user/${userId}`),
  
  getProject: (projectId: number, userId: number) => 
    api.get<Project>(`/api/projects/${projectId}/user/${userId}`),
  
  createProject: (data: CreateProjectDto) => 
    api.post<Project>('/api/projects', data),
  
  updateProject: (projectId: number, userId: number, data: UpdateProjectDto) => 
    api.put<Project>(`/api/projects/${projectId}/user/${userId}`, data),
  
  deleteProject: (projectId: number, userId: number) => 
    api.delete(`/api/projects/${projectId}/user/${userId}`),
};

// Tasks API
export const tasksApi = {
  getProjectTasks: (projectId: number, userId: number) => 
    api.get<Task[]>(`/api/tasks/project/${projectId}/user/${userId}`),
  
  getTask: (taskId: number, userId: number) => 
    api.get<Task>(`/api/tasks/${taskId}/user/${userId}`),
  
  createTask: (data: CreateTaskDto) => 
    api.post<Task>('/api/tasks', data),
  
  updateTask: (taskId: number, userId: number, data: UpdateTaskDto) => 
    api.put<Task>(`/api/tasks/${taskId}/user/${userId}`, data),
  
  deleteTask: (taskId: number, userId: number) => 
    api.delete(`/api/tasks/${taskId}/user/${userId}`),
  
  addTaskComment: (taskId: number, data: AddTaskCommentDto) => 
    api.post<Task>(`/api/tasks/${taskId}/comments`, data),
};

// Reports API
export const reportsApi = {
  getUserTaskReport: (managerUserId: number) => 
    api.get<UserTaskReport[]>(`/api/reports/user-tasks/${managerUserId}`),
};

export default api;

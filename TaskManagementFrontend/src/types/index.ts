export interface User {
  id: number;
  name: string;
  email: string;
  role: 'User' | 'Manager';
  createdAt: string;
}

export interface Project {
  id: number;
  name: string;
  description?: string;
  createdAt: string;
  updatedAt?: string;
  userId: number;
  userName: string;
  taskCount: number;
}

export interface Task {
  id: number;
  title: string;
  description?: string;
  status: TaskStatus;
  priority: TaskPriority;
  createdAt: string;
  updatedAt?: string;
  dueDate?: string;
  projectId: number;
  projectName: string;
  userId: number;
  userName: string;
  taskHistories: TaskHistory[];
}

export interface TaskHistory {
  id: number;
  comment: string;
  createdAt: string;
  userName: string;
}

export type TaskStatus = 'Pending' | 'InProgress' | 'Completed' | 'Cancelled';
export type TaskPriority = 'Low' | 'Medium' | 'High';

export interface CreateProjectDto {
  name: string;
  description?: string;
  userId: number;
}

export interface UpdateProjectDto {
  name: string;
  description?: string;
}

export interface CreateTaskDto {
  title: string;
  description?: string;
  priority: TaskPriority;
  dueDate?: string;
  projectId: number;
  userId: number;
}

export interface UpdateTaskDto {
  title: string;
  description?: string;
  status: TaskStatus;
  dueDate?: string;
}

export interface AddTaskCommentDto {
  comment: string;
  userId: number;
}

export interface UserTaskReport {
  userId: number;
  userName: string;
  userEmail: string;
  averageCompletedTasks: number;
  totalCompletedTasks: number;
  reportDate: string;
}

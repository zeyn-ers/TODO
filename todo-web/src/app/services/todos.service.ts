import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EMPTY, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';

export interface TodoDto {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  updatedAt?: string | null;
  dueDate?: string | null;
  priority: number;
  categoryId: number | null;
  categoryName?: string | null;
}

export interface CreateTodoDto {
  title: string;
  description: string;
  dueDate?: string | null;
  priority: number;
  categoryId?: number | null;
}

export interface UpdateTodoDto {
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string | null;
  priority: number;
  categoryId?: number | null;
}

@Injectable({ providedIn: 'root' })
export class TodosService {
  private http = inject(HttpClient);
  private snack = inject(MatSnackBar);

  readonly todos = signal<TodoDto[]>([]);

  loadAll() {
    this.http.get<TodoDto[]>('/api/todos').pipe(
      tap(list => this.todos.set(list ?? [])),
      catchError(err => {
        this.snack.open(this.msg(err, 'Todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  add(dto: CreateTodoDto) {
    this.http.post<TodoDto>('/api/todos', dto).pipe(
      tap(created => {
        this.todos.set([created, ...this.todos()]);
        this.snack.open('Todo eklendi', 'Kapat', { duration: 1200 });
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Todo eklenemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  update(id: number, dto: UpdateTodoDto) {
    const prev = this.todos();
    const patched = prev.map(t => t.id === id ? { ...t, ...dto } : t);
    this.todos.set(patched);

    this.http.put<TodoDto>(`/api/todos/${id}`, dto).pipe(
      tap(updated => {
        const list = this.todos().map(t => t.id === id ? updated : t);
        this.todos.set(list);
        this.snack.open('Todo güncellendi', 'Kapat', { duration: 1200 });
      }),
      catchError(err => {
        this.todos.set(prev);
        this.snack.open(this.msg(err, 'Todo güncellenemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  toggle(id: number) {
    const todo = this.todos().find(t => t.id === id);
    if (!todo) return;

    const dto: UpdateTodoDto = {
      title: todo.title,
      description: todo.description,
      isCompleted: !todo.isCompleted,
      dueDate: todo.dueDate,
      priority: todo.priority,
      categoryId: todo.categoryId ?? null
    };

    this.update(id, dto);
  }

  markCompleted(id: number) {
    this.http.patch<TodoDto>(`/api/todos/${id}/complete`, {}).pipe(
      tap(updated => {
        const list = this.todos().map(t => t.id === id ? updated : t);
        this.todos.set(list);
        this.snack.open('Todo tamamlandı', 'Kapat', { duration: 1200 });
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Todo tamamlanamadı'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  markPending(id: number) {
    this.http.patch<TodoDto>(`/api/todos/${id}/pending`, {}).pipe(
      tap(updated => {
        const list = this.todos().map(t => t.id === id ? updated : t);
        this.todos.set(list);
        this.snack.open('Todo beklemede', 'Kapat', { duration: 1200 });
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Todo güncellenemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  remove(id: number) {
    const prev = this.todos();
    const after = prev.filter(t => t.id !== id);
    this.todos.set(after);

    this.http.delete<void>(`/api/todos/${id}`).pipe(
      tap(() => this.snack.open('Todo silindi', 'Kapat', { duration: 1200 })),
      catchError(err => {
        this.todos.set(prev);
        this.snack.open(this.msg(err, 'Todo silinemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  loadByCategory(categoryId: number) {
    this.http.get<TodoDto[]>(`/api/todos/by-category/${categoryId}`).pipe(
      tap(list => this.todos.set(list ?? [])),
      catchError(err => {
        this.snack.open(this.msg(err, 'Kategori todoları alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  clearCompleted() {
    const completed = this.todos().filter(t => t.isCompleted);
    completed.forEach(todo => this.remove(todo.id));
  }

  private msg(err: any, fallback: string) {
    const server = err?.error?.message || err?.error?.title || err?.message;
    return server ? `${fallback}: ${server}` : fallback;
  }
}
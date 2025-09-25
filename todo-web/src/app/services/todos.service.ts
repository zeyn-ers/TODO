import { Injectable, inject, signal, ChangeDetectorRef, ApplicationRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EMPTY, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../environments/environment';

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
  notes: TodoNoteDto[];
}

export interface CreateTodoDto {
  title: string;
  description: string;
  dueDate?: string | null;
  priority: number;
  categoryId?: number | null;
  initialNote?: string;
}

export interface UpdateTodoDto {
  title: string;
  description: string;
  isCompleted: boolean;
  dueDate?: string | null;
  priority: number;
  categoryId?: number | null;
}

export interface TodoNoteDto {
  id: number;
  todoId: number;
  content: string;
  createdAt: string;
}

export interface CreateTodoNoteDto {
  content: string;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

@Injectable({ providedIn: 'root' })
export class TodosService {
  private http = inject(HttpClient);
  private snack = inject(MatSnackBar);
  private appRef = inject(ApplicationRef);

  readonly todos = signal<TodoDto[]>([]);
  readonly pagedTodos = signal<PagedResult<TodoDto> | null>(null);
  readonly selectedTodo = signal<TodoDto | null>(null);

  private tick() {
    this.appRef.tick();
  }

  loadAll() {
    this.http.get<TodoDto[]>(`${environment.apiBase}/todos`).pipe(
      tap(list => {
        const todos = (list ?? []).map(t => ({ ...t, notes: t.notes || [] }));
        this.todos.set(todos);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  loadPaged(pageNumber: number = 1, pageSize: number = 10) {
    // Backend'de pagination yok, normal listeyi kullan
    this.loadAll();
  }

  loadByCategory(categoryId: number) {
    // Backend'de category filter yok, client-side filter kullan
    this.loadAll();
  }

  loadCompleted() {
    // Backend'de completed filter yok, client-side filter kullan
    this.loadAll();
  }

  loadPending() {
    // Backend'de pending filter yok, client-side filter kullan
    this.loadAll();
  }

  add(dto: CreateTodoDto) {
    this.http.post<TodoDto>(`${environment.apiBase}/todos`, dto).pipe(
      tap(created => {
        this.todos.set([created, ...this.todos()]);
        this.snack.open('Todo eklendi', 'Kapat', { duration: 1200 });
        this.tick();
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
    this.tick();

    this.http.put<TodoDto>(`${environment.apiBase}/todos/${id}`, dto).pipe(
      tap(updated => {
        const list = this.todos().map(t => t.id === id ? updated : t);
        this.todos.set(list);
        this.snack.open('Todo güncellendi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.todos.set(prev);
        this.snack.open(this.msg(err, 'Todo güncellenemedi'), 'Kapat', { duration: 1800 });
        this.tick();
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

  remove(id: number) {
    const prev = this.todos();
    const after = prev.filter(t => t.id !== id);
    this.todos.set(after);
    this.tick();

    this.http.delete<void>(`${environment.apiBase}/todos/${id}`).pipe(
      tap(() => {
        this.snack.open('Todo silindi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.todos.set(prev);
        this.snack.open(this.msg(err, 'Todo silinemedi'), 'Kapat', { duration: 1800 });
        this.tick();
        return EMPTY;
      })
    ).subscribe();
  }

  clearCompleted() {
    const completed = this.todos().filter(t => t.isCompleted);
    completed.forEach(todo => this.remove(todo.id));
  }

  selectTodo(todo: TodoDto | null) {
    this.selectedTodo.set(todo);
    this.tick();
  }

  loadFiltered(categoryId?: number, done?: boolean, sort?: string, include = true) {
    // Backend'de filter parametreleri yok, normal listeyi kullan
    this.loadAll();
  }

  addNote(todoId: number, content: string) {
    this.http.post<TodoNoteDto>(`/api/todos/${todoId}/notes`, { content }).pipe(
      tap(note => {
        const selected = this.selectedTodo();
        if (selected && selected.id === todoId) {
          selected.notes.push(note);
          this.selectedTodo.set({ ...selected });
        }
        this.snack.open('Not eklendi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Not eklenemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  removeNote(todoId: number, noteId: number) {
    this.http.delete<void>(`/api/todos/${todoId}/notes/${noteId}`).pipe(
      tap(() => {
        const selected = this.selectedTodo();
        if (selected && selected.id === todoId) {
          selected.notes = selected.notes.filter(n => n.id !== noteId);
          this.selectedTodo.set({ ...selected });
        }
        this.snack.open('Not silindi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Not silinemedi'), 'Kapat', { duration: 1800 });
        return EMPTY;
      })
    ).subscribe();
  }

  private msg(err: any, fallback: string) {
    const server = err?.error?.message || err?.error?.title || err?.message;
    return server ? `${fallback}: ${server}` : fallback;
  }
}
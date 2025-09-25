import { Injectable, inject, signal, ChangeDetectorRef, ApplicationRef } from '@angular/core';
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
    this.http.get<TodoDto[]>('/api/v1/todos').pipe(
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
    this.http.get<PagedResult<TodoDto>>(`/api/v2/todos?pageNumber=${pageNumber}&pageSize=${pageSize}`).pipe(
      tap(result => {
        if (result) {
          result.items = result.items.map(t => ({ ...t, notes: t.notes || [] }));
        }
        this.pagedTodos.set(result);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Sayfalı todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of(null);
      })
    ).subscribe();
  }

  loadByCategory(categoryId: number) {
    this.http.get<TodoDto[]>(`/api/v2/todos/by-category/${categoryId}`).pipe(
      tap(list => {
        const todos = (list ?? []).map(t => ({ ...t, notes: t.notes || [] }));
        this.todos.set(todos);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Kategoriye göre todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  loadCompleted() {
    this.http.get<TodoDto[]>('/api/v2/todos/completed').pipe(
      tap(list => {
        const todos = (list ?? []).map(t => ({ ...t, notes: t.notes || [] }));
        this.todos.set(todos);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Tamamlanan todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  loadPending() {
    this.http.get<TodoDto[]>('/api/v2/todos/pending').pipe(
      tap(list => {
        const todos = (list ?? []).map(t => ({ ...t, notes: t.notes || [] }));
        this.todos.set(todos);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Bekleyen todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  add(dto: CreateTodoDto) {
    this.http.post<TodoDto>('/api/v1/todos', dto).pipe(
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

    this.http.put<TodoDto>(`/api/v1/todos/${id}`, dto).pipe(
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

    this.http.delete<void>(`/api/v1/todos/${id}`).pipe(
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
    let url = '/api/v2/todos?include=' + include;
    if (categoryId) url += '&categoryId=' + categoryId;
    if (done !== undefined) url += '&done=' + done;
    if (sort) url += '&sort=' + sort;

    this.http.get<TodoDto[]>(url).pipe(
      tap(list => {
        const todos = (list ?? []).map(t => ({ ...t, notes: t.notes || [] }));
        this.todos.set(todos);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Filtrelenmiş todo listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  addNote(todoId: number, content: string) {
    this.http.post<TodoNoteDto>(`/api/v2/todos/${todoId}/notes`, { content }).pipe(
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
    this.http.delete<void>(`/api/v2/todos/${todoId}/notes/${noteId}`).pipe(
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
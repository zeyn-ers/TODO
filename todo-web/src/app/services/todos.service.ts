import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, of, tap, map } from 'rxjs';

export interface TodoDto {
  id: number;
  title: string;
  description: string;
  isCompleted: boolean;
  createdAt: string;
  updatedAt?: string | null;
  dueDate?: string | null;
  priority: number;          // 1..3
  categoryId?: number | null;
  categoryName?: string | null;
}

export interface CreateTodoDto {
  title: string;
  description?: string;
  dueDate?: string | null;
  priority: number;          // zorunlu
  categoryId?: number | null;
}

export interface UpdateTodoDto extends CreateTodoDto {
  isCompleted: boolean;
}

@Injectable({ providedIn: 'root' })
export class TodosService {
  private http = inject(HttpClient);
  private base = '/api/v2/todos';

  private _todos = new BehaviorSubject<TodoDto[]>([]);
  readonly todos$ = this._todos.asObservable();

  get todos(): TodoDto[] {
    return this._todos.value;
  }

  load(includeRelations = true): Observable<TodoDto[]> {
    const url = `${this.base}?include=${includeRelations}`;
    return this.http.get<{ data: TodoDto[] }>(url).pipe(
      map(res => res.data),
      tap(data => this._todos.next(data)),
      catchError(err => {
        console.error('todos load error', err);
        this._todos.next([]);
        return of([]);
      })
    );
  }

  add(payload: { title: string; description?: string; categoryId?: number; dueDate?: string }): Observable<TodoDto | null> {
    // priority zorunlu -> varsayılan 1
    const body: CreateTodoDto = {
      title: payload.title,
      description: payload.description ?? '',
      categoryId: payload.categoryId,
      dueDate: payload.dueDate ?? null,
      priority: 1,
    };
    return this.http.post<{ data: TodoDto }>(this.base, body).pipe(
      map(res => res.data),
      tap(todo => this._todos.next([todo, ...this.todos])),
      catchError(err => {
        console.error('todo add error', err);
        return of(null);
      })
    );
  }

  toggle(id: number): Observable<TodoDto | null> {
    const t = this.todos.find(x => x.id === id);
    if (!t) return of(null);
    const body: UpdateTodoDto = {
      title: t.title,
      description: t.description,
      categoryId: t.categoryId ?? undefined,
      dueDate: t.dueDate ?? null,
      priority: t.priority ?? 1,
      isCompleted: !t.isCompleted,
    };
    return this.http.put<{ data: TodoDto }>(`${this.base}/${id}`, body).pipe(
      map(res => res.data),
      tap(todo => {
        const list: TodoDto[] = this.todos.map(x => (x.id === id ? todo : x));
        this._todos.next(list);
      }),
      catchError(err => {
        console.error('todo toggle error', err);
        return of(null);
      })
    );
  }

  update(id: number, patch: Partial<UpdateTodoDto>): Observable<TodoDto | null> {
    const t = this.todos.find(x => x.id === id);
    if (!t) return of(null);
    const body: UpdateTodoDto = {
      title: patch.title ?? t.title,
      description: patch.description ?? t.description,
      categoryId: patch.categoryId ?? t.categoryId ?? undefined,
      dueDate: patch.dueDate ?? t.dueDate ?? null,
      priority: patch.priority ?? t.priority ?? 1,
      isCompleted: patch.isCompleted ?? t.isCompleted,
    };
    return this.http.put<{ data: TodoDto }>(`${this.base}/${id}`, body).pipe(
      map(res => res.data),
      tap(todo => {
        const list: TodoDto[] = this.todos.map(x => (x.id === id ? todo : x));
        this._todos.next(list);
      }),
      catchError(err => {
        console.error('todo update error', err);
        return of(null);
      })
    );
  }

  remove(id: number): Observable<boolean> {
    return this.http.delete<{ data: boolean }>(`${this.base}/${id}`).pipe(
      map(res => res.data),
      tap(() => {
        const list: TodoDto[] = this.todos.filter(x => x.id !== id);
        this._todos.next(list);
      }),
      catchError(err => {
        console.error('todo delete error', err);
        return of(false);
      })
    );
  }
}

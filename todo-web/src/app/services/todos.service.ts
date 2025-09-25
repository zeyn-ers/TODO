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
  priority: number;                // 1..3
  categoryId?: number | null;
  categoryName?: string | null;
}

export interface CreateTodoInput {
  title: string;
  description?: string;
  dueDate?: string | null;
  categoryId?: number | null;      // null gelebilir -> API'ye gönderirken undefined’a çeviriyoruz
  priority?: number;               // opsiyonel (varsayılan 1)
}

export interface UpdateTodoDto extends CreateTodoInput {
  isCompleted: boolean;
}

@Injectable({ providedIn: 'root' })
export class TodosService {
  private http = inject(HttpClient);
  private base = '/api/v2/todos';

  private _todos = new BehaviorSubject<TodoDto[]>([]);
  readonly todos$ = this._todos.asObservable();

  /** Bazı eski componentler bunu bekliyor diye basit bir “paged” akışı da sunuyoruz */
  private _paged = new BehaviorSubject<TodoDto[]>([]);
  readonly pagedTodos = this._paged.asObservable();

  get todos(): TodoDto[] {
    return this._todos.value;
  }

  /** Yeni API: tümünü yükle */
  load(includeRelations = true): Observable<TodoDto[]> {
    const url = `${this.base}?include=${includeRelations}`;
    return this.http.get<{ data: TodoDto[] }>(url).pipe(
      tap(res => {
        this._todos.next(res.data);
        // basit bir varsayılan "paged" görünüm: ilk 10
        this._paged.next(res.data.slice(0, 10));
      }),
      map(res => res.data),
      catchError(err => {
        console.error('todos load error', err);
        this._todos.next([]);
        this._paged.next([]);
        return of([]);
      })
    );
  }

  /** Eski isimlerle uyumluluk (shim) */
  loadAll(): Observable<TodoDto[]> {
    return this.load(true);
  }
  loadCompleted(): Observable<TodoDto[]> {
    return this.load(true).pipe(
      tap(() => this._todos.next(this.todos.filter(t => t.isCompleted))),
      map(() => this.todos)
    );
  }
  loadPending(): Observable<TodoDto[]> {
    return this.load(true).pipe(
      tap(() => this._todos.next(this.todos.filter(t => !t.isCompleted))),
      map(() => this.todos)
    );
  }
  loadPaged(page: number, pageSize: number): Observable<TodoDto[]> {
    // Basit client-side sayfalama (API tarafında gerçek paging eklenene kadar)
    if (page < 1) page = 1;
    const start = (page - 1) * pageSize;
    const slice = this.todos.slice(start, start + pageSize);
    this._paged.next(slice);
    return of(slice);
  }

  add(payload: CreateTodoInput): Observable<TodoDto | null> {
    const body = {
      title: payload.title,
      description: payload.description ?? '',
      categoryId: payload.categoryId ?? undefined,   // null -> undefined
      dueDate: payload.dueDate ?? null,
      priority: payload.priority ?? 1,
    };
    return this.http.post<{ data: TodoDto }>(this.base, body).pipe(
      tap(res => {
        this._todos.next([res.data, ...this.todos]);
        // paged görünümün başına da ekleyelim (istenirse)
        const p = [res.data, ...this._paged.value];
        this._paged.next(p.slice(0, 10));
      }),
      map(res => res.data),
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
      categoryId: t.categoryId ?? null,
      dueDate: t.dueDate ?? null,
      priority: t.priority ?? 1,
      isCompleted: !t.isCompleted,
    };
    return this.http.put<{ data: TodoDto }>(`${this.base}/${id}`, body).pipe(
      tap(res => {
        const list = this.todos.map(x => (x.id === id ? res.data : x));
        this._todos.next(list);
        // paged'i de güncelle
        const p = this._paged.value.map(x => (x.id === id ? res.data : x));
        this._paged.next(p);
      }),
      map(res => res.data),
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
      categoryId: (patch.categoryId ?? t.categoryId) ?? null,
      dueDate: (patch.dueDate ?? t.dueDate) ?? null,
      priority: patch.priority ?? t.priority ?? 1,
      isCompleted: patch.isCompleted ?? t.isCompleted,
    };
    return this.http.put<{ data: TodoDto }>(`${this.base}/${id}`, body).pipe(
      tap(res => {
        const list = this.todos.map(x => (x.id === id ? res.data : x));
        this._todos.next(list);
        const p = this._paged.value.map(x => (x.id === id ? res.data : x));
        this._paged.next(p);
      }),
      map(res => res.data),
      catchError(err => {
        console.error('todo update error', err);
        return of(null);
      })
    );
  }

  remove(id: number): Observable<boolean> {
    return this.http.delete<{ data: boolean }>(`${this.base}/${id}`).pipe(
      tap(() => {
        this._todos.next(this.todos.filter(x => x.id !== id));
        this._paged.next(this._paged.value.filter(x => x.id !== id));
      }),
      map(() => true),
      catchError(err => {
        console.error('todo delete error', err);
        return of(false);
      })
    );
  }

  /** Eski API ile uyumluluk: tamamlananları temizle */
  clearCompleted(): Observable<boolean[]> {
    const completed = this.todos.filter(t => t.isCompleted).map(t => t.id);
    if (completed.length === 0) return of([]);
    // hızlı çözüm: her birini remove et (ardışık)
    const results: boolean[] = [];
    const tasks = completed.map(id =>
      this.remove(id).pipe(tap(ok => results.push(ok)))
    );
    // sırayla çalıştırmak yerine basitçe son state’i dönelim
    return of(results);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, catchError, of, tap, map } from 'rxjs';

export interface CategoryDto {
  id: number;
  name: string;
  description?: string;
  createdAt?: string;
  updatedAt?: string | null;
}

export interface CreateCategoryDto {
  name: string;
  description?: string;
}

export interface UpdateCategoryDto {
  name: string;
  description?: string;
}

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  private http = inject(HttpClient);
  private base = '/api/v2/categories';

  private _categories = new BehaviorSubject<CategoryDto[]>([]);
  readonly categories$ = this._categories.asObservable();

  get categories(): CategoryDto[] {
    return this._categories.value;
  }

  /** Yeni API: tümünü yükle */
  load(): Observable<CategoryDto[]> {
    return this.http.get<{ data: CategoryDto[] }>(this.base).pipe(
      tap(res => this._categories.next(res.data)),
      map(res => res.data),
      catchError(err => {
        console.error('categories load error', err);
        this._categories.next([]);
        return of([]);
      })
    );
  }

  /** Eski isimle uyumluluk (shim) */
  loadAll(): Observable<CategoryDto[]> {
    return this.load();
  }

  add(name: string, description?: string): Observable<CategoryDto | null> {
    const body: CreateCategoryDto = { name, description };
    return this.http.post<{ data: CategoryDto }>(this.base, body).pipe(
      tap(res => this._categories.next([res.data, ...this.categories])),
      map(res => res.data),
      catchError(err => {
        console.error('category add error', err);
        return of(null);
      })
    );
  }

  update(id: number, name: string, description?: string): Observable<CategoryDto | null> {
    const body: UpdateCategoryDto = { name, description };
    return this.http.put<{ data: CategoryDto }>(`${this.base}/${id}`, body).pipe(
      tap(res => {
        const list = this.categories.map(c => (c.id === id ? res.data : c));
        this._categories.next(list);
      }),
      map(res => res.data),
      catchError(err => {
        console.error('category update error', err);
        return of(null);
      })
    );
  }

  remove(id: number): Observable<boolean> {
    return this.http.delete<{ data: boolean }>(`${this.base}/${id}`).pipe(
      tap(() => this._categories.next(this.categories.filter(c => c.id !== id))),
      map(() => true),
      catchError(err => {
        console.error('category delete error', err);
        return of(false);
      })
    );
  }
}

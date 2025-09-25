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

  /** iç state */
  private _categories = new BehaviorSubject<CategoryDto[]>([]);
  /** ui tarafı bunu dinler */
  readonly categories$ = this._categories.asObservable();

  /** hafızadaki son liste */
  get categories(): CategoryDto[] {
    return this._categories.value;
  }

  /** tümünü çek */
  load(): Observable<CategoryDto[]> {
    return this.http.get<{ data: CategoryDto[] }>(this.base).pipe(
      map(res => res.data),
      tap(data => this._categories.next(data)),
      catchError(err => {
        console.error('categories load error', err);
        this._categories.next([]);
        return of([]);
      })
    );
  }

  add(name: string, description?: string): Observable<CategoryDto | null> {
    const body: CreateCategoryDto = { name, description };
    return this.http.post<{ data: CategoryDto }>(this.base, body).pipe(
      map(res => res.data),
      tap(category => this._categories.next([category, ...this.categories])),
      catchError(err => {
        console.error('category add error', err);
        return of(null);
      })
    );
  }

  update(id: number, name: string, description?: string): Observable<CategoryDto | null> {
    const body: UpdateCategoryDto = { name, description };
    return this.http.put<{ data: CategoryDto }>(`${this.base}/${id}`, body).pipe(
      map(res => res.data),
      tap(category => {
        const list: CategoryDto[] = this.categories.map(c => (c.id === id ? category : c));
        this._categories.next(list);
      }),
      catchError(err => {
        console.error('category update error', err);
        return of(null);
      })
    );
  }

  remove(id: number): Observable<boolean> {
    return this.http.delete<{ data: boolean }>(`${this.base}/${id}`).pipe(
      map(res => res.data),
      tap(() => {
        const list: CategoryDto[] = this.categories.filter(c => c.id !== id);
        this._categories.next(list);
      }),
      catchError(err => {
        console.error('category delete error', err);
        return of(false);
      })
    );
  }
}

import { Injectable, inject, signal, ApplicationRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EMPTY, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { MatSnackBar } from '@angular/material/snack-bar';
import { environment } from '../../environments/environment';

export interface CategoryDto {
  id: number;
  name: string;
  description?: string | null;
  isActive?: boolean;
  createdAt?: string;
}

@Injectable({ providedIn: 'root' })
export class CategoriesService {
  private http = inject(HttpClient);
  private snack = inject(MatSnackBar);
  private appRef = inject(ApplicationRef);

  readonly categories = signal<CategoryDto[]>([]);

  private tick() {
    this.appRef.tick();
  }

  loadAll() {
    this.http.get<CategoryDto[]>(`${environment.apiBase}/categories`).pipe(
      tap(list => {
        this.categories.set(list ?? []);
        this.tick();
      }),
      catchError(err => {
        this.snack.open(this.msg(err, 'Kategori listesi alınamadı'), 'Kapat', { duration: 1600 });
        return of([]);
      })
    ).subscribe();
  }

  add(name: string) {
    const prev = this.categories();
    const optimistic: CategoryDto = { id: Date.now(), name: name.trim() };

    this.categories.set([optimistic, ...prev]);
    this.tick();

    this.http.post<CategoryDto>(`${environment.apiBase}/categories`, { name, isActive: true }).pipe(
      tap(saved => {
        const withReal = [saved, ...prev];
        this.categories.set(withReal);
        this.snack.open('Kategori eklendi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.categories.set(prev);
        this.snack.open(this.msg(err, 'Kategori eklenemedi'), 'Kapat', { duration: 1800 });
        this.tick();
        return EMPTY;
      })
    ).subscribe();
  }

  update(id: number, name: string) {
    const prev = this.categories();
    const patched = prev.map(c => c.id === id ? { ...c, name } : c);
    this.categories.set(patched);
    this.tick();

    this.http.put<CategoryDto>(`${environment.apiBase}/categories/${id}`, { name, isActive: true }).pipe(
      tap(() => {
        this.snack.open('Kategori güncellendi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.categories.set(prev);
        this.snack.open(this.msg(err, 'Kategori güncellenemedi'), 'Kapat', { duration: 1800 });
        this.tick();
        return EMPTY;
      })
    ).subscribe();
  }

  remove(id: number) {
    const prev = this.categories();
    const after = prev.filter(c => c.id !== id);
    this.categories.set(after);
    this.tick();

    this.http.delete<void>(`${environment.apiBase}/categories/${id}`).pipe(
      tap(() => {
        this.snack.open('Kategori silindi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.categories.set(prev);
        this.snack.open(this.msg(err, 'Kategori silinemedi'), 'Kapat', { duration: 1800 });
        this.tick();
        return EMPTY;
      })
    ).subscribe();
  }

  toggleStatus(id: number) {
    const prev = this.categories();
    
    this.http.patch<CategoryDto>(`/api/categories/${id}/toggle-status`, {}).pipe(
      tap(updated => {
        const list = this.categories().map(c => c.id === id ? updated : c);
        this.categories.set(list);
        this.snack.open('Kategori durumu değiştirildi', 'Kapat', { duration: 1200 });
        this.tick();
      }),
      catchError(err => {
        this.categories.set(prev);
        this.snack.open(this.msg(err, 'Kategori durumu değiştirilemedi'), 'Kapat', { duration: 1800 });
        this.tick();
        return EMPTY;
      })
    ).subscribe();
  }

  private msg(err: any, fallback: string) {
    const server = err?.error?.message || err?.error?.title || err?.message;
    return server ? `${fallback}: ${server}` : fallback;
  }
}
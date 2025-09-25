import { Component, OnInit, inject, ApplicationRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatPaginatorModule } from '@angular/material/paginator';

import { TodosService, TodoDto } from '../services/todos.service';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatListModule, MatCheckboxModule,
    MatChipsModule, MatPaginatorModule
  ],
  template: `
  <mat-card>
    <mat-card-title>ToDo'lar</mat-card-title>
    <mat-card-content>
      <!-- Filtre Butonları -->
      <mat-chip-set style="margin-bottom:16px">
        <mat-chip (click)="loadAll()" [highlighted]="currentView === 'all'">Tümü</mat-chip>
        <mat-chip (click)="loadCompleted()" [highlighted]="currentView === 'completed'">Tamamlanan</mat-chip>
        <mat-chip (click)="loadPending()" [highlighted]="currentView === 'pending'">Bekleyen</mat-chip>
        <mat-chip (click)="loadPaged()" [highlighted]="currentView === 'paged'">Sayfalı (V2)</mat-chip>
      </mat-chip-set>

      <form [formGroup]="form" (ngSubmit)="onAdd()" style="display:flex;gap:8px;margin-bottom:16px">
        <mat-form-field appearance="outline" style="flex:1">
          <mat-label>Yeni todo</mat-label>
          <input matInput formControlName="title" required />
        </mat-form-field>
        <button mat-raised-button color="primary" type="submit">Ekle</button>
      </form>

      <!-- Sayfalı Görünüm -->
      <div *ngIf="currentView === 'paged' && pagedTodos()">
        <p>Toplam: {{pagedTodos()?.totalCount}} | Sayfa: {{pagedTodos()?.pageNumber}}/{{pagedTodos()?.totalPages}}</p>
        <mat-list>
          <mat-list-item *ngFor="let t of pagedTodos()?.items">
            <mat-checkbox [checked]="t.isCompleted" (change)="onToggle(t)"></mat-checkbox>
            <div matListItemTitle [style.textDecoration]="t.isCompleted ? 'line-through' : 'none'">
              {{ t.title }}
            </div>
            <button mat-icon-button (click)="onDelete(t)" color="warn">
              <mat-icon>delete</mat-icon>
            </button>
          </mat-list-item>
        </mat-list>
        <mat-paginator 
          [length]="pagedTodos()?.totalCount || 0"
          [pageSize]="10"
          [pageIndex]="(pagedTodos()?.pageNumber || 1) - 1"
          (page)="onPageChange($event)">
        </mat-paginator>
      </div>

      <!-- Normal Görünüm -->
      <mat-list *ngIf="currentView !== 'paged'">
        <mat-list-item *ngFor="let t of todos()">
          <mat-checkbox [checked]="t.isCompleted" (change)="onToggle(t)"></mat-checkbox>
          <div matListItemTitle [style.textDecoration]="t.isCompleted ? 'line-through' : 'none'">
            {{ t.title }}
          </div>
          <button mat-icon-button (click)="onDelete(t)" color="warn">
            <mat-icon>delete</mat-icon>
          </button>
        </mat-list-item>
      </mat-list>
    </mat-card-content>
  </mat-card>
  `
})
export class TodosComponent implements OnInit {
  private svc = inject(TodosService);
  private fb = inject(FormBuilder);
  private appRef = inject(ApplicationRef);

  todos = this.svc.todos;
  pagedTodos = this.svc.pagedTodos;
  form = this.fb.nonNullable.group({ title: [''] });
  currentView: 'all' | 'completed' | 'pending' | 'paged' = 'all';

  ngOnInit() {
    this.loadAll();
  }

  loadAll() {
    this.currentView = 'all';
    this.svc.loadAll();
  }

  loadCompleted() {
    this.currentView = 'completed';
    this.svc.loadCompleted();
  }

  loadPending() {
    this.currentView = 'pending';
    this.svc.loadPending();
  }

  loadPaged() {
    this.currentView = 'paged';
    this.svc.loadPaged(1, 10);
  }

  onPageChange(event: any) {
    this.svc.loadPaged(event.pageIndex + 1, event.pageSize);
  }

  onAdd() {
    const title = this.form.value.title?.trim();
    if (!title) return;
    
    this.svc.add({ title, description: '', priority: 1 });
    this.form.reset();
    this.appRef.tick();
  }

  onToggle(t: TodoDto) {
    this.svc.toggle(t.id);
    this.appRef.tick();
  }

  onDelete(t: TodoDto) {
    if (confirm(`Sil: ${t.title}?`)) {
      this.svc.remove(t.id);
      this.appRef.tick();
    }
  }
}
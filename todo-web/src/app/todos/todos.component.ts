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
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';

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
        <mat-chip (click)="loadPaged()" [highlighted]="currentView === 'paged'">Sayfalı (Basit)</mat-chip>
      </mat-chip-set>

      <form [formGroup]="form" (ngSubmit)="onAdd()" style="display:flex;gap:8px;margin-bottom:16px">
        <mat-form-field appearance="outline" style="flex:1">
          <mat-label>Yeni todo</mat-label>
          <input matInput formControlName="title" required />
        </mat-form-field>
        <button mat-raised-button color="primary" type="submit">Ekle</button>
      </form>

      <!-- Sayfalı Görünüm (basit client-side) -->
      <div *ngIf="currentView === 'paged'">
        <p>Toplam: {{todos.length}} | Sayfa: {{pageIndex+1}}</p>
        <mat-list>
          <mat-list-item *ngFor="let t of paged">
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
          [length]="todos.length"
          [pageSize]="pageSize"
          [pageIndex]="pageIndex"
          (page)="onPageChange($event)">
        </mat-paginator>
      </div>

      <!-- Normal Görünüm -->
      <mat-list *ngIf="currentView !== 'paged'">
        <mat-list-item *ngFor="let t of todos">
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

  // array (signal değil)
  todos: TodoDto[] = this.svc.todos;

  // simple paged view (client-side)
  paged: TodoDto[] = [];
  pageIndex = 0;
  pageSize = 10;

  form = this.fb.nonNullable.group({ title: [''] });
  currentView: 'all' | 'completed' | 'pending' | 'paged' = 'all';

  ngOnInit() {
    this.loadAll();
  }

  private refreshLocal() {
    this.todos = this.svc.todos;
    if (this.currentView === 'paged') {
      const start = this.pageIndex * this.pageSize;
      this.paged = this.todos.slice(start, start + this.pageSize);
    }
    this.appRef.tick();
  }

  loadAll() {
    this.currentView = 'all';
    this.svc.loadAll().subscribe(() => this.refreshLocal());
  }

  loadCompleted() {
    this.currentView = 'completed';
    this.svc.loadCompleted().subscribe(() => this.refreshLocal());
  }

  loadPending() {
    this.currentView = 'pending';
    this.svc.loadPending().subscribe(() => this.refreshLocal());
  }

  loadPaged() {
    this.currentView = 'paged';
    this.pageIndex = 0;
    this.svc.loadAll().subscribe(() => this.refreshLocal());
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.refreshLocal();
  }

  onAdd() {
    const title = this.form.value.title?.trim();
    if (!title) return;
    this.svc.add({ title, description: '', priority: 1 }).subscribe(() => {
      this.form.reset();
      this.refreshLocal();
    });
  }

  onToggle(t: TodoDto) {
    this.svc.toggle(t.id).subscribe(() => this.refreshLocal());
  }

  onDelete(t: TodoDto) {
    if (confirm(`Sil: ${t.title}?`)) {
      this.svc.remove(t.id).subscribe(() => this.refreshLocal());
    }
  }
}

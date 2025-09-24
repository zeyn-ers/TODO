import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatMenuModule } from '@angular/material/menu';

import { CategoriesService, CategoryDto } from '../services/categories.service';
import { CategoryEditDialogComponent } from './category-edit.dialog';
import { TodosService, TodoDto } from '../services/todos.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, FormsModule, NgFor,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatListModule,
    MatDialogModule, MatSnackBarModule, MatSelectModule,
    MatChipsModule, MatCheckboxModule, MatDividerModule, MatTooltipModule,
    MatMenuModule
  ],
  styles: [`
    :host { display:block; background:linear-gradient(180deg,#fafafa,#f6f7fb); padding:1rem; }
    .board { display:grid; grid-template-columns:1.25fr .85fr; gap:1rem; align-items:start; }
    @media (max-width:1080px){ .board{ grid-template-columns:1fr; } }
    .panel mat-card{ border-radius:16px; box-shadow:0 8px 24px rgba(30,41,59,.08); }
    .panel-header{ display:flex; align-items:center; gap:.5rem; }
    .panel-header h2{ margin:0; font-weight:700; }
    .add-row{ display:flex; gap:.5rem; align-items:center; margin:.5rem 0 1rem; }
    .todo-input{ font-size:1.05rem; }
    .big-btn{ height:48px; font-weight:700; letter-spacing:.3px; border-radius:999px; }
    .todo-item{ position:relative; padding-left:12px; border-left:6px solid var(--bar,#6366f1);
                border-radius:10px; background:#fff; margin-bottom:.5rem; box-shadow:0 4px 14px rgba(15,23,42,.06); }
    .todo-main{ display:flex; align-items:center; gap:.75rem; padding:.35rem .25rem .35rem .35rem; }
    .todo-title{ font-weight:600; }
    .todo-title.done{ text-decoration:line-through; color:#64748b; font-weight:500; }
    .todo-meta{ display:flex; gap:.5rem; align-items:center; color:#64748b; }
    .emoji-tag{ font-size:20px; width:28px; height:28px; display:grid; place-items:center; border-radius:8px; background:#f1f5f9; }
    .row{ display:flex; gap:.5rem; }
    .spacer{ flex:1; }
    .muted{ color:#64748b; }
    .category-chip{ margin-right:.35rem; margin-bottom:.35rem; }
    .card-section-title{ margin:.25rem 0 .75rem; font-weight:700; color:#334155; }
  `],
  template: `
  <div class="board">

    <!-- SOL: TODOS -->
    <section class="panel">
      <mat-card>
        <mat-card-header class="panel-header">
          <mat-icon color="primary">checklist</mat-icon>
          <h2>ToDo'lar</h2>
          <span class="spacer"></span>

          <!-- Filtre -->
          <mat-form-field appearance="outline" style="width:220px">
            <mat-label>Kategori filtresi</mat-label>
            <mat-select [(ngModel)]="filterCatId">
              <mat-option [value]="null">Hepsi</mat-option>
              <mat-option *ngFor="let c of categories()" [value]="c.id">{{ c.name }}</mat-option>
            </mat-select>
          </mat-form-field>

          <button mat-icon-button [matMenuTriggerFor]="more" matTooltip="Daha fazla">
            <mat-icon>more_vert</mat-icon>
          </button>
          <mat-menu #more="matMenu">
            <button mat-menu-item (click)="clearDone()"><mat-icon>layers_clear</mat-icon><span>Tamamlananları temizle</span></button>
          </mat-menu>
        </mat-card-header>

        <mat-card-content>

          <!-- Hızlı Ekle -->
          <div class="card-section-title">Todo Ekle</div>
          <form [formGroup]="todoForm" (ngSubmit)="addTodo()" class="add-row">
            <mat-form-field appearance="outline" class="spacer">
              <mat-label>Bir şey yaz ve Enter'a bas…</mat-label>
              <input matInput formControlName="title" class="todo-input" required />
            </mat-form-field>

            <mat-form-field appearance="outline" style="width:160px">
              <mat-label>Kategori</mat-label>
              <mat-select formControlName="categoryId">
                <mat-option [value]="null">— Yok —</mat-option>
                <mat-option *ngFor="let c of categories()" [value]="c.id">{{ c.name }}</mat-option>
              </mat-select>
            </mat-form-field>

            <button mat-raised-button color="primary" class="big-btn" type="submit">
              <mat-icon>add_task</mat-icon>&nbsp; Ekle
            </button>
          </form>

          <mat-divider></mat-divider>

          <!-- Liste -->
          <div style="margin-top:1rem">
            <div *ngFor="let t of filteredTodos()" class="todo-item" style="--bar:#6366f1">
              <div class="todo-main">
                <div class="emoji-tag" matTooltip="Todo">📝</div>
                <mat-checkbox [checked]="t.isCompleted" (change)="toggleDone(t)"></mat-checkbox>

                <div class="spacer">
                  <div [class.todo-title]="true" [class.done]="t.isCompleted">{{ t.title }}</div>
                  <div class="todo-meta">
                    <mat-chip *ngIf="t.categoryName" color="primary" selected size="small">
                      <mat-icon>folder</mat-icon>&nbsp;{{ t.categoryName }}
                    </mat-chip>
                    <span class="muted">{{ t.createdAt | date:'short' }}</span>
                  </div>
                </div>

                <button mat-icon-button (click)="removeTodo(t.id)" matTooltip="Sil">
                  <mat-icon>delete</mat-icon>
                </button>
              </div>
            </div>

            <div *ngIf="todos().length === 0" class="muted" style="padding:.5rem 0;">
              Henüz todo yok. Yukarıdan hemen ekleyebilirsin.
            </div>
          </div>
        </mat-card-content>
      </mat-card>
    </section>

    <!-- SAĞ: KATEGORİLER -->
    <section class="panel">
      <mat-card>
        <mat-card-header class="panel-header">
          <mat-icon color="primary">category</mat-icon>
          <h2>Kategoriler</h2>
        </mat-card-header>

        <mat-card-content>
          <form [formGroup]="catForm" (ngSubmit)="onAdd()" style="margin:.25rem 0 1rem;">
            <div class="row">
              <mat-form-field appearance="outline" class="spacer">
                <mat-label>Kategori adı</mat-label>
                <input matInput formControlName="name" required />
              </mat-form-field>
              <button mat-raised-button color="primary" type="submit">
                <mat-icon>add</mat-icon>&nbsp;Ekle
              </button>
            </div>
          </form>

          <mat-divider></mat-divider>

          <div style="margin-top:1rem">
            <mat-chip *ngFor="let c of categories()" class="category-chip" selected>
              <mat-icon>folder</mat-icon>&nbsp; {{ c.name }}
              <button mat-icon-button (click)="onEdit(c)" matTooltip="Düzenle" style="margin-left:.25rem">
                <mat-icon style="font-size:18px">edit</mat-icon>
              </button>
              <button mat-icon-button color="warn" (click)="onDelete(c)" matTooltip="Sil">
                <mat-icon style="font-size:18px">close</mat-icon>
              </button>
            </mat-chip>
          </div>
        </mat-card-content>
      </mat-card>
    </section>

  </div>
  `
})
export class CategoriesComponent implements OnInit {
  private svc = inject(CategoriesService);
  private todosSvc = inject(TodosService);
  private fb = inject(FormBuilder);
  private dialog = inject(MatDialog);
  private snack = inject(MatSnackBar);

  categories = this.svc.categories;
  catForm = this.fb.nonNullable.group({ name: ['', [Validators.required, Validators.maxLength(60)]] });

  todoForm = this.fb.nonNullable.group({
    title: ['', [Validators.required, Validators.maxLength(140)]],
    categoryId: [null as number | null],
  });

  todos = this.todosSvc.todos;
  filterCatId: number | null = null;

  ngOnInit() {
    this.svc.loadAll();
    this.todosSvc.loadAll();
  }

  onAdd() {
    const name = (this.catForm.value.name || '').trim();
    if (!name) return;
    this.svc.add(name);
    this.catForm.reset({ name: '' });
  }

  onEdit(c: CategoryDto) {
    const ref = this.dialog.open(CategoryEditDialogComponent, {
      data: { name: c.name }, width: '360px'
    });
    ref.afterClosed().subscribe(val => {
      const name = typeof val === 'string' ? val.trim() : '';
      if (name && name !== c.name) this.svc.update(c.id, name);
    });
  }

  onDelete(c: CategoryDto) {
    if (!confirm(`Silinsin mi?\n- ${c.name}`)) return;
    this.svc.remove(c.id);
  }

  addTodo() {
    if (this.todoForm.invalid) return;
    const v = this.todoForm.getRawValue();
    const title = v.title.trim();
    if (!title) return;
    this.todosSvc.add({ 
      title, 
      description: '', 
      priority: 1, 
      categoryId: v.categoryId ?? null 
    });
    this.todoForm.reset({ title: '', categoryId: v.categoryId ?? null });
  }

  toggleDone(t: TodoDto) { this.todosSvc.toggle(t.id); }
  removeTodo(id: number) { this.todosSvc.remove(id); }
  clearDone() { this.todosSvc.clearCompleted(); }

  filteredTodos = computed(() => {
    const list = this.todos();
    if (this.filterCatId == null) return list;
    return list.filter(t => t.categoryId === this.filterCatId);
  });
}
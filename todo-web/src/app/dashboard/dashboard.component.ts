import { Component, OnInit, inject, computed, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatChipsModule } from '@angular/material/chips';
import { MatSelectModule } from '@angular/material/select';
import { MatDividerModule } from '@angular/material/divider';
import { MatBadgeModule } from '@angular/material/badge';

import { TodosService, TodoDto, CreateTodoDto } from '../services/todos.service';
import { CategoriesService, CategoryDto } from '../services/categories.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatListModule, MatCheckboxModule,
    MatChipsModule, MatSelectModule, MatDividerModule, MatBadgeModule
  ],
  template: `
  <div class="dashboard">
    <!-- Sol Panel: Kategoriler -->
    <mat-card class="panel categories-panel">
      <mat-card-header>
        <mat-icon>category</mat-icon>
        <mat-card-title>Kategoriler</mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div class="add-form">
          <mat-form-field appearance="outline">
            <mat-label>Yeni kategori</mat-label>
            <input matInput [formControl]="categoryForm.controls.name" />
          </mat-form-field>
          <button mat-raised-button color="primary" (click)="addCategory()">
            <mat-icon>add</mat-icon>
          </button>
        </div>
        
        <mat-list>
          <mat-list-item 
            *ngFor="let cat of categories()" 
            [class.selected]="selectedCategory()?.id === cat.id"
            (click)="selectCategory(cat)">
            <mat-icon matListItemIcon>folder</mat-icon>
            <div matListItemTitle>{{cat.name}}</div>
            <div matListItemLine [matBadge]="getTodoCount(cat.id)" matBadgeColor="accent">
              {{cat.description}}
            </div>
          </mat-list-item>
          <mat-list-item 
            [class.selected]="selectedCategory() === null"
            (click)="selectCategory(null)">
            <mat-icon matListItemIcon>all_inclusive</mat-icon>
            <div matListItemTitle>Tümü</div>
            <div matListItemLine [matBadge]="todos().length" matBadgeColor="primary">
              Tüm todo'lar
            </div>
          </mat-list-item>
        </mat-list>
      </mat-card-content>
    </mat-card>

    <!-- Orta Panel: Todo'lar -->
    <mat-card class="panel todos-panel">
      <mat-card-header>
        <mat-icon>checklist</mat-icon>
        <mat-card-title>Todo'lar</mat-card-title>
        <div class="spacer"></div>
        <mat-chip-set>
          <mat-chip (click)="setSortBy('priority')" [highlighted]="sortBy() === 'priority'">
            Öncelik
          </mat-chip>
          <mat-chip (click)="setSortBy('duedate')" [highlighted]="sortBy() === 'duedate'">
            Tarih
          </mat-chip>
          <mat-chip (click)="toggleShowCompleted()" [highlighted]="!hideCompleted()">
            {{hideCompleted() ? 'Tamamlananları Göster' : 'Tamamlananları Gizle'}}
          </mat-chip>
        </mat-chip-set>
      </mat-card-header>
      <mat-card-content>
        <div class="add-form">
          <mat-form-field appearance="outline" class="flex-1">
            <mat-label>Yeni todo</mat-label>
            <input matInput [formControl]="todoForm.controls.title" />
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>Kategori</mat-label>
            <mat-select [formControl]="todoForm.controls.categoryId">
              <mat-option [value]="null">Kategori seç</mat-option>
              <mat-option *ngFor="let cat of categories()" [value]="cat.id">
                {{cat.name}}
              </mat-option>
            </mat-select>
          </mat-form-field>
          <mat-form-field appearance="outline">
            <mat-label>İlk not (opsiyonel)</mat-label>
            <input matInput [formControl]="todoForm.controls.initialNote" />
          </mat-form-field>
          <button mat-raised-button color="primary" (click)="addTodo()">
            <mat-icon>add_task</mat-icon>
          </button>
        </div>

        <mat-list>
          <mat-list-item 
            *ngFor="let todo of filteredTodos()" 
            [class.selected]="selectedTodo()?.id === todo.id"
            [class.completed]="todo.isCompleted"
            (click)="selectTodo(todo)">
            <mat-checkbox 
              matListItemIcon
              [checked]="todo.isCompleted" 
              (change)="toggleTodo(todo)"
              (click)="$event.stopPropagation()">
            </mat-checkbox>
            <div matListItemTitle [class.line-through]="todo.isCompleted">
              {{todo.title}}
            </div>
            <div matListItemLine>
              <mat-chip *ngIf="todo.categoryName" size="small">{{todo.categoryName}}</mat-chip>
              <span class="priority">Öncelik: {{todo.priority}}</span>
              <span *ngIf="todo.notes?.length" class="notes-count">
                <mat-icon>note</mat-icon> {{todo.notes.length}}
              </span>
            </div>
            <button mat-icon-button (click)="removeTodo(todo.id); $event.stopPropagation()">
              <mat-icon>delete</mat-icon>
            </button>
          </mat-list-item>
        </mat-list>
      </mat-card-content>
    </mat-card>

    <!-- Sağ Panel: Todo Detayları + Notlar -->
    <mat-card class="panel details-panel">
      <mat-card-header>
        <mat-icon>description</mat-icon>
        <mat-card-title>
          {{selectedTodo() ? 'Todo Detayları' : 'Todo Seçin'}}
        </mat-card-title>
      </mat-card-header>
      <mat-card-content>
        <div *ngIf="selectedTodo(); else noSelection">
          <div class="todo-details">
            <h3>{{selectedTodo()?.title}}</h3>
            <p>{{selectedTodo()?.description}}</p>
            <div class="meta">
              <mat-chip>Öncelik: {{selectedTodo()?.priority}}</mat-chip>
              <mat-chip *ngIf="selectedTodo()?.categoryName">
                {{selectedTodo()?.categoryName}}
              </mat-chip>
              <mat-chip [color]="selectedTodo()?.isCompleted ? 'accent' : 'warn'">
                {{selectedTodo()?.isCompleted ? 'Tamamlandı' : 'Bekliyor'}}
              </mat-chip>
            </div>
          </div>

          <mat-divider></mat-divider>

          <div class="notes-section">
            <h4>Notlar</h4>
            <div class="add-note">
              <mat-form-field appearance="outline" class="flex-1">
                <mat-label>Yeni not</mat-label>
                <input matInput [formControl]="noteForm.controls.content" />
              </mat-form-field>
              <button mat-raised-button color="accent" (click)="addNote()">
                <mat-icon>add</mat-icon>
              </button>
            </div>

            <div class="notes-list">
              <div *ngFor="let note of selectedTodo()?.notes" class="note-item">
                <div class="note-content">{{note.content}}</div>
                <div class="note-meta">
                  <small>{{note.createdAt | date:'short'}}</small>
                  <button mat-icon-button (click)="removeNote(note.id)">
                    <mat-icon>delete</mat-icon>
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>

        <ng-template #noSelection>
          <div class="no-selection">
            <mat-icon>touch_app</mat-icon>
            <p>Detayları görmek için bir todo seçin</p>
          </div>
        </ng-template>
      </mat-card-content>
    </mat-card>
  </div>
  `,
  styles: [`
    .dashboard {
      display: grid;
      grid-template-columns: 300px 1fr 400px;
      gap: 16px;
      height: calc(100vh - 120px);
      padding: 16px;
    }
    
    .panel {
      height: 100%;
      overflow: hidden;
    }
    
    .panel mat-card-content {
      height: calc(100% - 80px);
      overflow-y: auto;
    }
    
    .add-form {
      display: flex;
      gap: 8px;
      margin-bottom: 16px;
      align-items: center;
    }
    
    .flex-1 { flex: 1; }
    .spacer { flex: 1; }
    
    .selected {
      background: rgba(63, 81, 181, 0.1) !important;
    }
    
    .completed {
      opacity: 0.6;
    }
    
    .line-through {
      text-decoration: line-through;
    }
    
    .priority {
      margin-left: 8px;
      font-size: 12px;
      color: #666;
    }
    
    .notes-count {
      margin-left: 8px;
      display: flex;
      align-items: center;
      gap: 4px;
      font-size: 12px;
    }
    
    .todo-details .meta {
      display: flex;
      gap: 8px;
      margin-top: 16px;
    }
    
    .notes-section {
      margin-top: 16px;
    }
    
    .add-note {
      display: flex;
      gap: 8px;
      margin: 16px 0;
    }
    
    .note-item {
      background: #f5f5f5;
      padding: 12px;
      border-radius: 8px;
      margin-bottom: 8px;
    }
    
    .note-meta {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-top: 8px;
    }
    
    .no-selection {
      text-align: center;
      padding: 40px;
      color: #666;
    }
    
    .no-selection mat-icon {
      font-size: 48px;
      width: 48px;
      height: 48px;
    }
    
    @media (max-width: 1200px) {
      .dashboard {
        grid-template-columns: 1fr;
        grid-template-rows: auto auto 1fr;
      }
    }
  `]
})
export class DashboardComponent implements OnInit {
  private todosService = inject(TodosService);
  private categoriesService = inject(CategoriesService);
  private fb = inject(FormBuilder);

  todos = this.todosService.todos;
  categories = this.categoriesService.categories;
  selectedTodo = this.todosService.selectedTodo;
  
  selectedCategory = signal<CategoryDto | null>(null);
  sortBy = signal<string>('priority');
  hideCompleted = signal<boolean>(false);

  categoryForm = this.fb.nonNullable.group({
    name: ['', Validators.required]
  });

  todoForm = this.fb.nonNullable.group({
    title: ['', Validators.required],
    categoryId: [null as number | null],
    initialNote: ['']
  });

  noteForm = this.fb.nonNullable.group({
    content: ['', Validators.required]
  });

  filteredTodos = computed(() => {
    let todos = this.todos();
    
    // Kategori filtresi
    const selectedCat = this.selectedCategory();
    if (selectedCat) {
      todos = todos.filter(t => t.categoryId === selectedCat.id);
    }
    
    // Tamamlanan filtresi
    if (this.hideCompleted()) {
      todos = todos.filter(t => !t.isCompleted);
    }
    
    // Sıralama
    const sort = this.sortBy();
    if (sort === 'priority') {
      todos = todos.sort((a, b) => b.priority - a.priority);
    } else if (sort === 'duedate') {
      todos = todos.sort((a, b) => {
        const aDate = a.dueDate ? new Date(a.dueDate).getTime() : Number.MAX_VALUE;
        const bDate = b.dueDate ? new Date(b.dueDate).getTime() : Number.MAX_VALUE;
        return aDate - bDate;
      });
    }
    
    return todos;
  });

  ngOnInit() {
    this.categoriesService.loadAll();
    this.loadTodos();
  }

  loadTodos() {
    const categoryId = this.selectedCategory()?.id;
    const done = this.hideCompleted() ? false : undefined;
    const sort = this.sortBy();
    this.todosService.loadFiltered(categoryId, done, sort, true);
  }

  selectCategory(category: CategoryDto | null) {
    this.selectedCategory.set(category);
    this.loadTodos();
  }

  selectTodo(todo: TodoDto) {
    this.todosService.selectTodo(todo);
  }

  setSortBy(sort: string) {
    this.sortBy.set(sort);
    this.loadTodos();
  }

  toggleShowCompleted() {
    this.hideCompleted.set(!this.hideCompleted());
    this.loadTodos();
  }

  getTodoCount(categoryId: number): number {
    return this.todos().filter(t => t.categoryId === categoryId).length;
  }

  addCategory() {
    const name = this.categoryForm.value.name?.trim();
    if (!name) return;
    
    this.categoriesService.add(name);
    this.categoryForm.reset();
  }

  addTodo() {
    if (this.todoForm.invalid) return;
    
    const values = this.todoForm.getRawValue();
    const dto: CreateTodoDto = {
      title: values.title,
      description: '',
      priority: 2,
      categoryId: values.categoryId,
      initialNote: values.initialNote || undefined
    };
    
    this.todosService.add(dto);
    this.todoForm.reset();
    setTimeout(() => this.loadTodos(), 500);
  }

  toggleTodo(todo: TodoDto) {
    this.todosService.toggle(todo.id);
    setTimeout(() => this.loadTodos(), 500);
  }

  removeTodo(id: number) {
    this.todosService.remove(id);
    if (this.selectedTodo()?.id === id) {
      this.todosService.selectTodo(null);
    }
    setTimeout(() => this.loadTodos(), 500);
  }

  addNote() {
    const selected = this.selectedTodo();
    const content = this.noteForm.value.content?.trim();
    if (!selected || !content) return;
    
    this.todosService.addNote(selected.id, content);
    this.noteForm.reset();
  }

  removeNote(noteId: number) {
    const selected = this.selectedTodo();
    if (!selected) return;
    
    this.todosService.removeNote(selected.id, noteId);
  }
}
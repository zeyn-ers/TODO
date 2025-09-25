import { Component, OnInit, computed, effect, inject, signal } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { CategoriesService, CategoryDto } from '../services/categories.service';
import { TodosService, TodoDto } from '../services/todos.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgIf, NgFor],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  private fb = inject(FormBuilder);
  catSvc = inject(CategoriesService);
  todoSvc = inject(TodosService);

  // services tarafında BehaviorSubject -> asObservable vardı.
  // burada signals ile bridge yapıyoruz (hafif ve basit):
  categories = signal<CategoryDto[]>([]);
  todos = signal<TodoDto[]>([]);

  // UI state
  q = signal('');                      // arama
  selectedCatId = signal<number | 'all'>('all');   // kategori filtresi

  // quick add for category
  catForm = this.fb.group({ name: ['', [Validators.required, Validators.maxLength(60)]] });

  // quick add for todo
  todoForm = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    categoryId: <any>['all'],
    dueDate: [''],
  });

  // filtered todos (search + category)
  filtered = computed(() => {
    const q = this.q().toLowerCase().trim();
    const cid = this.selectedCatId();
    return this.todos().filter(t => {
      const byCat = cid === 'all' ? true : t.categoryId === cid;
      const byQ =
        !q ||
        t.title.toLowerCase().includes(q) ||
        (t.description ?? '').toLowerCase().includes(q);
      return byCat && byQ;
    });
  });

  ngOnInit(): void {
    this.catSvc.loadAll();
    this.todoSvc.loadAll();
    
    // Signal'ları direkt kullan
    effect(() => {
      this.categories.set(this.catSvc.categories());
      this.todos.set(this.todoSvc.todos());
    });

    // varsayılan: todo formu kategori seçimi sidebar seçimi ile eşitlensin
    effect(() => {
      const cid = this.selectedCatId();
      this.todoForm.patchValue({ categoryId: cid === 'all' ? 'all' : cid }, { emitEvent: false });
    });
  }

  // --- Sidebar actions
  addCategory() {
    const name = (this.catForm.value.name || '').trim();
    if (!name) return;
    this.catSvc.add(name);
    this.catForm.reset({ name: '' });
  }
  pickCat(id: number | 'all') { this.selectedCatId.set(id); }

  // --- Todos actions
  addTodo() {
    if (this.todoForm.invalid) return;
    const v = this.todoForm.value;
    this.todoSvc.add({
      title: v.title!,
      description: v.description || '',
      priority: 1,
      categoryId: v.categoryId === 'all' ? null : (v.categoryId as number),
      dueDate: v.dueDate ? new Date(v.dueDate as any).toISOString() : null,
    });
    this.todoForm.reset({ title: '', description: '', categoryId: this.selectedCatId(), dueDate: '' });
  }
  toggleDone(t: TodoDto) { this.todoSvc.toggle(t.id); }
  removeTodo(t: TodoDto) {
    if (confirm(`Silinsin mi?\n- ${t.title}`)) this.todoSvc.remove(t.id);
  }
}

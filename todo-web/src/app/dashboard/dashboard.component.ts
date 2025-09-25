import { Component, OnInit, computed, effect, inject, signal, Injector } from '@angular/core';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { CategoriesService, CategoryDto } from '../services/categories.service';
import { TodosService, TodoDto } from '../services/todos.service';

type CatIconMap = Record<number, string>;

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, NgIf, NgFor],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  private fb = inject(FormBuilder);
  private injector = inject(Injector);

  catSvc = inject(CategoriesService);
  todoSvc = inject(TodosService);

  // Store -> Signals
  categories = signal<CategoryDto[]>([]);
  todos = signal<TodoDto[]>([]);

  // UI state
  q = signal('');
  selectedCatId = signal<number | 'all'>('all');

  // Notion-style page info (localStorage)
  pageTitle = signal<string>(localStorage.getItem('pageTitle') ?? 'başlık');
  pageIcon  = signal<string>(localStorage.getItem('pageIcon')  ?? '🗒️');

  // Kategori emoji haritası (localStorage)
  catIcons = signal<CatIconMap>(JSON.parse(localStorage.getItem('catIcons') ?? '{}'));
  emojiPalette = [
    '📁','🗂️','📝','✅','🔥','⭐','💡','📚','🎯','⚙️',
    '💻','📅','🧠','🛠️','🧪','🎨','🧹','🥇','🪄','📌'
  ];
  emojiPickerFor = signal<number | 'page' | null>(null);

  // Forms
  catForm = this.fb.group({ name: ['', [Validators.required, Validators.maxLength(60)]] });
  todoForm = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: [''],
    categoryId: <any>['all'],
    dueDate: [''],
  });

  // Derived
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

  selectedCategoryName = computed(() => {
    const cid = this.selectedCatId();
    if (cid === 'all') return 'Tümü';
    const c = this.categories().find(cat => cat.id === cid);
    return c?.name ?? 'Seçilmedi';
  });

  // helpers
  iconForCategory(id: number): string {
    return this.catIcons()[id] ?? '📁';
  }
  setCatIcon(id: number, icon: string) {
    const next = { ...this.catIcons(), [id]: icon };
    this.catIcons.set(next);
    localStorage.setItem('catIcons', JSON.stringify(next));
    this.emojiPickerFor.set(null);
  }
  setPageIcon(icon: string) {
    this.pageIcon.set(icon);
    localStorage.setItem('pageIcon', icon);
    this.emojiPickerFor.set(null);
  }
  saveTitle(v: string) {
    this.pageTitle.set(v || 'başlık');
    localStorage.setItem('pageTitle', this.pageTitle());
  }

  catNameById(id?: number | null): string {
    if (id == null) return '—';
    const c = this.categories().find(cat => cat.id === id);
    return c?.name ?? '—';
  }

  private autoSync!: ReturnType<typeof effect>;

  ngOnInit(): void {
    // yükle
    this.catSvc.load().subscribe();
    this.todoSvc.load(true).subscribe();

    this.catSvc.categories$.subscribe(list => this.categories.set(list));
    this.todoSvc.todos$.subscribe(list => this.todos.set(list));

    // sidebar seçimi -> form kategori alanına
    this.autoSync = effect(() => {
      const cid = this.selectedCatId();
      this.todoForm.patchValue(
        { categoryId: cid === 'all' ? 'all' : cid },
        { emitEvent: false }
      );
    }, { injector: this.injector });
  }

  // Sidebar actions
  addCategory() {
    const name = (this.catForm.value.name || '').trim();
    if (!name) return;
    this.catSvc.add(name);
    this.catForm.reset({ name: '' });
  }
  pickCat(id: number | 'all') { this.selectedCatId.set(id); }

  // Todos actions
  addTodo() {
    if (this.todoForm.invalid) return;
    const v = this.todoForm.value;
    this.todoSvc.add({
      title: v.title!,
      description: v.description || '',
      categoryId: v.categoryId === 'all' ? undefined : (v.categoryId as number),
      dueDate: v.dueDate ? new Date(v.dueDate as any).toISOString() : undefined,
    });
    this.todoForm.reset({
      title: '',
      description: '',
      categoryId: this.selectedCatId(),
      dueDate: '',
    });
  }
  toggleDone(t: TodoDto) { this.todoSvc.toggle(t.id); }
  removeTodo(t: TodoDto) { if (confirm(`Silinsin mi?\n- ${t.title}`)) this.todoSvc.remove(t.id); }
}

import { Component, OnInit, inject, ApplicationRef } from '@angular/core';
import { CommonModule, NgFor } from '@angular/common';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { TodosService, TodoDto } from '../services/todos.service';

@Component({
  selector: 'app-todos',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule, NgFor,
    MatCardModule, MatFormFieldModule, MatInputModule,
    MatButtonModule, MatIconModule, MatListModule,
    MatCheckboxModule, MatDatepickerModule, MatNativeDateModule,
    MatSnackBarModule
  ],
  styles: [`
    .row { display:flex; gap:.5rem; align-items:center; }
    .spacer { flex:1; }
  `],
  template: `
  <mat-card>
    <mat-card-title>ToDo'lar</mat-card-title>
    <mat-card-content>

      <form [formGroup]="form" (ngSubmit)="onAdd()" class="row" style="margin:.5rem 0 1rem">
        <mat-form-field appearance="outline" class="spacer">
          <mat-label>Başlık</mat-label>
          <input matInput formControlName="title" placeholder="Yeni yapılacak…" required />
        </mat-form-field>

        <mat-form-field appearance="outline">
          <mat-label>Son tarih</mat-label>
          <input matInput [matDatepicker]="picker" formControlName="dueDate">
          <mat-datepicker-toggle matSuffix [for]="picker" aria-label="Tarih seçici"></mat-datepicker-toggle>
          <mat-datepicker #picker></mat-datepicker>
        </mat-form-field>

        <button mat-raised-button color="primary" type="submit" aria-label="Todo ekle">Ekle</button>
      </form>

      <mat-list>
        <mat-list-item *ngFor="let t of todos(); trackBy: trackId">
          <mat-checkbox [checked]="t.isCompleted" (change)="onToggle(t)" aria-label="Tamamlandı işareti"></mat-checkbox>
          <div matListItemTitle [style.textDecoration]="t.isCompleted ? 'line-through' : 'none'">
            {{ t.title }}
          </div>
          <div matListItemLine *ngIf="t.dueDate as d">Son tarih: {{ d | date:'mediumDate' }}</div>
          <span class="spacer"></span>
          <button mat-icon-button (click)="onQuickEdit(t)" aria-label="Düzenle">
            <mat-icon>edit</mat-icon>
          </button>
          <button mat-icon-button color="warn" (click)="onDelete(t)" aria-label="Sil">
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
  private fb  = inject(FormBuilder);
  private snack = inject(MatSnackBar);
  private appRef = inject(ApplicationRef);

  todos = this.svc.todos;
  form = this.fb.nonNullable.group({ title: [''], dueDate: [null as Date | null] });

  /** listelerde performans için */
  trackId = (_: number, x: { id: number }) => x.id;

  ngOnInit() { this.svc.loadAll(); }

  onAdd() {
    const v = this.form.value;
    const title = (v.title || '').trim();
    if (!title) return;

    const iso = v.dueDate ? new Date(v.dueDate).toISOString() : undefined;
    this.svc.add({ title, description: '', dueDate: iso, priority: 1 });
    this.form.reset();
    this.appRef.tick();
  }

  onToggle(t: TodoDto) {
    this.svc.toggle(t.id);
  }

  onQuickEdit(t: TodoDto) {
    const title = prompt('Yeni başlık:', t.title);
    if (title != null && title.trim() && title !== t.title) {
      this.svc.update(t.id, {
        title: title.trim(),
        description: t.description,
        isCompleted: t.isCompleted,
        dueDate: t.dueDate,
        priority: t.priority,
        categoryId: t.categoryId
      });
    }
  }

  onDelete(t: TodoDto) {
    if (confirm(`Silinsin mi?\n- ${t.title}`)) {
      this.svc.remove(t.id);
      this.snack.open('ToDo silindi', 'Kapat', { duration: 1200, panelClass: ['snack-warn'] });
    }
  }
}

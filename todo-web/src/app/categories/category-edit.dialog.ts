import { Component, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-edit-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  template: `
  <h2 mat-dialog-title>Kategori Düzenle</h2>
  <div mat-dialog-content>
    <form [formGroup]="form">
      <mat-form-field appearance="outline" style="width:100%">
        <mat-label>Ad</mat-label>
        <input matInput formControlName="name" />
      </mat-form-field>
    </form>
  </div>
  <div mat-dialog-actions align="end">
    <button mat-button (click)="close()">İptal</button>
    <button mat-raised-button color="primary" (click)="save()">Kaydet</button>
  </div>
  `
})
export class CategoryEditDialogComponent {
  private fb = inject(FormBuilder);
  private ref = inject(MatDialogRef<CategoryEditDialogComponent>);
  private data = inject<{ name: string }>(MAT_DIALOG_DATA);

  form = this.fb.nonNullable.group({ name: [this.data?.name || ''] });

  save()  { this.ref.close(this.form.value.name?.trim()); }
  close() { this.ref.close(); }
}

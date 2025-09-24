import { Routes } from '@angular/router';
import { CategoriesComponent } from './categories/categories.component';
import { TodosComponent } from './todos/todos.component';

export const routes: Routes = [
  { path: '', redirectTo: 'categories', pathMatch: 'full' },
  { path: 'categories', component: CategoriesComponent },
  { path: 'todos', component: TodosComponent },
  { path: '**', redirectTo: 'categories' }
];

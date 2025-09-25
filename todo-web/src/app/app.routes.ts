import { Routes } from '@angular/router';
import { CategoriesComponent } from './categories/categories.component';
import { TodosComponent } from './todos/todos.component';
import { DashboardComponent } from './dashboard/dashboard.component';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'categories', component: CategoriesComponent },
  { path: 'todos', component: TodosComponent },
  { path: '**', redirectTo: 'dashboard' }
];

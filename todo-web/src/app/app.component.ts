import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatToolbarModule, MatButtonModule, MatIconModule, MatChipsModule],
  template: `
    <mat-toolbar color="primary">
      <mat-icon>task_alt</mat-icon>
      <span style="margin-left:8px; font-weight:600;">TodoApp - N-Layer Architecture</span>
      
      <span style="flex:1"></span>
      
      <nav style="display:flex;gap:8px;">
        <button mat-stroked-button routerLink="/dashboard" routerLinkActive="mat-flat-button">
          <mat-icon>dashboard</mat-icon> Dashboard
        </button>
        <button mat-stroked-button routerLink="/categories" routerLinkActive="mat-flat-button">
          <mat-icon>category</mat-icon> Kategoriler
        </button>
        <button mat-stroked-button routerLink="/todos" routerLinkActive="mat-flat-button">
          <mat-icon>checklist</mat-icon> ToDo'lar
        </button>
      </nav>
      
      <mat-chip style="margin-left:16px; background:rgba(255,255,255,0.2); color:white;">
        <mat-icon>api</mat-icon> V1 & V2 API
      </mat-chip>
    </mat-toolbar>
    
    <main style="min-height:calc(100vh - 64px); background:#fafafa;">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {}
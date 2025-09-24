import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <header style="display:flex;gap:12px;align-items:center;padding:10px;border-bottom:1px solid #eee">
      <strong>Todo Web</strong>
      <nav style="display:flex;gap:10px;margin-left:auto">
        <a routerLink="/categories">Kategoriler</a>
        <a routerLink="/todos">ToDo'lar</a>
      </nav>
    </header>
    <main style="padding:16px">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {}

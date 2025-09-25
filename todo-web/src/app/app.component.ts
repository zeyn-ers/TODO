import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `
    <div class="header">
      <div class="brand">TodoWeb</div>
      <div class="search"><input placeholder="Ara..." /></div>
    </div>

    <!-- sayfalar burada render edilecek -->
    <router-outlet></router-outlet>
  `,
})
export class AppComponent {}

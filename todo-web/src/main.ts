// src/main.ts
import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideZoneChangeDetection } from '@angular/core'; // ✅ zoneless

import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';

bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient(),
    provideAnimations(),
    // ✅ Angular 20: zone.js olmadan çalış (signals ile uyumlu)
    provideZoneChangeDetection({ eventCoalescing: true, runCoalescing: true }),
  ],
}).catch(err => console.error(err));

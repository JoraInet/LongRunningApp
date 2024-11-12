import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { HttpClientModule, provideHttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { MessageService } from 'primeng/api';
import { TextProcessingComponent } from './components/text-processing/text-processing.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NO_ERRORS_SCHEMA } from '@angular/compiler';
import { provideStore } from '@ngrx/store';
import { metaReducers, reducers } from './reducers';
import { provideEffects } from '@ngrx/effects';
import { TextProcessingEffects } from './effects/text-processing.effects';

describe('AppComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        HttpClientModule,
        FormsModule
      ],
      declarations: [
        AppComponent,
        TextProcessingComponent,
      ],
      providers: [
        MessageService,
        provideHttpClient(),
        provideStore(reducers, {metaReducers}),
        provideEffects([TextProcessingEffects])
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  it(`should have as title 'LongRunningApp.WebApp'`, () => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.componentInstance;
    expect(app.title).toEqual('LongRunningApp.WebApp');
  });

  it('should render title', () => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('h1')?.textContent).toContain('Hello, LongRunningApp.WebApp');
  });
});

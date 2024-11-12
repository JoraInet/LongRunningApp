import { NgModule, isDevMode } from '@angular/core';
import { provideHttpClient  } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';
import { TextProcessingComponent } from './components/text-processing/text-processing.component';
import { ProgressBarComponent } from './components-shared/progress-bar/progress-bar.component';
import { provideStore } from '@ngrx/store';
import { reducers, metaReducers } from './reducers';
import { provideEffects } from '@ngrx/effects';
import { TextProcessingEffects } from './effects/text-processing.effects';

@NgModule({
  declarations: [
    AppComponent,
    TextProcessingComponent,
    ProgressBarComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    InputTextModule,
    ButtonModule,
    ToastModule
  ],
  providers: [
    MessageService, 
    provideHttpClient(),
    provideStore(reducers, {metaReducers}),
    provideEffects([TextProcessingEffects])
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

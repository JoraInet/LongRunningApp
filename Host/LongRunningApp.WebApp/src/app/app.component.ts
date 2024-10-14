import { Component } from '@angular/core';
import { TextProcessingComponent } from './components/text-processing/text-processing.component';
import { AppModule } from './app.module';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'LongRunningApp.WebApp';
}

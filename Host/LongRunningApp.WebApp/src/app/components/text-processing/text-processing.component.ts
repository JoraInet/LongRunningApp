import { Component, OnDestroy, HostListener } from '@angular/core';
import { Store } from '@ngrx/store';
import { AppState } from '../../reducers';
import { TextCancel, TextEntered } from '../../actions/text-processing/text-entry.actions';

@Component({
  selector: 'text-processing',
  templateUrl: './text-processing.component.html',
  styleUrl: './text-processing.component.css',
})
export class TextProcessingComponent implements OnDestroy {
  
  processedText: string = '';
  processing: boolean = false;
  responseResult: string = '';
  progress: number = 0;

  @HostListener('window:beforeunload', ['$event'])
  unloadHandler(event: Event) {
    this.cancelProcessingText();
  }

  constructor(
    private store: Store<AppState>
  ) {
    store.select(state => state.textProcessing.isInProgress)
         .subscribe(isInProgress => this.processing = isInProgress);
    store.select(state => state.textResult)
         .subscribe(textResultState => {
          this.responseResult = textResultState.textResult;
          this.progress = textResultState.progress;
         });
  }

  sendProcessingText() {
    this.store.dispatch(TextEntered({ text: this.processedText }));
  }

  cancelProcessingText() {
    this.store.dispatch(TextCancel());
  }

  ngOnDestroy(): void {
    this.cancelProcessingText();
  }

}

import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { ButtonModule } from 'primeng/button';
import { TextProcessorHubConnectionService } from '../../../services/hub-connections/v1/text-processor-hub-connection.service';
import { TextProcessorApiConnectionService } from '../../../services/api-connections/v1/text-processor-api-connection.service';

@Component({
  selector: 'text-processing',
  templateUrl: './text-processing.component.html',
  styleUrl: './text-processing.component.css',
})
export class TextProcessingComponent implements OnInit {
  processedText: string = '';
  responseResult: string[] = [];
  processing: boolean = false;

  constructor(
    private textProcessorHub: TextProcessorHubConnectionService,
    private textProcessorApi: TextProcessorApiConnectionService
  ) { }

  ngOnInit() {
    
    this.textProcessorHub.startConnection();
    this.textProcessorHub.addListener((responseText: string) => {
      this.responseResult.push(responseText);
    });
  }

  get concatenatedData(): string {
    return this.responseResult.join('');
  }

  sendProcessingText() {

    this.responseResult = [];
    this.processing = true;

    const Text = this.processedText;
    const ConnectionId = this.textProcessorHub.connectionId;

    this.textProcessorApi.sendProcessTextRequest({ ConnectionId, Text })
      .subscribe(
        response => {
          //console.log('TextProcessingComponent - Response:', response);
          //console.log(this.responseResult);
        },
        error => console.error('TextProcessingComponent - Error:', error),
        () => { this.processing = false; }
      );
  }

  cancelProcessingText() {
    this.textProcessorApi.cancelRequest();
  }

}

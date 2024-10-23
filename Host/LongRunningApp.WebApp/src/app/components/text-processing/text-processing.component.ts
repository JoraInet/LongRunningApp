import { Component, OnInit, HostListener } from '@angular/core';
import { MessageService } from 'primeng/api';
import { TextProcessorHubConnectionService } from '../../../services/hub-connections/v1/text-processor-hub-connection.service';
import { TextProcessorApiConnectionService } from '../../../services/api-connections/v1/text-processor-api-connection.service';
import { IProcessTextResponse } from "../../../models/v1/response.api.models";
import { IProcessingText } from '../../../models/v1/response.hub.models';

@Component({
  selector: 'text-processing',
  templateUrl: './text-processing.component.html',
  styleUrl: './text-processing.component.css',
})
export class TextProcessingComponent implements OnInit {
  
  processedText: string = '';
  responseResult: string[] = [];
  processing: boolean = false;
  processId: string = '';
  progress: number = 0;

  @HostListener('window:beforeunload', ['$event'])
  unloadHandler(event: Event) {
    this.cancelProcessingText();
  }

  constructor(
    private textProcessorHub: TextProcessorHubConnectionService,
    private textProcessorApi: TextProcessorApiConnectionService,
    private messageService: MessageService
  ) { }

  ngOnInit() {
    
    this.textProcessorHub.startConnection();
    this.textProcessorHub.addListenerProcessTextResponse((response: IProcessingText) => {
      this.responseResult.push(response.text);
      this.progress = Math.min(response.progressPercentage, 100);
      if(this.progress == 100 && this.processing)
      {
        this.processing = false;
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Process has completed.' });
      }
    });

  }

  get concatenatedData(): string {
    return this.responseResult.join('');
  }

  sendProcessingText() {

    this.responseResult = [];
    this.progress = 0;

    const text = this.processedText;
    const connectionId = this.textProcessorHub.connectionId;
    this.processing = true;

    this.textProcessorApi.sendProcessTextRequest({ connectionId, text })
      .subscribe(
        response => {
          this.processId = response.processId;
          this.messageService.add({ severity: 'info', summary: 'Info', detail: 'Process is running.' });
        },
        (error : IProcessTextResponse) => {
          this.processing = false;
          this.messageService.add({ severity: 'error', summary: 'Error', detail: error.errorMessage });
        });
  }

  cancelProcessingText() {
    if(this.processId === '')
    {
      return;
    }
    const processId = this.processId;
    this.textProcessorApi.sendCancelProcessTextRequest({ processId })
    .subscribe(
      response => {
        this.processId = '';
        this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Process has canceled.' });
        
      },
      (error : IProcessTextResponse) => {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: error.errorMessage });
      },
      () => { this.processing = false; }
    );
  }

}

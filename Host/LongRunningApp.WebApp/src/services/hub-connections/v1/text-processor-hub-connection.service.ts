
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { IProcessingText } from '../../../models/v1/response.hub.models';

@Injectable({
  providedIn: 'root'
})

export class TextProcessorHubConnectionService {

  private hubName: string = 'http://localhost:7145/hubs/v1/TextProcessor';
  private hubConnection: signalR.HubConnection | undefined;
  public connectionId: string = '';

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubName)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.hubConnection?.invoke('GetConnectionId').then(id => {
          this.connectionId = id;
        });
      })
      .catch(err => console.log('TextProcessorHub: error while starting connection [' + err + ']'));
  }

  public addListenerProcessTextResponse(callback: (data: IProcessingText) => void) {
    this.hubConnection?.on('ProcessTextResponse', callback);
  }

}


import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})

export class TextProcessorHubConnectionService {

  private hubName: string = 'https://localhost:7146/hubs/v1/TextProcessor';
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

  public addListener(callback: (data: string) => void) {
    this.hubConnection?.on('ProcessTextResponse', callback);
  }
  
}

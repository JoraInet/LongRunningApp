
import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { IProcessingText } from '../../../models/v1/response.hub.models';
import { Store } from '@ngrx/store';
import { AppState } from '../../../app/reducers';
import { ConnectionIdReceived } from '../../../app/actions/text-processing/connection-id.actions';

@Injectable({
  providedIn: 'root'
})

export class TextProcessorHubConnectionService {

  constructor(
    private store : Store<AppState>
  ) {}

  private hubName: string = 'http://localhost:7145/hubs/v1/TextProcessor';
  private hubConnection: signalR.HubConnection | undefined;

  public startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.hubName)
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.hubConnection?.invoke('GetConnectionId').then(id => {
          this.store.dispatch(ConnectionIdReceived({ connectionId: id }));
        });
      })
      .catch(err => console.log('TextProcessorHub: error while starting connection [' + err + ']'));
  }

  public addListenerProcessTextResponse(callback: (data: IProcessingText) => void) {
    this.hubConnection?.on('ProcessTextResponse', callback);
  }

}

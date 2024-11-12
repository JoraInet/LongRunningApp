import { inject, Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { TextProcessorApiConnectionService } from '../../services/api-connections/v1/text-processor-api-connection.service';
import { TextCancel, TextEntered } from '../actions/text-processing/text-entry.actions';
import { MessageService } from 'primeng/api';
import { catchError, map, mergeMap, of } from 'rxjs';
import { Store } from '@ngrx/store';
import { AppState } from '../reducers';
import { ICancelProcessTextRequest, IProcessTextRequest } from '../../models/v1/request.api.models';
import { HttpErrorResponse } from '@angular/common/http';
import { TextProcessingCancel, TextProcessingFinished, TextProcessingLoaded, TextProcessingLoadFailed } from '../actions/text-processing/text-processing.actions';
import { TextProcessorHubConnectionService } from '../../services/hub-connections/v1/text-processor-hub-connection.service';
import { IProcessingText } from '../../models/v1/response.hub.models';
import { TextResultClosed, TextResultReceived } from '../actions/text-processing/text-result.actions';


@Injectable()
export class TextProcessingEffects {

  private actions$: Actions = inject(Actions);
  private connectionId: string = '';
  private processId: string = '';

  constructor(
    private store : Store<AppState>,
    private textProcessorApi: TextProcessorApiConnectionService,
    private textProcessorHub: TextProcessorHubConnectionService,
    private messageService: MessageService) {

      textProcessorHub.startConnection();
      textProcessorHub.addListenerProcessTextResponse((response: IProcessingText) => {
        store.dispatch(TextResultReceived({ result: { text: response.text, progressPercentage: response.progressPercentage } }));
      });      

      store.select(state => state.hubConnectionId)
           .subscribe(connectionIdSatet => this.connectionId = connectionIdSatet.connectionId);

      store.select(state => state.textProcessing.processId)
           .subscribe(processIdState => {
            this.processId = processIdState.processId;
          });

      store.select(state => state.textProcessing.isInProgress)
           .subscribe(isInProgress => {
            if(isInProgress) {
              this.messageService.add({ severity: 'info', summary: 'Info', detail: 'Process is running.' });
            }
           });
      
      store.select(state => state.textProcessing.processError)
           .subscribe(processError => {
            this.messageService.add({ severity: 'error', summary: 'Error', detail: processError });
           })

      store.select(state => state.textResult.progress )
           .subscribe(progress => {
            if(progress === 100 )
            {
              store.dispatch(TextProcessingFinished());
              this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Process has completed.' });
            }
           });
    }

  processingTextRequest$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TextEntered),
      mergeMap(action => {
        this.store.dispatch(TextResultClosed());
        const request : IProcessTextRequest = { connectionId: this.connectionId, text: action.text };
        return this.textProcessorApi.sendProcessTextRequest(request).pipe(
          map(response => TextProcessingLoaded({response: response})),
          catchError((err : HttpErrorResponse) => of(TextProcessingLoadFailed({ response: err.error })))
        )
      })
    )
  );

  processingTextCancel$ = createEffect(() =>
    this.actions$.pipe(
      ofType(TextCancel),
      mergeMap(action => {
        const request : ICancelProcessTextRequest = { processId: this.processId };
        return this.textProcessorApi.sendCancelProcessTextRequest(request).pipe(
          map(response => {
            this.store.dispatch(TextResultClosed());
            this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Process has canceled.' });
            return TextProcessingCancel({response: response});
          }),
          catchError((err: HttpErrorResponse) => of(TextProcessingLoadFailed({ response: err.error })))
        )
      })
    )
  );

}

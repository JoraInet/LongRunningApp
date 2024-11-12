import { TestBed } from '@angular/core/testing';
import { provideMockActions } from '@ngrx/effects/testing';
import { Observable, of } from 'rxjs';

import { TextProcessingEffects } from './text-processing.effects';
import { TextProcessorHubConnectionService } from '../../services/hub-connections/v1/text-processor-hub-connection.service';
import { TextProcessorApiConnectionService } from '../../services/api-connections/v1/text-processor-api-connection.service';
import { MessageService } from 'primeng/api';
import { provideHttpClient } from '@angular/common/http';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { AppState } from '../reducers';

describe('TextProcessingEffects', () => {

   const appStateFake : AppState = {
     hubConnectionId : { connectionId : '1' },
     text : { text: 'test' },
     textProcessing : { isInProgress: false, processError: '', processId: { processId: ''} },
     textResult : { textResult: '', progress: 0 }
   }; 

  let actions$: Observable<any> = of({});
  let effects: TextProcessingEffects;

  let storeSpy : MockStore;
  let hubServiceSpy : jasmine.SpyObj<TextProcessorHubConnectionService>;
  let apiServiceSpy : jasmine.SpyObj<TextProcessorApiConnectionService>;
  let messageServiceSpy : jasmine.SpyObj<MessageService>;

  beforeEach(() => {
    const spyHubService = jasmine.createSpyObj('TextProcessorHubConnectionService', ['startConnection', 'addListenerProcessTextResponse']);
    const spyApiService = jasmine.createSpyObj('TextProcessorApiConnectionService', ['sendProcessTextRequest', 'sendCancelProcessTextRequest']);
    const spyMessageService = jasmine.createSpyObj('MessageService', ['add']);

    TestBed.configureTestingModule({
      providers: [
        TextProcessingEffects,
        provideHttpClient(),
        provideMockStore<AppState>(),
        {provide: TextProcessorHubConnectionService, useValue: spyHubService},
        {provide: TextProcessorApiConnectionService, useValue: spyApiService},
        {provide: MessageService, useValue: spyMessageService}, 
        provideMockActions(() => actions$)
      ]
    });

    storeSpy = TestBed.inject(MockStore);
    spyOn(storeSpy, 'select').and.returnValue(of(appStateFake));

    hubServiceSpy = TestBed.inject(TextProcessorHubConnectionService) as jasmine.SpyObj<TextProcessorHubConnectionService>;
    apiServiceSpy = TestBed.inject(TextProcessorApiConnectionService) as jasmine.SpyObj<TextProcessorApiConnectionService>;
    messageServiceSpy = TestBed.inject(MessageService) as jasmine.SpyObj<MessageService>;

    effects = TestBed.inject(TextProcessingEffects);
  });

  it('should be created', () => {
    expect(effects).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideHttpClient  } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { TextProcessingComponent } from './text-processing.component';
import { TextProcessorHubConnectionService } from '../../../services/hub-connections/v1/text-processor-hub-connection.service';
import { TextProcessorApiConnectionService } from '../../../services/api-connections/v1/text-processor-api-connection.service';
import { MessageService } from 'primeng/api';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NO_ERRORS_SCHEMA } from '@angular/compiler';

describe('TextProcessingComponent', () => {

  let component: TextProcessingComponent;
  let fixture: ComponentFixture<TextProcessingComponent>;

  let hubServiceSpy : jasmine.SpyObj<TextProcessorHubConnectionService>;
  let apiServiceSpy : jasmine.SpyObj<TextProcessorApiConnectionService>;
  let messageServiceSpy : jasmine.SpyObj<MessageService>;

  beforeEach(async () => {

    const spyHubService = jasmine.createSpyObj('TextProcessorHubConnectionService', ['startConnection', 'addListenerProcessTextResponse']);
    const spyApiService = jasmine.createSpyObj('TextProcessorApiConnectionService', ['sendProcessTextRequest', 'sendCancelProcessTextRequest']);
    const spyMessageService = jasmine.createSpyObj('MessageService', ['add']);

    await TestBed.configureTestingModule({
      imports: [
        FormsModule
      ],
      declarations: [
        TextProcessingComponent
      ],
      providers: [provideHttpClient(),
        {provide: TextProcessorHubConnectionService, useValue: spyHubService},
        {provide: TextProcessorApiConnectionService, useValue: spyApiService},
        {provide: MessageService, useValue: spyMessageService}
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();

    hubServiceSpy = TestBed.inject(TextProcessorHubConnectionService) as jasmine.SpyObj<TextProcessorHubConnectionService>;
    apiServiceSpy = TestBed.inject(TextProcessorApiConnectionService) as jasmine.SpyObj<TextProcessorApiConnectionService>;
    messageServiceSpy = TestBed.inject(MessageService) as jasmine.SpyObj<MessageService>;

    fixture = TestBed.createComponent(TextProcessingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { TextProcessingComponent } from './text-processing.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { NO_ERRORS_SCHEMA } from '@angular/compiler';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { AppState } from '../../reducers';

describe('TextProcessingComponent', () => {

  let component: TextProcessingComponent;
  let fixture: ComponentFixture<TextProcessingComponent>;
  let storeSpy : MockStore;

  beforeEach(async () => {

    await TestBed.configureTestingModule({
      imports: [
        FormsModule
      ],
      declarations: [
        TextProcessingComponent
      ],
      providers: [
        provideMockStore<AppState>(),
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA, NO_ERRORS_SCHEMA]
    }).compileComponents();
    
    storeSpy = TestBed.inject(MockStore);

    fixture = TestBed.createComponent(TextProcessingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

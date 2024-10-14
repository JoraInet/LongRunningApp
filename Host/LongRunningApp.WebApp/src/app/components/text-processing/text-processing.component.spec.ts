import { TestBed } from '@angular/core/testing';
import { RouterModule } from '@angular/router';
import { TextProcessingComponent } from './text-processing.component';

describe('TextProcessingComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([])
      ],
      declarations: [
        TextProcessingComponent
      ],
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(TextProcessingComponent);
    const app = fixture.componentInstance;
    expect(app).toBeTruthy();
  });

  //it(`should have as title 'LongRunningApp.WebApp'`, () => {
  //  const fixture = TestBed.createComponent(TextProcessingComponent);
  //  const app = fixture.componentInstance;
  //  expect(app.title).toEqual('LongRunningApp.WebApp');
  //});

  //it('should render title', () => {
  //  const fixture = TestBed.createComponent(TextProcessingComponent);
  //  fixture.detectChanges();
  //  const compiled = fixture.nativeElement as HTMLElement;
  //  expect(compiled.querySelector('h1')?.textContent).toContain('Hello, LongRunningApp.WebApp');
  //});
});

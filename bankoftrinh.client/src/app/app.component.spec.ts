import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting
} from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let httpMock: HttpTestingController;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AppComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should create the app', () => {
    expect(component).toBeTruthy();
  });

  it('should retrieve and render weather forecasts', () => {
    const mockForecasts = [
      {
        date: '2021-10-01',
        temperatureC: 20,
        temperatureF: 68,
        summary: 'Mild'
      },
      {
        date: '2021-10-02',
        temperatureC: 25,
        temperatureF: 77,
        summary: 'Warm'
      }
    ];

    fixture.detectChanges();

    const req = httpMock.expectOne('/weatherforecast');
    expect(req.request.method).toBe('GET');

    req.flush(mockForecasts);

    // Re-render the component after the HTTP response arrives.
    fixture.detectChanges();

    expect(component.forecasts).toEqual(mockForecasts);

    const renderedText = fixture.nativeElement.textContent;
    expect(renderedText).toContain('Mild');
    expect(renderedText).toContain('Warm');
  });
});

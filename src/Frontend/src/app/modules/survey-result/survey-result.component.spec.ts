import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyResultComponent } from './survey-result.component';

describe('SurveyResultComponent', () => {
  let component: SurveyResultComponent;
  let fixture: ComponentFixture<SurveyResultComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [SurveyResultComponent]
    });
    fixture = TestBed.createComponent(SurveyResultComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

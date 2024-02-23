import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { DataService } from 'src/app/core/services/data.service';
import { SurveyQuestion } from 'src/app/modules/survey/survey-question';

@Injectable({
  providedIn: 'root'
})
export class SurveyService extends DataService {
  override path = 'anonymous/survey';

  constructor(injector: Injector) {
    super(injector);
  }
  listQuestions() {
    return this.getList<SurveyQuestion>();
  }
}

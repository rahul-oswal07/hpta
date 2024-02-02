import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, Injector } from '@angular/core';
import { DataService } from 'src/app/core/services/data.service';

@Injectable({
  providedIn: 'root'
})
export class SurveyService extends DataService {
  override path = 'survey';

  constructor(injector: Injector) {
    super(injector);
  }
}

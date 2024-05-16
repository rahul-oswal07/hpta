import { Injectable, Injector } from '@angular/core';
import { DataService } from 'src/app/core/services/data.service';

@Injectable({
  providedIn: 'root'
})
export class AnswerService extends DataService {
  override path = 'answer';

  constructor(injector: Injector) {
    super(injector);
  }
  submitAnswers(answers: any) {
    return this.post(answers);
  }
}

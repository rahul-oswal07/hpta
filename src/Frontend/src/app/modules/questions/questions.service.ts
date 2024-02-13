import { Injectable, Injector } from '@angular/core';
import { Observable } from 'rxjs';
import { AvailabilityCheckResult } from 'src/app/core/models/availability-check-result';
import { DataService } from 'src/app/core/services/data.service';
import { Question } from 'src/app/modules/questions/question';

@Injectable({
  providedIn: 'root'
})
export class QuestionsService extends DataService {
  override path = 'question';

  constructor(injector: Injector) {
    super(injector);
  }
  listAllQuestions() {
    return this.getList<Question>();
  }

  checkQuestionAvailability(text: string, id?: number): Observable<AvailabilityCheckResult> {
    return this.getSingle<AvailabilityCheckResult>('check-question-availability', { id, text })
  }
  addQuestion(question: Question) {
    return this.post(question);
  }
  updateQuestion(question: Question) {
    return this.put(question);
  }
  deleteQuestion(id: number) {
    return this.delete(id.toString());
  }
}

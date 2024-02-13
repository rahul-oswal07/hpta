import { Pipe, PipeTransform } from '@angular/core';
import { answerTypes } from 'src/app/core/models/answer-types';

@Pipe({
  name: 'answerType'
})
export class AnswerTypePipe implements PipeTransform {

  transform(value?: number): string {
    return answerTypes.find(a => a.id === value)?.value ?? '(Unknown)';
  }

}

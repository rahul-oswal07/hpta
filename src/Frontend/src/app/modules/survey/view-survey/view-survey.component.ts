import { Component, OnInit, ViewChild } from '@angular/core';
import {
  FormArray,
  FormControl,
  FormGroup,
  UntypedFormArray,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { DataRetrievalStatus } from 'src/app/core/models/data-retrieval-status';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import { AnswerService } from 'src/app/modules/survey/answer.service';
import {
  SurveyQuestion,
} from 'src/app/modules/survey/survey-question';
import { SurveyService } from 'src/app/modules/survey/survey.service';

@Component({
  selector: 'app-view-survey',
  templateUrl: './view-survey.component.html',
  styleUrls: ['./view-survey.component.scss'],
})
export class ViewSurveyComponent implements OnInit {
  groupedQuestions: SurveyQuestion[][] = [];
  questions: SurveyQuestion[] = [];
  form = new UntypedFormGroup({
    questionGroups: new UntypedFormArray([]),
  });
  submitting = false;
  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;

  public get questionGroups(): FormArray {
    return this.form.get('questionGroups') as FormArray;
  }

  constructor(
    private surveyService: SurveyService,
    private answerService: AnswerService,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.dataStatusIndicator?.setLoading();
    this.surveyService.listQuestions().subscribe({
      next: (r) => {
        this.questions = r;
        this.groupQuestions();
      },
      error: () => {
        this.dataStatusIndicator?.setError(
          'An error occured while retrieving questions.'
        );
      },
      complete: () => {
        if (this.questions?.length > 0) {
          this.dataStatusIndicator?.setDefault();
        } else if (this.dataStatusIndicator?.status !== 'Error') {
          this.dataStatusIndicator?.setNoData();
        }
      },
    });
  }

  groupQuestions(): void {
    const groupSize = 5;
    this.groupedQuestions = this.questions.reduce(
      (acc: SurveyQuestion[][], question, index) => {
        const groupIndex = Math.floor(index / groupSize);

        if (!acc[groupIndex]) {
          acc[groupIndex] = [];
          this.questionGroups.push(new FormGroup({ questions: new FormArray([]) }));
        }

        acc[groupIndex].push(question);

        // Create a FormGroup for the question and add it to the FormArray
        const questionFormGroup = new FormGroup({
          questionNumber: new FormControl(question.questionNumber),
          rating: new FormControl('', Validators.required),
        });
        const questionsArray = this.questionGroups
          .at(groupIndex)
          .get('questions') as FormArray;
        questionsArray.push(questionFormGroup);
        return acc;
      },
      []
    );
  }

  flattenFormValues(
    formValues: any
  ): { questionNumber: number; rating?: number }[] {
    return Object.values(formValues).flatMap((category: any) =>
      Object.values(category).flatMap((subCategory: any) =>
        subCategory.questions.map((question: any) => ({
          questionNumber: question.questionNumber,
          rating: question.rating,
        }))
      )
    );
  }

  onSubmit() {
    const formValues = this.form.value;
    const answers = this.flattenFormValues(formValues);
    this.submitting = true;
    this.answerService.submitAnswers(1, answers).subscribe({
      next: () => {
        this.router.navigate(['/result']);
      },
      error: () => {
        this.submitting = false;
      },
      complete: () => {
        this.submitting = false;
      },
    });
  }
}

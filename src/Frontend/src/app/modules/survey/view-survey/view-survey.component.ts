import { Component, OnInit, ViewChild } from '@angular/core';
import { FormArray, FormControl, FormGroup, UntypedFormArray, UntypedFormGroup, Validators } from '@angular/forms';
import { DataRetrievalStatus } from 'src/app/core/models/data-retrieval-status';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import { SurveyService } from 'src/app/modules/survey/survey.service';

type Question = {
  questionNumber: number;
  question: string;
  subCategory: string;
  category: string;
};

type GroupedQuestions = {
  [category: string]: {
    [subCategory: string]: Question[];
  };
};


@Component({
  selector: 'app-view-survey',
  templateUrl: './view-survey.component.html',
  styleUrls: ['./view-survey.component.css']
})
export class ViewSurveyComponent implements OnInit {
  groupedQuestions: GroupedQuestions = {};
  questions: Question[] = [];
  form = new UntypedFormGroup({});
  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;
  constructor(private surveyService: SurveyService) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    if (this.dataStatusIndicator) {
      this.dataStatusIndicator.status = 'Loading';
    }
    this.surveyService.getList<Question>().subscribe({
      next: r => {
        this.questions = r;
        this.groupQuestions();
      }, error: () => {
        if (this.dataStatusIndicator) {
          this.dataStatusIndicator.errorMessage = 'An error occured while retrieving questions.';
        }
      }, complete: () => {
        if (!this.dataStatusIndicator) {
          return;
        }
        if (this.questions?.length > 0) {
          this.dataStatusIndicator.status = 'Default';
        } else if (this.dataStatusIndicator.status !== 'Error') {
          this.dataStatusIndicator.status = 'NoData';
        }
      }
    });
  }

  groupQuestions(): void {
    this.groupedQuestions = this.questions.reduce((acc, question) => {
      const { category, subCategory, questionNumber } = question;

      // Ensure the category exists in the accumulator
      if (!acc[category]) {
        acc[category] = {};
        this.form.addControl(category, new FormGroup({}));
      }

      // Ensure the subcategory exists in both the accumulator and the form
      if (!acc[category][subCategory]) {
        acc[category][subCategory] = [];
        const catForm = this.form.get(category) as FormGroup;
        catForm.addControl(subCategory, new FormGroup({ questions: new FormArray([]) }));
      }

      // Push the question to the groupedQuestions
      acc[category][subCategory].push(question);

      // Create a FormGroup for the question and add it to the FormArray
      const questionFormGroup = new FormGroup({
        questionNumber: new FormControl(questionNumber),
        rating: new FormControl(null, Validators.required)
      });
      const questionsArray = (this.form.get([category, subCategory, 'questions']) as FormArray);
      questionsArray.push(questionFormGroup);

      return acc;
    }, {} as GroupedQuestions);
  }

  getKeys(obj: any): string[] {
    return Object.keys(obj);
  }
  flattenFormValues(formValues: any): { questionNumber: number; rating: any }[] {
    return Object.values(formValues).flatMap((category: any) =>
      Object.values(category).flatMap((subCategory: any) =>
        subCategory.questions.map((question: any) => ({
          questionNumber: question.questionNumber,
          rating: question.rating
        }))
      )
    );
  }

  onSubmit() {
    const formValues = this.form.value;
    const flattenedValues = this.flattenFormValues(formValues);
    console.log(flattenedValues);
  }
}

import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, UntypedFormArray, UntypedFormGroup, Validators } from '@angular/forms';
import { SurveyService } from 'src/app/modules/survey/survey.service';

type Question = {
  id: number;
  text: string;
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
  private questions: Question[] = [];
  form = new UntypedFormGroup({});
  constructor(private surveyService: SurveyService) { }

  ngOnInit() {
    this.surveyService.getList<Question>().subscribe(r => {
      this.questions = r;
      this.groupQuestions();
    });
  }
  groupQuestions(): void {
    this.groupedQuestions = this.questions.reduce((acc, question) => {
      const { category, subCategory, id } = question;

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
        questionId: new FormControl(id),
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
  flattenFormValues(formValues: any): { questionId: number; rating: any }[] {
    return Object.values(formValues).flatMap((category: any) =>
      Object.values(category).flatMap((subCategory: any) =>
        subCategory.questions.map((question: any) => ({
          questionId: question.questionId,
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

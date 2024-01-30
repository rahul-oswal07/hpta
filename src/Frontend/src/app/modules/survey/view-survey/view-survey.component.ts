import { Component, OnInit } from '@angular/core';
import { FormArray, FormControl, FormGroup, UntypedFormArray, UntypedFormGroup, Validators } from '@angular/forms';

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
  private questions: Question[] = [
    {
      "id": 1,
      "text": "Is the mission (vision) for the organization clearly articulated & communicated by the leadership?",
      "subCategory": "Communication",
      "category": "Strategic Alignment"
    },
    {
      "id": 2,
      "text": "Are you able to communicate clearly with the team members?",
      "subCategory": "Communication",
      "category": "Strategic Alignment"
    },
    {
      "id": 3,
      "text": "Are the goals & objectives for the team clear?",
      "subCategory": "Goal Alignment",
      "category": "Strategic Alignment"
    },
    {
      "id": 4,
      "text": "Are you clear how your work helps achieve team’s goals?",
      "subCategory": "Goal Alignment",
      "category": "Strategic Alignment"
    },
    {
      "id": 5,
      "text": "Do you feel safe expressing your opinions, ideas during team meetings, discussions?",
      "subCategory": "Trust",
      "category": "Safe Environment"
    },
    {
      "id": 6,
      "text": "Do team members display high levels of cooperation and mutual support?",
      "subCategory": "Collaboration",
      "category": "Safe Environment"
    },
    {
      "id": 7,
      "text": "Are the ideas considered on their merit rather than from whom they are coming?",
      "subCategory": "Inclusivity",
      "category": "Safe Environment"
    },
    {
      "id": 8,
      "text": "Is everyone valued in the team irrespective of their background, gender, cultural differences?",
      "subCategory": "Diversity",
      "category": "Safe Environment"
    },
    {
      "id": 9,
      "text": "Do you feel secure that you will not be punished for your mistakes, when trying your best to achieve the goals?",
      "subCategory": "Fear of Failure",
      "category": "Safe Environment"
    },
    {
      "id": 10,
      "text": "Do you get constructive & actionable feedbacks?",
      "subCategory": "Adaptability",
      "category": "Ways of Working"
    },
    {
      "id": 11,
      "text": "Does the team respond well to changes?",
      "subCategory": "Adaptability",
      "category": "Ways of Working"
    },
    {
      "id": 12,
      "text": "Is the team able to take decisions in time bound manner?",
      "subCategory": "Decision Making",
      "category": "Ways of Working"
    },
    {
      "id": 13,
      "text": "Do team members act according to the decisions taken?",
      "subCategory": "Decision Making",
      "category": "Ways of Working"
    },
    {
      "id": 14,
      "text": "Does the team try to consciously improve regularly & frequently?",
      "subCategory": "Continuous Improvement",
      "category": "Growth Mindset"
    },
    {
      "id": 15,
      "text": "Are learning & development opportunities available to you?",
      "subCategory": "Professional Development",
      "category": "Growth Mindset"
    },
    {
      "id": 16,
      "text": "Are the learnings from even failed experiments valued?",
      "subCategory": "Rapid Experimentation & Risk taking",
      "category": "Growth Mindset"
    },
    {
      "id": 17,
      "text": "Are the boundaries within which to experiment clear?",
      "subCategory": "Rapid Experimentation & Risk taking",
      "category": "Growth Mindset"
    },
    {
      "id": 18,
      "text": "Does the team inspire you to try out new approaches, tools, novel solutions for the work?",
      "subCategory": "Innovation",
      "category": "Growth Mindset"
    },
    {
      "id": 19,
      "text": "Do team members get things done when they say they will?",
      "subCategory": "Accountability",
      "category": "Ownership"
    },
    {
      "id": 20,
      "text": "Do team members proactively communicate regarding any delays?",
      "subCategory": "Accountability",
      "category": "Ownership"
    },
    {
      "id": 21,
      "text": "Does the team take pride in the work they do?",
      "subCategory": "Quality",
      "category": "Ownership"
    },
    {
      "id": 22,
      "text": "Does the team interact with the stakeholders frequently?",
      "subCategory": "Stakeholder Management",
      "category": "Ownership"
    },
    {
      "id": 23,
      "text": "Is the team's work positively impacting the stakeholders?",
      "subCategory": "Stakeholder Management",
      "category": "Ownership"
    },
    {
      "id": 24,
      "text": "Does the leadership provide support when the team asks for help?",
      "subCategory": "Leadership",
      "category": "Ownership"
    },
    {
      "id": 25,
      "text": "Is the team able to get help / support from other teams in the organization to get the job done?",
      "subCategory": "Leadership",
      "category": "Ownership"
    },
    {
      "id": 26,
      "text": "Is the team enabled to take decisions as per the situation on its own?",
      "subCategory": "Empowerment",
      "category": "Ownership"
    },
    {
      "id": 27,
      "text": "Does the team have culture of celebration & recognition where successes are celebrated & recognized in meanigful ways",
      "subCategory": "Celebration & Recognition",
      "category": "Culture"
    },
    {
      "id": 28,
      "text": "Does the team ensure that every member adheres to the core values?",
      "subCategory": "Core Values",
      "category": "Culture"
    }
  ];
  form = new UntypedFormGroup({});
  constructor() { }

  ngOnInit() {
    this.groupQuestions();
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

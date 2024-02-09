export type SurveyQuestion = {
  questionNumber: number;
  question: string;
  subCategory: string;
  category: string;
};

export type GroupedQuestions = {
  [category: string]: {
    [subCategory: string]: SurveyQuestion[];
  };
};

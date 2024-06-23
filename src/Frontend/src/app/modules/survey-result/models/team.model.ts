import {
  ApexAnnotations,
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexFill,
  ApexGrid,
  ApexLegend,
  ApexPlotOptions,
  ApexStroke,
  ApexTheme,
  ApexTitleSubtitle,
  ApexTooltip,
  ApexXAxis,
  ApexYAxis
} from 'ng-apexcharts';

export interface TeamsModel {
  id: number;
  name: string;
}

export interface TeamDataModel {
    teamId: number;
    teamName: string;
    totalUsers: number;
    surveyResults: SurveyResultDataModel[];
}

export interface SurveyResultDataModel {
    surveyId: number;
    scores: ScoreModel[];
    teamPerformance: TeamPerformanceModel;
    respondedUsers: number;
}

export interface ScoreModel {
    categoryId: number;
    categoryName: string;
    average: number;
    description: string;
}

export interface TeamPerformanceModel {
  description: string;
  categories: CategoryPerformanceModel[];
}

export interface CategoryPerformanceModel {
  category: string;
  score: number;
  description: string;
}

export interface ChartOptions {
  series: ApexAxisChartSeries;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
  fill: ApexFill;
  yaxis: ApexYAxis;
  plotOptions: ApexPlotOptions;
  tooltip: ApexTooltip;
  legend: ApexLegend;
  colors: string[]
};

export interface KeyValue {
  key: string;
  value: string;
}

export interface TeamMemberModel {
  email: string;
  name: string;
}

export interface ChartDataRequestModel {
  surveyId: number[];
  email: string;
}

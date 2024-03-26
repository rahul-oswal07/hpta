import {
  ChartComponent,
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexDataLabels,
  ApexTitleSubtitle,
  ApexStroke,
  ApexGrid,
  ApexAnnotations,
  ApexFill,
  ApexYAxis,
} from 'ng-apexcharts';

export interface TeamsModel {
  id: number;
  name: string;
}

export interface ScoreModel {
  categoryId: number;
  categoryName: string;
  average: number;
}

export interface TeamDataModel {
  teamId: number;
  teamName: string;
  scores: ScoreModel[];
  teamPerformance: TeamPerformanceModel;
  totalUsers: number;
  respondedUsers: number;
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
  annotations: ApexAnnotations;
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  grid: ApexGrid;
  labels: string[];
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
  fill: ApexFill;
  yaxis: ApexYAxis;
}

export interface KeyValue {
  key: string;
  value: string;
}

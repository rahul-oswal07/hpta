import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexFill,
  ApexLegend,
  ApexPlotOptions,
  ApexStroke,
  ApexTitleSubtitle,
  ApexXAxis,
  ApexYAxis
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
  chart: ApexChart;
  xaxis: ApexXAxis;
  dataLabels: ApexDataLabels;
  stroke: ApexStroke;
  title: ApexTitleSubtitle;
  fill: ApexFill;
  yaxis: ApexYAxis;
  plotOptions: ApexPlotOptions;
  legend: ApexLegend;
}

export interface KeyValue {
  key: string;
  value: string;
}

export interface TeamMemberModel {
  email: string;
  name: string;
}

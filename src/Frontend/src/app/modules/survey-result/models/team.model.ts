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
} from 'ng-apexcharts';

export interface TeamsModel {
    id: number;
    name: string;
}

export interface ChartDataModel {
    teamId: number;
    teamName: string;
    categoryId: number;
    categoryName: string;
    average: number;
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
    fill : ApexFill
};

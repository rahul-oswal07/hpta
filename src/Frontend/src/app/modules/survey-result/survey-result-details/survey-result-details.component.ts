import { animate, state, style, transition, trigger } from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import { ChartOptions, TeamDataModel, TeamsModel } from 'src/app/modules/survey-result/models/team.model';
import { SurveyResultService } from 'src/app/modules/survey-result/services/survey-result.service';


const CATEGORY_COLORS: any = {
  'Culture': '#85b5c7',
  'Growth Mindset': '#b3ccff',
  'Ownership': '#ff9f80',
  'Safe Environment': '#80aaff',
  'Strategic Alignment': '#ffb366',
  'Ways of Working': '#ff8080',
};

@Component({
  selector: 'app-survey-result-details',
  templateUrl: './survey-result-details.component.html',
  styleUrls: ['./survey-result-details.component.css'],
  animations: [
    trigger('expandShrink', [
      state('expand', style({ width: 'calc(50% - 10px)', opacity: 1 })),
      state('shrink', style({ width: '0', opacity: 0 })),
      transition('shrink => expand', animate('300ms ease-out')),
      transition('expand => shrink', animate('300ms ease-in'))
    ])
  ]
})
export class SurveyResultDetailsComponent implements OnInit {

  teams: TeamsModel[];
  filteredOptions: TeamsModel[];
  chartData = {} as TeamDataModel;
  overallHPTAScore: number;
  teamId: number;
  chartOptions: ChartOptions;
  categoryChartOptions: ChartOptions;

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;

  constructor(private surveyResultService: SurveyResultService, private activatedRoute: ActivatedRoute, private cd: ChangeDetectorRef) { }

  ngOnInit() {
    this.activatedRoute.params.pipe(
      map(params => params['id'])
    ).subscribe({
      next: value => {
        this.categoryChartOptions = this.chartOptions = {} as ChartOptions;
        this.teamId = value;
        this._loadChartData(value);
      },
      error: (e: Error) => {
      }
    });

  }

  buildChartData(chartData: TeamDataModel, title: string) {
    const thisRef = this;
    const average = this.chartData.scores.map((data) => data.average)
    const categories = this.chartData.scores.map((data) => data.categoryName)
    const result = (average.reduce((sum, current) => sum + current, 0) / categories.length)
    this.overallHPTAScore = result;

    const chartOptions: ChartOptions = {
      series: [
        {
          name: 'Average',
          data: chartData.scores.map((data) => data.average),
        },
      ],
      fill: {
        colors: [function (data: any) {
          const category = data.w.globals.labels[data.dataPointIndex];
          return thisRef.getColorCode(category);
        }]
      },
      chart: {
        height: 350,
        type: 'bar',
        toolbar: {
          show: false
        },
        events: {
          dataPointSelection: (event, chartContext, config) => {
            const category = config.w.globals.labels[config.dataPointIndex] as string;
            console.log(this.teamId);

            this._loadCategoryChartData(this.teamId, category);;
            return;
          }
        }
      },
      title: {
        text: title,
      },
      dataLabels: {
        enabled: true,
        style: {
          fontSize: '14px',
          // fontFamily: 'Times New Roman',
          fontWeight: 'bold',
          colors: ['#333']
        },
      },
      xaxis: {
        categories: chartData.scores.map((data) => data.categoryName),
        axisTicks: {
          show: true
        },
        title: {
          text: undefined,
        },
        labels: {
          show: true
        },
      },
      yaxis: {
        show: true,
        min: 0,
        max: 5
      },
      annotations: {},
      grid: {},
      labels: [],
      stroke: {},
    };

    return chartOptions;
  }

  getColorCode(category: string): string {
    return CATEGORY_COLORS[category] || "hsl(" + Math.random() * 360 + ", 100%, 75%)";
  }

  private _loadChartData(teamId: number) {
    this.dataStatusIndicator?.setLoading();

    this.surveyResultService.getChartData(teamId).subscribe(data => {
      this.populateChart(data);
    }, err => {
      this.dataStatusIndicator?.setError("Error while loading the data!");
      this.overallHPTAScore = 0;
      this.chartOptions = {} as ChartOptions;
      this.chartData = {} as TeamDataModel;
    })
  }

  private _loadCategoryChartData(teamId: number, categoryName: string) {
    const categoryId = Number(this.chartData.scores.find((data) => data.categoryName == categoryName)?.categoryId)

    if (categoryId) {
      this.categoryChartOptions = {} as ChartOptions;
      this.surveyResultService.getCategoryChartData(categoryId, teamId).subscribe(data => {
        this.categoryChartOptions = this.buildChartData(data, `Report for Category : ${categoryName}`);
        this.cd.detectChanges();
      });
    }
  }

  private populateChart(data: TeamDataModel) {
    if (data.scores) {
      this.dataStatusIndicator?.setDefault();
      this.chartData = data;
      this.chartOptions = this.buildChartData(this.chartData, 'High Performing Team Report');

      const average = this.chartData.scores.map((data) => data.average);
      const categories = this.chartData.scores.map((data) => data.categoryName);
      const result = (average.reduce((sum, current) => sum + current, 0) / categories.length);
      this.overallHPTAScore = result;
    }
    else {
      this.overallHPTAScore = 0;
      this.chartOptions = {} as ChartOptions;
      this.chartData = {} as TeamDataModel;
      this.dataStatusIndicator?.setNoData();
    }
  }
}

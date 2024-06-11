import {
  animate,
  state,
  style,
  transition,
  trigger,
} from '@angular/animations';
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApexAxisChartSeries } from 'ng-apexcharts';
import { map } from 'rxjs';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import {
  ChartOptions,
  ScoreModel,
  TeamDataModel,
  TeamsModel,
} from 'src/app/modules/survey-result/models/team.model';
import { SurveyResultService } from 'src/app/modules/survey-result/services/survey-result.service';

const CATEGORY_COLORS: any = {
  Culture: '#85b5c7',
  'Growth Mindset': '#b3ccff',
  Ownership: '#ff9f80',
  'Safe Environment': '#80aaff',
  'Strategic Alignment': '#ffb366',
  'Ways of Working': '#ff8080',
};

@Component({
  selector: 'app-survey-result-details',
  templateUrl: './survey-result-details.component.html',
  styleUrls: ['./survey-result-details.component.scss'],
  animations: [
    trigger('expandShrink', [
      state('expand', style({ width: 'calc(50% - 10px)', opacity: 1 })),
      state('shrink', style({ width: '0', opacity: 0 })),
      transition('shrink => expand', animate('300ms ease-out')),
      transition('expand => shrink', animate('300ms ease-in')),
    ]),
  ],
})
export class SurveyResultDetailsComponent implements OnInit {
  teams: TeamsModel[];
  filteredOptions: TeamsModel[];
  chartData = {} as TeamDataModel;
  overallHPTAScore: number;
  teamId: number;
  surveyId: number;
  chartOptions: ChartOptions;
  categoryChartOptions: ChartOptions;

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;
  overviewGraphRadius = 25;
  overviewGraphData: { da: string, color: string, offset: number }[]

  email: string;

  constructor(
    private surveyResultService: SurveyResultService,
    private activatedRoute: ActivatedRoute,
    private cd: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.activatedRoute.params.pipe(map((params) => params['id'])).subscribe({
      next: (value) => {
        this.categoryChartOptions = this.chartOptions = {} as ChartOptions;
        this.teamId = value;
        this.email = "";
        this._loadChartData();
      },
      error: (e: Error) => { },
    });
    this.activatedRoute.queryParams.pipe(map((qp) => qp['survey'])).subscribe({
      next: (value) => {
        this.categoryChartOptions = this.chartOptions = {} as ChartOptions;
        this.surveyId = value;
        this._loadChartData();
      }
    });

    this.surveyResultService.getTeamMemberId().subscribe(memberId => {
      this.categoryChartOptions = this.chartOptions = {} as ChartOptions;
      this.email = memberId;
      this._loadChartData();
    });
  }
  buildChartData(chartData: TeamDataModel, title: string, xAxisTitle: string) {
    const thisRef = this;
    const average = this.chartData.scores.map((data) => data.average)
    const categories = this.chartData.scores.map((data) => data.categoryName)
    const result = (average.reduce((sum, current) => sum + current, 0) / categories.length)
    this.overallHPTAScore = result;

    const series: ApexAxisChartSeries = [{
      name: "Average",
      data: this.calculateAverage(chartData.scores)
    }];

    const chartOptions: ChartOptions = {
      series: series,
      chart: {
        type: 'bar',
        height: 350,
        stacked: true,
        toolbar: {
          show: false
        },
        events: {
          dataPointSelection: (event, chartContext, config) => {
            const category = config.w.globals.labels[config.dataPointIndex] as string;

            this._loadCategoryChartData(this.teamId, category);;
            return;
          }
        }
      },
      plotOptions: {
        bar: {
          horizontal: true,
          dataLabels: {
            total: {
              enabled: true,
              offsetX: 0,
              style: {
                fontSize: '13px',
                fontWeight: 700
              },
              formatter: function (value) {
                return value!;
              }
            }
          }
        },
      },
      stroke: {
        width: 1,
        colors: ['#fff']
      },
      title: {
        text: title
      },
      xaxis: {
        categories: chartData.scores.map((data) => data.categoryName),
        title: {
          text: xAxisTitle
        },
      },
      yaxis: {
        axisTicks: {
          show: false
        },
        labels: {
          show: true
        }
      },
      fill: {
        colors: [function (data: any) {
          const category = data.w.globals.labels[data.dataPointIndex];
          return thisRef.getColorCode(category);
        }]
      },
      legend: {
        position: 'top',
        horizontalAlign: 'left',
        offsetX: 40
      },
      dataLabels: {
        enabled: false,
        style: {
          fontSize: '14px',
          fontWeight: 'bold',
          colors: ['#333'],
        },
        formatter: function (value: number, { seriesIndex, dataPointIndex, w }) {
          let name = w.globals.labels[dataPointIndex];
          if (name.length > 15) {
            name = name.substring(0, 5) + '..';
          }
          return name;
        }
      }
    };

    return chartOptions;
  }

  getColorCode(category: string): string {
    return (
      CATEGORY_COLORS[category] || 'hsl(' + Math.random() * 360 + ', 100%, 75%)'
    );
  }

  private _loadChartData() {
    this.dataStatusIndicator?.setLoading();

    this.surveyResultService.getChartData(this.email, this.teamId, this.surveyId).subscribe(
      (data) => {
        this.populateChart(data);
        this.updateOverviewGraph();
      },
      (err) => {
        this.dataStatusIndicator?.setError('Error while loading the data!');
        this.overallHPTAScore = 0;
        this.chartOptions = {} as ChartOptions;
        this.chartData = {} as TeamDataModel;
      }
    );
  }

  private updateOverviewGraph() {
    const circumference = Math.floor(2 * Math.PI * (this.overviewGraphRadius + 10));
    const overviewGraphData = [];
    let _offset = 150;
    for (const s of (this.chartData?.scores ?? [])) {
      const val = Math.floor((s.average / ((this.chartData.scores.length + 1) * 5)) * circumference);
      const itm = {
        da: `${val} ${circumference}`,
        color: CATEGORY_COLORS[s.categoryName],
        offset: _offset
      }
      _offset += val;
      overviewGraphData.push(itm);
    }
    this.overviewGraphData = overviewGraphData;
  }
  private _loadCategoryChartData(teamId: number, categoryName: string) {
    const categoryId = Number(
      this.chartData.scores.find((data) => data.categoryName == categoryName)
        ?.categoryId
    );

    if (categoryId) {
      this.categoryChartOptions = {} as ChartOptions;
      this.surveyResultService
        .getCategoryChartData(this.email, categoryId, teamId, this.surveyId)
        .subscribe((data) => {
          this.categoryChartOptions = this.buildChartData(
            data,
            `Category : ${categoryName}`,
            'SCORE'
          );
          this.cd.detectChanges();
        });
    }
  }

  private populateChart(data: TeamDataModel) {
    if (data.scores) {
      this.dataStatusIndicator?.setDefault();
      this.chartData = data;
      this.chartOptions = this.buildChartData(
        this.chartData,
        'High Performing Team Assessment',
        "SCORE PER CATEGORY"
      );

      const average = this.chartData.scores.map((data) => data.average);
      const categories = this.chartData.scores.map((data) => data.categoryName);
      const result =
        average.reduce((sum, current) => sum + current, 0) / categories.length;
      this.overallHPTAScore = result;
    } else {
      this.overallHPTAScore = 0;
      this.chartOptions = {} as ChartOptions;
      this.chartData = {} as TeamDataModel;
      this.dataStatusIndicator?.setNoData();
    }
  }

  private calculateAverage(scores: ScoreModel[]): number[] {
    return scores.map((data) => data.average);
  }
}

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
  ChartDataRequestModel,
  SurveyResultDataModel
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
  surveyId: number[];
  chartOptions: ChartOptions;
  multiselectAverageChartOptions: ChartOptions;
  categoryChartOptions: ChartOptions;

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;
  overviewGraphRadius = 25;
  overviewGraphData: { da: string, color: string, offset: number }[]

  email: string;
  isMultiSelection: boolean;

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
        this.surveyId = Array.isArray(value) ? value : value.split(',');
        this.isMultiSelection = this.surveyId.length > 1;
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
    let chartOptions = {} as ChartOptions;

    if (chartData.surveyResults.length) {
      const scores = chartData.surveyResults[0].scores;

      const categories = scores.map((data) => data.categoryName);

      if (!this.isMultiSelection) {
        const average = scores.map((data) => data.average)
        const result = (average.reduce((sum, current) => sum + current, 0) / categories.length)
        this.overallHPTAScore = result;
      }

      let series: ApexAxisChartSeries = [];
      chartData.surveyResults.forEach(surveyResult => {
        series.push({
          name: surveyResult.surveyName,
          data: this.calculateAverage(surveyResult.scores)
        });
      })

      chartOptions = {
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
                enabled: this.isMultiSelection ? false : true,
                offsetX: 0,
                style: {
                  fontSize: '13px',
                  fontWeight: 900
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
          text: "Average"
        },
        xaxis: {
          categories: categories,
          title: {
            text: xAxisTitle
          }
        },
        yaxis: {
          axisTicks: {
            show: false
          },
          labels: {
            show: true
          }
        },
        fill: this.isMultiSelection ? {} : {
          colors: [function (data: any) {
            const category = data.w.globals.labels[data.dataPointIndex];
            return thisRef.getColorCode(category);
          }]
        },
        colors: this.isMultiSelection ? ['#85b5c7', '#80aaff', '#F9C80E', '#449DD1'] : [],
        legend: {
          position: 'top',
          horizontalAlign: 'left',
          offsetX: 40
        },
        dataLabels: {
          enabled: this.isMultiSelection ? true : false,
          style: {
            fontSize: '14px',
            fontWeight: 'bold',
            colors: ['#333']
          }
        },
        tooltip: {
          enabled: false
        },
        markers: {}
      };
    }

    return chartOptions;
  }

  buildMultiselectAverageChart() {
    if (this.chartData?.surveyResults) {
      this.multiselectAverageChartOptions = {
        series: [
          {
            name: "Average",
            data: this.calculateTotalAverage()
          }
        ],
        chart: {
          height: 350,
          width: 700,
          type: "line",
          toolbar: {
            show: false
          },
          zoom: {
            enabled: false
          }
        },
        stroke: {
          width: 7,
          curve: "smooth"
        },
        xaxis: {
          categories: this.chartData.surveyResults.map(x => x.surveyName)
        },
        title: {
          text: "Average of Selected Surveys",
        },
        fill: {
          type: "gradient",
          gradient: {
            shade: "dark",
            gradientToColors: ["#FDD835"],
            shadeIntensity: 1,
            type: "horizontal",
            opacityFrom: 1,
            opacityTo: 1,
            stops: [0, 100, 100, 100]
          }
        },
        markers: {
          size: 4,
          colors: ["#FFA41B"],
          strokeColors: "#fff",
          strokeWidth: 2,
          hover: {
            size: 7,
          }
        },
        yaxis: {
          min: 0,
          max: 5
        },
        dataLabels: {
          enabled: true,
        },
        plotOptions: {},
        colors: [],
        legend: {},
        tooltip: {
          enabled: false
        }
      };
    }
  }

  getColorCode(category: string): string {
    return (
      CATEGORY_COLORS[category] || 'hsl(' + Math.random() * 360 + ', 100%, 75%)'
    );
  }

  private _loadChartData() {
    this.multiselectAverageChartOptions = {} as ChartOptions;
    this.dataStatusIndicator?.setLoading();

    const chartRequest: ChartDataRequestModel = {
      surveyId: this.surveyId,
      email: this.email
    }

    this.surveyResultService.getChartData(chartRequest, this.teamId).subscribe(
      (data) => {
        this.populateChart(data);
        this.updateOverviewGraph();
        if (data.surveyResults?.length > 1) {
          this.buildMultiselectAverageChart();
        }
      },
      (err) => {
        this.dataStatusIndicator?.setError('Error while loading the data!');
        this.overallHPTAScore = 0;
        this.chartOptions = this.multiselectAverageChartOptions = {} as ChartOptions;
        this.chartData = {} as TeamDataModel;
      }
    );
  }

  private updateOverviewGraph() {
    if (!this.isMultiSelection && this.chartData.surveyResults?.length) {
      const circumference = Math.floor(2 * Math.PI * (this.overviewGraphRadius + 10));
      const overviewGraphData = [];
      let _offset = 150;
      const scores = this.chartData.surveyResults[0].scores;
      for (const s of (scores ?? [])) {
        const val = Math.floor((s.average / ((scores.length + 1) * 5)) * circumference);
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
  }

  private _loadCategoryChartData(teamId: number, categoryName: string) {
    const categoryId = Number(
      this.chartData.surveyResults[0].scores.find((data) => data.categoryName == categoryName)
        ?.categoryId
    );

    const chartRequest: ChartDataRequestModel = {
      surveyId: this.surveyId,
      email: this.email
    }

    if (categoryId) {
      this.categoryChartOptions = {} as ChartOptions;
      this.surveyResultService
        .getCategoryChartData(chartRequest, categoryId, teamId)
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
    if (data.surveyResults?.length) {
      this.dataStatusIndicator?.setDefault();
      this.chartData = data;
      this.chartOptions = this.buildChartData(
        this.chartData,
        'High Performing Team Assessment',
        "SCORE PER CATEGORY"
      );

      const scores = this.chartData.surveyResults[0].scores;

      const average = scores.map((data) => data.average);
      const categories = scores.map((data) => data.categoryName);
      const result =
        average.reduce((sum, current) => sum + current, 0) / categories.length;
      this.overallHPTAScore = result;
    } else {
      this.overallHPTAScore = 0;
      this.chartOptions = this.multiselectAverageChartOptions = {} as ChartOptions;
      this.chartData = {} as TeamDataModel;
      this.dataStatusIndicator?.setNoData();
    }
  }

  private calculateAverage(scores: ScoreModel[]): number[] {
    return scores.map((data) => data.average);
  }

  private calculateTotalAverage(): number[] {
    const averagePerSurvey: number[] = [];

    this.chartData.surveyResults.forEach(surveyResult => {
      const sumOfAverages = surveyResult.scores.map(x => x.average).reduce((a, b) => a + b, 0);
      const average = Number((sumOfAverages / surveyResult.scores.length).toFixed(2));
      averagePerSurvey.push(average);
    });

    return averagePerSurvey;
  }
}

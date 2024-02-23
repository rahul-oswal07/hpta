import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map } from 'rxjs';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import { ChartOptions, TeamDataModel } from 'src/app/modules/survey-result/models/team.model';
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
  styleUrls: ['./survey-result-details.component.css']
})
export class SurveyResultDetailsComponent implements OnInit {

  chartData = {} as TeamDataModel;
  overallHPTAScore: string | number;
  public chartOptions: ChartOptions;

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;
  constructor(private surveyResultService: SurveyResultService, private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.params.pipe(
      map(params => params['id'])
    ).subscribe({
      next: value => {
        this._loadChartData(value);
      },
      error: (e: Error) => {
      }
    });

  }
  private _loadChartData(teamId?: number) {
    this.dataStatusIndicator?.setLoading();

    this.surveyResultService.getChartData(teamId).subscribe(data => {

      if (data.scores) {
        this.dataStatusIndicator?.setDefault();
        this.chartData = data;
        this.buildChartData();
      }
      else {
        this.dataStatusIndicator?.setNoData();
      }
    });
  }
  buildChartData() {
    const thisRef = this;
    const average = this.chartData.scores.map((data) => data.average)
    const categories = this.chartData.scores.map((data) => data.categoryName)
    const result = (average.reduce((sum, current) => sum + current, 0) / categories.length)
    this.overallHPTAScore = result % 1 !== 0 ? result.toFixed(2) : result;

    this.chartOptions = {
      series: [
        {
          name: 'Average',
          data: average,
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
        }
      },
      title: {
        text: 'High Performing Team Report',
      },
      dataLabels: {
        enabled: true,
        style: {
          fontSize: '14px',
          fontFamily: 'Times New Roman',
          fontWeight: 'bold',
          colors: ['#333']
        },
      },
      xaxis: {
        categories: categories,
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
  }

  getColorCode(category: string): string {
    return CATEGORY_COLORS[category] || '#FFFFFF';
  }
}

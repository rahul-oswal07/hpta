import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResultService } from './services/survey-result.service';
import { ChartDataModel, ChartOptions, TeamsModel } from './models/team.model';

const CATEGORY_COLORS: any = {
  'Culture': '#85b5c7',
  'Growth Mindset': '#b3ccff',
  'Ownership': '#ff9f80',
  'Safe Environment': '#80aaff',
  'Strategic Alignment': '#ffb366',
  'Ways of Working': '#ff8080',
};

@Component({
  selector: 'app-survey-result',
  templateUrl: './survey-result.component.html',
  styleUrls: ['./survey-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class SurveyResultComponent implements OnInit {
  form: FormGroup;
  teams: TeamsModel[];
  filteredOptions: TeamsModel[];
  chartData: ChartDataModel[] = [];
  public chartOptions: ChartOptions;

  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService) {
    this._buildForm();
    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.teamService.getList<ChartDataModel>(value).subscribe(data => {
        this.chartData = data;
        this.buildChartData();
      });
    });

  }

  ngOnInit(): void {
    this.teamService.getList<TeamsModel>().subscribe(teams => {
      this.teams = this.filteredOptions = teams;
    });
  }

  onSearchInput(event: any): void {
    const searchValue = event.target.value.toLowerCase();
    this.filteredOptions = this.teams.filter(team => team.name.toLowerCase().includes(searchValue));
  }

  buildChartData() {
    const thisRef = this;

    this.chartOptions = {
      series: [
        {
          name: 'Average',
          data: this.chartData.map((data) => data.average),
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
        text: 'Survey report',
      },
      dataLabels: {
        enabled: false
      },
      xaxis: {
        categories: this.chartData.map((data) => data.categoryName),
        axisTicks: {
          show: false
        },
        title: {
          text: undefined,
        },
        labels: {
          show: false,
        },
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

  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: ['']
    })
  }
}

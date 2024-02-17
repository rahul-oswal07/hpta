import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResultService } from './services/survey-result.service';
import { TeamDataModel, ChartOptions, TeamsModel } from './models/team.model';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';

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
  chartData = {} as TeamDataModel;
  public chartOptions: ChartOptions;

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;

  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService) {
    this._buildForm();
    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.dataStatusIndicator?.setLoading();

      this.teamService.getChartData(value).subscribe(data => {

        if (data.scores) {
          this.dataStatusIndicator?.setDefault();
          this.chartData = data;
          this.buildChartData();
        }
        else {
          this.dataStatusIndicator?.setNoData();
        }
      });
    });

  }

  ngOnInit(): void {
    this.teamService.getTeams().subscribe(teams => {
      this.dataStatusIndicator?.setDefault();
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
          data: this.chartData.scores.map((data) => data.average),
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
        categories: this.chartData.scores.map((data) => data.categoryName),
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

  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: ['']
    })
  }
}

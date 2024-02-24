import { ChangeDetectorRef, Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
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
  disableDropdown: boolean = false;
  overallHPTAScore: string | number;
  teamId: number;
  userId: string;
  showDropdown: boolean;
  chartOptions: ChartOptions;
  categoryChartOptions: ChartOptions;
  isAnonymousUser: boolean

  @ViewChild(DataStatusIndicator)
  dataStatusIndicator?: DataStatusIndicator;

  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService, private cd: ChangeDetectorRef) {
    this._buildForm();

    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.teamId = value;
      this.categoryChartOptions = {} as ChartOptions;
      this._loadChartData(value);
    });

    this.form.controls['searchedInput'].valueChanges
      .subscribe(searchValue => {
        this.filteredOptions = this.teams.filter(team => team.name.toLowerCase().includes(searchValue));
      });
  }


  ngOnInit(): void {
    this._loadTeams();
  }

  private _loadTeams() {
    if (false) { TODO: //anonymous 
      this.isAnonymousUser = true;
      this.showDropdown = false;
      this.userId = '925b9ce499d44b818d390f26b04db51f';
      this._loadUserChartData(this.userId)
    }
    else {
      this.teamService.getTeams().subscribe(teams => {
        this.showDropdown = true;
        this.dataStatusIndicator?.setDefault();
        this.teams = this.filteredOptions = teams;
        if (false) { // TODO: not a super user
          this.disableDropdown = true;
          this.form.patchValue({ 'selectedTeam': this.teamId })
        }
      });
    }
  }


  buildChartData(chartData: TeamDataModel, title: string) {
    const thisRef = this;

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
          fontFamily: 'Times New Roman',
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

  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: [''],
      searchedInput: ['']
    })
  }

  private _loadChartData(teamId: number) {
    this.dataStatusIndicator?.setLoading();

    this.teamService.getTeamChartData(teamId).subscribe(data => {
      this.populateChart(data);
    });
  }

  private _loadCategoryChartData(teamId: number, categoryName: string) {
    const categoryId = Number(this.chartData.scores.find((data) => data.categoryName == categoryName)?.categoryId)

    if (categoryId) {
      this.categoryChartOptions = {} as ChartOptions;
      if (!this.isAnonymousUser) {
        this.teamService.getCategoryChartData(teamId, categoryId).subscribe(data => {
          this.categoryChartOptions = this.buildChartData(data, `Report for Category : ${categoryName}`);
          this.cd.detectChanges();
        });
      }
      else {
        this.teamService.getCategoryChartDataForUser(this.userId, categoryId).subscribe(data => {
          this.categoryChartOptions = this.buildChartData(data, `Report for Category : ${categoryName}`);
          this.cd.detectChanges();
        });
      }
    }
  }

  private _loadUserChartData(userId: string) {
    this.dataStatusIndicator?.setLoading();

    this.teamService.getUserChartData(userId).subscribe(data => {
      this.populateChart(data);
    });
  }

  private populateChart(data: TeamDataModel) {
    if (data.scores) {
      this.dataStatusIndicator?.setDefault();
      this.chartData = data;
      this.chartOptions = this.buildChartData(this.chartData, 'High Performing Team Report');

      const average = this.chartData.scores.map((data) => data.average);
      const categories = this.chartData.scores.map((data) => data.categoryName);
      const result = (average.reduce((sum, current) => sum + current, 0) / categories.length);
      this.overallHPTAScore = result % 1 !== 0 ? result.toFixed(2) : result;
    }
    else {
      this.overallHPTAScore = 0;
      this.chartOptions = {} as ChartOptions;
      this.chartData = {} as TeamDataModel;
      this.dataStatusIndicator?.setNoData();
    }
  }
}

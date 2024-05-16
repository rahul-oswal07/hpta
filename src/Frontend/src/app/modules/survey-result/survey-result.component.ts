import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResultService } from './services/survey-result.service';
import { TeamsModel } from './models/team.model';
import { ActivatedRoute, Router } from '@angular/router';
import { RoutingHelperService } from 'src/app/core/services/routing-helper.service';
import { SurveyService } from 'src/app/modules/survey/survey.service';
import { ListItem } from 'src/app/core/models/list-item';


@Component({
  selector: 'app-survey-result',
  templateUrl: './survey-result.component.html',
  styleUrls: ['./survey-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class SurveyResultComponent implements OnInit, OnDestroy {
  form: FormGroup;
  teams: TeamsModel[];
  surveys: ListItem[]
  filteredOptions: TeamsModel[];
  teamId: number;
  surveyId: number;
  teamMembers: string[];

  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService, private router: Router, private route: ActivatedRoute
    , private routingHelper: RoutingHelperService, private surveyService: SurveyService) {
    this._buildForm();

    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.teamId = value;
      this.router.navigate(['team', this.teamId], { relativeTo: this.route, queryParams: { survey: this.surveyId } });
      this.loadTeamMembers();
    });
    this.form.controls['selectedSurvey'].valueChanges.subscribe(value => {
      this.surveyId = value;
      if (this.teamId) {
        this.router.navigate(['team', this.teamId], { relativeTo: this.route, queryParams: { survey: this.surveyId } });
      }
    });
    this.form.controls['searchedInput'].valueChanges
      .subscribe(searchValue => {
        this.filteredOptions = this.teams.filter(team => team.name.replace(/\s/g, '').toLowerCase().includes(searchValue.toLowerCase()));
      });

    this.routingHelper.parameterChange().subscribe(params => {
      if (params && !this.teamId) {
        this.teamId = parseInt(params['id']);
      }
    });
  }


  ngOnInit(): void {
    this.surveyService.listSurveys().subscribe(r => {
      this.surveys = r;
      if (!this.surveyId && r.length > 0) {
        this.form.get('selectedSurvey')?.patchValue(r[r.length - 1].id);
      }
    })
    this._loadTeams();
  }

  private _loadTeams() {
    this.teamService.getTeams().subscribe(teams => {
      this.teams = this.filteredOptions = teams;
      if (!this.teamId) {
        this.teamId = this.teams[0].id
      }
      this.form.patchValue({ 'selectedTeam': this.teamId });
      this.loadTeamMembers();
    });
  }

  loadTeamMembers() {
    if (isNaN(this.teamId)) {
      return;
    }
    this.teamService.listMembers(this.teamId).subscribe(r => this.teamMembers = r);
  }

  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: [''],
      searchedInput: [''],
      selectedSurvey: ['']
    })
  }

  ngOnDestroy(): void {

  }
}

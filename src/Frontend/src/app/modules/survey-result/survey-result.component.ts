import { Component, OnDestroy, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResultService } from './services/survey-result.service';
import { TeamsModel } from './models/team.model';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { RoutingHelperService } from 'src/app/core/services/routing-helper.service';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-survey-result',
  templateUrl: './survey-result.component.html',
  styleUrls: ['./survey-result.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class SurveyResultComponent implements OnInit, OnDestroy {
  form: FormGroup;
  teams: TeamsModel[];
  filteredOptions: TeamsModel[];
  teamId: number;
  teamMembers$: Observable<string[]>;

  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService, private router: Router, private route: ActivatedRoute
    , private routingHelper: RoutingHelperService) {
    this._buildForm();

    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.router.navigate(['team', value], { relativeTo: this.route })
    });

    this.form.controls['searchedInput'].valueChanges
      .subscribe(searchValue => {
        this.filteredOptions = this.teams.filter(team => team.name.toLowerCase().includes(searchValue));
      });

    this.routingHelper.parameterChange().subscribe(params => {
      console.log('a');
      if (params && !this.teamId) {
        this.teamId = parseInt(params['id']);
      }
    });
  }


  ngOnInit(): void {
    this._loadTeams();
  }

  private _loadTeams() {
    this.teamService.getTeams().subscribe(teams => {
      this.teams = this.filteredOptions = teams;
      if (!this.teamId) {
        this.teamId = this.teams[0].id
      }
      this.form.patchValue({ 'selectedTeam': this.teamId });
      this.teamMembers$ = this.teamService.listMembers(this.teamId);
    });
  }

  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: [''],
      searchedInput: ['']
    })
  }

  ngOnDestroy(): void {

  }
}

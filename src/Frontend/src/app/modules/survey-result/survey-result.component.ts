import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { SurveyResultService } from './services/survey-result.service';
import { TeamDataModel, ChartOptions, TeamsModel } from './models/team.model';
import { DataStatusIndicator } from 'src/app/modules/data-status-indicator/data-status-indicator.component';
import { ActivatedRoute, Route, Router } from '@angular/router';


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

  showDropdown: boolean = false;
  constructor(private formBuilder: FormBuilder, private teamService: SurveyResultService, private router: Router, private route: ActivatedRoute) {
    this._buildForm();
    this.form.controls['selectedTeam'].valueChanges.subscribe(value => {
      this.router.navigate(['team', value], { relativeTo: this.route })
    });
  }


  ngOnInit(): void {
    this._enableDisableDropdown();
  }

  private _loadTeams() {
    this.teamService.getTeams().subscribe(teams => {
      this.teams = this.filteredOptions = teams;
    });
  }

  onSearchInput(event: any): void {
    event.stopPropagation();
    const searchValue = event.target.value.toLowerCase();
    this.filteredOptions = this.teams.filter(team => team.name.toLowerCase().includes(searchValue));
  }



  private _buildForm() {
    this.form = this.formBuilder.group({
      selectedTeam: ['']
    })
  }



  private _enableDisableDropdown() {
    this.showDropdown = true; //TODO: Compute the logic based on role
    if (!this.showDropdown) {
      this.router.navigate(['view'], { relativeTo: this.route })
    }
    else {
      this._loadTeams();
    }
  }
}

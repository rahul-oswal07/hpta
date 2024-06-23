import { Injectable, Injector } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { DataService } from "src/app/core/services/data.service";
import { ChartDataRequestModel, TeamDataModel, TeamMemberModel, TeamsModel } from "src/app/modules/survey-result/models/team.model";

@Injectable({
  providedIn: 'root'
})
export class SurveyResultService extends DataService {
  private teamMemberId$: Subject<string> = new Subject<string>();
  override path = 'team';

  constructor(injector: Injector) {
    super(injector);
  }

  getTeams() {
    return this.getList<TeamsModel>();
  }

  getChartData(chartRequest: ChartDataRequestModel, teamId?: number): Observable<TeamDataModel> {
    if (!!teamId) {
      return this.post(chartRequest, ['result', teamId.toString()]);
    }
    return this.post(chartRequest, ['result']);
  }

  getCategoryChartData(chartRequest: ChartDataRequestModel, categoryId: number, teamId?: number): Observable<TeamDataModel> {
    if (!!teamId) {
      return this.post(chartRequest, ['result-category', categoryId.toString(), teamId.toString()]);
    }
    return this.post(chartRequest, ['result-category', categoryId.toString()]);
  }

  listMembers(teamId: number) {
    return this.getList<TeamMemberModel>([teamId.toString(), 'members']);
  }

  updateTeamMemberId(email: string): void {
    this.teamMemberId$.next(email);
  }

  getTeamMemberId(): Observable<string> {
    return this.teamMemberId$;
  }
}

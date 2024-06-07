import { Injectable, Injector } from "@angular/core";
import { Observable, Subject } from "rxjs";
import { DataService } from "src/app/core/services/data.service";
import { TeamDataModel, TeamMemberModel, TeamsModel } from "src/app/modules/survey-result/models/team.model";

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

  getChartData(email: string, teamId?: number, surveyId?: number) {
    if (!!teamId) {
      if (!!email) {
        return this.getSingle<TeamDataModel>(['result', teamId.toString()], { surveyId, email });
      }
      return this.getSingle<TeamDataModel>(['result', teamId.toString()], { surveyId });
    }
    return this.getSingle<TeamDataModel>(['result'], { surveyId });
  }

  getCategoryChartData(email: string, categoryId: number, teamId?: number, surveyId?: number) {
    if (!!teamId) {
      if (!!email) {
        return this.getSingle<TeamDataModel>(['result-category', categoryId.toString(), teamId.toString()], { surveyId, email });
      }
      return this.getSingle<TeamDataModel>(['result-category', categoryId.toString(), teamId.toString()], { surveyId });
    }
    return this.getSingle<TeamDataModel>(['result-category', categoryId.toString()], { surveyId });
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

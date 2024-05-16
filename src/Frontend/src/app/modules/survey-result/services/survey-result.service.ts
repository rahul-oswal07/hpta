import { Injectable, Injector } from "@angular/core";
import { DataService } from "src/app/core/services/data.service";
import { TeamDataModel, TeamsModel } from "src/app/modules/survey-result/models/team.model";

@Injectable({
  providedIn: 'root'
})
export class SurveyResultService extends DataService {
  override path = 'team';

  constructor(injector: Injector) {
    super(injector);
  }

  getTeams() {
    return this.getList<TeamsModel>();
  }

  getChartData(teamId?: number, surveyId?: number) {
    if (!!teamId) {
      return this.getSingle<TeamDataModel>(['result', teamId.toString()], { surveyId });
    }
    return this.getSingle<TeamDataModel>(['result'], { surveyId });
  }

  getCategoryChartData(categoryId: number, teamId?: number, surveyId?: number) {
    if (!!teamId) {
      return this.getSingle<TeamDataModel>(['result-category', categoryId.toString(), teamId.toString()], { surveyId });
    }
    return this.getSingle<TeamDataModel>(['result-category', categoryId.toString()], { surveyId });
  }

  listMembers(teamId: number) {
    return this.getList<string>([teamId.toString(), 'members']);
  }
}

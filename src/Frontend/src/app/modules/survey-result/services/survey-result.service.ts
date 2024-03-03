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

  getChartData(teamId?: number) {
    if (!!teamId) {
      return this.getSingle<TeamDataModel>(['result', teamId.toString()]);
    }
    return this.getSingle<TeamDataModel>(['result']);
  }

  getCategoryChartData(categoryId: number, teamId?: number) {
    if (!!teamId) {
      return this.getSingle<TeamDataModel>(['result-category', categoryId.toString(), teamId.toString()]);
    }
    return this.getSingle<TeamDataModel>(['result-category', categoryId.toString()]);
  }

  listMembers(teamId: number) {
    return this.getList<string>([teamId.toString(), 'members']);
  }
}

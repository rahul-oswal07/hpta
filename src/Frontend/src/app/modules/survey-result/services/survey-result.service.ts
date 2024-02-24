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

  getTeamChartData(teamId: number) {
    return this.getSingle<TeamDataModel>(['team-chart', teamId.toString()]);
  }

  getUserChartData(userId: string) {
    return this.getSingle<TeamDataModel>(['user-chart', userId]);
  }

  getCategoryChartData(teamId: number, categoryId: number) {
    return this.getSingle<TeamDataModel>(['category-chart', teamId.toString(), categoryId.toString()]);
  }

  getCategoryChartDataForUser(userId: string, categoryId: number) {
    return this.getSingle<TeamDataModel>(['user-category-chart', userId, categoryId.toString()]);
  }
}

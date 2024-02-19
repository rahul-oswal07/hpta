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

  getChartData(teamId: number) {
    return this.getSingle<TeamDataModel>(teamId.toString());
  }

  getTeams() {
    return this.getList<TeamsModel>();
  }
}

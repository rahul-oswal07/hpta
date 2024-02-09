import { Injectable, Injector } from "@angular/core";
import { DataService } from "src/app/core/services/data.service";
import { ChartDataModel, TeamsModel } from "src/app/modules/survey-result/models/team.model";

@Injectable({
  providedIn: 'root'
})
export class SurveyResultService extends DataService {
  override path = 'team';

  constructor(injector: Injector) {
    super(injector);
  }
  getChartData(value: any) {
    return this.getList<ChartDataModel>(value);
  }

  getTeams() {
    return this.getList<TeamsModel>();
  }
}

import { Injectable, Injector } from "@angular/core";
import { DataService } from "src/app/core/services/data.service";

@Injectable({
    providedIn: 'root'
  })
  export class SurveyResultService extends DataService {
    override path = 'team';
  
    constructor(injector: Injector) {
      super(injector);
    }
  }
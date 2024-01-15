import { AppConfigService } from "src/app/core/services/app-config.service";

export function initializeApp(appConfigService: AppConfigService) {
  return (): Promise<any> => {
    return appConfigService.getConfig().toPromise();
  }
}

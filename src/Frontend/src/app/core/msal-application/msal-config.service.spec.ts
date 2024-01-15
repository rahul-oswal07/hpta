import { TestBed } from '@angular/core/testing';

import { MsalConfigService } from './msal-config.service';

describe('MsalConfigService', () => {
  let service: MsalConfigService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MsalConfigService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

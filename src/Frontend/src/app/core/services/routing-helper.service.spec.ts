import { TestBed } from '@angular/core/testing';

import { RoutingHelperService } from './routing-helper.service';

describe('RoutingHelperService', () => {
  let service: RoutingHelperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(RoutingHelperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

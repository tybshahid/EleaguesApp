/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { EleaguesService } from './eleagues.service';

describe('Service: Eleagues', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [EleaguesService]
    });
  });

  it('should ...', inject([EleaguesService], (service: EleaguesService) => {
    expect(service).toBeTruthy();
  }));
});

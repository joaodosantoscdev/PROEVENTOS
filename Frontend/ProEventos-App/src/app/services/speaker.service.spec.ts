/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { SpeakerService } from './speaker.service';

describe('Service: Speaker', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [SpeakerService]
    });
  });

  it('should ...', inject([SpeakerService], (service: SpeakerService) => {
    expect(service).toBeTruthy();
  }));
});

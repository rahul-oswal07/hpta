/**
 * Helper classes to deal with protractor timeouts:
 * https://github.com/angular/protractor/blob/master/docs/timeouts.md
 */

import { Subscription } from 'rxjs';
import { NgZone } from '@angular/core';
import { AsyncScheduler } from 'rxjs/internal/scheduler/AsyncScheduler';

class LeaveZoneSchduler {
  constructor(private readonly zone: NgZone, private readonly scheduler: AsyncScheduler) { }

  /**
   * Schedule a task to be run outside Angular zone.
   * @param args
   */
  schedule(...args: any[]): Subscription {
    return this.zone.runOutsideAngular(() =>
      this.scheduler.schedule.apply(this.scheduler, args)
    );
  }
}

class EnterZoneScheduler {
  constructor(private readonly zone: NgZone, private readonly scheduler: AsyncScheduler) { }

  /**
   * Schedule a task to be run inside Angular zone.
   */
  schedule(...args: any[]): Subscription {
    return this.zone.run(() =>
      this.scheduler.schedule.apply(this.scheduler, args)
    );
  }
}

/**
 * Create new LeaveZoneScheduler.
 * @param zone NgZone reference
 * @param scheduler scheduler (for example asyncScheduler from rxjs)
 */
export function leaveZone(zone: NgZone, scheduler: AsyncScheduler): AsyncScheduler {
  return new LeaveZoneSchduler(zone, scheduler) as any;
}

/**
 * Create new EnterZoneScheduler.
 * @param zone NgZone reference
 * @param scheduler scheduler (for example asyncScheduler from rxjs)
 */
export function enterZone(zone: NgZone, scheduler: AsyncScheduler): AsyncScheduler {
  return new EnterZoneScheduler(zone, scheduler) as any;
}

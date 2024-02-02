/**
 * @license
 * Copyright 2022 Google LLC
 *
 * Use of this source code is governed by an MIT-style
 * license that can be found in the LICENSE file or at
 * https://opensource.org/licenses/MIT.
 */

import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Inject,
  Input,
  LOCALE_ID,
  OnDestroy,
  OnInit,
  Output,
} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, ValidatorFn } from '@angular/forms';
import { Subscription, of } from 'rxjs';
import { take } from 'rxjs/operators';
import {
  setControlDisabled,
  timeStringValidator,
  valueValidator,
} from '../../../util';
import {
  ActiveFilter,
  Column,
  DateFilterOperation,
  DateFilterParams,
  FilterForm,
  FilterOption,
} from '../../../models';
import { DateTime } from 'luxon';

@Component({
  selector: 'app-filter-date-form',
  templateUrl: './filter-date-form.component.html',
  styleUrls: ['./filter-date-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterDateFormComponent implements FilterForm, OnInit, OnDestroy {
  @Input() filterOption!: Column;
  @Input() filter?: ActiveFilter;
  @Output() apply = new EventEmitter<void>();

  get invalid(): boolean {
    return this.form.invalid;
  }
  get DateFilterOperation(): typeof DateFilterOperation {
    return DateFilterOperation;
  }
  get dateLabel(): string {
    return this.operationCtrl.value !== DateFilterOperation.Between ? 'Date' : 'Min Date';
  }

  readonly form: FormGroup;
  readonly operationCtrl: FormControl;
  readonly dateCtrl: FormControl;
  readonly timeCtrl: FormControl;
  readonly date2Ctrl: FormControl;
  readonly time2Ctrl: FormControl;
  readonly operations = Object.entries(DateFilterOperation)
    .map(([_, value]) => value)
    .sort();
  constructor(@Inject(LOCALE_ID) private locale: string, fb: FormBuilder) {
    this.form = fb.group({
      operation: (this.operationCtrl = fb.control(DateFilterOperation.Equal)),
      date: (this.dateCtrl = fb.control({ value: null, disabled: false }, valueValidator)),
      time: (this.timeCtrl = fb.control({ value: null, disabled: false }, timeStringValidator)),
      date2: (this.date2Ctrl = fb.control({ value: null, disabled: true }, valueValidator)),
      time2: (this.time2Ctrl = fb.control({ value: null, disabled: true }, timeStringValidator)),
    });
    this.operationCtrl.valueChanges.subscribe((operation: DateFilterOperation) => {
      setControlDisabled(this.date2Ctrl, operation !== DateFilterOperation.Between);
      setControlDisabled(this.time2Ctrl, operation !== DateFilterOperation.Between);
    });
  }

  ngOnInit(): void {
    if (this.filter?.params) {
      this.reset(this.filter.params as DateFilterParams);
    }
  }

  ngOnDestroy(): void {

  }

  getLabel(): string {
    const operation: DateFilterOperation = this.operationCtrl.value;
    const range = this.getRange();
    if (operation === DateFilterOperation.Between) {
      return `${this.filterOption.label} between ${range.start} and ${range.end}`;
    }
    return `${this.filterOption.label} ${operation.toLocaleLowerCase(
      this.locale
    )} ${range.start}`;
  }

  getValue(): DateFilterParams {
    const range = this.getRange();
    return {
      operation: this.operationCtrl.value,
      value: range.start || undefined,
      value2: range.end || undefined,
      timeSpecified: this.getTimeSpecified(),
      time2Specified: this.getTime2Specified(),
    };
  }

  private getRange(): { start: string | null; end: string | null } {
    const date = this.dateCtrl.value as DateTime;
    const operation: DateFilterOperation = this.operationCtrl.value;
    let start: string | null;
    let end: string | null;
    switch (operation) {
      case DateFilterOperation.Equal:
        start = date.startOf('day').toISO();
        end = date.endOf('day').toISO();
        // start = localDateTimeToUtcSeconds(
        //   date,
        //   this.timeCtrl.value || '00:00',
        //   this.timezoneOffset
        // );
        // end = localDateTimeToUtcSeconds(
        //   date,
        //   this.timeCtrl.value || '23:59:59',
        //   this.timezoneOffset
        // );
        break;
      case DateFilterOperation.GreaterThan:
      case DateFilterOperation.LessThanOrEqual:
        start = date.endOf('day').toISO();
        end = start;
        break;
      case DateFilterOperation.GreaterThanOrEqual:
      case DateFilterOperation.LessThan:
        start = date.startOf('day').toISO();
        end = start;
        break;
      case DateFilterOperation.Between:
        start = date.startOf('day').toISO();
        end = date.endOf('day').toISO();
        break;
    }
    return { start, end };
  }

  private getTimeSpecified(): boolean {
    return !!this.timeCtrl.value;
  }

  private getTime2Specified(): boolean {
    return !!(this.operationCtrl.value === DateFilterOperation.Between && this.time2Ctrl.value);
  }

  private reset(params?: DateFilterParams): void {
    if (!params) {
      this.form.reset();
      return;
    }
    // const date =
    //   params.value != null ? timeToDate(params.value, this.timezoneOffset) : null;
    // const time =
    //   params.value != null && params.timeSpecified
    //     ? formatTime(params.value, undefined, this.timezoneOffset)
    //     : null;
    // const date2 =
    //   params.value2 != null ? timeToDate(params.value2, this.timezoneOffset) : null;
    // const time2 =
    //   params.value2 != null && params.time2Specified
    //     ? formatTime(params.value2, undefined, this.timezoneOffset)
    //     : null;
    // this.form.reset({
    //   operation: params?.operation ?? null,
    //   date,
    //   time,
    //   date2,
    //   time2,
    // });
  }

}


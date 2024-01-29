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
  OnInit,
  Output,
} from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { setControlDisabled, valueValidator } from '../../../util';
import {
  ActiveFilter,
  Column,
  FilterForm,
  FilterOption,
  NumberFilterOperation,
  NumberFilterParams,
} from '../../../models';

@Component({
  selector: 'app-filter-number-form',
  templateUrl: './filter-number-form.component.html',
  styleUrls: ['./filter-number-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FilterNumberFormComponent implements FilterForm, OnInit {
  @Input() filterOption!: Column;
  @Input() filter?: ActiveFilter;
  @Output() apply = new EventEmitter<void>();

  get invalid(): boolean {
    return this.form.invalid;
  }
  get NumberFilterOperation(): typeof NumberFilterOperation {
    return NumberFilterOperation;
  }
  get valueLabel(): string {
    return this.operationCtrl.value !== NumberFilterOperation.Between ? 'Value' : 'Min Value';
  }

  readonly form: FormGroup;
  readonly operationCtrl: FormControl;
  readonly valueCtrl: FormControl;
  readonly value2Ctrl: FormControl;
  readonly operations = Object.entries(NumberFilterOperation)
    .map(([_, value]) => value)
    .sort();

  constructor(@Inject(LOCALE_ID) private locale: string, fb: FormBuilder) {
    this.form = fb.group({
      operation: (this.operationCtrl = fb.control(NumberFilterOperation.Equal)),
      value: (this.valueCtrl = fb.control({ value: null, disabled: false }, valueValidator)),
      value2: (this.value2Ctrl = fb.control({ value: null, disabled: true }, valueValidator)),
    });
    this.operationCtrl.valueChanges.subscribe((operation: NumberFilterOperation) => {
      setControlDisabled(this.valueCtrl, operation === NumberFilterOperation.Empty);
      setControlDisabled(this.value2Ctrl, operation !== NumberFilterOperation.Between);
    });
  }

  ngOnInit(): void {
    if (this.filter?.params) {
      this.form.reset(this.filter.params);
    }
  }

  getLabel(): string {
    const operation: NumberFilterOperation = this.operationCtrl.value;
    if (operation === NumberFilterOperation.Empty) {
      return `${this.filterOption.label} is empty`;
    }
    if (operation === NumberFilterOperation.Between) {
      return `${this.filterOption.label} between ${this.valueCtrl.value} and ${this.value2Ctrl.value}`;
    }
    return `${this.filterOption.label} ${operation.toLocaleLowerCase(this.locale)} ${this.valueCtrl.value
      }`;
  }

  getValue(): NumberFilterParams {
    return this.form.value;
  }

}

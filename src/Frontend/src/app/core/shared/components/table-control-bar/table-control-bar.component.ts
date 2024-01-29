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
  Input,
  Output,
  ViewEncapsulation
} from '@angular/core';
import { Column, ActiveFilter } from '../../../models';
import { FilterService } from 'src/app/core/shared/services/filter.service';
import { positionTopLeftRelativeToTopLeft } from 'src/app/core/util';

@Component({
  selector: 'app-table-control-bar',
  templateUrl: './table-control-bar.component.html',
  styleUrls: ['./table-control-bar.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.None,
  host: {
    class: 'kbs-table-control-bar'
  }
})
export class TableControlBarComponent {
  @Input() filters: ActiveFilter[] = [];
  // @Input() filterOptions?: any[];
  @Output() filtersChange = new EventEmitter<ActiveFilter[]>();

  @Input() displayColumns?: Column[];
  @Output() displayColumnsChange = new EventEmitter<Column[]>();

  constructor(private readonly filterService: FilterService) {

  }

  onDisplayColumnChange(column: Column, active: boolean): void {
    column.hidden = !active;
    this.displayColumnsChange.emit(this.displayColumns);
  }

  onFilterEdited(filter: ActiveFilter) {

  }

  onFilterRemoved(filter: ActiveFilter) {
    const idx = this.filters.findIndex(f => f.id === filter.id);
    if (idx >= 0) {
      this.filters.splice(idx, 1);
      this.filtersChange.emit(this.filters);
    }
  }

  onAddFilter(data: { field: Column, event: MouseEvent }) {
    this.filterService.createFilter(data.field,
      data.event.target as HTMLElement,
      { id: data.field.id, label: data.field.label },
      positionTopLeftRelativeToTopLeft).subscribe(r => {
        if (r) {
          this.filters = [...this.filters, r];
          this.filtersChange.emit(this.filters);
        }
      });
  }

  onEditFilter(event: { filter: ActiveFilter, element: HTMLElement }): void {
    if (event.filter != null && !!this.displayColumns) {
      const column = this.displayColumns.find(c => c.id === event.filter.id);
      if (!column) {
        return;
      }
      this.filterService.createFilter(column,
        event.element,
        event.filter,
        positionTopLeftRelativeToTopLeft);
    }
  }

  trackDisplayColumnBy(index: number, displayColumn: Column): string {
    return displayColumn.id;
  }
}

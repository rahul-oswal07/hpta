import { Injectable } from '@angular/core';
import { DialogPosition, MatDialog } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { ActiveFilter, Column, FilterOption } from 'src/app/core/models';
import { FilterComponent } from 'src/app/core/shared/components/filter/filter.component';
import { positionTopLeftRelativeToTopRight } from 'src/app/core/util';

@Injectable()
export class FilterService {
  constructor(private dialog: MatDialog) {}

  createFilter(
    filterOption: Column,
    relativeTo: HTMLElement,
    filter: ActiveFilter,
    dialogPositionStrategy: (
      relativeTo: HTMLElement
    ) => DialogPosition = positionTopLeftRelativeToTopRight
  ): Observable<ActiveFilter | undefined> {
    // // If it doesn't have a form, the filter doesn't require params
    // if (!filterOption.form) {
    //   return of({ id: filterOption.id, label: filterOption.label });
    // }

    // Use the filter dialog to obtain params
    const position = dialogPositionStrategy?.apply(null, [relativeTo]);
    const dialogRef = this.dialog.open<FilterComponent, any, ActiveFilter>(FilterComponent, {
      backdropClass: 'filter-backdrop',
      panelClass: 'filter-dialog',
      position,
    });
    dialogRef.componentInstance.filterOption = filterOption;
    dialogRef.componentInstance.filter = filter;
    return dialogRef.afterClosed();
  }
}

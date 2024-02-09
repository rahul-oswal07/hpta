import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
import { DialogService } from 'src/app/modules/dialog/dialog.service';

export interface BlockNavigationIfChange {
  canDeactivate: () => boolean
}

export const UnsavedChangesGuard: CanDeactivateFn<BlockNavigationIfChange> = (component: BlockNavigationIfChange) => {
  if (component.canDeactivate) {
    if (component.canDeactivate()) {
      return true;
    }
    return inject(DialogService).showConfirm('You have unsaved changes. Do you want to discard them?', 'Yes', 'No');
  }
  return true
}

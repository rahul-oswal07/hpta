import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Observable, map } from 'rxjs';
import { DialogComponent } from 'src/app/modules/dialog/dialog.component';
import { ComponentType } from '@angular/cdk/portal';

export interface DialogData {
  title: string;
  message: string;
  okText: string;
  cancelText?: string;
}

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(public dialog: MatDialog) { }

  showAlert(data: DialogData | string, okText?: string): Observable<any> {
    const config = new MatDialogConfig<DialogData>();
    config.data = this.createDialogData(data, okText);
    config.maxWidth = '500px';
    const dialogRef = this.dialog.open(DialogComponent, config);
    return dialogRef.afterClosed();
  }

  createDialogData(data: DialogData | string, okText?: string, cancelText?: string): DialogData {
    let dialogData = {} as DialogData;
    if (typeof data === 'string' || data instanceof String) {
      dialogData.message = data as string;
    }
    else {
      dialogData = data as DialogData;
    }
    if (!dialogData.okText || okText) {
      dialogData.okText = okText || 'OK';
    }
    if (!dialogData.cancelText || cancelText) {
      dialogData.cancelText = cancelText;
    }
    if (!dialogData.title) {
      if (dialogData.cancelText) {
        dialogData.title = 'Confirmation';
      }
      else {
        dialogData.title = 'Alert!!!';
      }
    }
    return dialogData;
  }
  showConfirm(data: DialogData | string, okText?: string, cancelText?: string): Observable<boolean> {
    const config = new MatDialogConfig<DialogData>();
    config.data = this.createDialogData(data, okText || 'OK', cancelText || 'Cancel');
    config.maxWidth = '500px';
    const dialogRef = this.dialog.open(DialogComponent, config);
    return dialogRef.afterClosed().pipe(map(dr => {
      return dr === true;
    }));
  }

  /*
   * Opens a modal dialog containing the given component.
   * @param componentOrTemplateRef Type of the component to load into the dialog,
   *     or a TemplateRef to instantiate as the dialog content.
   * @param config Extra configuration options.
   * @returns Reference to the newly-opened dialog.
   */
  showComponent<T, D>(component: ComponentType<T>, config: MatDialogConfig<D>): Observable<any> {
    const dialogRef = this.dialog.open(component, config);
    return dialogRef.afterClosed();
  }
  showFlyoutDialog<T, D>(component: ComponentType<T>, data: D): Observable<any> {
    const config = {
      maxHeight: '100%',
      maxWidth: '100%',
      position: { right: '0', bottom: '0' },
      panelClass: 'fly-out-dialog',
      data
    }
    const dialogRef = this.dialog.open(component, config);
    return dialogRef.afterClosed();
  }
}

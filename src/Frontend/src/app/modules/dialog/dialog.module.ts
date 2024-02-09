import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DialogComponent } from './dialog.component';
import { DialogService } from 'src/app/modules/dialog/dialog.service';
import { MatDialogConfig, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
  ],
  providers: [
    DialogService,
    { provide: MatDialogConfig, useValue: { closeOnNavigation: true } }
  ],
  declarations: [DialogComponent]
})
export class DialogModule { }

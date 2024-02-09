import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataStatusIndicator } from './data-status-indicator.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  imports: [
    CommonModule,
    MatProgressSpinnerModule
  ],
  declarations: [DataStatusIndicator],
  exports: [
    DataStatusIndicator
  ]
})
export class DataStatusIndicatorModule { }

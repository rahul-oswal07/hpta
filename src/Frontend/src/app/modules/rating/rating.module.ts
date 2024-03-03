import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RatingComponent } from './rating.component';
import { RatingViewComponent } from './rating-view/rating-view.component';



@NgModule({
  declarations: [
    RatingComponent,
    RatingViewComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    RatingComponent, RatingViewComponent
  ]
})
export class RatingModule { }

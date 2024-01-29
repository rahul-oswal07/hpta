import { Pipe, PipeTransform } from '@angular/core';
import { spotBiddingTypes } from 'src/app/core/shared/models/indent';

@Pipe({ name: 'spotBiddingTypePipe' })
export class SpotBiddingTypePipe implements PipeTransform {
  transform(id: number): string {
    return spotBiddingTypes.find(type => type.id === id)?.value ?? 'Unknown';
  }
}

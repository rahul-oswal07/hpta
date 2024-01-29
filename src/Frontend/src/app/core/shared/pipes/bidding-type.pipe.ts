import { Pipe, PipeTransform } from '@angular/core';
import { biddingTypes } from 'src/app/core/shared/models/indent';

@Pipe({ name: 'biddingTypePipe' })
export class BiddingTypePipe implements PipeTransform {
  transform(id?: number): string {
    return biddingTypes.find(type => type.id === id)?.value ?? 'Unknown';
  }
}

import { Pipe, PipeTransform } from '@angular/core';
import { quotationStatus } from 'src/app/quotation/quotation';

@Pipe({
  name: 'quotationStatus'
})
export class QuotationStatusPipe implements PipeTransform {
  transform(id?: number): string {
    return quotationStatus.find(type => type.id === id)?.value ?? 'Pending';
  }
}

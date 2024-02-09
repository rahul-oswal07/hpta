import { DataService } from 'src/app/core/services/data.service';
import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { ListItem } from 'src/app/core/models/list-item';
import { AvailabilityCheckResult } from 'src/app/core/models/availability-check-result';
import { SubCategory } from './sub-category';


@Injectable({
  providedIn: 'root'
})
export class SubCategoriesService extends DataService {
  override path = 'subcategory';

  constructor(injector: Injector) {
    super(injector);
  }
  listAllSubCategories() {
    return this.getList<SubCategory>();
  }
  listSubCategoryItems() {
    return this.getList<ListItem>('list');
  }
  getSubCategoryById(id: number) {
    return this.getSingle<SubCategory>(id.toString());
  }
  checkNameAvailability(name: string, id?: number): Observable<AvailabilityCheckResult> {
    return this.getSingle<AvailabilityCheckResult>('check-name-availability', { id, name })
  }
  addSubCategory(subCategory: SubCategory) {
    return this.post(subCategory);
  }
  updateSubCategory(subCategory: SubCategory) {
    return this.put(subCategory);
  }
  deleteSubCategory(id: number) {
    return this.delete(id.toString());
  }
}

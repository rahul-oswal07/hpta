import { DataService } from 'src/app/core/services/data.service';
import { Injectable, Injector } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { Category } from 'src/app/modules/categories/category';
import { ListItem } from 'src/app/core/models/list-item';
import { AvailabilityCheckResult } from 'src/app/core/models/availability-check-result';

@Injectable({
  providedIn: 'root',
})
export class CategoriesService extends DataService {
  override path = 'category';

  constructor(injector: Injector) {
    super(injector);
  }
  listAllCategories() {
    return this.getList<Category>();
  }
  listCategoryItems() {
    return this.getList<ListItem>('list');
  }
  getCategoryById(id: number) {
    return this.getSingle<Category>(id.toString());
  }
  checkNameAvailability(
    name: string,
    id?: number
  ): Observable<AvailabilityCheckResult> {
    return this.getSingle<AvailabilityCheckResult>('check-name-availability', {
      id,
      name,
    });
  }
  addCategory(category: Category) {
    return this.post(category);
  }
  updateCategory(category: Category) {
    return this.put(category);
  }
  deleteCategory(id: number) {
    return this.delete(id.toString());
  }
}

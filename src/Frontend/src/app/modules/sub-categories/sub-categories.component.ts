import { Component, OnInit } from '@angular/core';
import { SubCategory } from './sub-category';
import { SubCategoriesService } from './sub-categories.service';
import { RoutingHelperService } from 'src/app/core/services/routing-helper.service';
import { DialogService } from '../dialog/dialog.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-sub-categories',
  templateUrl: './sub-categories.component.html',
  styleUrls: ['./sub-categories.component.scss'],
})
export class SubCategoriesComponent implements OnInit {
  originalSubCategories: SubCategory[] = [];
  categoryDictionary: {
    [key: string]: string;
  } = {};
  subCategories: {
    [key: string]: SubCategory[];
  } = {};
  activeCategoryId: number | null = null;
  private _searchTerm: string = '';

  get searchTerm(): string {
    return this._searchTerm;
  }

  set searchTerm(value: string) {
    this._searchTerm = value;
    this.applyFilter();
  }
  isLoading = false;
  constructor(
    private subCategoryService: SubCategoriesService,
    private routingHelper: RoutingHelperService,
    private dialogService: DialogService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.routingHelper.parameterChange().subscribe((params) => {
      if (!params) {
        return;
      }
      this.activeCategoryId = params['id'] ? +params['id'] : null;
    });
    this.loadSubCategories();
    this.subCategoryService.reloadRequest$.subscribe(() =>
      this.loadSubCategories()
    );
  }

  loadSubCategories() {
    this.isLoading = true;
    this.subCategoryService
      .listAllSubCategories()
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (result) => {
          this.originalSubCategories = result;
          this.categoryDictionary = result.reduce((acc, subCategory) => {
            acc[subCategory.categoryId!] = subCategory.categoryName!;
            return acc;
          }, {} as { [key: string]: string });
          this.applyFilter();
        },
        error: (e: Error) => {
          this.snackBar.open(e.message, 'Okay');
        },
      });
  }
  groupSubCategoriesByCategory(subCategories: SubCategory[]): {
    [key: string]: SubCategory[];
  } {
    const groupedSubCategories: { [key: string]: SubCategory[] } = {};
    subCategories.forEach((subCategory: SubCategory) => {
      const categoryId = (subCategory.categoryId || 0).toString();
      if (!groupedSubCategories[categoryId]) {
        groupedSubCategories[categoryId] = [];
      }
      groupedSubCategories[categoryId].push(subCategory);
    });
    return groupedSubCategories;
  }
  applyFilter() {
    const result = this.originalSubCategories.filter((subCategory) =>
      subCategory.name?.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
    this.subCategories = this.groupSubCategoriesByCategory(result);
  }
  clearSearch() {
    this.searchTerm = '';
    // this.applyFilter();
  }
  deleteSubCategory(id: number) {
    this.dialogService
      .showConfirm(
        'Are you sure you want to delete this category?',
        'Yes',
        'No'
      )
      .subscribe((confirm) => {
        if (confirm) {
          this.subCategoryService.deleteSubCategory(id).subscribe(() => {
            this.loadSubCategories();
          });
        }
      });
  }
}

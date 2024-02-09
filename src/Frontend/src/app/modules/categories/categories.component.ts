import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, finalize, map, switchMap } from 'rxjs';
import { RoutingHelperService } from 'src/app/core/services/routing-helper.service';
import { CategoriesService } from 'src/app/modules/categories/categories.service';
import { Category } from 'src/app/modules/categories/category';
import { DialogService } from 'src/app/modules/dialog/dialog.service';

@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  originalCategories: Category[] = [];
  categories: Category[] = [];
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
  constructor(private categoryService: CategoriesService,
    private routingHelper: RoutingHelperService,
    private dialogService: DialogService,
    private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.routingHelper.parameterChange().subscribe(params => {
      if (!params) {
        return;
      }
      this.activeCategoryId = params['id'] ? +params['id'] : null;
    });
    this.loadCategories();
    this.categoryService.reloadRequest$.subscribe(() => this.loadCategories());
  }

  loadCategories() {
    this.isLoading = true;
    this.categoryService.listAllCategories()
      .pipe(finalize(() => this.isLoading = false))
      .subscribe({
        next: categories => {
          this.originalCategories = categories;
          this.applyFilter();
        }, error: (e: Error) => {
          this.snackBar.open(e.message, 'Okay')
        }
      });
  }
  applyFilter() {
    this.categories = this.originalCategories.filter(category =>
      category.name?.toLowerCase().includes(this.searchTerm.toLowerCase())
    );
  }
  clearSearch() {
    this.searchTerm = '';
    // this.applyFilter();
  }
  deleteCategory(id: number) {
    this.dialogService.showConfirm('Are you sure you want to delete this category?', 'Yes', 'No').subscribe(confirm => {
      if (confirm) {
        this.categoryService.deleteCategory(id).subscribe(() => {
          this.loadCategories();
        })
      }
    })
  }
}

import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { finalize } from 'rxjs';
import { DialogService } from 'src/app/modules/dialog/dialog.service';
import { Question } from 'src/app/modules/questions/question';
import { QuestionsService } from 'src/app/modules/questions/questions.service';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.css']
})
export class QuestionsComponent implements OnInit, AfterViewInit {
  displayedColumns: string[] = ['answerType', 'categoryName', 'subCategoryName', 'text'];
  dataSource: MatTableDataSource<Question>;
  activeCategoryId: number | null = null;

  isLoading = false;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  constructor(private questionService: QuestionsService,
    private snackBar: MatSnackBar,
    private dialogService: DialogService) { }

  ngOnInit() {
    this.questionService.reloadRequest$.subscribe(() => this.loadQuestions());
    this.questionService.requestReload();
  }
  ngAfterViewInit(): void {

  }
  loadQuestions() {
    this.isLoading = true;
    this.questionService.listAllQuestions()
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe({
        next: (result) => {
          this.dataSource = new MatTableDataSource(result);
          this.dataSource.paginator = this.paginator;
          this.dataSource.sort = this.sort;
        },
        error: (e: Error) => {
          this.snackBar.open(e.message, 'Okay');
        },
      });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  clearFilter() {
    this.dataSource.filter = '';

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  deleteQuestion(id: number) {
    this.dialogService
      .showConfirm(
        'Are you sure you want to delete this question?',
        'Yes',
        'No'
      )
      .subscribe((confirm) => {
        if (confirm) {
          this.questionService.deleteQuestion(id).subscribe(() => {
            this.loadQuestions();
          });
        }
      });
  }
}

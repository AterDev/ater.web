import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmDialogComponent } from 'src/app/components/confirm-dialog/confirm-dialog.component';
import { SystemLogsService } from 'src/app/services/admin/system-logs/system-logs.service';
import { SystemLogsItemDto } from 'src/app/services/admin/system-logs/models/system-logs-item-dto.model';
import { SystemLogsFilterDto } from 'src/app/services/admin/system-logs/models/system-logs-filter-dto.model';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormGroup } from '@angular/forms';
import { UserActionType } from 'src/app/services/admin/enum/models/user-action-type.model';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.css']
})
export class IndexComponent implements OnInit {
  @ViewChild(MatPaginator, { static: true }) paginator!: MatPaginator;
  ActionType = UserActionType;
  isLoading = true;
  isProcessing = false;
  total = 0;
  data: SystemLogsItemDto[] = [];
  columns: string[] = ['actionUserName', 'targetName', 'route', 'actionType', 'description', 'createdTime', 'actions'];
  dataSource!: MatTableDataSource<SystemLogsItemDto>;
  dialogRef!: MatDialogRef<{}, any>;
  @ViewChild('myDialog', { static: true })
  myTmpl!: TemplateRef<{}>;
  mydialogForm!: FormGroup;
  filter: SystemLogsFilterDto;
  pageSizeOption = [12, 20, 50];
  constructor(
    private service: SystemLogsService,
    private snb: MatSnackBar,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private router: Router,
  ) {

    this.filter = {
      pageIndex: 1,
      pageSize: 12
    };
  }

  ngOnInit(): void {
    this.getList();
  }

  getList(event?: PageEvent): void {
    if (event) {
      this.filter.pageIndex = event.pageIndex + 1;
      this.filter.pageSize = event.pageSize;
    }
    this.service.filter(this.filter)
      .subscribe({
        next: (res) => {
          if (res) {
            if (res.data) {
              this.data = res.data;
              this.total = res.count;
              this.dataSource = new MatTableDataSource<SystemLogsItemDto>(this.data);
            }
          }
        },
        error: (error) => {
          this.snb.open(error.detail);
          this.isLoading = false;
        },
        complete: () => {
          this.isLoading = false;
        }
      });
  }
  
  filterDate = (d: Date | null): boolean => {
    const now = new Date();
    return d ? d <= now : false;
  };

  jumpTo(pageNumber: string): void {
    const number = parseInt(pageNumber);
    if (number > 0 && number < this.paginator.getNumberOfPages()) {
      this.filter.pageIndex = number;
      this.getList();
    }
  }

  /**
   * 编辑
   */
  edit(id: string): void {
    this.router.navigate(['../edit/' + id], { relativeTo: this.route });
  }
}

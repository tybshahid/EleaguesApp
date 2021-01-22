import { Component, OnInit, TemplateRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { League } from '../_models/league';
import { EleaguesService } from '../_services/eleagues.service';
import { AlertifyService } from '../_services/alertify.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-leagues',
  templateUrl: './leagues.component.html',
  styleUrls: ['./leagues.component.css']
})
export class LeaguesComponent implements OnInit {
  leagues: League[];
  pagination: Pagination;
  selectedIndex = -1;
  modalRef: BsModalRef;

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private modalService: BsModalService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.leagues = data['leagues'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['leagues'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.selectedIndex = -1;
    this.pagination.currentPage = event.page;
    this.loadLeagues();
  }

  loadLeagues() {
    this.eleaguesService
      .getLeagues(this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(
        (res: PaginatedResult<League[]>) => {
          this.leagues = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }

  setSelected(id: number) {
    this.selectedIndex = id;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  setPath(path: string) {
    if (path) {
      path = '../Uploads/Rules/' + path;
      return true;
    } else {
      return false;
    }
  }
}

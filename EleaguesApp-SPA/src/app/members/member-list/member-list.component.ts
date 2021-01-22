import { Component, OnInit } from '@angular/core';
import { User } from '../../_models/user';
import { AlertifyService } from '../../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { EleaguesService } from 'src/app/_services/eleagues.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  users: User[];
  countries = [
    {value: 'af', text: 'Afghanistan'},
    {value: 'au', text: 'Australia'},
    {value: 'bh', text: 'Bahrain'},
    {value: 'bb', text: 'Barbados'},
    {value: 'ca', text: 'Canada'},
    {value: 'de', text: 'Germany'},
    {value: 'in', text: 'India'},
    {value: 'it', text: 'Italy'},
    {value: 'pk', text: 'Pakistan'},
    {value: 'qa', text: 'Qatar'},
    {value: 'lc', text: 'Saint Lucia'},
    {value: 'sg', text: 'Singapore'},
    {value: 'se', text: 'Sweden'},
    {value: 'tt', text: 'Trinidad and Tobago'},
    {value: 'ae', text: 'UAE'},
    {value: 'gb', text: 'United Kingdom'},
    {value: 'us', text: 'USA'}
  ];
  userParams: any = {};
  pagination: Pagination;

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.userParams.userName = '';
    this.userParams.knownAs = '';
    this.userParams.country = '';
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.users = data['users'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['users'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadUsers();
  }

  resetFilters() {
    this.userParams.userName = '';
    this.userParams.knownAs = '';
    this.userParams.country = '';
    this.loadUsers();
  }

  loadUsers() {
    this.eleaguesService
      .getUsers(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.userParams
      )
      .subscribe(
        (res: PaginatedResult<User[]>) => {
          this.users = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}

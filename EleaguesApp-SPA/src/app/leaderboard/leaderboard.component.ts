import { Component, OnInit } from '@angular/core';
import { Leaderboard } from '../_models/leaderboard';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { EleaguesService } from '../_services/eleagues.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { DropdownListItem } from '../_models/dropdownListItem';

@Component({
  selector: 'app-leaderboard',
  templateUrl: './leaderboard.component.html',
  styleUrls: ['./leaderboard.component.css']
})
export class LeaderboardComponent implements OnInit {
  leaderboard: Leaderboard[];
  pagination: Pagination;
  selectedIndex = -1;
  gameParams: any = {};
  leagues: DropdownListItem[];
  players: DropdownListItem[];

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.gameParams.leagueId = 0;
    this.gameParams.winnerId = 0;
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.leagues = data['dropdowns'].leagues;
      // tslint:disable-next-line: no-string-literal
      this.players = data['dropdowns'].opponents;

      // tslint:disable-next-line: no-string-literal
      this.leaderboard = data['leaderboard'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['leaderboard'].pagination;
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.load();
  }

  resetFilters() {
    this.gameParams.leagueId = 0;
    this.gameParams.winnerId = 0;
    this.load();
  }

  setSelected(id: number) {
    this.selectedIndex = id;
  }

  load() {
    this.eleaguesService
      .getLeaderboard(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.gameParams
      )
      .subscribe(
        (res: PaginatedResult<Leaderboard[]>) => {
          this.leaderboard = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}

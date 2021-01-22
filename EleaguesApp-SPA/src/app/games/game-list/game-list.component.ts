import { Component, OnInit } from '@angular/core';
import { Pagination, PaginatedResult } from 'src/app/_models/pagination';
import { EleaguesService } from 'src/app/_services/eleagues.service';
import { AlertifyService } from 'src/app/_services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { GameList } from 'src/app/_models/gameList';
import { DropdownListItem } from 'src/app/_models/dropdownListItem';

@Component({
  selector: 'app-game-list',
  templateUrl: './game-list.component.html',
  styleUrls: ['./game-list.component.css']
})
export class GameListComponent implements OnInit {
  games: GameList[];
  gameParams: any = {};
  pagination: Pagination;
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
    this.gameParams.opponentId = 0;
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.leagues = data['dropdowns'].leagues;
      // tslint:disable-next-line: no-string-literal
      this.players = data['dropdowns'].opponents;

      // tslint:disable-next-line: no-string-literal
      this.games = data['games'].result;
      // tslint:disable-next-line: no-string-literal
      this.pagination = data['games'].pagination;
    });

    // this.players = Array.from(this.players.reduce((m, t) => m.set(t.id, t), new Map()).values());
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadGames();
  }

  resetFilters() {
    this.gameParams.leagueId = 0;
    this.gameParams.winnerId = 0;
    this.gameParams.opponentId = 0;
    this.loadGames();
  }

  loadGames() {
    this.eleaguesService
      .getGames(
        this.pagination.currentPage,
        this.pagination.itemsPerPage,
        this.gameParams
      )
      .subscribe(
        (res: PaginatedResult<GameList[]>) => {
          this.games = res.result;
          this.pagination = res.pagination;
        },
        error => {
          this.alertify.error(error);
        }
      );
  }
}

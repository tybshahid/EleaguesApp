import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { map } from 'rxjs/operators';
import { League } from '../_models/league';
import { User } from '../_models/user';
import { DropdownListItem } from '../_models/dropdownListItem';
import { GameManagerModel } from '../_models/GameManagerModel';
import { Game } from '../_models/game';
import { GameList } from '../_models/gameList';
import { Leaderboard } from '../_models/leaderboard';

@Injectable({
  providedIn: 'root'
})
export class EleaguesService {
  baseUrl = environment.apiUrl;
  formData: FormData;

  constructor(private http: HttpClient) {}

  // User Requests
  getUser(id): Observable<User> {
    return this.http.get<User>(this.baseUrl + 'users/' + id);
  }

  getUsers(
    page?,
    itemsPerPage?,
    userParams?
  ): Observable<PaginatedResult<User[]>> {
    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<
      User[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParams != null) {
      params = params.append('userName', userParams.userName);
      params = params.append('knownAs', userParams.knownAs);
      params = params.append('country', userParams.country);
    }

    return this.http
      .get<User[]>(this.baseUrl + 'users', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  // League Requests
  getLeaguesList(): Observable<DropdownListItem[]> {
    return this.http.get<DropdownListItem[]>(
      this.baseUrl + 'leagues/leaguesList'
    );
  }

  getLeague(id: number) {
    return this.http.get<League>(this.baseUrl + 'leagues/' + id);
    // let leagueResult: League = new League();
    // return this.http
    //   .get<League>(this.baseUrl + 'leagues/' + id, { observe: 'response' })
    //   .pipe(
    //     map(response => {
    //       leagueResult = response.body;
    //       return leagueResult;
    //     })
    //   );
  }

  getLeagues(page?, itemsPerPage?): Observable<PaginatedResult<League[]>> {
    const paginatedResult: PaginatedResult<League[]> = new PaginatedResult<
      League[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http
      .get<League[]>(this.baseUrl + 'leagues', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  saveLeague(league: League, file: File) {
    this.formData = new FormData();
    if (file) {
      this.formData.append('file', file, file.name);
    }
    Object.keys(league).forEach(key => this.formData.append(key, league[key]));

    this.formData.delete('startDate');
    this.formData.delete('endDate');
    this.formData.append('startDate', new Date(league.startDate).toUTCString());
    this.formData.append('endDate', new Date(league.endDate).toUTCString());

    return this.http.post(this.baseUrl + 'leagues', this.formData);
  }

  // Game Requests
  getGameManager(): Observable<GameManagerModel> {
    return this.http.get<GameManagerModel>(this.baseUrl + 'games/manager');
  }

  getGameManagerPublic(): Observable<GameManagerModel> {
    return this.http.get<GameManagerModel>(
      this.baseUrl + 'games/manager/public'
    );
  }

  saveGame(game: Game, file: File) {
    this.formData = new FormData();
    if (file) {
      this.formData.append('file', file, file.name);
    }
    Object.keys(game).forEach(key => this.formData.append(key, game[key]));
    return this.http.post(this.baseUrl + 'games', this.formData);
  }

  getGames(
    page?,
    itemsPerPage?,
    gameParams?
  ): Observable<PaginatedResult<GameList[]>> {
    const paginatedResult: PaginatedResult<GameList[]> = new PaginatedResult<
      GameList[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (gameParams != null) {
      params = params.append('leagueId', gameParams.leagueId);
      params = params.append('winnerId', gameParams.winnerId);
      params = params.append('opponentId', gameParams.opponentId);
    }

    return this.http
      .get<GameList[]>(this.baseUrl + 'games', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  // Leaderboard Requests
  getLeaderboard(
    page?,
    itemsPerPage?,
    gameParams?
  ): Observable<PaginatedResult<Leaderboard[]>> {
    const paginatedResult: PaginatedResult<Leaderboard[]> = new PaginatedResult<
    Leaderboard[]
    >();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (gameParams != null) {
      params = params.append('leagueId', gameParams.leagueId);
      params = params.append('winnerId', gameParams.winnerId);
    }

    return this.http
      .get<Leaderboard[]>(this.baseUrl + 'leaderboard', { observe: 'response', params })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  updateUser(model: any, file: File) {
    this.formData = new FormData();
    if (file) {
      this.formData.append('file', file, file.name);
    }
    Object.keys(model).forEach(key => this.formData.append(key, model[key]));
    return this.http.post(this.baseUrl + 'users/update', this.formData);
  }
}

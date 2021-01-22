import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EleaguesService } from '../_services/eleagues.service';
import { GameList } from '../_models/gameList';

@Injectable()
export class GameListResolver implements Resolve<GameList[]> {
  pageNumber = 1;
  pageSize = 6;

  constructor(
    private eleaguesService: EleaguesService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<GameList[]> {
    return this.eleaguesService.getGames(this.pageNumber, this.pageSize).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}

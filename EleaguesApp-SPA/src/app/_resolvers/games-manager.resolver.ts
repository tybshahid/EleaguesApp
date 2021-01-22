import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EleaguesService } from '../_services/eleagues.service';
import { GameManagerModel } from '../_models/GameManagerModel';

@Injectable()
export class GamesManagerResolver implements Resolve<GameManagerModel> {
  constructor(
    private eleaguesService: EleaguesService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<GameManagerModel> {
    if (route.data.public) {
      return this.eleaguesService.getGameManagerPublic().pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving data');
          this.router.navigate(['/']);
          return of(null);
        })
      );
    } else {
      return this.eleaguesService.getGameManager().pipe(
        catchError(error => {
          this.alertify.error('Problem retrieving data');
          this.router.navigate(['/']);
          return of(null);
        })
      );
    }
  }
}

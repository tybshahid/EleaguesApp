import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EleaguesService } from '../_services/eleagues.service';
import { Leaderboard } from '../_models/leaderboard';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class LeaderboardResolver implements Resolve<Leaderboard[]> {
  pageNumber = 1;
  pageSize = 10;
  gameParams: any = {};

  constructor(
    private eleaguesService: EleaguesService,
    private router: Router,
    private alertify: AlertifyService,
    private authService: AuthService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Leaderboard[]> {
    if (route.data.memberIdRoute) {
      this.gameParams.leagueId = 0;
      this.gameParams.winnerId = route.params[`id`];
      return this.eleaguesService
        .getLeaderboard(this.pageNumber, this.pageSize, this.gameParams)
        .pipe(
          catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/']);
            return of(null);
          })
        );
    } else if (route.data.memberIdToken) {
      this.gameParams.leagueId = 0;
      this.gameParams.winnerId = this.authService.decodedToken.nameid;
      return this.eleaguesService
        .getLeaderboard(this.pageNumber, this.pageSize, this.gameParams)
        .pipe(
          catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/']);
            return of(null);
          })
        );
    } else {
      return this.eleaguesService
        .getLeaderboard(this.pageNumber, this.pageSize)
        .pipe(
          catchError(error => {
            this.alertify.error('Problem retrieving data');
            this.router.navigate(['/']);
            return of(null);
          })
        );
    }
  }
}

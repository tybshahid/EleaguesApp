import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { League } from '../_models/league';
import { EleaguesService } from '../_services/eleagues.service';

@Injectable()
export class LeaguesResolver implements Resolve<League[]> {
  pageNumber = 1;
  pageSize = 6;

  constructor(
    private eleaguesService: EleaguesService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<League[]> {
    return this.eleaguesService.getLeagues(this.pageNumber, this.pageSize).pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}

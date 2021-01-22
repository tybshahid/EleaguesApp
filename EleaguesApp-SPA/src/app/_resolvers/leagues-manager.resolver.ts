import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { DropdownListItem } from '../_models/dropdownListItem';
import { EleaguesService } from '../_services/eleagues.service';

@Injectable()
export class LeaguesManagerResolver implements Resolve<DropdownListItem[]> {

  constructor(
    private eleaguesService: EleaguesService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<DropdownListItem[]> {
    return this.eleaguesService.getLeaguesList().pipe(
      catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/']);
        return of(null);
      })
    );
  }
}

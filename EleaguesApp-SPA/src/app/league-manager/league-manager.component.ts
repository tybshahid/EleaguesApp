import {
  Component,
  OnInit,
  ChangeDetectorRef,
  AfterContentChecked
} from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertifyService } from '../_services/alertify.service';
import { DropdownListItem } from '../_models/dropdownListItem';
import { EleaguesService } from '../_services/eleagues.service';
import { League } from '../_models/league';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-league-manager',
  templateUrl: './league-manager.component.html',
  styleUrls: ['./league-manager.component.css']
})
export class LeagueManagerComponent implements OnInit, AfterContentChecked {
  leaguesList: DropdownListItem[];
  league: League = new League();
  bsConfig: Partial<BsDatepickerConfig>;
  file: File;

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private changeDedectionRef: ChangeDetectorRef,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.leaguesList = data['leaguesList'];
    });

    this.bsConfig = {
      containerClass: 'theme-red',
      dateInputFormat: 'yyyy-MM-dd'
    };
  }

  ngAfterContentChecked(): void {
    this.changeDedectionRef.detectChanges();
  }

  onLeagueChanged(event) {
    this.eleaguesService
      .getLeague(event.target.value)
      .subscribe(league => (this.league = league));
  }

  onFileChanged(event) {
    this.file = event.target.files[0];
  }

  submit(leaguesForm: NgForm) {
    this.eleaguesService.saveLeague(this.league, this.file).subscribe(
      () => {
        this.alertify.success('Saved succesfully');
      },
      error => {
        this.alertify.error(error);
      },
      () => {
        leaguesForm.reset();
        this.eleaguesService
          .getLeaguesList().subscribe(leaguesList => this.leaguesList = leaguesList);
      }
    );
  }
}

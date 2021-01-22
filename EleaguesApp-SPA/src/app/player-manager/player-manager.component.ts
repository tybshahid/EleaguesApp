import { Component, OnInit } from '@angular/core';
import { DropdownListItem } from '../_models/dropdownListItem';
import { AlertifyService } from '../_services/alertify.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { EleaguesService } from '../_services/eleagues.service';

@Component({
  selector: 'app-player-manager',
  templateUrl: './player-manager.component.html',
  styleUrls: ['./player-manager.component.css']
})
export class PlayerManagerComponent implements OnInit {
  playersList: DropdownListItem[];
  model: any = {};
  file: File;

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    this.model.id = 0;
    this.model.password = '';
    this.model.isActive = true;

    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.playersList = data['dropdowns'].opponents;
    });
  }

  onFileChanged(event) {
    this.file = event.target.files[0];
  }

  submit(playersForm: NgForm) {
    this.eleaguesService.updateUser(this.model, this.file).subscribe(
      () => {
        this.alertify.success('Saved succesfully');
      },
      error => {
        this.alertify.error(error);
      },
      () => {
        playersForm.reset();
      }
    );
  }

}

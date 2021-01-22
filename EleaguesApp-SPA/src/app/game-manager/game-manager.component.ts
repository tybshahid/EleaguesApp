import { Component, OnInit } from '@angular/core';
import { EleaguesService } from '../_services/eleagues.service';
import { AlertifyService } from '../_services/alertify.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GameManagerModel } from '../_models/GameManagerModel';
import { Game } from '../_models/game';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-game-manager',
  templateUrl: './game-manager.component.html',
  styleUrls: ['./game-manager.component.css']
})
export class GameManagerComponent implements OnInit {
  gameManagerModel: GameManagerModel;
  game: Game = new Game();
  file: File;
  fileAttached = false;
  winner: string;
  opponent: string;

  constructor(
    private eleaguesService: EleaguesService,
    private alertify: AlertifyService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      // tslint:disable-next-line: no-string-literal
      this.gameManagerModel = data['gameManagerModel'];
    });

    this.game.stage = 'League';
    this.game.result = 'Standard';
    this.game.wickets = 10;
    this.game.fifties = 0;
    this.game.hundreds = 0;
    this.game.fiveWickets = 0;
    this.game.opponentWickets = 10;
    this.game.opponentFifties = 0;
    this.game.opponentHundreds = 0;
    this.game.opponentFiveWickets = 0;
  }

  onFileChanged(event) {
    this.file = event.target.files[0];
    if (this.file) {
      this.fileAttached = true;
    }
  }

  setWinner(event) {
    this.winner = this.gameManagerModel.winners.find(
      x => x.id === +event.target.value
    ).value;
  }

  setOpponent(event) {
    this.opponent = this.gameManagerModel.opponents.find(
      x => x.id === +event.target.value
    ).value;
  }

  submit(gameForm: NgForm) {
    this.eleaguesService.saveGame(this.game, this.file).subscribe(
      () => {
        this.alertify.success('Saved succesfully');
      },
      error => {
        this.alertify.error(error);
      },
      () => {
        this.router.navigate(['/games']);
        // this.fileAttached = false;
        // gameForm.reset();
      }
    );
  }
}

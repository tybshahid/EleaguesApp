import { Component, OnInit, Input, TemplateRef } from '@angular/core';
import { GameList } from 'src/app/_models/gameList';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
  selector: 'app-game-card',
  templateUrl: './game-card.component.html',
  styleUrls: ['./game-card.component.css']
})
export class GameCardComponent implements OnInit {
  @Input() game: GameList;
  modalRef: BsModalRef;

  constructor(private modalService: BsModalService) {}

  ngOnInit() {
    if (this.game.fileName) {
      this.game.fileName = '../Uploads/Games/' + this.game.fileName;
    }
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
}

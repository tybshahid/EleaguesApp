/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GameManagerComponent } from './game-manager.component';

describe('GameManagerComponent', () => {
  let component: GameManagerComponent;
  let fixture: ComponentFixture<GameManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

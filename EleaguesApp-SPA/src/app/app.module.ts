import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LeaguesComponent } from './leagues/leagues.component';
import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  BsDropdownModule,
  TabsModule,
  BsDatepickerModule,
  ButtonsModule,
  PaginationModule,
  CollapseModule,
  ModalModule
} from 'ngx-bootstrap';
import { JwtModule } from '@auth0/angular-jwt';
import { ErrorInterceptorProvider } from './_services/error.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';
import { RequestInterceptorProvider } from './_services/request.interceptor';
import { TimeAgoPipe } from 'time-ago-pipe';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberViewResolver } from './_resolvers/member-view.resolver';
import { LeagueManagerComponent } from './league-manager/league-manager.component';
import { LeaguesResolver } from './_resolvers/leagues.resolver';
import { LeaguesManagerResolver } from './_resolvers/leagues-manager.resolver';
import { GameManagerComponent } from './game-manager/game-manager.component';
import { GamesManagerResolver } from './_resolvers/games-manager.resolver';
import { NumericDirective } from './_directives/numeric.directive';
import { GameListComponent } from './games/game-list/game-list.component';
import { GameCardComponent } from './games/game-card/game-card.component';
import { GameListResolver } from './_resolvers/game-list.resolver';
import { LeaderboardResolver } from './_resolvers/leaderboard.resolver';
import { PlayerManagerComponent } from './player-manager/player-manager.component';

export function tokenGetter() {
  return localStorage.getItem('token');
}

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberCardComponent,
    MemberDetailComponent,
    LeaguesComponent,
    TimeAgoPipe,
    LeaderboardComponent,
    LeagueManagerComponent,
    GameManagerComponent,
    NumericDirective,
    GameListComponent,
    GameCardComponent,
    PlayerManagerComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    BsDropdownModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ButtonsModule.forRoot(),
    CollapseModule.forRoot(),
    PaginationModule.forRoot(),
    TabsModule.forRoot(),
    RouterModule.forRoot(appRoutes),
    NgxSpinnerModule,
    // NgxGalleryModule,
    // FileUploadModule,
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ['localhost:5000'],
        blacklistedRoutes: ['localhost:5000/api/auth']
      }
    })
  ],
  providers: [
    ErrorInterceptorProvider,
    RequestInterceptorProvider,
    LeaguesManagerResolver,
    GamesManagerResolver,
    GameListResolver,
    LeaguesResolver,
    MemberListResolver,
    MemberDetailResolver,
    MemberViewResolver,
    LeaderboardResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}

import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { LeaguesComponent } from './leagues/leagues.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberListResolver } from './_resolvers/member-list.resolver';
import { LeaderboardComponent } from './leaderboard/leaderboard.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberViewResolver } from './_resolvers/member-view.resolver';
import { LeagueManagerComponent } from './league-manager/league-manager.component';
import { LeaguesResolver } from './_resolvers/leagues.resolver';
import { LeaguesManagerResolver } from './_resolvers/leagues-manager.resolver';
import { GameManagerComponent } from './game-manager/game-manager.component';
import { GamesManagerResolver } from './_resolvers/games-manager.resolver';
import { GameListComponent } from './games/game-list/game-list.component';
import { GameListResolver } from './_resolvers/game-list.resolver';
import { LeaderboardResolver } from './_resolvers/leaderboard.resolver';
import { PlayerManagerComponent } from './player-manager/player-manager.component';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    children: [
      {
        path: 'leagues',
        component: LeaguesComponent,
        resolve: { leagues: LeaguesResolver }
      },
      {
        path: 'leagueManager',
        component: LeagueManagerComponent,
        canActivate: [AuthGuard],
        resolve: {
          leaguesList: LeaguesManagerResolver
        }
      },
      {
        path: 'members',
        component: MemberListComponent,
        resolve: { users: MemberListResolver }
      },
      {
        path: 'members/:id',
        component: MemberDetailComponent,
        data: { memberIdRoute: true },
        resolve: { user: MemberDetailResolver, leaderboard: LeaderboardResolver }
      },
      {
        path: 'member/view',
        component: MemberDetailComponent,
        data: { memberIdToken: true },
        resolve: { user: MemberViewResolver, leaderboard: LeaderboardResolver }
      },
      {
        path: 'games',
        component: GameListComponent,
        data: { public: true },
        resolve: { games: GameListResolver, dropdowns: GamesManagerResolver }
      },
      {
        path: 'gameManager',
        component: GameManagerComponent,
        canActivate: [AuthGuard],
        resolve: { gameManagerModel: GamesManagerResolver }
      },
      {
        path: 'leaderboard',
        component: LeaderboardComponent,
        data: { public: true },
        resolve: { leaderboard: LeaderboardResolver, dropdowns: GamesManagerResolver }
      },
      {
        path: 'playerManager',
        component: PlayerManagerComponent,
        canActivate: [AuthGuard],
        data: { public: true },
        resolve: { dropdowns: GamesManagerResolver }
      }
    ]
  },
  { path: '**', redirectTo: '', pathMatch: 'full' }
];

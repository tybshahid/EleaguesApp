import { DropdownListItem } from './dropdownListItem';

export interface GameList {
  id: number;
  league: string;
  winner: string;
  winnerPhotoUrl: string;
  opponent: string;
  stage: string;
  round: number;
  result: string;
  runs: number;
  wickets: number;
  overs: number;
  opponentRuns: number;
  opponentWickets: number;
  opponentOvers: number;
  fileName: string;
  createdBy: string;
  createdOn: any;
}

export class League {
  id: number;
  name: string;
  difficulty: string;
  format: string;
  rounds: number;
  teamFormat: string;
  matchTiming: string;
  powerplay: string;
  normalizeSkills: string;
  ropeSettings: string;
  pitchSettings: string;
  winPoints: number;
  lostPoints: number;
  participationPoints: number;
  startDate: Date;
  endDate: Date;
  isClosed: boolean;
  winner: string;
  isGroupLeague: boolean;
  isDefaultLeague: boolean;
  rulesFileName: string;
}

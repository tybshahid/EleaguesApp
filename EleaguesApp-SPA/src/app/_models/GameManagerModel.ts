import { DropdownListItem } from './dropdownListItem';

export interface GameManagerModel {
    leagues: DropdownListItem[];
    winners: DropdownListItem[];
    opponents: DropdownListItem[];
}

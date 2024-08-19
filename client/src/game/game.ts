import type { Player } from './player/player';

export class GameSettings {
	constructor(data?: GameSettings) {
		Object.assign(this, data);
	}

	MAX_TURN_DURATION = 10000;
	MAX_INVENTORY_SLOTS = 8;
	MAX_PLAYER_HEALTH = 8;
	MAX_PATRONS_IN_RIFLE = 8;

	ITEM_CHANNELING_TIME = 3000;
	ITEM_CHANNELING_CHECK_INTERVAL = 100;
	ITEMS_PER_PLAYER_COEF = 10;
	ITEMS_PER_ROUND = 3;
	ITEMS_PER_ROUND_INCREMENT = 1;
	FATIGUE_ROUND = 6;
	FATIGUE_ITEMS_TO_LOSE = 1;
	FATIGUE_DAMAGE_PER_ITEM = 1;
}

export enum GameStatus {
	PREPARING,
	IN_PROGRESS,
	PAUSED,
	FINISHED,
}

export interface Game {
	players: Player[];
	settings: GameSettings;
	current: Player | null;
	lastCaster: Player | null;
	status: GameStatus;

	round: number;
	turn: {
		thinker: number | null;
		time: number;
	};
}

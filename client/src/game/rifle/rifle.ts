import type { Modifier } from '@/game/modifier/modifier';
import type { Player } from '@/game/player/player';

export enum RifleStatus {
	NOT_LOADED,
	READY,
	SHOOTING,
	PULLING,
	LOADING,
}

export enum RiflePatron {
	UNKNOWN,
	CHARGED,
	BLANK,
}

export interface Rifle {
	target: Player | null;
	position: number;

	status: RifleStatus;
	modifiers: Modifier[];
	isMissing: boolean;

	patrons: {
		current: RiflePatron;
		charged: number;
		blank: number;
		sequence: boolean[];
	};
}

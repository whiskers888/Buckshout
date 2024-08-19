export enum ModifierTargetType {
	PLAYER,
	RIFLE,
	ITEM,
}

export enum ModifierState {
	PLAYER_STUNNED,
	PLAYER_DEAD,
	PLAYER_TURN_TIME_LIMITED,
	PLAYER_EVASION,
	PLAYER_ADDICTED,
	PLAYER_CHAINED,

	RIFLE_BONUS_DAMAGE,

	ITEM_INVISIBLE,
	ITEM_CANNOT_BE_STOLEN,
	ITEM_LOST_ON_TURN_ENDED,
}

export class Modifier {
	constructor(data: Modifier) {
		Object.assign(this, data);
	}

	id!: string;
	targetType!: ModifierTargetType;
	name!: string;
	description!: string;
	duration!: number;
	icon!: string;
	isBuff!: boolean;
	state!: ModifierState[];
}

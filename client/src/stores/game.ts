import { defineStore } from 'pinia';

interface GameSettings {}

export enum ItemBehavior {
	NO_TARGET,
	UNIT_TARGET,
	IMMEDIATE,
	CUSTOM,
}

export enum UnitTargetType {
	NONE,
	PLAYER,
	ITEM,
}

export enum UnitTargetTeam {
	NONE,
	ALLY,
	ENEMY,
	ANY,
}

export enum ItemType {
	DEFAULT,
	TRAP,
}

export enum ItemModifier {
	CANNOT_BE_STOLEN,
	INVISIBLE,
}

export class Item {
	constructor(data: Item) {
		Object.assign(this, data);
	}

	id = '';
	name = '';
	description = '';
	lore = '';
	model = '';

	behavior = [ItemBehavior.NO_TARGET];
	targetType = UnitTargetType.NONE;
	targetTeam = UnitTargetTeam.NONE;
	type = ItemType.DEFAULT;

	modifiers: ItemModifier[] = [];
}

export interface Player {
	id: string;
	health: number;
	name: string;
	inventory: Item[];
}

export interface Game {
	players: Player[];
	settings: GameSettings;
}

export const useGame = defineStore('game', {
	state: (): Game => ({
		players: [],
		settings: {},
	}),
	actions: {
		addPlayer(player: Player) {
			this.players.push(player);
		},
		removePlayer(player: Player) {
			this.players = this.players.filter(it => it.id != player.id);
		},
	},
});

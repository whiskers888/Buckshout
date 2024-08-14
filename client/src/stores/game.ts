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
	FRIENDLY,
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

const behaviorTooltip = {
	[ItemBehavior.NO_TARGET]: 'ненаправленная',
	[ItemBehavior.UNIT_TARGET]: 'направленная',
	[ItemBehavior.IMMEDIATE]: 'мгновенного применения',
	[ItemBehavior.CUSTOM]: '',
};

const itemTypeTooltip = {
	[ItemType.DEFAULT]: 'обычный',
	[ItemType.TRAP]: 'ловушка',
};

const targetTypeTooltip = {
	[UnitTargetType.NONE]: '',
	[UnitTargetType.ITEM]: 'предмет',
	[UnitTargetType.PLAYER]: 'игрок',
};

const targetTeamTooltip = {
	[UnitTargetTeam.NONE]: '',
	[UnitTargetTeam.FRIENDLY]: 'союзный',
	[UnitTargetTeam.ENEMY]: 'вражеский',
	[UnitTargetTeam.ANY]: 'любой',
};

export class Item {
	constructor(data: Item) {
		Object.assign(this, data);
	}

	get typeTooltip() {
		return itemTypeTooltip[this.type];
	}

	get behaviorTooltip() {
		const result = this.behavior.map(it => behaviorTooltip[it]);
		return result.join(',');
	}

	get targetTooltip() {
		return `${targetTeamTooltip[this.targetTeam]} ${targetTypeTooltip[this.targetType]}`;
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

export class Player {
	constructor(data: Player) {
		console.log(this);
		this.id = data.id;
		this.health = data.health;
		this.name = data.name;
		this.inventory = data.inventory.map(it => new Item(it));
	}
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
			this.players.push(new Player(player));
		},
		removePlayer(player: Player) {
			this.players = this.players.filter(it => it.id != player.id);
		},
	},
});

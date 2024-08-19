import { Action, connection } from '@/api';
import { defineStore } from 'pinia';
import { useRooms } from './room';
import { useRifle } from './rifle';

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

	addModifier(modifier: Modifier) {
		this.modifiers.push(new Modifier(modifier));
	}
	removeModifier(id: string) {
		this.modifiers = this.modifiers.filter(it => it.id !== id);
	}
	is(state: ModifierState) {
		for (const modifier of this.modifiers) {
			if (modifier.state.includes(state)) {
				return true;
			}
		}
		return false;
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

	modifiers: Modifier[] = [];
}

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

const PLAYER_COLORS = ['#c23a3a', '#2b63c2', '#2cc22b', '#a83ac2', '#cd9b3d'];

export class Player {
	constructor(data: Player, context: Game) {
		this.context = context;
		this.id = data.id;
		this.health = data.health;
		this.name = data.name;
		this.inventory = [];
		this.modifiers = [];

		this.team = data.team;
		this.avatar = data.avatar;

		data.modifiers.forEach(it => {
			this.addModifier(it);
		});

		this.color = PLAYER_COLORS[context.players.length];

		data.inventory.forEach(it => {
			this.addItem(it);
		});
	}
	get isOwnedByUser() {
		return this.id === connection.connectionId;
	}
	get isCurrent() {
		return this.id === this.context.current?.id;
	}
	get isAlive() {
		return this.health > 0;
	}

	addItem(item: Item) {
		if (this.inventory.length >= this.context.settings.MAX_INVENTORY_SLOTS) return;
		this.inventory.push(new Item(item));
	}
	hasItem(item: Item) {
		return !!this.inventory.find(it => it.id === item.id);
	}
	removeItem(item: Item) {
		this.inventory = this.inventory.filter(it => it.id !== item.id);
	}

	addModifier(modifier: Modifier) {
		this.modifiers.push(new Modifier(modifier));
	}
	removeModifier(id: string) {
		this.modifiers = this.modifiers.filter(it => it.id !== id);
	}
	is(state: ModifierState) {
		for (const modifier of this.modifiers) {
			if (modifier.state.includes(state)) {
				return true;
			}
		}
		return false;
	}

	damage(value: number) {
		this.health -= value;
		if (this.health < 0) {
			this.health = 0;
		}
	}
	heal(value: number) {
		this.health += value;
		if (this.health > this.context.settings.MAX_PLAYER_HEALTH) {
			this.health = this.context.settings.MAX_PLAYER_HEALTH;
		}
	}

	context: Game;
	id: string;
	health: number;
	name: string;
	team: string;
	inventory: Item[];
	modifiers: Modifier[];
	avatar: number;

	color: string;
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
	status: GameStatus;

	round: number;
	turn: {
		thinker: number | null;
		time: number;
	};
}

export const useGame = defineStore('game', {
	state: (): Game => ({
		players: [],
		current: null,
		status: GameStatus.PREPARING,
		settings: new GameSettings(),

		round: 1,
		turn: {
			thinker: null,
			time: 0,
		},
	}),
	getters: {
		playerById: state => {
			return (id: Player['id']) => state.players.find(it => it.id === id)!;
		},
		player: state => {
			return (target: Player) => state.players.find(it => it.id === target.id)!;
		},
	},
	actions: {
		clear() {
			const lastInterval = setInterval(() => {});
			for (let i = lastInterval; i > 0; i--) {
				clearInterval(i);
			}
			const lastTimeout = setTimeout(() => {});
			for (let i = lastTimeout; i > 0; i--) {
				clearTimeout(i);
			}
		},
		update(data: Game) {
			this.status = data.status;
			this.settings = new GameSettings(data.settings);
			this.players = [];
			data.players.forEach(it => {
				this.players.push(new Player(it, this.$state));
			});
		},
		start() {
			this.clear();
			this.turn.thinker = setInterval(() => {
				this.turn.time -= 1000;
			}, 1000);
		},

		addPlayer(player: Player) {
			this.players.push(new Player(player, this.$state));
		},
		removePlayer(player: Player) {
			this.players = this.players.filter(it => it.id != player.id);
		},

		setRound(value: number) {
			this.round = value;
		},

		applyDamage(target: Player, value: number) {
			this.player(target).damage(value);
		},
		applyHeal(target: Player, value: number) {
			this.player(target).heal(value);
		},

		startTurn(target: Player, time: number) {
			this.turn.time = time;
			const player = this.playerById(target.id);
			this.current = player;
		},

		addItem(target: Player, item: Item) {
			this.playerById(target.id)?.addItem(item);
		},
		removeItem(target: Player, item: Item) {
			this.playerById(target.id)?.removeItem(item);
		},

		applyModifier(modifier: Modifier, target: Player, _item?: Item) {
			switch (modifier.targetType) {
				case ModifierTargetType.PLAYER:
					this.playerById(target.id)?.addModifier(modifier);
					break;
				case ModifierTargetType.ITEM:
					this.playerById(target.id)
						?.inventory.find(it => it.id === _item!.id)!
						.addModifier(modifier);
					break;
				case ModifierTargetType.RIFLE:
					useRifle().addModifier(modifier);
					break;
			}
		},
		removeModifier(modifier: Modifier, target: Player, item: Item) {
			switch (modifier.targetType) {
				case ModifierTargetType.PLAYER:
					this.playerById(target.id)?.removeModifier(modifier.id);
					break;
				case ModifierTargetType.ITEM:
					this.playerById(target.id)
						?.inventory.find(it => it.id === item.id)!
						.removeModifier(modifier.id);
					break;
				case ModifierTargetType.RIFLE:
					useRifle().removeModifier(modifier.id);
					break;
			}
		},

		invokeStart() {
			const rooms = useRooms();
			connection.invoke(Action.START_GAME, rooms.current?.name);
		},
		invokeAim(target: Player) {
			const rooms = useRooms();
			connection.invoke(Action.TAKE_AIM, rooms.current?.name, target.id);
		},
		invokeShoot(target: Player) {
			const rooms = useRooms();
			connection.invoke(Action.SHOOT, rooms.current?.name, target.id);
		},
		invokeUseItem(item: Item, target: Player, targetItem?: Item) {
			const rooms = useRooms();
			connection.invoke(Action.USE_ITEM, rooms.current?.name, item.id, target.id, targetItem?.id || null);
		},
	},
});

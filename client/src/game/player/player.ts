import type { Game } from '@/game/game';
import { Item } from '@/game/item/item';
import { Modifier, type ModifierState } from '@/game/modifier/modifier';
import { useLocalPlayer } from '@/stores/player';

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
		const localPlayer = useLocalPlayer();
		return this.id === localPlayer.id;
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

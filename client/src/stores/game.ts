import { defineStore } from 'pinia';

import { useRifle } from './rifle';
import { useRooms } from './room';

import { Action, connection } from '@/api';
import { type Game, GameSettings, GameStatus } from '@/game/game';
import type { Item } from '@/game/item/item';
import { type Modifier, ModifierTargetType } from '@/game/modifier/modifier';
import { Player } from '@/game/player/player';

export const useGame = defineStore('game', {
	state: (): Game => ({
		players: [],
		current: null,
		lastCaster: null,
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

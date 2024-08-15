import { defineStore } from 'pinia';
import { useGame, type Player } from './game';

export interface Rifle {
	target: Player | null;
	position: number;
}

export const useRifle = defineStore('rifle', {
	state: (): Rifle => ({
		target: null,
		position: 0,
	}),
	getters: {
		offset: state => {
			return state.position + 'px';
		},
	},
	actions: {
		aim(target: Player) {
			const game = useGame();
			const player = game.players.find(it => it.id === target.id);

			if (!player) return;

			const index = game.players.findIndex(it => it === player);

			this.position = index * 350;
			this.target = player;
		},
	},
});

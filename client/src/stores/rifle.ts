import { defineStore } from 'pinia';

import { useGame } from './game';
import { useSound } from './sound';

import { Modifier, ModifierState } from '@/game/modifier/modifier';
import type { Player } from '@/game/player/player';
import { type Rifle, RiflePatron, RifleStatus } from '@/game/rifle/rifle';
import { shuffle } from '@/shared/utils/shuffle';

export const useRifle = defineStore('rifle', {
	state: (): Rifle => ({
		target: null,
		position: 0,
		status: RifleStatus.NOT_LOADED,
		isMissing: false,
		modifiers: [],
		patrons: {
			current: RiflePatron.UNKNOWN,
			charged: 0,
			blank: 0,
			sequence: [],
		},
	}),
	getters: {
		offset: state => {
			return state.position + 'px';
		},
	},
	actions: {
		aim(target: Player) {
			const game = useGame();

			const player = game.playersOrder.find(it => it.id === target.id);

			if (!player) return;

			const index = game.playersOrder.findIndex(it => it === player);

			this.position = index * 265;
			this.target = player;
		},
		shoot(isCharged: boolean, isMissing: boolean) {
			const sound = useSound();
			if (isCharged) sound.play('buckshot', 'rifle');
			else sound.play('blankshot', 'rifle');

			this.isMissing = isMissing;
			this.status = RifleStatus.SHOOTING;
			this.patrons.current = isCharged ? RiflePatron.CHARGED : RiflePatron.BLANK;

			if (this.target?.is(ModifierState.PLAYER_BLINDED)) this.target = null;
		},
		pull(isCharged: boolean) {
			const sound = useSound();
			setTimeout(() => {
				sound.play('pull', 'rifle');
				this.status = RifleStatus.PULLING;
				this.patrons.current = isCharged ? RiflePatron.CHARGED : RiflePatron.BLANK;

				setTimeout(() => {
					this.status = RifleStatus.READY;
				}, 1500);
			}, 500);
		},
		check(isCharged: boolean) {
			this.patrons.current = isCharged ? RiflePatron.CHARGED : RiflePatron.BLANK;
		},
		load(patrons: number, charged: number) {
			const sound = useSound();

			setTimeout(() => {
				this.status = RifleStatus.LOADING;
				this.patrons.charged = charged;
				this.patrons.blank = patrons - charged;
				this.patrons.current = RiflePatron.UNKNOWN;

				this.patrons.sequence = [];

				this.patrons.sequence.push(...new Array(this.patrons.charged).fill(true));
				this.patrons.sequence.push(...new Array(this.patrons.blank).fill(false));

				this.patrons.sequence = shuffle(this.patrons.sequence);

				setTimeout(() => {
					const load = setInterval(() => {
						if (this.patrons.sequence.length) {
							sound.play('load', 'rifle');
							this.patrons.sequence.shift();
						} else {
							this.status = RifleStatus.READY;
							clearInterval(load);
						}
					}, 500);
				}, 5000);
			}, 2200);
		},

		addModifier(modifier: Modifier) {
			this.modifiers.push(new Modifier(modifier));
		},
		removeModifier(id: string) {
			this.modifiers = this.modifiers.filter(it => it.id !== id);
		},
		is(state: ModifierState) {
			for (const modifier of this.modifiers) {
				if (modifier.state.includes(state)) {
					return true;
				}
			}
			return false;
		},
	},
});

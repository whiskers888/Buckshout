import { defineStore } from 'pinia';
import { Modifier, ModifierState, useGame, type Player } from './game';

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

	patrons: {
		current: RiflePatron;
		charged: number;
		blank: number;
		sequence: boolean[];
	};
}

const buckshotSound = new Audio('/sounds/buckshot.wav');
const blankshotSound = new Audio('/sounds/blankshot.wav');
const pullSound = new Audio('/sounds/pull.wav');
const loadSound = new Audio('/sounds/load.wav');

const play = (sound: HTMLAudioElement) => {
	sound.currentTime = 0;
	sound.play();
};

export const useRifle = defineStore('rifle', {
	state: (): Rifle => ({
		target: null,
		position: 0,
		status: RifleStatus.NOT_LOADED,
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
			const player = game.players.find(it => it.id === target.id);

			if (!player) return;

			const index = game.players.findIndex(it => it === player);

			this.position = index * 350;
			this.target = player;
		},
		shoot(isBuckshot: boolean) {
			if (isBuckshot) play(buckshotSound);
			else play(blankshotSound);

			this.status = RifleStatus.SHOOTING;
			this.patrons.current = isBuckshot ? RiflePatron.CHARGED : RiflePatron.BLANK;
		},
		pull(isBuckshot: boolean) {
			setTimeout(() => {
				play(pullSound);
				this.status = RifleStatus.PULLING;
				this.patrons.current = isBuckshot ? RiflePatron.CHARGED : RiflePatron.BLANK;

				setTimeout(() => {
					this.status = RifleStatus.READY;
				}, 1500);
			}, 500);
		},
		load(patrons: number, charged: number) {
			setTimeout(() => {
				this.status = RifleStatus.LOADING;
				this.patrons.charged = charged;
				this.patrons.blank = patrons - charged;
				this.patrons.current = RiflePatron.UNKNOWN;

				this.patrons.sequence = [];

				this.patrons.sequence.push(...new Array(this.patrons.charged).fill(true));
				this.patrons.sequence.push(...new Array(this.patrons.blank).fill(false));

				const shuffle = (array: any[]) => {
					for (let i = array.length - 1; i > 0; i--) {
						const j = Math.floor(Math.random() * (i + 1));
						const temp = array[i];
						array[i] = array[j];
						array[j] = temp;
					}
				};
				shuffle(this.patrons.sequence);

				setTimeout(() => {
					const load = setInterval(() => {
						if (this.patrons.sequence.length) {
							play(loadSound);
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

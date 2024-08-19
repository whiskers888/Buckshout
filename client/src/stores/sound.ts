import { defineStore } from 'pinia';

export const useSound = defineStore('sound', {
	state: () => ({
		cache: {} as Record<string, HTMLAudioElement>,
	}),
	actions: {
		play(sound: string, folder: string = 'items') {
			if (!sound) return;
			if (!this.cache[sound]) {
				this.cache[sound] = new Audio(`/sounds/${folder ? `${folder}/` : ''}${sound}.wav`);
			}
			this.cache[sound].currentTime = 0;
			this.cache[sound].play();
		},
	},
});

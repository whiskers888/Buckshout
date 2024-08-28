import { defineStore } from 'pinia';

import { useGame } from './game';

import { type Item, ItemType } from '@/game/item/item';
import type { Player } from '@/game/player/player';

export interface ChainItem {
	initiator: Player;
	item: Item;
	target?: Player;
	targetItem?: Item;
	time: number;
}

export const useItems = defineStore('items', {
	state: () => ({
		chain: [] as ChainItem[],
		timer: null as number | null,
	}),
	actions: {
		add(initiator: Player, item: Item, target?: Player, targetItem?: Item) {
			if (this.timer) clearInterval(this.timer);
			const game = useGame();
			const chainItem = {
				initiator,
				item,
				target: item.type == ItemType.TRAP ? undefined : target,
				targetItem: item.type == ItemType.TRAP ? undefined : targetItem,
				time: game.settings.ITEM_CHANNELING_TIME,
			};
			this.chain.push(chainItem);
			this.timer = setInterval(() => {
				chainItem.time -= 1000;
			}, 1000);
		},
		clear() {
			this.chain = [];
			if (this.timer) clearInterval(this.timer);
		},
	},
});

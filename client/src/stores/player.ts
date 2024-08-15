import { connection } from '@/api';
import { defineStore } from 'pinia';
import { ItemBehavior, useGame, type Item } from './game';

export enum PlayerActivity {
	WAITING,
	DECIDES_CANCEL,
	JUST_TURN,
	USING_ITEM,
}

interface LocalPlayer {
	id: string;
	activity: PlayerActivity;
}

export const usePlayer = defineStore('player', {
	state: (): LocalPlayer => ({
		id: connection.connectionId!,
		activity: PlayerActivity.WAITING,
	}),
	getters: {
		isCurrent: state => {
			return state?.id === connection.connectionId;
		},
	},
	actions: {
		beginUse(item: Item) {
			const game = useGame();

			if (!this.isCurrent && this.activity !== PlayerActivity.DECIDES_CANCEL) return;
			if (this.activity === PlayerActivity.DECIDES_CANCEL) {
				if (!item.behavior.includes(ItemBehavior.CUSTOM)) return;
			}

			if (item.behavior.includes(ItemBehavior.NO_TARGET)) {
				game.invokeUseItem(item, game.current!);
			} else if (item.behavior.includes(ItemBehavior.UNIT_TARGET)) {
				//
			}
		},
	},
});

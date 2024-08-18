import { defineStore } from 'pinia';
import { ItemBehavior, ModifierState, Player, UnitTargetTeam, UnitTargetType, useGame, type Item } from './game';

export enum PlayerActivity {
	WAITING,
	DECIDES_CANCEL,
	JUST_TURN,
	USING_ITEM,
}

interface LocalPlayer {
	id: string;
	activity: PlayerActivity;
	itemToUse: Item | null;
}

function canTargetAnything(state: LocalPlayer) {
	return state.activity === PlayerActivity.USING_ITEM;
}

function canTargetTeam(state: LocalPlayer, target: Player) {
	const game = useGame();
	const player = game.playerById(state.id);
	if (state.itemToUse?.targetTeam === UnitTargetTeam.NONE) return false;
	if (state.itemToUse?.targetTeam === UnitTargetTeam.ENEMY && target.team === player.team) return false;
	if (state.itemToUse?.targetTeam === UnitTargetTeam.FRIENDLY && target.team !== player.team) return false;
	return canTargetAnything(state);
}

export const usePlayer = defineStore('player', {
	state: (): LocalPlayer => ({
		id: '',
		activity: PlayerActivity.WAITING,
		itemToUse: null,
	}),
	getters: {
		isCurrent: state => {
			const game = useGame();
			return game.current?.id === state.id;
		},
		canTargetPlayer: state => (target: Player) => {
			if (state.itemToUse?.targetType !== UnitTargetType.PLAYER) return false;
			return canTargetTeam(state, target);
		},
		canTargetItem: state => (target: Player, item: Item) => {
			if (state.itemToUse?.targetType !== UnitTargetType.ITEM) return false;
			if (item.is(ModifierState.ITEM_INVISIBLE)) return false;
			return canTargetTeam(state, target);
		},
	},
	actions: {
		setConnectionId(id: string) {
			this.id = id;
		},
		setActivity(act: PlayerActivity) {
			if (act === PlayerActivity.WAITING && this.isCurrent) {
				this.activity = PlayerActivity.JUST_TURN;
			} else this.activity = act;
		},
		cancelUse() {
			this.setActivity(PlayerActivity.WAITING);
			this.itemToUse = null;
		},
		beginUse(item: Item) {
			const game = useGame();

			if (!game.playerById(this.id!).hasItem(item)) return;
			if (this.activity === PlayerActivity.DECIDES_CANCEL) {
				if (item.behavior.includes(ItemBehavior.CUSTOM)) game.invokeUseItem(item, game.current!);
				else return;
			}
			if (!this.isCurrent || item.behavior.includes(ItemBehavior.CUSTOM)) return;
			if (item.behavior.includes(ItemBehavior.NO_TARGET)) {
				game.invokeUseItem(item, game.current!);
			} else if (item.behavior.includes(ItemBehavior.UNIT_TARGET)) {
				this.setActivity(PlayerActivity.USING_ITEM);
				this.itemToUse = item;
			}
		},
		selectAsTarget(player: Player, item?: Item) {
			const game = useGame();

			if (this.canTargetPlayer(player)) game.invokeUseItem(this.itemToUse!, player);

			if (item && this.canTargetItem(player, item)) game.invokeUseItem(this.itemToUse!, player, item);
		},
	},
});

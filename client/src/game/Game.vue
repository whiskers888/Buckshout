<script setup lang="ts">
import Player from './player/Player.vue';
import Rifle from './rifle/Rifle.vue';
import { useGame } from '@/stores/game';

import Toolbar from './Toolbar.vue';

const game = useGame();
</script>

<template>
	<v-container class="game-container actions-disabled">
		<Toolbar />
		<div class="game-field">
			<div class="players">
				<Player
					v-for="player in game.players"
					:key="player.id"
					:player="player"
				/>
			</div>
			<Rifle />
		</div>
	</v-container>
</template>

<style scoped>
.game-container {
	user-select: none;
}

/* .actions-disabled {
	pointer-events: none;
} */

.flex {
	display: flex;
	align-items: center;
	gap: 6px;
	padding: 10px;
}

.game-field {
	display: flex;
	max-width: 100%;
	gap: 20px;
	padding-top: 150px;
}

.players {
	max-width: 60%;
	display: flex;
	flex-direction: column;
}
</style>

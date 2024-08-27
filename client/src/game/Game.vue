<script setup lang="ts">
import Player from './player/Player.vue';
import Rifle from './rifle/Rifle.vue';
import Toolbar from './Toolbar.vue';

import { useGame } from '@/stores/game';
const game = useGame();
</script>

<template>
	<v-container class="game-container actions-disabled">
		<Toolbar />
		<div class="game-field">
			<div class="players">
				<Player
					v-for="player in game.playersOrder"
					:key="player.id"
					:player="player"
				/>
			</div>
			<Rifle />
		</div>
		<div
			v-if="game.trap"
			class="trap-effect"
		>
			<div
				class="trap-info"
				:style="{
					borderColor: game.trap.initiator.color,
				}"
			>
				<h3 class="trap-header">Ловушка!</h3>
				<div class="trap-items">
					<div class="trap-item">
						<img
							:src="`https://api.multiavatar.com/${game.trap.initiator.avatar}.png`"
							alt="?"
							style="border-radius: 50%"
						/>
						<div>{{ game.trap.initiator.name }}</div>
					</div>
					<div class="trap-item">
						<img :src="`/models/items/${game.trap.item.model}.png`" />
						<div>{{ game.trap.item.name }}</div>
					</div>
				</div>
			</div>
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

.trap-effect {
	position: absolute;
	top: 0;
	left: 0;
	right: 0;
	bottom: 0;
	display: flex;
	justify-content: center;
	align-items: center;
	z-index: 100;
}

.trap-info {
	width: 400px;
	max-width: 90%;
	border: 1px solid;
	border-radius: 10px;
	box-shadow: 0px 0px 20px 20px rgb(var(--v-theme-background));
	padding: 10px 10px 20px;
	background: rgb(var(--v-theme-background));
}

.trap-info h3 {
	text-align: center;
	font-size: 30px;
	margin-bottom: 20px;
}

.trap-items {
	display: flex;
	justify-content: space-around;
}

.trap-item {
	display: flex;
	flex-direction: column;
	justify-content: center;
	align-items: center;
	gap: 10px;
}

.trap-info img {
	width: 100px;
}
</style>

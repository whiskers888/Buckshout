<script setup lang="ts">
import { GameStatus, useGame } from '@/stores/game';
import Player from './Player.vue';
import Rifle from './Rifle.vue';

const game = useGame();
</script>

<template>
	<v-container class="fill-height">
		<div>
			<div>
				<v-btn
					v-if="game.status === GameStatus.PREPARING"
					text="Начать игру"
					@click="game.invokeStart"
				/>
				<div v-else>
					<div style="display: flex; gap: 6px">
						<div
							class="current-player-color"
							:style="{ background: game.current?.color }"
						></div>
						<p>
							{{ game.current?.isOwnedByUser ? 'Ваша очередь!' : `Очередь: ${game.current?.name}` }},
							Осталось {{ game.turn.time / 1000 }} сек.
						</p>
					</div>
				</div>
			</div>
			<div style="display: flex">
				<div>
					<Player
						v-for="player in game.players"
						:key="player.id"
						:player="player"
					/>
				</div>
				<Rifle />
			</div>
		</div>
	</v-container>
</template>

<style scoped>
.current-player-color {
	width: 20px;
	height: 20px;
	border-radius: 50%;
}
</style>

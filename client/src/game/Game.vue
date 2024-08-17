<script setup lang="ts">
import { GameStatus, useGame } from '@/stores/game';
import Player from './Player.vue';
import Rifle from './Rifle.vue';
import { useRifle } from '@/stores/rifle';
import { useRooms } from '@/stores/room';
import { PlayerActivity, usePlayer } from '@/stores/player';

const rooms = useRooms();
const game = useGame();
const rifle = useRifle();
const localPlayer = usePlayer();
</script>

<template>
	<v-container>
		<div class="game-toolbar">
			<v-btn
				color="red"
				class="mr-4"
				text="Выйти"
				@click="rooms.leave"
			/>
			<v-btn
				v-if="game.status === GameStatus.PREPARING"
				text="Начать игру"
				@click="game.invokeStart"
			/>
			<div
				v-else
				style="width: 100%"
			>
				<div style="display: flex; justify-content: space-between">
					<div class="flex">
						<div
							class="current-player-color"
							:style="{ background: game.current?.color }"
						/>
						<p>
							{{ game.current?.isOwnedByUser ? 'Ваша очередь!' : `Очередь: ${game.current?.name}` }},
							Осталось {{ game.turn.time / 1000 }} сек.
						</p>
					</div>
					<div>
						<v-tooltip
							location="bottom"
							text="Патроны заряжаются в случайном порядке, их кол-во скрыто во время раунда!"
						>
							<template v-slot:activator="{ props }">
								<div
									class="flex"
									v-bind="props"
								>
									<v-icon
										icon="mdi-bullet"
										v-for="patron in game.settings.MAX_PATRONS_IN_RIFLE"
										:color="
											patron <= rifle.patrons.sequence.length
												? rifle.patrons.sequence[patron - 1]
													? '#b60202'
													: '#4949a3'
												: '#3b3b3b'
										"
										:key="patron"
									/>
								</div>
							</template>
						</v-tooltip>
					</div>
				</div>
				<div
					v-if="localPlayer.activity === PlayerActivity.USING_ITEM"
					class="flex"
				>
					<p>
						Выберите цель ({{ localPlayer.itemToUse?.targetTooltip }}), чтобы применить [{{
							localPlayer.itemToUse?.name
						}}].
					</p>
					<v-btn
						text="Отмена"
						@click="localPlayer.cancelUse"
					/>
				</div>
			</div>
		</div>
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
.flex {
	display: flex;
	align-items: center;
	gap: 6px;
	padding: 10px;
}

.current-player-color {
	width: 20px;
	height: 20px;
	border-radius: 50%;
}

.game-toolbar {
	display: flex;
	position: fixed;
	align-items: center;
	top: 0;
	left: 0;
	right: 0;
	justify-content: space-between;
	padding: 4px 20px;
	background: rgb(var(--v-theme-background));
	z-index: 1000;
}

.game-field {
	display: flex;
	max-width: 100%;
	gap: 20px;
	padding-top: 100px;
}

.players {
	max-width: 60%;
	display: flex;
	flex-direction: column;
}
</style>

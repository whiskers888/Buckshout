<script setup lang="ts">
import { GameStatus, useGame } from '@/stores/game';
import Player from './Player.vue';
import Rifle from './Rifle.vue';
import { useRifle } from '@/stores/rifle';
import { useRooms } from '@/stores/room';

const rooms = useRooms();
const game = useGame();
const rifle = useRifle();
</script>

<template>
	<v-container class="fill-height">
		<div>
			<div>
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
				<div v-else>
					<div style="display: flex; justify-content: space-between; padding: 10px">
						<div style="display: flex; align-items: center; gap: 6px">
							<div
								class="current-player-color"
								:style="{ background: game.current?.color }"
							></div>
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
									<div v-bind="props">
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

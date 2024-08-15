<script setup lang="ts">
import { useGame, type Player } from '@/stores/game';
import Item from './items/Item.vue';
import { RifleStatus, useRifle } from '@/stores/rifle';

const game = useGame();
const rifle = useRifle();

const { player } = defineProps<{
	player: Player;
}>();
</script>

<template>
	<div
		:class="[
			'player',
			{
				me: player.isOwnedByUser,
				current: player.isCurrent,
			},
		]"
		:style="{ borderColor: player.color }"
	>
		<div class="player-status">
			<div class="player-info">
				<div class="player-avatar">?</div>
				<div>
					<h3>{{ player.name }} {{ player.isOwnedByUser ? '(Вы)' : '' }}</h3>

					<v-tooltip
						location="right"
						:text="`Здоровье игрока: ${player.health}/${game.settings.MAX_PLAYER_HEALTH} ед.`"
					>
						<template v-slot:activator="{ props }">
							<div v-bind="props">
								<span
									v-for="hp in game.settings.MAX_PLAYER_HEALTH"
									:style="{ color: hp <= player.health ? '#f00' : '#000' }"
									:key="hp"
								>
									❤
								</span>
							</div>
						</template>
					</v-tooltip>
				</div>
			</div>
			<div
				v-if="game.isYourTurn"
				class="player-actions"
			>
				<v-tooltip
					v-if="rifle.target !== player"
					location="bottom"
					text="Прицелиться"
				>
					<template v-slot:activator="{ props }">
						<v-btn
							v-bind="props"
							icon="mdi-target"
							:disabled="rifle.status !== RifleStatus.READY"
							@click="game.invokeAim(player)"
						/>
					</template>
				</v-tooltip>
				<v-tooltip
					v-else
					location="bottom"
					text="Выстрелить"
				>
					<template v-slot:activator="{ props }">
						<v-btn
							v-bind="props"
							icon="mdi-bullet"
							:disabled="rifle.status !== RifleStatus.READY"
							@click="game.invokeShoot(player)"
						/>
					</template>
				</v-tooltip>
			</div>
		</div>
		<div class="player-inventory">
			<Item
				v-for="item in 8"
				:key="player.inventory[item - 1]?.id ?? item"
				:item="player.inventory[item - 1]"
			/>
		</div>
	</div>
</template>

<style scoped>
.player {
	display: flex;
	flex-direction: column;
	justify-content: space-between;
	width: 500px;
	padding: 20px;
	border: 3px solid;
	margin: 10px;
	border-radius: 10px;
	height: 350px;
}

.current {
	background: #a7a7a73b;
}

.me {
	background: #00ff0014;
}

.player-status {
	display: flex;
	justify-content: space-between;
	align-items: center;
}

.player-info {
	display: flex;
	align-items: center;
	cursor: default;
	gap: 10px;
}

.player-avatar {
	width: 50px;
	height: 50px;
	background: #000;
	border-radius: 10px;
	display: flex;
	justify-content: center;
	align-items: center;
	font-size: 20px;
	font-weight: bold;
	color: #fff;
}

.player-inventory {
	display: grid;
	grid-template-rows: repeat(2, 1fr);
	grid-template-columns: repeat(4, 1fr);
	grid-gap: 2px;
	padding: 10px 0;
}
</style>

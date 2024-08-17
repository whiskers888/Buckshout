<script setup lang="ts">
import { useGame, type Player } from '@/stores/game';
import Item from './items/Item.vue';
import { RifleStatus, useRifle } from '@/stores/rifle';
import { usePlayer } from '@/stores/player';

const game = useGame();
const localPlayer = usePlayer();
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
				target: localPlayer.canTargetPlayer(player),
			},
		]"
		:style="{ borderColor: player.color }"
	>
		<div class="player-status">
			<div class="player-info">
				<div class="player-avatar">
					<img
						:src="`https://api.multiavatar.com/${player.avatar}.png`"
						alt="?"
					/>
				</div>
				<div style="max-width: 100%">
					<div class="player-name">{{ player.isOwnedByUser ? '[Вы]' : '' }} {{ player.name }}</div>

					<v-tooltip
						location="right"
						:text="`Здоровье игрока: ${player.health}/${game.settings.MAX_PLAYER_HEALTH} ед.`"
					>
						<template v-slot:activator="{ props }">
							<div v-bind="props">
								<v-icon
									v-for="hp in game.settings.MAX_PLAYER_HEALTH"
									icon="mdi-heart"
									:color="hp <= player.health ? '#f00' : '#000'"
									:key="hp"
								/>
							</div>
						</template>
					</v-tooltip>
				</div>
			</div>
			<div
				v-if="localPlayer.isCurrent"
				class="player-actions"
			>
				<template v-if="localPlayer.canTargetPlayer(player)">
					<v-tooltip
						location="bottom"
						:text="`Использовать [${localPlayer.itemToUse?.name}]`"
					>
						<template v-slot:activator="{ props }">
							<v-btn
								v-bind="props"
								icon="mdi-teddy-bear"
								@click="localPlayer.selectAsTarget(player)"
							/>
						</template>
					</v-tooltip>
				</template>
				<template v-else>
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
				</template>
			</div>
		</div>
		<div class="player-modifiers">
			<b>Статус:</b>
			<v-tooltip
				v-for="modifier in player.modifiers"
				:key="modifier.name"
				location="right"
			>
				<template v-slot:activator="{ props }">
					<v-icon
						v-bind="props"
						:style="{
							border: `1px solid ${modifier.isBuff ? '#0f0' : '#f00'}`,
							borderRadius: '50%',
							padding: '12px',
							fontSize: '18px',
						}"
						:icon="`mdi-${modifier.icon}`"
					/>
				</template>
				<div class="modifier-tooltip">
					<h3>{{ modifier.name }}</h3>
					<p class="item-tooltip-description">{{ modifier.description }}</p>
				</div>
			</v-tooltip>
		</div>
		<div class="player-inventory">
			<Item
				v-for="item in game.settings.MAX_INVENTORY_SLOTS"
				:key="player.inventory[item - 1]?.id ?? item"
				:owner="player"
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
	width: 409px;
	padding: 20px;
	border: 3px solid;
	margin: 10px;
	border-radius: 10px;
	max-width: 100%;
	position: relative;
	overflow: hidden;
}

.player::before {
	content: '';
	width: 60px;
	height: 60px;
	display: block;
	position: absolute;
	top: -30px;
	left: -40px;
	rotate: 40deg;
	background: v-bind('player.color');
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
	max-width: 70%;
}

.player-avatar {
	width: 50px;
	min-width: 50px;
	height: 50px;
	/* background: #000; */
	/* border-radius: 10px; */
	display: flex;
	justify-content: center;
	align-items: center;
	font-size: 20px;
	font-weight: bold;
	color: #fff;
}

.player-avatar img {
	width: 100%;
}

.player-name {
	white-space: nowrap;
	overflow: hidden;
	text-overflow: ellipsis;
	font-weight: bold;
	font-size: 16px;
}

.player-modifiers {
	display: flex;
	gap: 6px;
	padding: 10px;
	border: 1px solid;
	border-radius: 10px;
	margin-top: 10px;
}

.player-inventory {
	display: grid;
	grid-template-rows: repeat(2, 1fr);
	grid-template-columns: repeat(4, 1fr);
	grid-gap: 2px;
	padding: 10px 0;
}
</style>

<script setup lang="ts">
import type { Player } from './player';

import Item from '@/game/item/Item.vue';
import { ModifierState } from '@/game/modifier/modifier';
import { RifleStatus } from '@/game/rifle/rifle';
import { useGame } from '@/stores/game';
import { PlayerActivity, useLocalPlayer } from '@/stores/player';
import { useRifle } from '@/stores/rifle';

const game = useGame();
const localPlayer = useLocalPlayer();
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
				dead: player.is(ModifierState.PLAYER_DEAD),
			},
		]"
		:style="{ borderColor: player.color }"
	>
		<div class="player-status">
			<div class="player-info">
				<div class="player-avatar">
					<img
						:id="`Player-${player.id}`"
						:src="`https://api.multiavatar.com/${player.avatar}.png`"
						alt="?"
					/>
				</div>
				<div style="max-width: 100%">
					<div class="player-name">
						<v-tooltip
							location="right"
							text="Это Вы, единственный и неповторимый!"
						>
							<template v-slot:activator="{ props }">
								<v-icon
									v-if="player.isOwnedByUser"
									v-bind="props"
									:color="player.color"
									icon="mdi-star"
								/>
							</template>
						</v-tooltip>

						<v-tooltip
							location="right"
							:text="player.isOwnedByUser ? 'И сейчас Ваш ход!' : 'Этот игрок сейчас ходит.'"
						>
							<template v-slot:activator="{ props }">
								<v-icon
									v-if="player.isCurrent"
									v-bind="props"
									:color="player.color"
									icon="mdi-shoe-sneaker"
								/>
							</template>
						</v-tooltip>

						<span>[{{ player.team }}] {{ player.name }}</span>
					</div>

					<v-tooltip
						location="right"
						:text="`Здоровье игрока: ${player.health}/${game.settings.MAX_PLAYER_HEALTH} ед.`"
					>
						<template v-slot:activator="{ props }">
							<div
								v-bind="props"
								style="display: flex"
							>
								<div
									v-for="hp in game.settings.MAX_PLAYER_HEALTH"
									:key="hp"
									:class="{
										shine: hp <= player.health && hp > player.prevHealth,
									}"
									style="position: relative; border-radius: 50%"
								>
									<v-icon
										icon="mdi-heart"
										:class="{
											damage: hp > player.health && hp <= player.prevHealth,
										}"
										:color="hp <= player.health ? '#f00' : '#000'"
									/>
								</div>
							</div>
						</template>
					</v-tooltip>
				</div>
			</div>
			<div
				v-if="
					localPlayer.isCurrent &&
					!player.is(ModifierState.PLAYER_DEAD) &&
					localPlayer.activity !== PlayerActivity.DECIDES_CANCEL
				"
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
	width: 400px;
	padding: 10px;
	border: 3px solid;
	margin: 8px;
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

.player.dead {
	opacity: 0.3;
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
	border-radius: 50%;
}

.player-name {
	white-space: nowrap;
	overflow: hidden;
	text-overflow: ellipsis;
	font-weight: bold;
	font-size: 16px;
	display: flex;
	align-items: center;
	gap: 4px;
}

.player-modifiers {
	display: flex;
	gap: 6px;
	padding: 6px;
	border: 1px solid;
	border-radius: 10px;
	margin-top: 10px;
}

.player-inventory {
	display: grid;
	grid-template-rows: repeat(2, 1fr);
	grid-template-columns: repeat(4, 1fr);
	grid-gap: 2px;
	margin-top: 6px;
}

.damage {
	animation: pulse-hp 0.5s linear infinite !important;
}

@keyframes pulse-hp {
	0% {
		color: #f00;
	}
	50% {
		opacity: #fff;
	}
	100% {
		opacity: #f00;
	}
}

.shine::before {
	background: linear-gradient(to right, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.3) 100%);
	content: '';
	display: block;
	top: 15%;
	height: 50%;
	left: -50%;
	position: absolute;
	transform: skewX(-60deg);
	width: 80%;
	z-index: 2;
	animation: shine 0.85s infinite linear;
}
</style>

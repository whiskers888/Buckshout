<script setup lang="ts">
import { ref } from 'vue';

import { GameStatus } from './game';
import { ModifierState } from './modifier/modifier';

import { useGame } from '@/stores/game';
import { useItems } from '@/stores/items';
import { PlayerActivity, useLocalPlayer } from '@/stores/player';
import { useRooms } from '@/stores/room';

const rooms = useRooms();
const game = useGame();
const localPlayer = useLocalPlayer();
const items = useItems();

function onFocusChanged(value: boolean) {
	if (!value) game.invokeSetTeam(game.playerById(localPlayer.id).team);
}
</script>

<template>
	<div class="game-toolbar">
		<v-btn
			color="red"
			class="mr-4"
			text="Выйти"
			@click="rooms.leave"
		/>
		<div
			v-if="game.status === GameStatus.PREPARING"
			style="display: flex; align-items: center; gap: 20px"
		>
			<v-text-field
				width="200"
				v-model:model-value="game.playerById(localPlayer.id).team"
				label="Команда"
				hide-details
				prepend-inner-icon="mdi-account-multiple"
				@update:focused="onFocusChanged"
			/>
			<v-btn
				text="Начать игру"
				@click="game.invokeStart"
			/>
		</div>
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
						{{ localPlayer.isCurrent ? 'Ваша очередь!' : `Очередь: ${game.current?.name}` }}, Осталось
						{{ game.turn.time / 1000 }} сек.
					</p>
				</div>
			</div>
			<div class="flex">
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
				<div class="chain-items">
					<div
						v-for="chainItem in items.chain"
						:key="chainItem.item.id"
						class="chain-item"
						:style="{ borderColor: chainItem.initiator.color }"
					>
						<div class="chain-player">
							<img
								:src="`https://api.multiavatar.com/${chainItem.initiator.avatar}.png`"
								alt="?"
							/>
							<div class="player-name">{{ chainItem.initiator.name }}</div>
						</div>
						<div class="chain-affected-item">
							<img
								:src="
									!chainItem.item.is(ModifierState.ITEM_INVISIBLE)
										? `/models/items/${chainItem.item.model}.png`
										: '/models/items/unknown.png'
								"
							/>
							<div class="clock">
								{{ chainItem.time / 1000 }}
							</div>
						</div>
						<div
							v-if="chainItem.targetItem"
							class="chain-affected-item"
						>
							<img
								:src="
									!chainItem.targetItem.is(ModifierState.ITEM_INVISIBLE)
										? `/models/items/${chainItem.targetItem.model}.png`
										: '/models/items/unknown.png'
								"
							/>
						</div>
						<div
							v-if="chainItem.target"
							class="chain-player"
						>
							<img
								:src="`https://api.multiavatar.com/${chainItem.target.avatar}.png`"
								alt="?"
							/>
							<div class="player-name">{{ chainItem.target.name }}</div>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
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

.chain-item {
	display: flex;
	gap: 8px;
	border: 2px solid;
	border-radius: 8px;
	padding: 4px 8px;
}

.chain-items {
	display: flex;
	gap: 20px;
}

.chain-item img {
	height: 30px;
	border-radius: 50%;
}

.chain-item p {
	max-width: 100px;
}

.chain-player,
.chain-affected-item {
	display: flex;
	flex-direction: column;
	align-items: center;
	justify-content: space-between;
	gap: 2px;
}

.player-name {
	max-width: 70px;
	overflow: hidden;
	white-space: nowrap;
	text-overflow: ellipsis;
}

.clock {
	border: 1px solid;
	border-radius: 50%;
	padding: 2px;
	font-size: 12px;
	width: 24px;
	height: 24px;
	display: flex;
	justify-content: center;
	align-items: center;
}
</style>

<script setup lang="ts">
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
		<div style="display: flex; justify-content: center">
			<v-btn
				color="red"
				class="mr-4"
				text="Выйти"
				@click="rooms.leave"
			/>
			<v-tooltip location="right">
				<template v-slot:activator="{ props }">
					<v-icon
						v-bind="props"
						class="item-info"
						icon="mdi-information"
					/>
				</template>
				<div class="general-info">
					<h3>Общая информация об игре</h3>
					<b>Патроны:</b>
					<p>
						-
						<v-icon
							icon="mdi-bullet"
							color="#f00"
						/>
						заряженный - наносит 1 ед. урона, + эффекты предметов, если они были применены.
					</p>
					<p>
						-
						<v-icon
							icon="mdi-bullet"
							color="#00f"
						/>
						холостой - не наносит урон.
					</p>

					<b>Игроки:</b>
					<p>
						-
						<v-icon
							icon="mdi-heart"
							color="#f00"
						/>
						имеют {{ game.settings.INIT_PLAYER_HEALTH }} ед. здоровья в начале игры.
					</p>
					<p>- максимальное кол-во здоровья {{ game.settings.MAX_PLAYER_HEALTH }} ед.</p>
					<p>
						-
						<v-icon
							icon="mdi-help-rhombus"
							color="#666"
						/>{{ game.settings.ALWAYS_HIDDEN_PLAYER_HEALTH }} ед. здоровья всегда скрыты для всех игроков.
					</p>
					<p>- имеют {{ game.settings.MAX_INVENTORY_SLOTS }} слотов под предметы.</p>

					<b>Предметы:</b>
					<p>- применяются в течение {{ game.settings.ITEM_CHANNELING_TIME / 1000 }} сек., и делятся на:</p>
					<p>- обычные: видны всем игрокам, эффект срабатывает сразу после применения.</p>
					<p>
						- ловушки: видны только владельцу, начинают работать после завершения хода игрока, срабатывают
						при выполнении описанного условия.
					</p>

					<b>В начале игры:</b>
					<p>- все игроки случайно перемешиваются.</p>
					<p>- случайный игрок получает право хода.</p>

					<b>В начале каждого раунда:</b>
					<p>
						- в дробовик заряжается случайное кол-во патронов, от 2 до
						{{ game.settings.MAX_PATRONS_IN_RIFLE }}, при этом 1 из них всегда заряжен, и 1 всегда холостой.
					</p>
					<p>
						Последовательность, отображаемая при зарядке никак не связана с той последовательностью, в
						которой патроны заряжаются на самом деле.
					</p>
					<p>
						- игроки получают
						{{ game.settings.ITEMS_PER_ROUND }} предмета(ов), а это число увеличивается на
						{{ game.settings.ITEMS_PER_ROUND_INCREMENT }}, пока не достигнет
						{{ game.settings.MAX_ITEMS_PER_ROUND }}.
					</p>

					<b>В свой ход (длится до {{ game.settings.MAX_TURN_DURATION / 1000 }} сек.) игрок может:</b>
					<p>- использовать неограниченное кол-во предметов (но не позднее, чем за 10 сек до конца хода).</p>
					<p>- целиться в любого игрока.</p>
					<p>- совершить 1 выстрел.</p>

					<b>При выстреле:</b>
					<p>
						- в другого игрока: ход передается этому игроку, независимо от того, какой был патрон, если
						выстрел убил игрока, ход перейдет к следующему игроку после него.
					</p>
					<p>- в себя: если патрон был холостой, ход передается тому же игроку, иначе следующему игроку.</p>

					<b>При истечении времени на ход:</b>
					<p>- ход передается следующему игроку.</p>

					<b>Начиная с {{ game.settings.FATIGUE_ROUND }} раунда:</b>
					<p>
						- игроки устают, и начинают терять {{ game.settings.FATIGUE_ITEMS_TO_LOSE }} предмет(а), а это
						число увеличивается на {{ game.settings.FATIGUE_ITEMS_TO_LOSE_INCREMENT }}.
					</p>
					<p>
						- за каждый предмет, который игрок должен был потерять, но не смог, он получает
						{{ game.settings.FATIGUE_DAMAGE_PER_ITEM }} ед. урона.
					</p>

					<b>Игра завершается:</b>
					<p>
						- если при начале следующего хода в живых остались только игроки одной команды - победа этой
						команды.
					</p>
					<p>- если при начале следующего хода в живых не осталось никого - ничья.</p>
				</div>
			</v-tooltip>
		</div>
		<div
			v-if="game.status === GameStatus.PREPARING"
			style="display: flex; align-items: center; gap: 20px"
		>
			<v-text-field
				v-if="game.playerById(localPlayer.id)"
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
					<!-- <div
						class="current-player-color"
						:style="{ background: game.current?.color }"
					/> -->
					<v-icon
						:color="game.current?.color"
						icon="mdi-shoe-sneaker"
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

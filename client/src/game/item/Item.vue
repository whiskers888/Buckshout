<script setup lang="ts">
import { type Item, ItemBehavior } from './item';

import { ModifierState } from '@/game/modifier/modifier';
import type { Player } from '@/game/player/player';
import { RifleStatus } from '@/game/rifle/rifle';
import { useNotifier } from '@/stores/notifier';
import { PlayerActivity, useLocalPlayer } from '@/stores/player';
import { useRifle } from '@/stores/rifle';

const localPlayer = useLocalPlayer();
const rifle = useRifle();
const notifier = useNotifier();

function onItemClick() {
	if (item?.removing) return;
	if (rifle.status !== RifleStatus.READY) {
		if (localPlayer.isCurrent) notifier.error('Дробовик еще не заряжен!');
		return;
	}
	if (localPlayer.activity === PlayerActivity.USING_ITEM) {
		localPlayer.selectAsTarget(owner, item);
	} else {
		localPlayer.beginUse(item!);
	}
}

const { owner, item } = defineProps<{
	owner: Player;
	item?: Item;
}>();
</script>

<template>
	<div class="item-container">
		<div
			v-if="item && !owner.is(ModifierState.PLAYER_DEAD) && !localPlayer.is(ModifierState.PLAYER_BLINDED)"
			:class="[
				'item',
				{
					shine: item.adding,
					removing: item.removing,
					target: localPlayer.canTargetItem(owner, item),
					special: item?.behavior.includes(ItemBehavior.CUSTOM) && owner.id === localPlayer.id,
					active: localPlayer.activity === PlayerActivity.DECIDES_CANCEL,
					clay: item.is(ModifierState.ITEM_CLAY),
				},
			]"
			@click="onItemClick"
		>
			<v-tooltip location="right">
				<template v-slot:activator="{ props }">
					<v-icon
						v-bind="props"
						class="item-info"
						icon="mdi-information"
					/>
				</template>
				<div
					v-if="!item.is(ModifierState.ITEM_INVISIBLE)"
					class="item-tooltip"
				>
					<h3>{{ item.name }}</h3>
					<div class="item-behavior">
						<div>Тип: {{ item.typeTooltip }}</div>
						<div>Способность: {{ item.behaviorTooltip }}</div>
						<div v-if="item.behavior.includes(ItemBehavior.UNIT_TARGET)">
							Цель: {{ item.targetTooltip }}
						</div>
					</div>
					<p class="item-tooltip-description">{{ item.description }}</p>
					<p
						v-if="item.lore"
						class="item-tooltip-lore"
					>
						{{ item.lore }}
					</p>
				</div>
				<div v-else>
					<h3>Неизвестно</h3>
					<p class="item-tooltip-description">Этот предмет скрыт от Вашего взора.</p>
				</div>
			</v-tooltip>
			<img
				class="item-model"
				:src="
					!item.is(ModifierState.ITEM_INVISIBLE)
						? `/models/items/${item.model}.png`
						: '/models/items/unknown.png'
				"
				:alt="!item.is(ModifierState.ITEM_INVISIBLE) ? item.name : 'Неизвестно'"
			/>
			<div class="item-modifiers">
				<v-tooltip
					v-for="modifier in item.modifiers"
					:key="modifier.name"
					location="right"
				>
					<template v-slot:activator="{ props }">
						<v-icon
							v-bind="props"
							:style="{
								border: `1px solid ${modifier.isBuff ? '#0f0' : '#f00'}`,
								borderRadius: '50%',
								padding: '10px',
								fontSize: '14px',
								background: 'rgb(var(--v-theme-background))',
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
		</div>
	</div>
</template>

<style scoped>
.item-container {
	display: flex;
	justify-content: center;
	align-items: center;
	padding: 4px;
	border: 1px solid;
	border-radius: 6px;
	overflow: hidden;
	height: 60px;
	position: relative;
}
.item {
	display: flex;
	align-items: center;
	justify-content: center;
	width: 100%;
	height: 100%;
	border-radius: 10px;
	border: solid 2px transparent;
}
.item-model {
	max-width: 100%;
	max-height: 100%;
}

.item-info {
	position: absolute;
	top: 0;
	right: 0;
	color: rgb(146, 162, 177) !important;
	z-index: 10;
}

.item-tooltip {
	max-width: 400px;
}

.item-tooltip h3 {
	margin-bottom: 6px;
}

.item-behavior {
	border-top: 1px solid;
	border-bottom: 1px solid;
	padding: 8px 0;
}

.item-tooltip-description {
	white-space: pre-wrap;
	padding: 8px 0;
}

.item-tooltip-lore {
	border-top: 1px solid;
	font-style: italic;
	padding: 8px 0;
	font-size: 12px;
}

.item-modifiers {
	position: absolute;
	left: 0;
	bottom: 0;
	right: 0;
	padding: 2px;
	display: flex;
	gap: 2px;
}

/*  */
.special {
	opacity: 0.4;
}
.special.active {
	opacity: 1;
	--border-angle: 0turn;
	--main-bg: conic-gradient(from var(--border-angle), #213, #112 5%, #112 60%, #213 95%);
	--gradient-border: conic-gradient(from var(--border-angle), transparent 25%, #08f, #f03 99%, transparent);
	background:
		var(--main-bg) padding-box,
		var(--gradient-border) border-box,
		var(--main-bg) border-box;
	background-position: center center;
	-webkit-animation: bg-spin 3s linear infinite;
	animation: bg-spin 3s linear infinite;
}
@-webkit-keyframes bg-spin {
	to {
		--border-angle: 1turn;
	}
}
@keyframes bg-spin {
	to {
		--border-angle: 1turn;
	}
}

@property --border-angle {
	syntax: '<angle>';
	inherits: true;
	initial-value: 0turn;
}

.removing {
	animation: pulse 1s linear infinite !important;
}

.shine::before {
	background: linear-gradient(to right, rgba(255, 255, 255, 0) 0%, rgba(255, 255, 255, 0.3) 100%);
	content: '';
	display: block;
	height: 100%;
	left: -75%;
	position: absolute;
	top: 0;
	transform: skewX(-25deg);
	width: 50%;
	z-index: 2;
	animation: shine 0.85s infinite linear;
}

.clay img {
	filter: contrast(60%) brightness(60%) sepia(50%) saturate(10) grayscale(20%);
}
</style>

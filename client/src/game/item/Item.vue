<script setup lang="ts">
import { PlayerActivity, useLocalPlayer } from '@/stores/player';
import type { Player } from '../player/player';
import { ItemBehavior, type Item } from './item';
import { ModifierState } from '../modifier/modifier';
import { useRifle } from '@/stores/rifle';
import { RifleStatus } from '../rifle/rifle';
import { useNotifier } from '@/stores/notifier';

const localPlayer = useLocalPlayer();
const rifle = useRifle();
const notifier = useNotifier();

function onItemClick() {
	if (rifle.status !== RifleStatus.READY) {
		notifier.error('Винтовка еще не заряжена!');
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
			v-if="item"
			:class="[
				'item',
				{
					target: localPlayer.canTargetItem(owner, item),
					special: item?.behavior.includes(ItemBehavior.CUSTOM) && owner.id === localPlayer.id,
					active: localPlayer.activity === PlayerActivity.DECIDES_CANCEL,
				},
			]"
			@click="onItemClick"
		>
			<v-tooltip location="right">
				<template v-slot:activator="{ props }">
					<img
						v-bind="props"
						class="item-model"
						:src="
							!item.is(ModifierState.ITEM_INVISIBLE)
								? `/models/items/${item.model}.png`
								: '/models/items/unknown.png'
						"
						:alt="!item.is(ModifierState.ITEM_INVISIBLE) ? item.name : 'Неизвестно'"
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
					<p class="item-tooltip-description">Этот предмет скрыт от вашего взора.</p>
				</div>
			</v-tooltip>
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
	height: 80px;
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
</style>

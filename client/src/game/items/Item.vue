<script setup lang="ts">
import type { Item } from '@/stores/game';

const { item } = defineProps<{
	item?: Item;
}>();
</script>

<template>
	<div class="item-container">
		<div
			v-if="item"
			class="item"
		>
			<v-tooltip location="right">
				<template v-slot:activator="{ props }">
					<img
						v-bind="props"
						class="item-model"
						:src="`/models/${item.model}.png`"
						:alt="item.name"
					/>
				</template>
				<div class="item-tooltip">
					<h3>{{ item.name }}</h3>
					<p class="item-tooltip-description">{{ item.description }}</p>
					<p
						v-if="item.lore"
						class="item-tooltip-lore"
					>
						{{ item.lore }}
					</p>
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
	padding: 10px;
	border: 1px solid;
	border-radius: 6px;
	overflow: hidden;
}
.item {
	display: flex;
	align-items: center;
	justify-content: center;
}
.item-model {
	width: 100px;
}

.item-tooltip {
	max-width: 500px;
}

.item-tooltip-description {
	white-space: pre-wrap;
}

.item-tooltip-lore {
	border-top: 1px solid;
	font-style: italic;
	margin-top: 6px;
	padding: 6px 0;
}
</style>

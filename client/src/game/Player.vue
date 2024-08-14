<script setup lang="ts">
import type { Player } from '@/stores/game';
import Item from './items/Item.vue';

const { player } = defineProps<{
	player: Player;
}>();
</script>

<template>
	<div class="player">
		<div class="player-info">
			<div>{{ player.name }}</div>
			<v-tooltip
				location="right"
				:text="`Здоровье игрока: ${player.health} ед.`"
			>
				<template v-slot:activator="{ props }">
					<div v-bind="props">
						<span
							v-for="hp in player.health"
							:key="hp"
						>
							❤
						</span>
					</div>
				</template>
			</v-tooltip>
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
	width: 500px;
	padding: 20px;
	border: 1px solid;
	margin: 10px;
	border-radius: 10px;
}

.player-info {
	display: flex;
	justify-content: space-between;
	align-items: center;
	cursor: default;
}

.player-inventory {
	display: grid;
	grid-template-rows: repeat(2, 1fr);
	grid-template-columns: repeat(4, 1fr);
	grid-gap: 2px;
	padding: 10px 0;
}
</style>

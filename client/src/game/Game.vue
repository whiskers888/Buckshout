<script setup lang="ts">
import { useGame } from '@/stores/game';
import Player from './Player.vue';

const game = useGame();
</script>

<template>
	<v-container class="fill-height">
		<div>
			<Player
				v-for="player in game.players"
				:key="player.id"
				:player="player"
			/>
		</div>
	</v-container>
</template>

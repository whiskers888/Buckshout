<script setup lang="ts">
import { useRooms } from '@/stores/room';
import { onBeforeRouteLeave } from 'vue-router';
import Game from '@/game/Game.vue';

const rooms = useRooms();

onBeforeRouteLeave(() => {
	console.log('LEAVE');
	rooms.invokeLeave();
});
</script>

<template>
	<v-app>
		<v-main scrollable>
			<Game />
		</v-main>
	</v-app>
</template>

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
	<v-container class="fill-height">
		<v-btn
			color="danger"
			text="Выйти"
			@click="rooms.leave"
		/>

		<Game />
	</v-container>
</template>

<script setup lang="ts">
import { GameStatus } from '@/stores/game';
import { useRooms } from '@/stores/room';

const rooms = useRooms();
</script>

<template>
	<v-container class="fill-height">
		<v-card
			class="mx-auto mb-4"
			width="80%"
			v-for="room in rooms.items.filter(it => it.game.status === GameStatus.PREPARING)"
			:key="room.name"
		>
			<v-card-title>
				<div style="display: flex; align-items: center; justify-content: space-between">
					<span>{{ room.name }}</span>
					<v-btn
						icon="mdi-connection"
						text="Подключиться"
						@click="rooms.join(room.name)"
					/>
				</div>
			</v-card-title>
			<v-card-text>Кол-во игроков: {{ room.game.players.length }}</v-card-text>
		</v-card>
	</v-container>
</template>

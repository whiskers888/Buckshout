<script setup lang="ts">
import { ref } from 'vue';
import type { VForm } from 'vuetify/components';

import { useRooms } from '@/stores/room';

const required = [(value: string) => !!value || 'Это обязательное поле'];

const name = ref('');
const form = ref<VForm>();

const { create } = useRooms();

async function submit() {
	if (!form.value?.isValid) return;

	await create(name.value);
}
</script>

<template>
	<v-container class="fill-height">
		<v-card
			class="mx-auto"
			width="300"
		>
			<v-card-title>Создание комнаты</v-card-title>
			<v-form
				class="pa-4"
				ref="form"
				@submit.prevent="submit"
			>
				<v-text-field
					class="mb-2"
					v-model="name"
					label="Название"
					prepend-inner-icon="mdi-account"
					autofocus
					:rules="required"
				/>

				<v-btn
					color="secondary"
					text="Создать"
					type="submit"
					block
				/>
			</v-form>
		</v-card>
	</v-container>
</template>

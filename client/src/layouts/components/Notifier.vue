<template>
	<v-snackbar
		v-model="isActive"
		location="top"
		:color="type"
		:timeout="duration"
	>
		{{ text }}

		<template v-slot:actions>
			<v-btn
				color="surface"
				icon
				@click="hide"
			>
				<template #default>
					<v-icon color="white">mdi-close</v-icon>
				</template>
			</v-btn>
		</template>
	</v-snackbar>
</template>

<script setup lang="ts">
import { useNotifier } from '@/stores/notifier';
import { storeToRefs } from 'pinia';

const notifier = useNotifier();
const { isActive, text, duration, type } = storeToRefs(notifier);
const { hide } = notifier;
</script>

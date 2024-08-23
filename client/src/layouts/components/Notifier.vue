<template>
	<div id="notifications">
		<v-slide-y-transition group>
			<v-alert
				v-for="[id, notification] in notifications"
				theme="dark"
				:key="id"
				:color="notification.type"
			>
				{{ notification.text }}

				<template v-slot:close>
					<v-btn
						color="surface"
						icon
						@click="hide(id)"
					>
						<template #default>
							<v-icon color="white">mdi-close</v-icon>
						</template>
					</v-btn>
				</template>
			</v-alert>
		</v-slide-y-transition>
	</div>
</template>

<script setup lang="ts">
import { storeToRefs } from 'pinia';

import { useNotifier } from '@/stores/notifier';

const notifier = useNotifier();
const { notifications } = storeToRefs(notifier);
const { hide } = notifier;
</script>

<style scoped>
#notifications {
	position: fixed;
	display: flex;
	justify-content: center;
	top: 10px;
	left: 0;
	right: 0;
	padding: 10px;
	z-index: 2000;
	max-width: 30%;
	margin: 0 auto;
	flex-direction: column;
	gap: 10px;
}
</style>

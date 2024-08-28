<script setup lang="ts">
import { computed } from 'vue';
import { useRoute } from 'vue-router';
import { useTheme } from 'vuetify';

import { useNavigation } from '@/stores/navigation';

const theme = useTheme();
const route = useRoute();
const navigation = useNavigation();

const isDark = computed(() => {
	return theme.global.current.value.dark;
});

function toggleTheme() {
	theme.global.name.value = isDark.value ? 'light' : 'dark';
	localStorage.setItem('theme', theme.global.name.value);
}
</script>

<template>
	<v-app-bar :elevation="1">
		<template v-slot:prepend>
			<v-tooltip
				location="right"
				text="Меню"
			>
				<template v-slot:activator="{ props }">
					<v-app-bar-nav-icon
						v-bind="props"
						@click="navigation.toggle"
					/>
				</template>
			</v-tooltip>
		</template>

		<v-app-bar-title>{{ route.meta.name }}</v-app-bar-title>

		<template #append>
			<v-tooltip
				location="bottom"
				:text="`${isDark ? 'Светлая' : 'Темная'} тема`"
			>
				<template v-slot:activator="{ props }">
					<v-btn
						v-bind="props"
						:icon="isDark ? 'mdi-weather-sunny' : 'mdi-weather-night'"
						@click="toggleTheme"
					/>
				</template>
			</v-tooltip>
		</template>
	</v-app-bar>
</template>

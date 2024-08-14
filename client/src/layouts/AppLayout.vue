<script setup lang="ts">
import AppHeader from './components/AppHeader.vue';
import AppNavigation from './components/AppNavigation.vue';
import AppFooter from './components/AppFooter.vue';
</script>

<template>
	<v-app>
		<AppHeader />
		<AppNavigation />
		<v-main scrollable>
			<v-container
				fluid
				class="h-full"
			>
				<router-view v-slot="{ Component }">
					<keep-alive>
						<Suspense>
							<component :is="Component" />
						</Suspense>
					</keep-alive>
				</router-view>
			</v-container>
		</v-main>
		<AppFooter />
	</v-app>
</template>

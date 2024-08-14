<script setup lang="ts">
import { useNavigation } from '@/stores/navigation';
import { storeToRefs } from 'pinia';
import type { RouteMeta, RouteRecordSingleViewWithChildren } from 'vue-router';

const navigation = useNavigation();
const { items, paths, isOpened } = storeToRefs(navigation);

function hasChildren(item: any): item is RouteRecordSingleViewWithChildren {
	return item.children !== undefined;
}
function isInvisible(item: any): item is RouteMeta & { invisible: true } {
	return item.meta.invisible;
}
</script>

<template>
	<v-navigation-drawer
		v-model="isOpened"
		disable-route-watcher
		width="210"
	>
		<v-list
			nav
			:opened="paths"
			open-strategy="multiple"
		>
			<template v-for="item in items">
				<v-list-group
					v-if="(hasChildren(item) && item.children.filter(it => !it.meta?.invisible)?.length) || 0 > 0"
					:key="item.path"
					:value="item.path"
				>
					<template v-slot:activator="{ props }">
						<v-list-item
							v-bind="props"
							:title="item.meta?.name"
							:prepend-icon="item.meta?.icon"
						/>
					</template>

					<v-list-item
						v-for="child in (hasChildren(item) && item.children.filter(it => !it.meta?.invisible)) || []"
						:key="child.path"
						:to="{ name: child.name }"
						:title="child.meta?.name"
						:prepend-icon="child.meta?.icon"
					/>
				</v-list-group>
				<v-list-item
					v-else-if="!isInvisible(item)"
					:key="`e-${item.path}`"
					:to="{ name: item.name }"
					:title="item.meta?.name"
					:prepend-icon="item.meta?.icon"
				/>
			</template>
		</v-list>
	</v-navigation-drawer>
</template>

/// <reference types="vite/client" />
import 'vue-router';

declare module 'vue-router' {
	export type RouteRecordRaw = {
		name: number;
	};
	export interface RouteMeta {
		name: string;
		icon: string;
		invisible?: boolean;
	}
}

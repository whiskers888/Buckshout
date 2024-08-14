import { routes } from '@/router';
import { defineStore } from 'pinia';

type RouteView = (typeof routes)[number]['name'];
const find = <V extends RouteView>(id: V): TypeSafeRouteFind<typeof routes, V> => {
	return routes.find(entry => entry.name === id) as TypeSafeRouteFind<typeof routes, V>;
};

type UnReadOnly<T> = {
	-readonly [K in keyof T]: T[K];
};
type TypeSafeRouteFind<T extends readonly unknown[], V> =
	UnReadOnly<T> extends [infer Head, ...infer Tail]
		? Head extends { readonly name: V }
			? Head
			: TypeSafeRouteFind<Tail, V>
		: never;

const items = find('app').children;

export const useNavigation = defineStore('navigation', {
	state: () => ({
		items,
		isOpened: true,
	}),
	getters: {
		paths: state => {
			return state.items.map(it => it.path);
		},
	},
	actions: {
		toggle() {
			this.isOpened = !this.isOpened;
		},
	},
});

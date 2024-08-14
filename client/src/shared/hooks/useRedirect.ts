import router, { routes } from '@/router';
import type { RouteParamsRaw, RouteRecordSingleViewWithChildren } from 'vue-router';

type Routes = typeof routes;

type Paths<T> =
	T extends Array<infer Item>
		? Item extends { name: string }
			? Item extends RouteRecordSingleViewWithChildren
				? `${Item['name']}` | `${Paths<Item['children']>}`
				: `${Item['name']}`
			: never
		: never;

type View = Paths<Routes>;

export const useRedirect = (name: View, params?: RouteParamsRaw) => {
	router.push({ name, params });
};

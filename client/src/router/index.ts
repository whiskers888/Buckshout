import { createRouter, createWebHistory, type RouteRecordRaw } from 'vue-router';

export const routes = [
	{
		path: '/auth',
		name: 'auth',
		component: () => import('@/views/auth/AuthView.vue'),
	},
	{
		path: '/',
		name: 'app',
		component: () => import('@/layouts/AppLayout.vue'),
		redirect: { name: 'connect' },
		children: [
			{
				path: 'create',
				meta: {
					name: 'Создать игру',
					icon: 'mdi-pencil-outline',
				},
				name: 'create',
				component: () => import('@/views/create/CreateView.vue'),
			},
			{
				path: 'connect',
				meta: {
					name: 'Подключиться',
					icon: 'mdi-connection',
				},
				name: 'connect',
				component: () => import('@/views/connect/ConnectView.vue'),
			},
			{
				path: 'room/:name',
				meta: {
					name: 'Комната',
					icon: 'mdi-connection',
					invisible: true,
				},
				name: 'room',
				component: () => import('@/views/room/RoomView.vue'),
			},
		],
	},
	{
		path: '/:pathMatch(.*)',
		name: '404',
		component: () => import('@/views/404/NotFoundView.vue'),
	},
] as const satisfies RouteRecordRaw[];

const router = createRouter({
	history: createWebHistory(import.meta.env.BASE_URL),
	routes,
});

export default router;

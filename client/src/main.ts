import '@/assets/styles/root.css';

import { createApp } from 'vue';
import { createPinia } from 'pinia';
import vuetify from '@/plugins/vuetify';

import App from '@/App.vue';
import router from '@/router';
import { useSession } from '@/stores/session';
import { connection } from '@/api';
import { init } from '@/game/scripts';
import { useRooms } from '@/stores/room';

const app = createApp(App);

router.beforeEach((to, from, next) => {
	const session = useSession();
	if (!session.login && to.name !== 'auth') {
		next({ name: 'auth' });
	} else {
		if (to.name === 'room') {
			console.log('JOIN');
			const rooms = useRooms();
			rooms.invokeJoin(to.params.name as string);
		}
		next();
	}
});

app.use(createPinia());
app.use(router);
app.use(vuetify);

connection.start();

app.mount('#app');

init();

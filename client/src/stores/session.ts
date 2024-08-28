import { defineStore } from 'pinia';

import { useRedirect } from '@/shared/hooks/useRedirect';

interface Session {
	login: string;
}

export const useSession = defineStore('session', {
	state: (): Session => ({
		login: '',
	}),
	actions: {
		signIn(login: string) {
			this.login = login;
			useRedirect('connect');
		},
	},
});

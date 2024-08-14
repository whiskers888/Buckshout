import { useRedirect } from '@/shared/hooks/useRedirect';
import { defineStore } from 'pinia';

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

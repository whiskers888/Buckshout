import { defineStore } from 'pinia';

type NotificationType = 'success' | 'error' | 'info' | 'alert';

interface Notification {
	type: NotificationType;
	text: string;
}

export const useNotifier = defineStore('notifier', {
	state: () => ({
		notifications: new Map<number, Notification>(),
	}),
	actions: {
		error(text: string, duration = 5000) {
			this.show('error', text, duration);
		},
		info(text: string, duration = 5000) {
			this.show('info', text, duration);
		},
		alert(text: string, duration = 5000) {
			this.show('alert', text, duration);
		},
		success(text: string, duration = 5000) {
			this.show('success', text, duration);
		},
		hide(id: number) {
			this.notifications.delete(id);
		},
		show(type: NotificationType, text: string, duration: number) {
			const id = Date.now();
			this.notifications.set(id, {
				text,
				type,
			});
			setTimeout(() => this.hide(id), duration);
		},
	},
});

import { defineStore } from 'pinia';

type NotificationType = 'success' | 'error' | 'info' | 'alert';

export const useNotifier = defineStore('notifier', {
	state: () => ({
		isActive: false,
		type: 'info' as NotificationType,
		duration: 5000,
		text: '',
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
		hide() {
			this.isActive = false;
		},
		show(type: NotificationType, text: string, duration: number) {
			this.isActive = true;
			this.type = type;
			this.duration = duration;
			this.text = text;
		},
	},
});

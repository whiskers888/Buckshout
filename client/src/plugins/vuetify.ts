import '@mdi/font/css/materialdesignicons.css';
import 'vuetify/styles';
import { ru } from 'vuetify/locale';

import { createVuetify, type ThemeDefinition } from 'vuetify';

const themeSettings: ThemeDefinition = {
	colors: {
		primary: '#373a36',
		'primary-light': '#e6e2dd',
		secondary: '#d48166',
	},
};

const theme =
	localStorage.getItem('theme') ||
	(window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light');

export default createVuetify({
	locale: {
		locale: 'ru',
		messages: { ru },
	},
	theme: {
		themes: {
			light: themeSettings,
			dark: {
				...themeSettings,
				colors: {
					...themeSettings.colors,
					'primary-light': '#494846',
				},
			},
		},
		defaultTheme: theme,
	},
	defaults: {
		VIcon: {
			color: 'secondary',
		},
		VAutocomplete: {
			variant: 'outlined',
			density: 'compact',
			itemValue: 'id',
			itemTitle: 'name',
		},
		VTabs: {
			bgColor: 'primary',
			density: 'compact',
		},
		VToolbar: {
			color: 'surface',
			density: 'compact',
		},
	},
});

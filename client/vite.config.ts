import { URL, fileURLToPath } from 'node:url';

import { templateCompilerOptions } from '@tresjs/core';
import vue from '@vitejs/plugin-vue';
import { defineConfig } from 'vite';
import vueDevTools from 'vite-plugin-vue-devtools';
import vuetify from 'vite-plugin-vuetify';

export default defineConfig({
	plugins: [
		vue({
			...templateCompilerOptions,
		}),
		vuetify({
			// styles: {
			// 	configFile: './src/plugins/settings.scss',
			// },
		}),
		vueDevTools(),
	],
	resolve: {
		alias: {
			'@': fileURLToPath(new URL('./src', import.meta.url)),
		},
	},
	server: {
		host: true,
	},
});

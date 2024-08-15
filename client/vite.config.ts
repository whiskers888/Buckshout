import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import vue from '@vitejs/plugin-vue';
import vuetify from 'vite-plugin-vuetify';
import vueDevTools from 'vite-plugin-vue-devtools';
import { templateCompilerOptions } from '@tresjs/core';

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

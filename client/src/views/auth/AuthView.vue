<script setup lang="ts">
import { useSession } from '@/stores/session';
import { ref } from 'vue';
import type { VForm } from 'vuetify/components';

const required = [(value: string) => !!value || 'Это обязательное поле'];

const login = ref('');
const form = ref<VForm>();

const { signIn } = useSession();

async function submit() {
	if (!form.value?.isValid) return;

	await signIn(login.value);
}
</script>

<template>
	<v-container class="fill-height">
		<v-card
			class="mx-auto"
			width="300"
		>
			<v-card-title>Авторизация</v-card-title>
			<v-form
				class="pa-4"
				ref="form"
				@submit.prevent="submit"
			>
				<v-text-field
					class="mb-2"
					v-model="login"
					label="Никнейм"
					prepend-inner-icon="mdi-account"
					autofocus
					:rules="required"
				/>

				<v-btn
					color="secondary"
					text="Войти"
					type="submit"
					block
				/>
			</v-form>
		</v-card>
	</v-container>
</template>

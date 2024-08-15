<script setup lang="ts">
import { useRifle } from '@/stores/rifle';

const rifle = useRifle();
</script>

<template>
	<div class="rifle-container">
		<img
			class="rifle-model"
			:src="`/models/rifle.png`"
			alt="дробовик"
		/>
	</div>
</template>

<style scoped>
.rifle-model {
	width: 500px;
	transform: scale(-1, 1) translateY(v-bind('rifle.offset'));
}
</style>

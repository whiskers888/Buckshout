<script setup lang="ts">
import { RiflePatron, RifleStatus, useRifle } from '@/stores/rifle';

const rifle = useRifle();
</script>

<template>
	<div
		:class="[
			'rifle-container',
			{
				'rifle-buckshot-kickback':
					rifle.status == RifleStatus.SHOOTING && rifle.patrons.current == RiflePatron.CHARGED,
			},
		]"
	>
		<img
			v-if="rifle.status == RifleStatus.SHOOTING && rifle.patrons.current == RiflePatron.CHARGED"
			class="rifle-buckshot-smoke"
			src="/models/rifle/particles/smoke.gif"
		/>
		<img
			class="rifle-model"
			src="/models/rifle/rifle.png"
		/>

		<div v-if="rifle.status == RifleStatus.PULLING">
			<img
				v-if="rifle.patrons.current == RiflePatron.CHARGED"
				class="rifle-bullet"
				src="/models/rifle/particles/buck.png"
			/>
			<img
				v-else-if="rifle.patrons.current == RiflePatron.BLANK"
				class="rifle-bullet"
				src="/models/rifle/particles/blank.png"
			/>
		</div>
	</div>
</template>

<style scoped>
.rifle-container {
	transform: scale(-1, 1) translateY(v-bind('rifle.offset'));
	position: relative;
	transition: all 0.3s linear;
	max-width: 40%;
	height: 0;
}

.rifle-model {
	width: 500px;
	max-width: 100%;
}

.rifle-buckshot-smoke {
	width: 130px;
	position: absolute;
	left: 404px;
	top: -25px;
	transform: scale(-1, 1);
}

.rifle-buckshot-kickback {
	rotate: 2deg;
}

.rifle-bullet {
	position: absolute;
	width: 30px;
	rotate: 35deg;
	left: 208px;
	top: 25px;
	transform: rotate3d(-0.3, 1, 0, 30deg);
	animation: rotate 2s infinite;
}

@-webkit-keyframes rotate {
	30% {
		transform: translate3d(0, -10px, 0);
	}
	80% {
		opacity: 1;
	}
	100% {
		transform: translate3d(50px, 100px, 0);
		opacity: 0;
	}
}
</style>

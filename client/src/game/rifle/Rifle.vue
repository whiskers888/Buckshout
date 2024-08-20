<script setup lang="ts">
import { useRifle } from '@/stores/rifle';
import { RiflePatron, RifleStatus } from './rifle';
import { PlayerActivity, useLocalPlayer } from '@/stores/player';

const rifle = useRifle();
const localPlayer = useLocalPlayer();
</script>

<template>
	<div class="rifle-container">
		<div class="rifle-modifiers">
			<v-tooltip
				v-for="modifier in rifle.modifiers"
				:key="modifier.name"
				location="bottom"
			>
				<template v-slot:activator="{ props }">
					<v-icon
						v-bind="props"
						:style="{
							border: `1px solid ${modifier.isBuff ? '#0f0' : '#f00'}`,
							borderRadius: '50%',
							padding: '12px',
							fontSize: '18px',
						}"
						:icon="`mdi-${modifier.icon}`"
					/>
				</template>
				<div class="modifier-tooltip">
					<h3>{{ modifier.name }}</h3>
					<p class="rifle-tooltip-description">{{ modifier.description }}</p>
				</div>
			</v-tooltip>
		</div>
		<div class="rifle-model-container">
			<div v-if="rifle.status == RifleStatus.SHOOTING && rifle.patrons.current == RiflePatron.CHARGED">
				<img
					class="rifle-buckshot-smoke"
					src="/models/rifle/particles/smoke.gif"
				/>
				<div
					v-if="rifle.isMissing"
					class="rifle-miss"
				>
					MMIS
				</div>
			</div>
			<img
				:style="{ opacity: rifle.status == RifleStatus.LOADING ? 0.4 : 1 }"
				:class="[
					'rifle-model',
					{
						'rifle-buckshot-kickback':
							rifle.status == RifleStatus.SHOOTING && rifle.patrons.current == RiflePatron.CHARGED,
					},
				]"
				src="/models/rifle/rifle.png"
			/>

			<div v-if="rifle.status == RifleStatus.PULLING">
				<img
					v-if="rifle.patrons.current == RiflePatron.CHARGED"
					class="rifle-bullet falling"
					src="/models/rifle/particles/buck.png"
				/>
				<img
					v-else-if="rifle.patrons.current == RiflePatron.BLANK"
					class="rifle-bullet falling"
					src="/models/rifle/particles/blank.png"
				/>
			</div>

			<div v-if="localPlayer.activity === PlayerActivity.CHECKING_RIFLE">
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
	</div>
</template>

<style scoped>
.rifle-container {
	max-width: 40%;
	height: 0;
}

.rifle-model-container {
	transform: scale(-1, 1) translateY(v-bind('rifle.offset'));
	position: relative;
	transition: all 1s linear;
}

.rifle-model {
	width: 500px;
	max-width: 100%;
	transition: all 0.1s linear;
}

.rifle-buckshot-smoke {
	position: absolute;
	width: 30%;
	left: 85%;
	top: -25%;
	transform: scale(-1, 1);
}

.rifle-miss {
	position: absolute;
	color: #f00;
	transform: scale(-1, 1);
	left: 85%;
	font-weight: bold;
	animation: fly 2s infinite;
}

.rifle-buckshot-kickback {
	rotate: -2deg;
}

.rifle-bullet {
	position: absolute;
	width: 7%;
	rotate: 35deg;
	left: 41%;
	top: 18%;
	transform: rotate3d(-0.3, 1, 0, 30deg);
}

.rifle-bullet.falling {
	animation: fall 2s infinite;
}

@-webkit-keyframes fly {
	100% {
		transform: translate3d(-10px, -100px, 0) scale(-1, 1);
		opacity: 0;
	}
}

@-webkit-keyframes fall {
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

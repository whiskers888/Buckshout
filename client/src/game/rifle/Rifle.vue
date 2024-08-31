<script setup lang="ts">
import { RiflePatron, RifleStatus } from './rifle';

import { useGame } from '@/stores/game';
import { PlayerActivity, useLocalPlayer } from '@/stores/player';
import { useRifle } from '@/stores/rifle';

const rifle = useRifle();
const game = useGame();
const localPlayer = useLocalPlayer();
</script>

<template>
	<div class="rifle-container">
		<div class="rifle-model-container">
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
				:src="rifle.getModel()"
			/>
			<div>
				<v-tooltip
					location="bottom"
					text="Патроны заряжаются в случайном порядке, их кол-во скрыто во время раунда!"
				>
					<template v-slot:activator="{ props }">
						<div
							class="flex"
							v-bind="props"
						>
							<v-icon
								icon="mdi-bullet"
								v-for="patron in game.settings.MAX_PATRONS_IN_RIFLE"
								:color="
									patron <= rifle.patrons.sequence.length
										? rifle.patrons.sequence[patron - 1]
											? '#b60202'
											: '#4949a3'
										: '#3b3b3b'
								"
								:key="patron"
							/>
						</div>
					</template>
				</v-tooltip>
			</div>

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
					class="magnifier"
					src="/models/items/magnifier.png"
				/>
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
	transform: translateY(v-bind('rifle.offset'));
	position: relative;
	transition: all 1s linear;
}

.rifle-modifiers {
	display: flex;
	gap: 4px;
}

.rifle-model {
	width: 500px;
	max-width: 100%;
	transition: all 0.1s linear;
}

.rifle-buckshot-smoke {
	position: absolute;
	width: 30%;
	left: -15%;
	top: -25%;
}

.rifle-miss {
	position: absolute;
	color: #f00;
	left: 0;
	font-weight: bold;
	animation: fly 2s infinite;
}

.rifle-buckshot-kickback {
	rotate: 2deg;
}

.magnifier {
	width: 25%;
	position: absolute;
	left: 37.5%;
	top: 6%;
	transform: scale(-1, 1);
}

.rifle-bullet {
	position: absolute;
	width: 8%;
	left: 52%;
	top: 18%;
}

.rifle-bullet.falling {
	animation: fall 2s infinite;
}

@-webkit-keyframes fly {
	100% {
		transform: translate3d(10px, -100px, 0);
		opacity: 0;
	}
}

@-webkit-keyframes fall {
	30% {
		transform: translate3d(-6px, -10px, 0);
	}
	60% {
		opacity: 1;
	}
	100% {
		transform: translate3d(20px, 120px, 0);
		opacity: 0;
	}
}
</style>

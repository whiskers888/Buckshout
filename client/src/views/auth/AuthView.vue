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
	<v-container class="release-container">
		<div class="letter">
			<h2>General Release of Liability</h2>
			<hr />
			<br />
			<p>
				This General Release ("Release") is made on 22 day of January, 2018 between ████████ at
				████████████████████████████████ ("Releasor") and ████████ at ████████████████████████ ("Releasee").
			</p>
			<br />
			<ol>
				<li>
					<p>
						Releasor and anyone claiming on Releasor's behalf releases and forever discharges Releasee and
						its affiliates, successors, officers, employees, representatives, partners, agents and anyone
						claiming through them (collectively, the “Released Parties”), in their individual and/or
						corporate capacities from any and all claims, liabilities, obligations, promises, agreements,
						disputes, demands, damages, causes of action of any nature and kind, known or unknown, which
						Releasor has or ever had or may in the future have against Releasee or any of the Released
						Parties arising out of or relating to: the termination of a contractual relationship between the
						Releasor and the Releasee (“Claims”).
					</p>
				</li>
				<li>
					<p>
						In exchange for the release of Claims, Releasee will provide Releasor a payment in the amountof
						$10,000.00. In consideration of such payment, Releasor agrees to accept the payment as full and
						complete settlement and satisfaction of any present and prospective claims.
					</p>
				</li>
				<li>
					<p>
						This Release shall not be in any way construed as an admission by the Releasee that it has acted
						wrongfully with respect to Releasor or any other person, that it admits liability or
						responsibility at any time for any purpose, or that Releasor has any rights whatsoever against
						the Releasee.
					</p>
				</li>
				<li>
					<p>
						This Release shall be binding upon the parties and their respective heirs, administrators,
						personal representatives, executors, successors and assigns. Releasor has the authority to
						release the Claims and has not assigned or transferred any Claims to any other party. The
						provisions of this Release are severable. If any provision is held to be invalid or
						unenforceable, it shall not affect the validity or enforceability of any other provision. This
						Release constitutes the entire agreement between the parties and supersedes any and all prior
						oral or written agreements or understandings between the parties concerning the subject matter
						of this Release
					</p>
				</li>
			</ol>

			<v-form
				class="mt-2"
				ref="form"
				@submit.prevent="submit"
			>
				<div class="sign">
					<h3>YOUR SIGN:</h3>
					<v-text-field
						class="mb-2"
						v-model="login"
						label="Никнейм"
						prepend-inner-icon="mdi-account"
						autofocus
						:rules="required"
					/>
				</div>

				<v-btn
					class="mt-2"
					color="secondary"
					text="Подписать"
					type="submit"
					block
				/>
			</v-form>
		</div>
	</v-container>
</template>

<style scoped>
.release-container {
	display: flex;
	justify-content: center;
	align-items: center;
	height: 100dvh;
	max-width: 100%;
}

h2 {
	font-size: 20px;
	text-align: center;
	text-transform: uppercase;
}

.sign {
	text-align: right;
	display: flex;
	align-items: center;
	gap: 10px;
}

.letter {
	background: #fafafa;
	box-shadow:
		0 0 10px rgba(0, 0, 0, 0.3),
		0 0 300px 25px rgba(222, 198, 122, 0.7) inset;
	width: 460px;
	max-width: 90%;
	padding: 20px;
	position: relative;
	color: #333;
	font-size: 1.1dvh;
}

.letter:before,
.letter:after {
	content: '';
	background: #fafafa;
	box-shadow:
		0 0 8px rgba(0, 0, 0, 0.2),
		inset 0 0 300px rgba(222, 198, 122, 0.7);
	height: 100%;
	width: 100%;
	position: absolute;
	z-index: -2;
	transition: 0.5s;
}

.letter:before {
	left: -5px;
	top: 2px;
	transform: rotate(-1.5deg);
}

.letter:after {
	right: -3px;
	top: 0px;
	transform: rotate(2.4deg);
}

.letter:hover:before {
	transform: rotate(0deg);
	border: solid rgba(111, 99, 61, 0.4);
	border-width: 0px 0px 0px 1px;
	left: -6px;
	top: -6px;
}

.letter:hover:after {
	transform: rotate(0deg);
	border: solid rgba(111, 99, 61, 0.4);
	border-width: 0px 0px 0px 1px;
	right: 3px;
	top: -3px;
}
</style>

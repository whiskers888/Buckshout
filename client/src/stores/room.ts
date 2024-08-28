import { defineStore } from 'pinia';

import { useGame } from './game';
import { useSession } from './session';

import { Action, connection } from '@/api';
import type { Game } from '@/game/game';
import { useRedirect } from '@/shared/hooks/useRedirect';

interface Room {
	name: string;
	game: Game;
}

interface RoomState {
	current: Room | null;
	items: Room[];
}

export const useRooms = defineStore('room', {
	state: (): RoomState => ({
		current: null,
		items: [],
	}),

	actions: {
		create(name: string) {
			connection.invoke(Action.CREATE_ROOM, name);
		},
		update(room: Room) {
			const oldRoom = this.items.find(it => it.name === room.name);
			if (oldRoom) Object.assign(oldRoom, room);
		},
		join(name: string) {
			useRedirect('room', { name: name });
		},
		leave() {
			useRedirect('connect');
		},
		invokeJoin(name: string) {
			const session = useSession();
			connection.invoke(Action.JOIN_ROOM, session.login, name);
		},
		invokeLeave() {
			if (!this.current) return;
			connection.invoke(Action.LEAVE_ROOM, this.current.name);
		},
		setCurrent(room: Room) {
			const game = useGame();

			this.current = room;
			game.update(room.game);
		},
		clearCurrent() {
			const game = useGame();

			this.current = null;
			game.clear();
			game.$reset();
		},
		add(room: Room) {
			this.items.push(room);
		},
		remove(name: string) {
			this.items = this.items.filter(it => it.name != name);
		},
	},
});

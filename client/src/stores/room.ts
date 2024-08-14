import { Action, connection } from '@/api';
import { defineStore } from 'pinia';
import { useSession } from './session';
import { useRedirect } from '@/shared/hooks/useRedirect';
import { useGame, type Game } from './game';

interface Room {
	name: string;
	game: Game;
}

export const useRooms = defineStore('room', {
	state: () => ({
		current: null as Room | null,
		items: [] as Room[],
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
			game.$patch(room.game);
		},
		clearCurrent() {
			const game = useGame();

			this.current = null;
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

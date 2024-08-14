import { connection, Event } from '@/api';
import { useGame } from '@/stores/game';
import { useRooms } from '@/stores/room';

function on(e: Event, callback: (data: any) => void) {
	connection.on(e, ans => {
		console.log(e, ans.value.data);
		try {
			callback(ans.value.data);
		} catch (e) {
			console.error(e);
		}
	});
}

function off(events: Event[]) {
	events.forEach(e => {
		connection.off(e);
	});
}

window.onbeforeunload = function () {
	const rooms = useRooms();

	rooms.invokeLeave();
};

export function init() {
	const rooms = useRooms();
	const game = useGame();

	on(Event.CONNECTED, e => {
		rooms.items = e.rooms;
	});
	on(Event.DISCONNECTED, () => {
		rooms.invokeLeave();
	});

	on(Event.ROOM_CREATED, e => {
		rooms.add(e.room);
		if (e.initiator === connection.connectionId) {
			rooms.join(e.room.name);
		}
	});
	on(Event.ROOM_UPDATED, e => {
		rooms.update(e.room);
	});
	on(Event.ROOM_REMOVED, e => {
		rooms.remove(e.name);
	});
	on(Event.ROOM_JOINED, e => {
		rooms.setCurrent(e.room);
	});
	on(Event.ROOM_LEFT, () => {
		rooms.clearCurrent();
	});

	on(Event.PLAYER_CONNECTED, e => {
		game.addPlayer(e.target);
	});
	on(Event.PLAYER_DISCONNECTED, e => {
		game.removePlayer(e.target);
	});
}

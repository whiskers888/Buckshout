import { connection, Event } from '@/api';
import { Player, useGame } from '@/stores/game';
import { PlayerActivity, usePlayer } from '@/stores/player';
import { useRifle } from '@/stores/rifle';
import { useRooms } from '@/stores/room';

interface EventData {
	target?: Player;
	initiator?: Player;
	special: any;
}

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
	const rifle = useRifle();
	const player = usePlayer();

	on(Event.CONNECTED, e => {
		rooms.items = e.rooms;
		player.setConnectionId(connection.connectionId!);
	});
	on(Event.DISCONNECTED, () => {
		rooms.invokeLeave();
	});

	on(Event.MESSAGE_RECEIVED, () => {});
	on(Event.MESSAGE_INITIATOR_RECEIVED, () => {});

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

	on(Event.GAME_STARTED, e => {
		game.update(e.special['GAME']);
		game.start();
	});
	on(Event.TURN_CHANGED, e => {
		game.startTurn(e.target, e.special['TIME']);
		player.setActivity(PlayerActivity.WAITING);
	});
	on(Event.TURN_SKIPPED, e => {});

	on(Event.ITEM_RECEIVED, e => {
		game.addItem(e.target, e.special['ITEM']);
	});
	on(Event.ITEM_USED, e => {
		game.removeItem(e.initiator, e.special['ITEM']);
		player.setActivity(PlayerActivity.DECIDES_CANCEL);
	});
	on(Event.ITEM_EFFECTED, e => {
		player.setActivity(PlayerActivity.WAITING);
	});
	on(Event.ITEM_STOLEN, e => {
		game.removeItem(e.target, e.special['TARGET_ITEM']);
	});
	on(Event.ITEM_CANCELED, e => {});

	on(Event.ROUND_STARTED, e => {
		game.setRound(e.special['ROUND']);
	});

	on(Event.RIFLE_LOADED, e => {
		rifle.load(e.special['COUNT'], e.special['CHARGED']);
	});
	on(Event.RIFLE_AIMED, e => {
		rifle.aim(e.target);
	});
	on(Event.RIFLE_SHOT, e => {
		rifle.shoot(e.special['IS_CHARGED']);
	});
	on(Event.RIFLE_PULLED, e => {
		rifle.pull(e.special['IS_CHARGED']);
	});

	on(Event.DAMAGE_TAKEN, e => {
		game.applyDamage(e.target, e.special['VALUE']);
	});
	on(Event.HEALTH_RESTORED, e => {
		game.applyHeal(e.target, e.special['VALUE']);
	});
}

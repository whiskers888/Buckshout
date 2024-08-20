import { HubConnectionBuilder } from '@microsoft/signalr';
import { Event } from './event';
import { Action } from './action';
export { Event, Action };

export const connection = new HubConnectionBuilder()
	//'http://25.2.157.178:5000/room'
	//'https://localhost:5001/room'
	.withUrl('http://192.168.0.104:5000/room')
	.withAutomaticReconnect()
	.build();

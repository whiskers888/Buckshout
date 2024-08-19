import { HubConnectionBuilder } from '@microsoft/signalr';
import { Event } from './event';
import { Action } from './action';
export { Event, Action };

export const connection = new HubConnectionBuilder()
	//'http://25.2.157.178:5000/room'
	.withUrl('https://localhost:5001/room')
	.withAutomaticReconnect()
	.build();

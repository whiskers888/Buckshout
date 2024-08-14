import { HubConnectionBuilder } from '@microsoft/signalr';
import { Event } from './event';
import { Action } from './action';
export { Event, Action };

export const connection = new HubConnectionBuilder()
	.withUrl('https://localhost:5001/room')
	.withAutomaticReconnect()
	.build();

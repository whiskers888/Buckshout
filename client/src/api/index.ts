import { HubConnectionBuilder } from '@microsoft/signalr';

import { Action } from './action';
import { Event } from './event';
export { Event, Action };

export const connection = new HubConnectionBuilder()
	//'http://25.2.157.178:5000/room'
	//'https://localhost:5001/room'
	// http://26.223.201.60:5000
	.withUrl('http://26.223.201.60:5000/room')
	.withAutomaticReconnect()
	.build();

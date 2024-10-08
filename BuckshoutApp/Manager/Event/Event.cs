﻿namespace BuckshoutApp.Manager.Events
{
    public enum Event
    {
        CONNECTED,
        DISCONNECTED,
        RECONNECTED,
        LEAVE,

        ROOM_CREATED,
        ROOM_ADMIN_CHANGED,
        ROOM_JOINED,
        ROOM_LEFT,
        ROOM_UPDATED,
        ROOM_REMOVED,

        GAME_STARTED,
        GAME_PAUSED,
        GAME_RESUMED,
        GAME_FINISHED,

        MESSAGE,
        SECRET_MESSAGE,
        PLAY_SOUND,

        PLAYER_CONNECTED,
        PLAYER_DISCONNECTED,
        PLAYER_LOST,
        PLAYER_WON,

        ROUND_STARTED,
        ROUND_FINISHED,

        TURN_CHANGED,
        TURN_EXPIRED,
        TURN_SKIPPED,

        RIFLE_LOADED,
        RIFLE_PULLED,
        RIFLE_AIMED,
        RIFLE_SHOT,
        RIFLE_CHECKED,
        RIFLE_EMPTIED,

        ITEM_RECEIVED,
        ITEM_REMOVED,
        ITEM_USED,
        ITEM_CANCELED,
        ITEM_EFFECTED,
        ITEM_STOLEN,

        MODIFIER_APPLIED,
        MODIFIER_REMOVED,

        TRAP_SET,
        TRAP_TRIGGERED,

        BEFORE_DAMAGE_TAKE,
        DAMAGE_TAKEN,
        HEALTH_RESTORED,
    }
}

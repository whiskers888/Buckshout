
namespace BuckshoutApp.Manager.Events
{
    public enum Event
    {
        GAME_CREATED,
        GAME_STARTED,
        GAME_PAUSED,
        GAME_RESUMED,
        GAME_FINISHED,

        MESSAGE_RECEIVED,
        MESSAGE_INITIATOR_RECEIVED,

        PLAYER_CONNECTED,
        PLAYER_DISCONNECTED ,
        PLAYER_LOST,
        PLAYER_WON,

        ROUND_STARTED,
        ROUND_FINISHED,

        TURN_CHANGED,
        TURN_EXPIRED,
        TURN_SKIPPED,

        RIFLE_LOADED,
        RIFLE_AIMED,
        RIFLE_SHOT,
        RIFLE_EMPTIED,

        ITEM_RECEIVED,
        ITEM_USED,
        ITEM_CANCELED,
        ITEM_EFFECTED,
        ITEM_STOLEN,

        TRAP_SET,
        TRAP_TRIGGERED,

        DAMAGE_TAKEN,
        HEALTH_RESTORED,

    }
}

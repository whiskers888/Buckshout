﻿namespace BuckshoutApp.Modifiers
{
    public enum ModifierKey
    {
        PLAYER_DEAD,
        PLAYER_HANDCUFFS,
        PLAYER_DOLLAR,
        PLAYER_STOPWATCH,
        PLAYER_CHAINED,

        PLAYER_CHURCH_CROSS,
        PLAYER_COVER,
        PLAYER_DEFIBRILLATOR,

        PLAYER_TRAP,
        PLAYER_BLOOD_PACK,


        RIFLE_HACKSAW,


        ITEM_CANNOT_BE_STOLEN,
        ITEM_ADRENALINE
    }

    public enum ModifierState
    {
        PLAYER_STUNNED,
        PLAYER_DEAD,
        PLAYER_TURN_TIME_LIMITED,
        PLAYER_EVASION,
        PLAYER_ADDICTED,
        PLAYER_CHAINED,
        PLAYER_CORRUPTED,

        RIFLE_BONUS_DAMAGE,

        ITEM_INVISIBLE,
        ITEM_CANNOT_BE_STOLEN,
        ITEM_LOST_ON_TURN_ENDED
    }
}

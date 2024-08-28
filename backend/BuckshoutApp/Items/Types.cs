namespace BuckshoutApp.Items
{
    public enum ItemTargetType
    {
        NONE,
        PLAYER,
        ITEM,
    }
    public enum ItemTargetTeam
    {
        NONE,
        FRIENDLY,
        ENEMY,
        ANY
    }

    public enum ItemBehavior
    {
        NO_TARGET,
        UNIT_TARGET,
        IMMEDIATE,
        CUSTOM
    }

    public enum ItemType
    {
        DEFAULT,
        TRAP,
    }

    public enum ItemEvent
    {
        USED,
        EFFECTED,
        CANCELED
    }

    public enum ItemState
    {
        IN_BOX,
        IN_HAND,
        USING,
        DELAYED,
        CANCELED,
        REMOVED,
        NOT_ALLOWED,
    }
}

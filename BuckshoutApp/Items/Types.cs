namespace BuckshoutApp.Items
{
    public enum TargetType
    {
        NONE,
        PLAYER,
        ITEM,
    }
    public enum TargetTeam
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
    public enum ItemModifier
    {
        CANNOT_BE_STOLEN,
        INVISIBLE,
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

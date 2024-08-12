namespace BuckshoutApp.Items
{
    public enum TargetType
    {
        NONE = 0,
        PLAYER = 1,
        ITEM = 2,
    }
    public enum TargetTeam
    {
        NONE = 0,
        FRIENDLY = 1,
        ENEMY = 2,
        ANY = 3
    }

    public enum ItemBehavior
    {
        NO_TARGET = 0,
        UNIT_TARGET = 1,
        IMMEDIATE = 2,
        CUSTOM = 3
    }

    public enum ItemType
    {
        DEFAULT = 0,
        TRAP = 1,
    }
    public enum ItemModifier
    {
        CANNOT_BE_STOLEN = 0,
        INVISIBLE = 1,
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

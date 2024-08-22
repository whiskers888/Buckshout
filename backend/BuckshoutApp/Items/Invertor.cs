using BuckshoutApp.Context;

namespace BuckshoutApp.Items
{
    public class Invertor(GameContext context) : Item(context)
    {
        public override string Name => "Инвертор";
        public override string Description => "Меняет тип заряженного в данный момент патрона на противоположный.";
        public override string Model => "invertor";
        internal override void BeforeUse(EventData e)
        {
            var patron = Context.Rifle.Patrons.LastOrDefault();
            if (patron == null) Disallow(e, "В дробовике не осталось патронов!");
        }
        public override void Effect(EventData e)
        {
            var patron = Context.Rifle.Patrons.Last();
            patron.IsCharged = !patron.IsCharged;
        }
    }
}

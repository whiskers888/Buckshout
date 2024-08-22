using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Hacksaw(GameContext context) : Item(context)
    {
        public override string Name => "Ножовка";
        public override string Description => "Следующий выстрел дробовика нанесет 2 ед. урона.\n" +
                                               "Эффект не пропадет, если ход завершился без выстрела.\n" +
                                               "Нельзя использовать, если эффект этого предмета уже применен к дробовику.";
        public override string Model => "hacksaw";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "hacksaw/saw"},
            {ItemEvent.EFFECTED, "hacksaw/fall"}
        };

        internal override void BeforeUse(EventData e)
        {
            if (Context.Rifle.Is(ModifierState.RIFLE_BONUS_DAMAGE))
                Disallow(e, "Ножовка уже применена к дробовику!");
        }
        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.RIFLE_HACKSAW);
            modifier.Apply(e.target, Context.Rifle);
            modifier.Remove(Event.RIFLE_SHOT);
        }
    }
}

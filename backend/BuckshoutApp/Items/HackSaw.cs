using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Hacksaw(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Ножовка";
        public override string Description => $"Следующий выстрел дробовика нанесет на {BONUS_DAMAGE} ед. урона больше.\n" +
                                               "Эффект не пропадет, если ход завершился без выстрела, но пропадет при выстреле, даже если он был холостой.\n" +
                                               "Нельзя использовать, если эффект этого предмета уже применен к дробовику.";
        public override string Lore => "Даже не спрашивайте, как дробовик возвращиется в прежнее состояние.";
        public override string Model => "hacksaw";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "hacksaw/saw"},
            {ItemEvent.EFFECTED, "hacksaw/fall"}
        };

        public int BONUS_DAMAGE = 1;

        internal override void BeforeUse(EventData e)
        {
            if (Context.Rifle.Is(ModifierState.RIFLE_BONUS_DAMAGE))
                Disallow(e, "Ножовка уже применена к дробовику!");
        }
        public override void Effect(EventData e)
        {
            var modifier = Context.ModifierManager.CreateModifier(ModifierKey.RIFLE_HACKSAW);
            modifier.Value = BONUS_DAMAGE;
            modifier.Apply(e.Target!, Context.Rifle);
            modifier.Remove(Event.RIFLE_SHOT);
        }
    }
}

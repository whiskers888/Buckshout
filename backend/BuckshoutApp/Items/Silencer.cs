using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Silencer(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Глушитель";
        public override string Description => $"Следующий выстрел дробовика будет с надетым глушителем\n" +
                                               "Наложит эффект, который закроет цели его кол-во здоровья до следующего раунда.\n" +
                                               "Нельзя использовать, если эффект этого предмета уже применен к дробовику.";
        public override string Lore => "...";
        public override string Model => "silencer";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            /*{ItemEvent.USED, "hacksaw/saw"},
            {ItemEvent.EFFECTED, "hacksaw/fall"}*/
        };

        internal override void BeforeUse(EventData e)
        {
            if (Context.Rifle.Is(ModifierState.RIFLE_SILENCED))
                Disallow(e, "Глушитель уже применен к дробовику!");
        }
        public override void Effect(EventData e)
        {
            var modifierRifle = Context.ModifierManager.CreateModifier(ModifierKey.RIFLE_SILENCER);
            modifierRifle.Apply(e.Target!, Context.Rifle);
            modifierRifle.Remove(Event.RIFLE_SHOT, null, (damageE) =>
            {
                var modifierPlayer = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DISORIENTATION);
                modifierPlayer.Apply(damageE.Target!);
            });
        }
    }
}

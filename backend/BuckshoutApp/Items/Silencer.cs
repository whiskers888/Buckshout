using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Modifiers;

namespace BuckshoutApp.Items
{
    public class Silencer(GameContext context) : Item(context)
    {
        public override string Name { get; set; } = "Глушитель";
        public override string Description => "Следующий выстрел дробовика лишит цель возможности видеть свое здоровье до следующего раунда.\n" +
                                              "Эффект не пропадет, если ход завершился без выстрела, но пропадет при выстреле, даже если он был холостой.\n" +
                                              "Нельзя использовать, если эффект этого предмета уже применен к дробовику.";
        public override string Lore => "Ццц...";
        public override string Model => "silencer";
        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "silencer/screw"}
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
            modifierRifle.Remove(Event.RIFLE_SHOT, null, (shotE) =>
            {
                if ((bool)shotE.Special["IS_CHARGED"] && !(bool)shotE.Special["IS_MISSING"])
                {
                    var modifierPlayer = Context.ModifierManager.CreateModifier(ModifierKey.PLAYER_DISORIENTATION);
                    modifierPlayer.Apply(shotE.Target!);
                }
            });
        }
    }
}

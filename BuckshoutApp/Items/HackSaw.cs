using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;
using BuckshoutApp.Manager.Rifle;

namespace BuckshoutApp.Items
{
    public class Hacksaw:Item
    {
        public Hacksaw(GameContext context) : base(context) { }

        public override string Name => "Ножовка";
        public override string Description => "Следующий выстрел дробовика нанесет 2 ед. урона.\n" +
                                               "Эффект не пропадет, если ход завершился без выстрела.\n" +
                                               "Нельзя использовать, если эффект этого предмета уже применен к дробовику.";
        public override string Model => "hacksaw";
        internal override void BeforeUse(EventData e)
        {
            if(Context.Rifle.Modifiers.Contains(RifleModifier.DOUBLE_DAMAGE))
            {
                Disallow(e,"Ножовка уже применена к дробовику!");
            }
        }
        public override void Effect(EventData e)
        {
            Context.Rifle.Modifiers.Add(RifleModifier.DOUBLE_DAMAGE);

            Context.EventManager.Once(Event.RIFLE_SHOT/*TURN_CHANGED*/, (e) =>
            {
                Context.Rifle.Modifiers.Remove(RifleModifier.DOUBLE_DAMAGE);
            });
        }
    }
}

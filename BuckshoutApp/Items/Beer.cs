using BuckshoutApp.Context;
using BuckshoutApp.Manager.Events;

namespace BuckshoutApp.Items
{
    public class Beer(GameContext context) : Item(context)
    {
        public override string Name => "Пиво";
        public override string Description => "Сбрасывает текущий патрон из дробовика.\n" +
                                              "Если после этого в дробовике не останется патронов, начнется следующий раунд.";
        public override string Lore => "Зачем стрелять в людей, когда можно пить пиво?";
        public override string Model => "beer";

        public override Dictionary<ItemEvent, string> SoundSet { get; set; } = new Dictionary<ItemEvent, string>()
        {
            {ItemEvent.USED, "beer/open"},
            {ItemEvent.EFFECTED, "beer/drink"},
        };

        public override void Effect(EventData e)
        {
            e.special.Add("IS_CHARGED", Context.Rifle.NextPatron());
            Context.EventManager.Trigger(Event.RIFLE_PULLED, e);
        }
    }
}

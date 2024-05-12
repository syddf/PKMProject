using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intimidate : EnterAbilityBase
{
    public override List<Event> Trigger(BattleManager InManager, Event SourceEvent)
    {
        List<BattlePokemon> Enemies = InManager.GetOpppoitePokemon(ReferencePokemon);
        List<Event> NewEvents = new List<Event>();
        if(Enemies[0].HasAbility("精神力", InManager, null, Enemies[0]))
        {
            List<string> Messages = new List<string>();
            Messages.Add(Enemies[0].GetName() + "因为精神力不受影响！");

            MessageAnimationFakeEvent FakeEvent = new MessageAnimationFakeEvent(Messages);
            NewEvents.Add(FakeEvent);
        }
        else
        {
            NewEvents.Add(new StatChangeEvent(Enemies[0], "Atk", -1));
        }
        
        return NewEvents;
    }
}

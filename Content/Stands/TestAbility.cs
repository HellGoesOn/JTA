using JTA.Common.Players;
using JTA.Common.Stands;
using Terraria;

namespace JTA.Content.Stands
{
    public class TestAbility : StandAbility
    {
        public TestAbility() 
        {
            Name = "Test Ability";
            Description = "It tests abilities. Duh";
        }

        public override void UseAbility(Player player, Stand stand)
        {
            Main.NewText($"You used Test Ability, very cool. Slot is {StandPlayer.Get(player).selectedAbilityIndex}, by the way");
        }
    }
}

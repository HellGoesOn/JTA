using JTA.Common.Stands;
using JTA.Common.Systems;
using JTA.Common.UI;
using JTA.Content.Stands;
using Steamworks;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace JTA.Common.Players
{
    public class StandPlayer : ModPlayer
    {
        internal int selectedAbilityIndex;
        internal Stand stand;

        public int unallocatedPerkPoints;

        public override void Initialize()
        {
            selectedAbilityIndex = 0;
            stand = StandDefinitions.Get("StarPlatinum");
            //stand = new Stand {
            //    name = "Test"
            //};
            stand.abilities.AddRange([new TestAbility()
                {
                Name = "Test 1",
                Description = "Test desc 1"
            }, new TestAbility()
            {
                Name = "Test 2",
                Description = "Test desc 2"
            }, new TestAbility()
            {
                Name = "Test 3",
                Description = "Test desc 3"
            }]);

            stand.AddPerk(new TestPerk());

            for(int i = 1; i <= 5; i++) {
                var perks = stand.GetPerks();

                var topPerk = new TestPerk() {
                    path = PerkPath.Top,
                    tier = i
                };

                var lastTopPerk = perks.LastOrDefault(x => x.path == PerkPath.Top);

                if (lastTopPerk != null)
                    topPerk.requiredPerks = [lastTopPerk.id];

                var midPerk = new TestPerk() {
                    path = PerkPath.Mid,
                    tier = i
                };

                var lastMidPerk = perks.LastOrDefault(x => x.path == PerkPath.Mid);

                if (lastMidPerk != null)
                    midPerk.requiredPerks = [lastMidPerk.id];

                var botPerk = new TestPerk() {
                    path = PerkPath.Bot,
                    tier = i
                };

                var botTopPerk = perks.LastOrDefault(x => x.path == PerkPath.Bot);

                if (botTopPerk != null)
                    botPerk.requiredPerks = [botTopPerk.id];

                stand.AddPerks([topPerk, midPerk, botPerk]);
            }
        }

        public void SetStand(Stand stand)
        {
            this.stand = stand; 
        }

        public override void PostUpdate()
        {
            stand?.Update(Player);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            base.ProcessTriggers(triggersSet);

            if (stand == null)
                return;

            if(KeybindSystem.OpenAbilityMenu.JustPressed) {
                var ui = ModContent.GetInstance<AbilitySelectorUISystem>();
                ui.ToggleUI();
            }

            if (KeybindSystem.SummonStand.JustPressed) {
                stand.Summon(Player);
                Main.NewText("Test");
            }

            if(KeybindSystem.UseAbilityButton.JustPressed) {
                stand.abilities[selectedAbilityIndex].UseAbility(Player, stand);
            }
        }

        public static StandPlayer Get(Player player = null)
        {
            player ??= Main.LocalPlayer;

            return player.GetModPlayer<StandPlayer>();
        }

        public bool IsStandUser => stand != null;
    }
}

using JTA.Common.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace JTA.Common.UI
{
    [Autoload(Side = ModSide.Client)]
    public class AbilitySelectorUISystem : ModSystem
    {
        UserInterface abilityInterface;
        internal AbilitySelectorUI abilitySelectorUI;

        public override void Load()
        {
            abilitySelectorUI = new AbilitySelectorUI();
            abilityInterface = new UserInterface();

            abilitySelectorUI.Activate();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            if(abilityInterface?.CurrentState != null)
                abilityInterface.Update(gameTime);
        }

        public void ToggleUI()
        {
            if (abilityInterface.CurrentState != null) {
                abilityInterface.SetState(null);
                abilitySelectorUI.ToggleStandPerks(null);
                return;
            }
            abilityInterface.SetState(abilitySelectorUI);
            abilitySelectorUI.Open();
            abilitySelectorUI.ToggleStandPerks(StandPlayer.Get().stand);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");

            if (mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "JTA: Ability Selector",
                        delegate {
                            if (abilityInterface?.CurrentState != null) {
                                abilitySelectorUI?.Draw(Main.spriteBatch);
                            }

                            return true;
                        },
                    InterfaceScaleType.UI
                        )
                    );
            }
        }
    }
}

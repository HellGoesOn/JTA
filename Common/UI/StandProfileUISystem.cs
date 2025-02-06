using JTA.Common.Players;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;

namespace JTA.Common.UI
{
    [Autoload(Side = ModSide.Client)]
    public class StandProfileUISystem : ModSystem
    {
        UserInterface standProfileInterface;
        internal StandProfileUI standProfileUI;

        public override void Load()
        {
            standProfileUI = new StandProfileUI();
            standProfileInterface = new UserInterface();

            standProfileUI.Activate();

            ToggleUI();
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            if (standProfileInterface?.CurrentState != null)
                standProfileInterface.Update(gameTime);
        }

        public void ToggleUI()
        {
            if (standProfileInterface.CurrentState != null) {
                standProfileInterface.SetState(null);
                return;
            }
            standProfileInterface.SetState(standProfileUI);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");

            if (mouseTextIndex != -1) {
                layers.Insert(mouseTextIndex,
                    new LegacyGameInterfaceLayer(
                        "JTA: Stand Profile",
                        delegate {
                            if (standProfileInterface?.CurrentState != null) {
                                standProfileUI?.Draw(Main.spriteBatch);
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

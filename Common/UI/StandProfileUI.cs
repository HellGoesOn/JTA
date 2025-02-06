using JTA.Common.Stands;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace JTA.Common.UI
{
    public class StandProfileUI : UIState
    {
        int oldWidth, oldHeight;

        UIPanel panel;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.Width.Set(64, 0);
            panel.Height.Set(64, 0);
            panel.Top.Set(Main.screenHeight - 80, 0);
            panel.Left.Set(20, 0);

            Append(panel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Main.screenWidth != oldWidth || Main.screenHeight != oldHeight) {
                panel.Top.Set(Main.screenHeight - 80, 0);
                panel.Left.Set(20, 0);
                Recalculate();
            }

            oldWidth = Main.screenWidth;
            oldHeight = Main.screenHeight;
        }

        public override void Recalculate()
        {
            base.Recalculate();
        }
    }
}

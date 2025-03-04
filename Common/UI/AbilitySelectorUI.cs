using Microsoft.Xna.Framework;
using System.Linq;
using JTA.Common.Players;
using JTA.Common.Systems;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;
using JTA.Common.Stands;
using System.Collections.Generic;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using JTA.Content;

namespace JTA.Common.UI
{
    public class AbilitySelectorUI : UIState
    {
        UIPanel panel;
        UIText tooltip, abilityName, abilityDescription;
        DraggablePanel perkPanel;

        public override void OnInitialize()
        {
            panel = new();
            panel.Width.Set(200, 0);
            panel.Height.Set(200, 0);
            //panel.VAlign = 0.5f;
            //panel.HAlign = 0.5f;
            panel.Top.Set(Main.screenHeight - 220, 0);
            panel.Left.Set(80, 0);

            tooltip = new("");
            tooltip.VAlign = 0.64f;
            tooltip.HAlign = 0.5f;

            abilityDescription = new("");
            abilityName = new("");
            abilityName.HAlign = 0.5f;

            abilityDescription.HAlign = 0.5f;
            abilityDescription.VAlign = 0.5f;

            panel.Append(abilityName);
            panel.Append(abilityDescription);

            Append(panel);
            Append(tooltip);

            //IgnoresMouseInteraction = true;
            //perkPanel = new DraggablePanel();
            //perkPanel.Width.Set(2000, 0);
            //perkPanel.Height.Set(400, 0);

            //perkPanel.HAlign = 0.5f;
            //perkPanel.VAlign = 0.5f;
            //perkPanel.style = DraggablePanel.DragStyle.Horizontal;
            //Append(perkPanel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var player = StandPlayer.Get();
            if (!player.IsStandUser) {
                ModContent.GetInstance<AbilitySelectorUISystem>().ToggleUI();
                return;
            }

            panel.Top.Set(Main.screenHeight - 220, 0);
            panel.Left.Set(80, 0);

            var left = string.Join(' ', KeybindSystem.ArrowLeft.GetAssignedKeys());
            var right = string.Join(' ', KeybindSystem.ArrowRight.GetAssignedKeys());
            //var up = string.Join(' ', KeybindSystem.ArrowUp.GetAssignedKeys());
            //var down = string.Join(' ', KeybindSystem.ArrowDown.GetAssignedKeys());
            tooltip.SetText($@"{left} / {right} to switch ability");

            var direction = KeybindSystem.GetPressedAxis(KeybindSystem.ArrowLeft, KeybindSystem.ArrowRight);

            player.selectedAbilityIndex += direction;


            int abilityCount = StandDefinitions.Stands[player.stand].Abilities.Count;

            if (player.selectedAbilityIndex >= abilityCount)
                player.selectedAbilityIndex = 0;

            if (player.selectedAbilityIndex < 0)
                player.selectedAbilityIndex = abilityCount - 1;


            var ability = StandDefinitions.Stands[player.stand].Abilities[player.selectedAbilityIndex];
            abilityName.SetText(ability.Name);
            abilityDescription.SetText(ability.Description);

            Recalculate();
        }
        //public void ToggleStandPerks(Stand stand)
        //{
        //    perkPanel?.RemoveAllChildren();

        //    if (stand != null) {
        //        List<StandPerk> perks = stand.GetPerks();

        //        float[] alignments = [0.12f, 0.5f, 0.88f];

        //        foreach (StandPerk perk in perks) {

        //            Asset<Texture2D> perkTexture = ModContent.Request<Texture2D>(perk.texturePath);
        //            float leftOffset = perk.path == PerkPath.Mid ? 96 : 0;

        //            var image = new UIImage(perkTexture);
        //            image.VAlign = alignments[(int)perk.path];
        //            image.Left.Set(leftOffset + 192 * perk.tier, 0);

        //            perkPanel.Append(image);
        //        }
        //    }

        //    //if (stand == null) {
        //    //    RemoveChild(perkPanel);
        //    //    perkPanel = null;
        //    //    return;
        //    //}

        //    //perkPanel = new();
        //    //perkPanel.HAlign = 0.5f;
        //    //perkPanel.VAlign = 0.5f;
        //    //Append(perkPanel);
        //}

        public void Open()
        {
            // TO-DO: setup
        }

        //public void CreatePerkPanel(Stand stand)
        //{
        //}
    }
}

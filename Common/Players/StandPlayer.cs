using JTA.Common.Stands;
using JTA.Common.Systems;
using JTA.Common.UI;
using JTA.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace JTA.Common.Players
{
    public class StandPlayer : ModPlayer
    {
        public const int UNSUMMONED = -999;
        public const string NOSTAND = "None";

        public int activeStandProjectile;
        public string stand;
        public int selectedAbilityIndex;

        public float damageReduction;

        /// <summary>
        /// IDs of active perks;
        /// </summary>
        public List<int> activePerks;

        public override void Initialize()
        {
            activePerks = [];
            activeStandProjectile = UNSUMMONED;
            stand = "Star Platinum";
            selectedAbilityIndex = 0;
        }

        public override void ResetEffects()
        {
            base.ResetEffects();
            if (activeStandProjectile != UNSUMMONED) {
                Projectile proj = Main.projectile[activeStandProjectile];
                if (proj.ModProjectile is StandProjectile stand)
                    if (stand.CurrentAnimation == "Block")
                        Player.controlUseItem = false;
            }
        }

        public override void PostUpdate()
        {
            base.PostUpdate();
            damageReduction = 0.0f;
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= (1.0f - damageReduction);
        }

        public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write((byte)PacketType.SyncPlayer);
            packet.Write((byte)Player.whoAmI);
            packet.Write((int)activeStandProjectile);
            packet.Write((int)selectedAbilityIndex);
            packet.Write(stand);
            packet.Send(toWho, fromWho);
        }

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (!IsStandUser)
                return;

            if(KeybindSystem.UseAbilityButton.JustPressed) {
                StandDefinitions.Stands[stand].Abilities[selectedAbilityIndex].Use(Player);
            }

            if (KeybindSystem.OpenAbilityMenu.JustPressed) {
                var ui = ModContent.GetInstance<AbilitySelectorUISystem>();
                ui.ToggleUI();
            }

            if (KeybindSystem.SummonStand.JustPressed) {
                if (activeStandProjectile == UNSUMMONED) {
                    // summon logic
                    activeStandProjectile = Projectile.NewProjectile(
                        Player.GetSource_FromThis("JTA: Summon"), 
                        Player.Center,
                        Vector2.Zero,
                        StandDefinitions.Stands[stand].SummonedStandId,
                        10,
                        1,
                        Main.myPlayer);
                }
            }
        }

        public void ReceiveSyncPlayer(BinaryReader reader)
        {
            activeStandProjectile = reader.ReadInt32();
            selectedAbilityIndex = reader.ReadInt32();
            stand = reader.ReadString();
        }

        public override void CopyClientState(ModPlayer targetCopy)
        {
            StandPlayer clone = (StandPlayer)targetCopy;
            clone.activeStandProjectile = activeStandProjectile;
            clone.selectedAbilityIndex = selectedAbilityIndex;
            clone.stand = stand;
        }

        public override void SendClientChanges(ModPlayer clientPlayer)
        {
            StandPlayer clone = (StandPlayer)clientPlayer;

            if (activeStandProjectile != clone.activeStandProjectile || selectedAbilityIndex != clone.selectedAbilityIndex || stand != clone.stand) {
                SyncPlayer(-1, Main.myPlayer, false);
            }
        }

        public static StandPlayer Get(Player player = null)
        {
            player ??= Main.LocalPlayer;

            return player.GetModPlayer<StandPlayer>();
        }

        public bool IsStandUser => stand != NOSTAND;

        public Projectile GetStandProjectile()
        {
            if (activeStandProjectile != UNSUMMONED)
                return Main.projectile[activeStandProjectile];

            return null;
        }
    }
}

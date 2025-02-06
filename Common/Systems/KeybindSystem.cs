using Microsoft.Xna.Framework.Input;
using Terraria.ModLoader;

namespace JTA.Common.Systems
{
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind UseAbilityButton { get; private set; }
        public static ModKeybind OpenAbilityMenu { get; private set; }
        public static ModKeybind SummonStand { get; private set; }


        public static ModKeybind ArrowUp { get; private set; }
        public static ModKeybind ArrowDown { get; private set; }
        public static ModKeybind ArrowLeft { get; private set; }
        public static ModKeybind ArrowRight { get; private set; }



        public override void Load()
        {
            SummonStand = KeybindLoader.RegisterKeybind(Mod, "Summon Stand", "Z");
            UseAbilityButton = KeybindLoader.RegisterKeybind(Mod, "Use Ability", "X");
            OpenAbilityMenu = KeybindLoader.RegisterKeybind(Mod, "Open Ability Menu", "F");

            ArrowUp = KeybindLoader.RegisterKeybind(Mod, "Menu Up", Keys.Up);
            ArrowDown = KeybindLoader.RegisterKeybind(Mod, "Menu Down", Keys.Down);
            ArrowLeft = KeybindLoader.RegisterKeybind(Mod, "Menu Left", Keys.Left);
            ArrowRight = KeybindLoader.RegisterKeybind(Mod, "Menu Right", Keys.Right);
        }

        public override void Unload()
        {
            UseAbilityButton = null;
            OpenAbilityMenu = null;
            SummonStand = null;

            ArrowDown = null;
            ArrowLeft = null;
            ArrowRight = null;
            ArrowUp = null;
        }

        public static int GetPressedAxis(ModKeybind negative, ModKeybind positive)
        {
            if (negative.JustPressed) return -1;
            if (positive.JustPressed) return 1;

            return 0;
        }
    }
}

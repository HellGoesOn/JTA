using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;

namespace JTA.Common.Stands
{
    public abstract class StandPerk
    {
        public int id;

        public PerkPath path;

        public bool active;

        public bool autoUnlock;

        public List<int> requiredPerks = [];

        public string name;

        public string description;

        public string texturePath;

        /// <summary>
        /// used to automatically place perk in the UI;
        /// </summary>
        public int tier;

        public StandPerk() 
        {
            tier = 0;
            path = PerkPath.Mid;
            name = "Please, give me a name!";
            description = "Ahoy, you forgot to set this!";
            texturePath = "JTA/Assets/Textures/NoPerkTexture";
        }

        public void UpdatePerk(Stand stand, Player player)
        {
            if (active)
                Update(stand, player);

            if (autoUnlock && !active && CanObtain(stand))
                active = true;
        }

        protected virtual void Update(Stand stand, Player player)
        {

        }

        public virtual void OnUnlock(Stand stand, Player player)
        {

        }

        public virtual void OnLose(Stand stand, Player player)
        {

        }

        public StandPerk GetCopy()
        {
            var thisType = GetType();
            var perk = (StandPerk)Activator.CreateInstance(thisType);

            var fields = thisType.GetFields(BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields) {
                field.SetValue(perk, field.GetValue(this));
            }

            return perk;
        }

        public bool CanObtain(Stand stand) => requiredPerks.Count <= 0 || stand.GetPerks().Count(x => x.active && requiredPerks.Contains(x.id)) >= requiredPerks.Count; 
    }

    public enum PerkPath
    {
        Top,
        Mid,
        Bot
    }
}

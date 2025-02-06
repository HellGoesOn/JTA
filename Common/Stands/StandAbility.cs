using System;
using System.Reflection;
using Terraria;

namespace JTA.Common.Stands
{
    public abstract class StandAbility
    {
        public string Name;
        public string Description;

        public virtual void UseAbility(Player player, Stand stand)
        {

        }

        public virtual void Update(Player player, Stand stand)
        {

        }

        public StandAbility GetCopy()
        {
            var thisType = GetType();
            var ability = (StandAbility)Activator.CreateInstance(thisType);

            var fields = thisType.GetFields(BindingFlags.Public);

            foreach (var field in fields) {
                field.SetValue(ability, field.GetValue(this));
            }

            return ability;
        }
    }
}

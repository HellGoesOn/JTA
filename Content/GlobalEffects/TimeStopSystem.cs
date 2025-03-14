using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Content.GlobalEffects
{
    // TO-DO: implement
    public class TimeStopSystem : ModSystem
    {
        List<TimeStopInstance> _list;

        public override void OnModLoad()
        {
            _list = [];
        }

        public static void StopTimeFor(Entity who, int howLong)
        {
            TimeStopSystem system = ModContent.GetInstance<TimeStopSystem>();
            TimeStopOwnerType type = TimeStopOwnerType.None;

            if(who is Player)
                type = TimeStopOwnerType.Player;
            if(who is NPC)
                type = TimeStopOwnerType.NPC;
            if(who is Projectile)
                type = TimeStopOwnerType.Projectile;

            system._list.Add(new(who.whoAmI, howLong, type));
        }

        public override void PreUpdateTime()
        {
            base.PreUpdateTime();

            for(int i = _list.Count-1; i >= 0; i--) {
                TimeStopInstance instance = _list[i];

                if(--instance.timeLeft <= 0)
                    _list.RemoveAt(i);
            }
        }
    }

    public class TimeStopInstance
    {
        public int owner;
        public int timeLeft;
        public TimeStopOwnerType ownerType;

        public TimeStopInstance(int owner, int timeLeft, TimeStopOwnerType ownerType)
        {
            this.owner = owner;
            this.timeLeft = timeLeft;
            this.ownerType = ownerType;
        }
    }

    public enum TimeStopOwnerType
    {
        None,
        Player,
        NPC,
        Projectile
    }
}

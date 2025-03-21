using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace JTA.Common.Systems
{
    // TO-DO: implement
    public class TimeStopSystem : ModSystem
    {
        List<TimeStopInstance> _list;

        public override void OnModLoad()
        {
            _list = [];
        }

        public static TimeStopSystem Get() => ModContent.GetInstance<TimeStopSystem>();

        public static void StopTimeFor(Entity who, int howLong)
        {
            TimeStopSystem system = ModContent.GetInstance<TimeStopSystem>();
            TimeStopOwnerType type = TimeStopOwnerType.None;

            if (who is Player)
                type = TimeStopOwnerType.Player;
            if (who is NPC)
                type = TimeStopOwnerType.NPC;
            if (who is Projectile)
                type = TimeStopOwnerType.Projectile;

            system._list.Add(new(who.whoAmI, howLong, type));
        }

        public override void PreUpdateTime()
        {
            base.PreUpdateTime();

            if (IsTimeStopped)
                Main.time -= Main.dayRate;

            for (int i = _list.Count - 1; i >= 0; i--)
            {
                TimeStopInstance instance = _list[i];
                if (--instance.timeLeft <= 0)
                    _list.RemoveAt(i);
            }
        }

        public bool DidIStopTime(Entity whoAmI) 
        {
            TimeStopOwnerType type = TimeStopOwnerType.None;

            if (whoAmI is Player)
                type = TimeStopOwnerType.Player;

            if(whoAmI is NPC)
                type = TimeStopOwnerType.NPC;

            if (whoAmI is Projectile)
                type = TimeStopOwnerType.Projectile;

            return _list.Any(x => x.ownerType == type && whoAmI.whoAmI == x.owner);
        }

        public bool IsTimeStopped => _list.Count > 0;
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

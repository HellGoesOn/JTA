using System;
using JTA.Common.Stands;

namespace JTA.Common
{
    public class LearnableAbility(StandAbility ability, Func<bool> condition)
    {
        public StandAbility abilityToLearn = ability;
        public Func<bool> condition = condition;
    }
}

using System.Collections.Generic;

namespace JTA.Content
{
    public class Stand
    {
        public int SummonedStandId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<StandAbility> Abilities { get; set; }

        public StandAI standAI;

        public Stand(int summonedStandId, string name, string desc = "") 
        {
            SummonedStandId = summonedStandId;
            Name = name;
            Description = desc;
            Abilities = [];
            standAI = null;
        }
    }
}

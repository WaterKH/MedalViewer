using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MedalViewer.Medal
{
    public class MedalCombatAbility
    {

        public string Direction { get; set; }
        public string Attribute { get; set; }
        public string Tier { get; set; }
        public string Duration { get; set; } // Turn(s)/ Attack(s)

        //public MedalCombatAbility() { }

        //public MedalCombatAbility(string[] ability)
        //{
        //    Direction = ability[0];
        //    Attribute = ability[1];
        //    Tier = ability[2];
        //    Duration = ability[3];
        //}

        //public MedalCombatAbility(string dir, string attr, string tier, string dur)
        //{
        //    Direction = dir;
        //    Attribute = attr;
        //    Tier = tier;
        //    Duration = dur;
        //}
    }
}

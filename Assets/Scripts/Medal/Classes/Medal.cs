using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedalViewer.Medal
{
    public class Medal
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public int Star { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public string Attribute_PSM { get; set; }
        public string Attribute_UR { get; set; }

        public string Discriminator { get; set; }

        public int BaseAttack { get; set; }
        public int MaxAttack { get; set; }
        public int BaseDefense { get; set; }
        public int MaxDefense { get; set; }
        public int TraitSlots { get; set; }
        public int BasePetPoints { get; set; }
        public int MaxPetPoints { get; set; }
        public string Ability { get; set; }
        public string AbilityDescription { get; set; }
        public string Target { get; set; }
        public int Gauge { get; set; }
        public double BaseMultiplierLow { get; set; }
        public double BaseMultiplierHigh { get; set; }
        public double MaxMultiplierLow { get; set; }
        public double MaxMultiplierHigh { get; set; }
        public double GuiltMultiplierLow { get; set; }
        public double GuiltMultiplierHigh { get; set; }
        public double SubslotMultiplier { get; set; }
        public int Tier { get; set; }
        public string SupernovaName { get; set; }
        public string SupernovaDescription { get; set; }
        public string SupernovaDamage { get; set; }
        public string SupernovaTarget { get; set; }

        public string Effect { get; set; }
        public string Effect_Description { get; set; }

        public Medal() { }

        public Medal(string[] medalInfo)
        {
            Id = string.IsNullOrEmpty(medalInfo[0]) ? -1 : int.Parse(medalInfo[0]);
            Name = string.IsNullOrEmpty(medalInfo[1]) ? "" : medalInfo[1];
            ImageURL = string.IsNullOrEmpty(medalInfo[2]) ? "" : medalInfo[2];
            Star = string.IsNullOrEmpty(medalInfo[3]) ? 0 : int.Parse(medalInfo[3]);
            Class = string.IsNullOrEmpty(medalInfo[4]) ? "" : medalInfo[4];
            Type = string.IsNullOrEmpty(medalInfo[5]) ? "" : medalInfo[5];
            Attribute_PSM = string.IsNullOrEmpty(medalInfo[6]) ? "" : medalInfo[6];
            Attribute_UR = string.IsNullOrEmpty(medalInfo[7]) ? "" : medalInfo[7];
            BaseAttack = string.IsNullOrWhiteSpace(medalInfo[8]) ? 0 : int.Parse(medalInfo[8]);
            MaxAttack = string.IsNullOrEmpty(medalInfo[9]) ? 0 : int.Parse(medalInfo[9]);
            BaseDefense = string.IsNullOrEmpty(medalInfo[10]) ? 0 : int.Parse(medalInfo[10]);
            MaxDefense = string.IsNullOrEmpty(medalInfo[11]) ? 0 : int.Parse(medalInfo[11]);
            TraitSlots = string.IsNullOrEmpty(medalInfo[12]) ? 0 : int.Parse(medalInfo[12]);
            BasePetPoints = string.IsNullOrEmpty(medalInfo[13]) ? 0 : int.Parse(medalInfo[13]);
            MaxPetPoints = string.IsNullOrEmpty(medalInfo[14]) ? 0 : int.Parse(medalInfo[14]);
            Ability = string.IsNullOrEmpty(medalInfo[15]) ? "" : medalInfo[15];
            AbilityDescription = string.IsNullOrEmpty(medalInfo[16]) ? "" : medalInfo[16];
            Target = string.IsNullOrEmpty(medalInfo[17]) ? "" : medalInfo[17];
            Gauge = string.IsNullOrEmpty(medalInfo[18]) ? 0 : int.Parse(medalInfo[18]);
            BaseMultiplierLow = string.IsNullOrEmpty(medalInfo[19]) ? 0.0 : double.Parse(medalInfo[19]);
            BaseMultiplierHigh = string.IsNullOrEmpty(medalInfo[20]) ? 0.0 : double.Parse(medalInfo[20]);
            MaxMultiplierLow = string.IsNullOrEmpty(medalInfo[21]) ? 0.0 : double.Parse(medalInfo[21]);
            MaxMultiplierHigh = string.IsNullOrEmpty(medalInfo[22]) ? 0.0 : double.Parse(medalInfo[22]);
            SubslotMultiplier = string.IsNullOrEmpty(medalInfo[23]) ? 0.0 : double.Parse(medalInfo[23]);
            Tier = string.IsNullOrEmpty(medalInfo[24]) ? 0 : int.Parse(medalInfo[24]);
            SupernovaName = string.IsNullOrEmpty(medalInfo[25]) ? "" : medalInfo[25];
            SupernovaDescription = string.IsNullOrEmpty(medalInfo[26]) ? "" : medalInfo[26];
            SupernovaDamage = string.IsNullOrEmpty(medalInfo[27]) ? "" : medalInfo[27];
            SupernovaTarget = string.IsNullOrEmpty(medalInfo[28]) ? "" : medalInfo[28];
            Effect_Description = string.IsNullOrEmpty(medalInfo[29]) ? "" : medalInfo[29];

            GuiltMultiplierLow = MaxMultiplierLow != 0 ? MaxMultiplierLow * Constants.TierConversion[Tier] :
                                                   BaseMultiplierLow * Constants.TierConversion[Tier];
            GuiltMultiplierHigh = MaxMultiplierHigh != 0 ? MaxMultiplierHigh * Constants.TierConversion[Tier] :
                                       BaseMultiplierHigh * Constants.TierConversion[Tier];

            GuiltMultiplierLow = Math.Round(GuiltMultiplierLow, 2);
            GuiltMultiplierHigh = Math.Round(GuiltMultiplierHigh, 2);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalDisplay : MonoBehaviour
    {
        public int Id;
        public string Name;
        public string ImageURL;
        public int Star;
        public string Attribute_PSM;
        public string Attribute_UR;
        public int BaseStrength;
        public int MaxStrength;
        public int BaseDefense;
        public int MaxDefense;
        public int TraitSlots;
        public int BasePetPoints;
        public int MaxPetPoints;
        public string Ability;
        public string AbilityDescription;
        public string BaseMultiplierLow;
        public string BaseMultiplierHigh;
        public string Target;
        public string MaxMultiplierLow;
        public string MaxMultiplierHigh;
        public int Gauge;
        public string GuiltMultiplierLow;
        public string GuiltMultiplierHigh;
        public int Tier;
        public double SubslotMultiplier;

        public bool IsSupernova;
        public string SupernovaName;
        public string SupernovaDescription;
        public string SupernovaDamage;
        public string SupernovaTarget;

        public string Effect;
        public string Effect_Description;

        public void MapVariables(Medal medal)
        {
            Id = medal.Id;
            Name = medal.Name;
            ImageURL = medal.ImageURL;
            Star = medal.Star;
            Attribute_PSM = medal.Attribute_PSM;
            Attribute_UR = medal.Attribute_UR;

            if (medal.Star == 1 || medal.Star == 2)
            {
                var convertedMedal = medal;

                BaseStrength = convertedMedal.BaseAttack;
                MaxStrength = convertedMedal.MaxAttack;
                BaseDefense = convertedMedal.BaseDefense;
                MaxDefense = convertedMedal.MaxDefense;
                TraitSlots = convertedMedal.TraitSlots;
                BasePetPoints = convertedMedal.BasePetPoints;
                MaxPetPoints = convertedMedal.MaxPetPoints;
                Ability = convertedMedal.Ability;
                AbilityDescription = convertedMedal.AbilityDescription;
                BaseMultiplierLow = convertedMedal.BaseMultiplierLow.ToString();
                BaseMultiplierHigh = convertedMedal.BaseMultiplierHigh.ToString();
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
                Tier = convertedMedal.Tier;
            }
            else if (medal.Star == 3 || medal.Star == 4 || medal.Star == 5)
            {
                var convertedMedal = medal;

                BaseStrength = convertedMedal.BaseAttack;
                MaxStrength = convertedMedal.MaxAttack;
                BaseDefense = convertedMedal.BaseDefense;
                MaxDefense = convertedMedal.MaxDefense;
                TraitSlots = convertedMedal.TraitSlots;
                BasePetPoints = convertedMedal.BasePetPoints;
                MaxPetPoints = convertedMedal.MaxPetPoints;
                Ability = convertedMedal.Ability;
                AbilityDescription = convertedMedal.AbilityDescription;
                BaseMultiplierLow = convertedMedal.BaseMultiplierLow.ToString();
                BaseMultiplierHigh = convertedMedal.BaseMultiplierHigh.ToString();
                MaxMultiplierLow = convertedMedal.MaxMultiplierLow.ToString();
                MaxMultiplierHigh = convertedMedal.MaxMultiplierHigh.ToString();
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
                Tier = convertedMedal.Tier;
            }
            else if (medal.Star == 6)
            {
                var convertedMedal = medal;

                BaseStrength = convertedMedal.BaseAttack;
                MaxStrength = convertedMedal.MaxAttack;
                BaseDefense = convertedMedal.BaseDefense;
                MaxDefense = convertedMedal.MaxDefense;
                TraitSlots = convertedMedal.TraitSlots;
                BasePetPoints = convertedMedal.BasePetPoints;
                MaxPetPoints = convertedMedal.MaxPetPoints;
                Ability = convertedMedal.Ability;
                AbilityDescription = convertedMedal.AbilityDescription;
                BaseMultiplierLow = convertedMedal.BaseMultiplierLow.ToString();
                BaseMultiplierHigh = convertedMedal.BaseMultiplierHigh.ToString();
                MaxMultiplierLow = convertedMedal.MaxMultiplierLow.ToString();
                MaxMultiplierHigh = convertedMedal.MaxMultiplierHigh.ToString();
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
                GuiltMultiplierLow = convertedMedal.GuiltMultiplierLow.ToString();
                GuiltMultiplierHigh = convertedMedal.GuiltMultiplierHigh.ToString();
                Tier = convertedMedal.Tier;
                
                if (!string.IsNullOrEmpty(convertedMedal.SupernovaName))
                {
                    IsSupernova = true;
                    SupernovaName = convertedMedal.SupernovaName;
                    SupernovaDescription = convertedMedal.SupernovaDescription;
                    SupernovaDamage = convertedMedal.SupernovaDamage;
                    SupernovaTarget = convertedMedal.SupernovaTarget;
                }
            }
            else if (medal.Star == 7)
            {
                var convertedMedal = medal;

                BaseStrength = convertedMedal.BaseAttack;
                MaxStrength = convertedMedal.MaxAttack;
                BaseDefense = convertedMedal.BaseDefense;
                MaxDefense = convertedMedal.MaxDefense;
                TraitSlots = convertedMedal.TraitSlots;
                BasePetPoints = convertedMedal.BasePetPoints;
                MaxPetPoints = convertedMedal.MaxPetPoints;
                Ability = convertedMedal.Ability;
                AbilityDescription = convertedMedal.AbilityDescription;
                MaxMultiplierLow = convertedMedal.MaxMultiplierLow.ToString();
                MaxMultiplierHigh = convertedMedal.MaxMultiplierHigh.ToString();
                Target = convertedMedal.Target;
                GuiltMultiplierLow = convertedMedal.GuiltMultiplierLow.ToString();
                GuiltMultiplierHigh = convertedMedal.GuiltMultiplierHigh.ToString();
                Gauge = convertedMedal.Gauge;
                SubslotMultiplier = convertedMedal.SubslotMultiplier;
                Tier = convertedMedal.Tier;

                if (!string.IsNullOrEmpty(convertedMedal.SupernovaName))
                {
                    IsSupernova = true;
                    SupernovaName = convertedMedal.SupernovaName;
                    SupernovaDescription = convertedMedal.SupernovaDescription;
                    SupernovaDamage = convertedMedal.SupernovaDamage;
                    SupernovaTarget = convertedMedal.SupernovaTarget;
                }
            }
            else
            {
                var convertedMedal = medal;

                Effect = convertedMedal.Effect;
                Effect_Description = convertedMedal.Effect_Description;
            }
        }
    }
}

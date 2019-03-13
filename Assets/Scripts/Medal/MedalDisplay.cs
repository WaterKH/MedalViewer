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
        public string BaseMultiplier;
        public string Target;
        public string MaxMultiplier;
        public int Gauge;
        public string GuiltMultiplier;
        public int Tier;
        public string SubslotMultiplier;

        public bool IsSupernova;
        public string SupernovaDescription;

        public string Effect;
        public string Effect_Description;

        public void MapVariables(Medal medal)//, TextAsset MedalData)
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
                BaseMultiplier = convertedMedal.BaseMultiplierHigh != "" ? $"x{convertedMedal.BaseMultiplierLow} - {convertedMedal.BaseMultiplierHigh}" : $"x{convertedMedal.BaseMultiplierLow}";
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
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
                BaseMultiplier = convertedMedal.BaseMultiplierHigh != "" ? $"x{convertedMedal.BaseMultiplierLow} - {convertedMedal.BaseMultiplierHigh}" : $"x{convertedMedal.BaseMultiplierLow}";
                MaxMultiplier = convertedMedal.MaxMultiplierHigh != "" ? $"x{convertedMedal.MaxMultiplierLow} - {convertedMedal.MaxMultiplierHigh}" : $"x{convertedMedal.MaxMultiplierLow}";
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
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
                BaseMultiplier = convertedMedal.BaseMultiplierHigh != "" ? $"x{convertedMedal.BaseMultiplierLow} - {convertedMedal.BaseMultiplierHigh}" : $"x{convertedMedal.BaseMultiplierLow}";
                MaxMultiplier = convertedMedal.MaxMultiplierHigh != "" ? $"x{convertedMedal.MaxMultiplierLow} - {convertedMedal.MaxMultiplierHigh}" : $"x{convertedMedal.MaxMultiplierLow}";
                Target = convertedMedal.Target;
                Gauge = convertedMedal.Gauge;
                GuiltMultiplier = convertedMedal.GuiltMultiplierHigh != "" ? $"x{convertedMedal.GuiltMultiplierLow} - {convertedMedal.GuiltMultiplierHigh}" : $"x{convertedMedal.GuiltMultiplierLow}";
                Tier = convertedMedal.Tier;

                if (!string.IsNullOrEmpty(SupernovaDescription))
                {
                    IsSupernova = true;
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
                MaxMultiplier = convertedMedal.MaxMultiplierHigh != "" ? $"x{convertedMedal.MaxMultiplierLow} - {convertedMedal.MaxMultiplierHigh}" : $"x{convertedMedal.MaxMultiplierLow}";
                Target = convertedMedal.Target;
                GuiltMultiplier = convertedMedal.GuiltMultiplierHigh != "" ? $"x{convertedMedal.GuiltMultiplierLow} - {convertedMedal.GuiltMultiplierHigh}" : $"x{convertedMedal.GuiltMultiplierLow}";
                Gauge = convertedMedal.Gauge;
                SubslotMultiplier = convertedMedal.SubslotMultiplier;
                Tier = convertedMedal.Tier;

                if (!string.IsNullOrEmpty(SupernovaDescription))
                {
                    IsSupernova = true;
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

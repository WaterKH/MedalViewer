using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MedalViewer.Medal
{
    public class MedalFilterManager
    {
        #region PSMUR

        public bool Power { get; set; }
        public bool Speed { get; set; }
        public bool Magic { get; set; }
        public bool Upright { get; set; }
        public bool Reversed { get; set; }

        #endregion

        #region Star

        public bool OneStar { get; set; }
        public bool TwoStar { get; set; }
        public bool ThreeStar { get; set; }
        public bool FourStar { get; set; }
        public bool FiveStar { get; set; }
        public bool SixStar { get; set; }
        public bool SevenStar { get; set; }

        #endregion

        #region Type

        public bool Attack { get; set; }
        public bool EXP { get; set; }
        public bool Boost { get; set; }
        public bool Cost { get; set; }
        public bool Evolve { get; set; }
        public bool Sell { get; set; }

        #endregion

        #region Tier

        public bool Tier1 { get; set; }
        public bool Tier2 { get; set; }
        public bool Tier3 { get; set; }
        public bool Tier4 { get; set; }
        public bool Tier5 { get; set; }
        public bool Tier6 { get; set; }
        public bool Tier7 { get; set; }
        public bool Tier8 { get; set; }
        public bool Tier9 { get; set; }
        public bool Tier10 { get; set; }

        #endregion

        #region Target

        public bool Single { get; set; }
        public bool All { get; set; }
        public bool Random { get; set; }

        #endregion

        #region Range

        public int LowRange { get; set; }
        public int HighRange { get; set; }

        #endregion

        public List<int> Tiers = new List<int>();

        // TODO Add more filters, but will need to rework the database if we want to filter by Boosts/Saps/Heals/ Etc

        #region Boosts & Saps

        public bool GeneralAttackBoost { get; set; }
        public bool PowerAttackBoost { get; set; }
        public bool SpeedAttackBoost { get; set; }
        public bool MagicAttackBoost { get; set; }
        public bool UprightAttackBoost { get; set; }
        public bool ReversedAttackBoost { get; set; }
        public bool GeneralAttackSap { get; set; }
        public bool PowerAttackSap { get; set; }
        public bool SpeedAttackSap { get; set; }
        public bool MagicAttackSap { get; set; }
        public bool GeneralDefenseBoost { get; set; }
        public bool PowerDefenseBoost { get; set; }
        public bool SpeedDefenseBoost { get; set; }
        public bool MagicDefenseBoost { get; set; }
        public bool GeneralDefenseSap { get; set; }
        public bool PowerDefenseSap { get; set; }
        public bool SpeedDefenseSap { get; set; }
        public bool MagicDefenseSap { get; set; }
        public bool UprightDefenseSap { get; set; }
        public bool ReversedDefenseSap { get; set; }

        #endregion

        private int currentLowestRange = 1;
        private int currentHighestRange = 81;


        // Static singleton instance
        private static MedalFilterManager instance;

        // Static singleton property
        public static MedalFilterManager Instance => instance ?? (instance = new MedalFilterManager());

    //private void Awake()
        //{
        //    DefaultFilters();
        //}

        public MedalFilterManager()
        {
            this.DefaultFilters();
        }

        public void DefaultFilters()
        {
            Power = true;
            Speed = true;
            Magic = true;
            Reversed = true;
            Upright = true;

            SixStar = true;
            SevenStar = true;

            Attack = true;

            //Tier3 = true;
            //Tier5 = true;
            //Tier6 = true;
            Tier7 = true;
            Tier8 = true;
            Tier9 = true;
            Tier10 = true;

            Single = true;
            All = true;
            Random = true;

            LowRange = 30;
            HighRange = 81;

            Tiers.Clear();
            Tiers.AddRange(new int[] { 7, 8, 9, 10 });
        }

        public string GenerateFilterQuery()
        {
            Tiers.Clear();

            var selections = "MU.Id MUId, MU.Name MUName, MU.Image, MU.Star, CTL.Class Class, CTL.Type Type, AL.PSM PSM, AL.UR UR, MU.BaseAttack, MU.MaxAttack, MU.BaseDefense, MU.MaxDefense, MU.TraitSlots, " +
                "PPL.BasePoints, PPL.MaxPoints, MU.Ability, MU.AbilityDescription, MU.Target MUTarget, MU.Gauge, MU.BaseMultiplierLow, MU.BaseMultiplierHigh, MU.MaxMultiplierLow, MU.MaxMultiplierHigh, " +
                "MU.SubslotMultiplier, MU.Tier, SL.Name SLName, SL.Description SLDescription, SL.Damage SLDamage, SL.Target SLTarget, EL.Description ELDescription";

            var query = $"Select {selections} From MedalUpdated MU, AttributeLookup AL, ClassTypeLookup CTL, PetPointsLookup PPL, SupernovaLookup SL, EffectLookup EL, TierLookup TL WHERE " +
                $"(AL.Id = MU.AttributeId AND CTL.Id = MU.ClassTypeId AND PPL.Id = MU.PetPointsId AND SL.Id = MU.SupernovaId AND EL.Id = MU.EffectId AND TL.Id = MU.Tier) AND ";

            var psm = "";
            #region PSM
            if (Power)
                psm += @" AL.PSM = 'Power'";

            if (Speed)
                psm += string.IsNullOrEmpty(psm) ? @" AL.PSM = 'Speed'" : @" OR AL.PSM = 'Speed'";

            if (Magic)
                psm += string.IsNullOrEmpty(psm) ? @" AL.PSM = 'Magic'" : @" OR AL.PSM = 'Magic'";
            #endregion
            query += string.IsNullOrEmpty(psm) ? "" : $"({psm}) AND ";

            var ur = "";
            #region UR
            if (Upright)
                ur += @" AL.UR = 'Upright'";

            if (Reversed)
                ur += string.IsNullOrEmpty(ur) ? @" AL.UR = 'Reversed'" : @" OR AL.UR = 'Reversed'";
            #endregion
            query += string.IsNullOrEmpty(ur) ? "" : $"({ur}) AND ";

            var star = "";
            #region Star
            if (OneStar)
                star += @" Star = 1";

            if (TwoStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 2" : @" OR Star = 2";

            if (ThreeStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 3" : @" OR Star = 3";

            if (FourStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 4" : @" OR Star = 4";

            if (FiveStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 5" : @" OR Star = 5";

            if (SixStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 6" : @" OR Star = 6";

            if (SevenStar)
                star += string.IsNullOrEmpty(star) ? @" Star = 7" : @" OR Star = 7";
            #endregion
            query += string.IsNullOrEmpty(star) ? "" : $"({star}) AND ";

            var type = "";
            #region Type
            if (Attack)
                type += @" Type = 'Attack'";

            if (EXP)
                type += string.IsNullOrEmpty(type) ? @" Type = 'EXP'" : @" OR Type = 'EXP'";

            if (Boost)
                type += string.IsNullOrEmpty(type) ? @" Type = 'Boost'" : @" OR Type = 'Boost'";

            if (Cost)
                type += string.IsNullOrEmpty(type) ? @" Type = 'Cost'" : @" OR Type = 'Cost'";

            if (Evolve)
                type += string.IsNullOrEmpty(type) ? @" Type = 'Evolve'" : @" OR Type = 'Evolve'";

            if (Sell)
                type += string.IsNullOrEmpty(type) ? @" Type = 'Sell'" : @" OR Type = 'Sell'";
            #endregion
            query += string.IsNullOrEmpty(type) ? "" : $"({type}) AND ";

            // TODO Is adding the tiers into the Tiers variable appropriate here?
            var tier = "";
            #region Tier
            if (Tier1)
            {
                tier += " Tier = 1";
                Tiers.Add(1);
            }

            if (Tier2)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 2" : @" OR Tier = 2";
                Tiers.Add(2);
            }

            if (Tier3)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 3" : @" OR Tier = 3";
                Tiers.Add(3);
            }

            if (Tier4)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 4" : @" OR Tier = 4";
                Tiers.Add(4);
            }

            if (Tier5)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 5" : @" OR Tier = 5";
                Tiers.Add(5);
            }

            if (Tier6)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 6" : @" OR Tier = 6";
                Tiers.Add(6);
            }

            if (Tier7)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 7" : @" OR Tier = 7";
                Tiers.Add(7);
            }

            if (Tier8)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 8" : @" OR Tier = 8";
                Tiers.Add(8);
            }

            if (Tier9)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 9" : @" OR Tier = 9";
                Tiers.Add(9);
            }

            if (Tier10)
            {
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 10" : @" OR Tier = 10";
                Tiers.Add(10);
            }
            #endregion
            query += string.IsNullOrEmpty(tier) ? "" : $"({tier}) AND ";
            if (string.IsNullOrEmpty(tier))
                Tiers.AddRange(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            var target = "";
            #region Target
            if (Single)
                target += " MU.Target = 'Single'";

            if (All)
                target += string.IsNullOrEmpty(target) ? @" MU.Target = 'All'" : @" OR MU.Target = 'All'";

            if (Random)
                target += string.IsNullOrEmpty(target) ? @" MU.Target = 'Random'" : @" OR MU.Target = 'Random'";
            #endregion
            query += string.IsNullOrEmpty(target) ? "" : $"({target}) AND ";

            var range = "";
            #region Range
            if (LowRange >= currentLowestRange && HighRange <= currentHighestRange)
                range += $"(((BaseMultiplierLow * TL.Multiplier Between {LowRange} AND {HighRange}) OR (BaseMultiplierHigh * TL.Multiplier Between {LowRange} AND {HighRange})) OR " +
                    $"((MaxMultiplierLow * TL.Multiplier Between {LowRange} AND {HighRange}) OR (MaxMultiplierHigh * TL.Multiplier Between {LowRange} AND {HighRange})))";// OR" +
                    //$"(GuiltMultiplierLow Between {LowRange} AND {HighRange}) OR (GuiltMultiplierHigh Between {LowRange} AND {HighRange})";
            //else
                // TODO Do something for fixed damage here?
            #endregion
            query += string.IsNullOrEmpty(range) ? "" : $"({range})";

            var querySplit = query.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var checkResult = querySplit[querySplit.Length - 1];

            if (checkResult == "Where" || checkResult == "AND")
                query = query.Substring(0, query.Length - checkResult.Length - 1);
            //Debug.Log(checkResult);
            Debug.Log(query);
            return query;
        }
    }
}
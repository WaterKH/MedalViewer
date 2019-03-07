using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public class MedalFilter : MonoBehaviour
    {
    #region PSMUR

    public bool Power = false;
        public bool Speed = false;
    public bool Magic = false;
    public bool Upright = false;
    public bool Reversed = false;

    #endregion

    #region Star

    public bool OneStar = false;
    public bool TwoStar = false;
    public bool ThreeStar = false;
    public bool FourStar = false;
    public bool FiveStar = false;
    public bool SixStar = false;
    public bool SevenStar = false;

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

        public bool Tier1 = false;
    public bool Tier2 = false;
    public bool Tier3 = false;
    public bool Tier4 = false;
    public bool Tier5 = false;
    public bool Tier6 = false;
    public bool Tier7 = false;
    public bool Tier8 = false;
    public bool Tier9 = false;

    #endregion

    #region Target

    public bool Single = false;
    public bool All = false;
    public bool Random = false;

    #endregion

    // TODO Add more filters, but will need to rework the database if we want to filter by Boosts/Saps/Heals/ Etc
    #region Boosts & Saps

    public bool GeneralAttackBoost = false;
    public bool PowerAttackBoost = false;
    public bool SpeedAttackBoost = false;
    public bool MagicAttackBoost = false;
    public bool UprightAttackBoost = false;
    public bool ReversedAttackBoost = false;
    public bool GeneralAttackSap = false;
    public bool PowerAttackSap = false;
    public bool SpeedAttackSap = false;
    public bool MagicAttackSap = false;
    public bool GeneralDefenseBoost = false;
    public bool PowerDefenseBoost = false;
    public bool SpeedDefenseBoost = false;
    public bool MagicDefenseBoost = false;
    public bool GeneralDefenseSap = false;
    public bool PowerDefenseSap = false;
    public bool SpeedDefenseSap = false;
    public bool MagicDefenseSap = false;
    public bool UprightDefenseSap = false;
    public bool ReversedDefenseSap = false;

    #endregion


    public void GenerateFilterQuery()
        {
            var query = "Select * From Medal Where ";

            var psm = "";
            #region PSM
            if (Power)
                psm += @" AttributePSM = 'Power'";

            if(Speed)
                psm += string.IsNullOrEmpty(psm) ? @" AttributePSM = 'Speed'" : @" OR AttributePSM = 'Speed'";

            if (Magic)
                psm += string.IsNullOrEmpty(psm) ? @" AttributePSM = 'Magic'" : @" OR AttributePSM = 'Magic'";
            #endregion
            query += string.IsNullOrEmpty(psm) ? "" : $"{psm} AND ";

            var ur = "";
            #region UR
            if (Upright)
                ur += @" AttributeUR = 'Upright'";

            if (Reversed)
                ur += string.IsNullOrEmpty(ur) ? @" AttributeUR = 'Reversed'" : @" OR AttributeUR = 'Reversed'";
            #endregion
            query += string.IsNullOrEmpty(ur) ? "" : $"{ur} AND ";

            var star = "";
            #region Star
            if(OneStar)
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
            query += string.IsNullOrEmpty(star) ? "" : $"{star} AND ";

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
            query += string.IsNullOrEmpty(type) ? "" : $"{type} AND ";

            var tier = "";
            #region Tier
            if (Tier1)
                tier += " Tier = 1";

            if (Tier2)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 2" : @" OR Tier = 2";

            if (Tier3)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 3" : @" OR Tier = 3";

            if (Tier4)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 4" : @" OR Tier = 4";
            
            if (Tier5)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 5" : @" OR Tier = 5";

            if (Tier6)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 6" : @" OR Tier = 6";

            if (Tier7)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 7" : @" OR Tier = 7";

            if (Tier8)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 8" : @" OR Tier = 8";

            if (Tier9)
                tier += string.IsNullOrEmpty(tier) ? @" Tier = 9" : @" OR Tier = 9";
            #endregion
            query += string.IsNullOrEmpty(tier) ? "" : $"{tier} AND ";

            var target = "";
            #region Target
            if (Single)
                target += " Target = 'Single'";

            if (All)
                target += string.IsNullOrEmpty(target) ? @" Target = 'All'" : @" OR Target = 'All'";

            if (Random)
                target += string.IsNullOrEmpty(target) ? @" Target = 'Random'" : @" OR Target = 'Random'";
            #endregion
            query += string.IsNullOrEmpty(target) ? "" : $"{target}";


            var querySplit = query.Split(' ');
            var checkResult = querySplit[querySplit.Length - 2];

            if(checkResult == "Where" || checkResult == "AND")
                query = query.Substring(0, query.Length - checkResult.Length - 1);
        Debug.Log(checkResult);
        Debug.Log(query);
            //return query;
        }
    }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MedalViewer.Medal
{
    public class MedalAbility
    {

        //public int ID = 0;
        public List<MedalCombatAbility> STR = new List<MedalCombatAbility>();
        public List<MedalCombatAbility> DEF = new List<MedalCombatAbility>();


        public string Deal = "";
        public string IgnoreAttributes = "";

        public string Inflicts = "";
        //public string MoreDamage = ""; // Only Critical Hit for now
        public string DamagePlus = "";

        public string DispelEnemy = "";
        public string DispelPlayer = "";

        public string Heal = "";
        public string Esuna = "";

        public string Gauge = "";

        public string Count = "";

        public string Copy = "";

        public string SPBonus = "";

        public string NextMedal = "";

        public string Mirrors = "";


        // Key: STR/DEF - Key: Raise/Lower/LowerPlayer - Value: Images
        public Dictionary<string, Dictionary<string, List<Texture2D>>> CombatImages;
        public Dictionary<string, Texture2D> MiscImages;

        public void SetUpDisplayAbility()
        {
            InitImages();

            foreach (var str in STR)
            {
                var str_image = Resources.Load(ImagePaths.CombatPaths["STR"][str.Direction][str.Attribute]) as Texture2D;
                CombatImages["STR"][str.Direction].Add(str_image);
            }

            foreach (var def in DEF)
            {
                var def_image = Resources.Load(ImagePaths.CombatPaths["DEF"][def.Direction][def.Attribute]) as Texture2D;
                CombatImages["DEF"][def.Direction].Add(def_image);
            }

            if (Inflicts != "")
            {
                var infl_image = Resources.Load(ImagePaths.MiscPaths["INFL"][Inflicts]) as Texture2D;
                MiscImages["INFL"] = infl_image;
            }

            if (DamagePlus != "")
            {
                var dam_image = Resources.Load(ImagePaths.MiscPaths["DAMAGE+"][DamagePlus]) as Texture2D;
                MiscImages["DAMAGE+"] = dam_image;
            }

            if (DispelEnemy != "")
            {
                var dispel_image = Resources.Load(ImagePaths.MiscPaths["DISPEL"][DispelEnemy]) as Texture2D;
                MiscImages["DISPELENEMY"] = dispel_image;
            }

            if (DispelPlayer != "")
            {
                var dispel_image = Resources.Load(ImagePaths.MiscPaths["DISPEL"][DispelPlayer]) as Texture2D;
                MiscImages["DISPELPLAYER"] = dispel_image;
            }

            if (Mirrors != "")
            {
                var mirror_image = Resources.Load(ImagePaths.MiscPaths["MIRROR"][Mirrors]) as Texture2D;
                MiscImages["MIRROR"] = mirror_image;
            }

            if (Heal != "")
            {
                var heal_image = Resources.Load(ImagePaths.MiscPaths["HEAL"][Heal]) as Texture2D;
                MiscImages["HEAL"] = heal_image;
            }

            if (Gauge != "")
            {
                var gauge_image = Resources.Load(ImagePaths.MiscPaths["GAUGE"][Gauge]) as Texture2D;
                MiscImages["GAUGE"] = gauge_image;
            }

            if (Esuna != "")
            {
                var esuna_image = Resources.Load(ImagePaths.MiscPaths["ESUNA"][Esuna]) as Texture2D;
                MiscImages["ESUNA"] = esuna_image;
            }

            if (Count != "")
            {
                var count_image = Resources.Load(ImagePaths.MiscPaths["COUNT"][Count]) as Texture2D;
                MiscImages["COUNT"] = count_image;
            }

            if (SPBonus != "")
            {
                var spbonus_image = Resources.Load(ImagePaths.MiscPaths["SPBONUS"][SPBonus]) as Texture2D;
                MiscImages["SPBONUS"] = spbonus_image;
            }

            if (NextMedal != "")
            {
                var next_image = Resources.Load(ImagePaths.MiscPaths["NEXTMEDAL"][NextMedal]) as Texture2D;
                MiscImages["NEXTMEDAL"] = next_image;
            }

            if(IgnoreAttributes != "")
            {
                var next_image = Resources.Load(ImagePaths.MiscPaths["IGNORE"][IgnoreAttributes]) as Texture2D;
                MiscImages["IGNORE"] = next_image;
            }

            if(Copy != "")
            {
                var copy_image = Resources.Load(ImagePaths.MiscPaths["COPY"][Copy]) as Texture2D;
                MiscImages["COPY"] = copy_image;
            }
        }

        public void InitImages()
        {
            CombatImages = new Dictionary<string, Dictionary<string, List<Texture2D>>>
            {
                { "STR", new Dictionary<string, List<Texture2D>>() }
            };
            CombatImages["STR"].Add("Raises", new List<Texture2D>());
            CombatImages["STR"].Add("Lowers", new List<Texture2D>());

            CombatImages.Add("DEF", new Dictionary<string, List<Texture2D>>());
            CombatImages["DEF"].Add("Raises", new List<Texture2D>());
            CombatImages["DEF"].Add("Lowers", new List<Texture2D>());
            CombatImages["DEF"].Add("PlayerLowers", new List<Texture2D>()); // TODO Redesign backend to account for player

            MiscImages = new Dictionary<string, Texture2D>
            {
                { "INFL", null },
                { "HEAL", null },
                { "GAUGE", null },
                { "ESUNA", null },
                { "COUNT", null },
                { "DAMAGE+", null },
                { "NEXTMEDAL", null },
                { "SPBONUS", null },
                { "DISPELPLAYER", null },
                { "DISPEL", null },
                { "MIRROR", null },
                { "IGNORE", null },
                { "COPY", null }
            };
        }
    }
}

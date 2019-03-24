using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalSpotlightDisplayManager : MonoBehaviour
    {
        public TraitManager TraitManager;
        MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();
        public Loading Loading;

        public CanvasGroup MedalHighlight;
        public CanvasGroup MedalSupernova;
        public CanvasGroup MedalTraits;
        public CanvasGroup MedalSkills;

        public CanvasGroup Player;
        public CanvasGroup Enemy;
        public CanvasGroup Effects;
        public CanvasGroup Guilt;

        #region Icons

        public RawImage PSMAttribute;
        public RawImage URAttribute;
        public RawImage Tier;
        public RawImage Target;
        public RawImage Gauges;

        public Color32 Reversed;// = new Color(171, 0, 255, 255);
        public Color32 Upright;// = new Color(255f, 255f, 0f, 255f);

        #endregion

        #region MedalName

        public Text MedalName;

        #endregion

        #region Content
        
        #region Player
        
        public Text PlayerTurns;

        public RawImage[] PlayerAttack;
        public Text[] PlayerAttackMults;
        public RawImage[] PlayerDefense;
        public Text[] PlayerDefenseMults;
        
        #endregion

        #region Enemy
        
        public Text EnemyTurns;

        public RawImage[] EnemyAttack;
        public Text[] EnemyAttackMults;
        public RawImage[] EnemyDefense;
        public Text[] EnemyDefenseMults;

        #endregion
        
        #region Effects

        public RawImage[] EffectImages;
        public Text[] EffectTexts;

        #endregion

        #region Skill

        public RawImage SkillImage;
        public Text SkillText;

        public Dictionary<string, string> SkillConversion = new Dictionary<string, string>
        {
            { "Attack Boost All I", "1.2" }, {"Attack Boost All II", "1.4" },

            { "Attack Boost I", "1.2" }, { "Attack Boost II", "1.4" }, { "Attack Boost III", "1.6" },
            { "Attack Boost IV", "1.8" }, { "Attack Boost V", "2.0" }, { "Attack Boost VI", "2.2" },
            { "Attack Boost VII", "2.4" }, { "Attack Boost VIII", "2.6" }, { "Attack Boost IX", "2.8" },

            { "Attack Boost II & AP+", "1.4" }, { "Attack Boost III & AP+", "1.6" },
            { "Attack Boost VI & AP+", "2.2" }, { "Attack Boost IX & AP+", "2.8" },

            { "Attack Boost II & LP+", "1.4" }, { "Attack Boost III & LP+", "1.6" },
            { "Attack Boost IV & LP+", "1.8" }, { "Attack Boost V & LP+", "2.0" }, { "Attack Boost VI & LP+", "2.2" },
            { "Attack Boost VII & LP+", "2.4" }, { "Attack Boost VIII & LP+", "2.6" },

            { "Attack Boost III Max & GA1/2", "1.6" },
            { "Attack Boost IV Max & GA0/1/2", "1.8" }, { "Attack Boost V Max & GA0/1/2", "2.0" }, { "Attack Boost VI Max & GA0/1/2", "2.2" },
            { "Attack Boost VII Max & GA0/1/2", "2.4" }, { "Attack Boost VIII Max & GA0/1/2", "2.6" },

            { "Null", "" }
        };

        #endregion

        #region Traits

        //public CanvasGroup Traits;
        public RawImage[] TraitSlots;
        //public Text[] TraitSlotsText;
        public RawImage CurrentTraitSlot;
        
        public RawImage InitialImage;

        public Dictionary<string, string> TraitConversion = new Dictionary<string, string>
        {
            { "Ground Enemy DEF -60%", "-60%G" }, { "Aerial Enemy DEF -60%", "-60%A" },
            { "Damage in Raids +40%", "+40%R" }, { "Extra Attack: 40% Power", "+40%EA"},
            { "DEF +2000", "+2000D" }, { "STR +1000", "+1000S"},
            { "Null", "" }
        };

        #endregion Traits

        #region Stats

        public Text Defense;
        public Slider DefenseSlider;
        public Text AddedDefense;

        public Text Strength;
        public Slider StrengthSlider;
        public Text AddedStrength;

        public Text Multiplier;
        public Button SwapMultiplier;
        public Text MultiplierIdentifier;

        public Button[] MultiplierOrbs;
        public Button[] GuiltButtons;
        public Slider GuiltSlider;
        public Text GuiltValue;

        public Tuple<int, int>[] GuiltByTier = { 
            Tuple.Create(10, 25),
            Tuple.Create(20, 50),
            Tuple.Create(40, 100),
            Tuple.Create(60, 130),
            Tuple.Create(80, 150),
            Tuple.Create(100, 180),
            Tuple.Create(120, 200),
            Tuple.Create(150, 230),
            Tuple.Create(200, 280)
        };

        public string BaseMultiplierLow;
        public string BaseMultiplierHigh;
        public string MaxMultiplierLow;
        public string MaxMultiplierHigh;
        public string GuiltMultiplierLow;
        public string GuiltMultiplierHigh;

        #endregion

        #region Vars

        public Text Deals;
        public Text SPABonus;
        public Text Skill;
        public Text[] TraitValues;

        public int CurrSPIndex = 0;
        public string[] SPABonusValues = new string[]
        {
            "0", "15", "20", "30", "40", "60", "70", "80", "90",
            "100", "110", "120", "130", "140", "150", "160", "170", "180", "190",
            "200", "210", "260"
        };

        //public SPATKBonusManager SPATKBonusManager; // Needed?

        #endregion

        #region Total
        
        public Text FinalDamageOutput;
        
        #endregion

        #endregion

        #region MedalInfo

        public CanvasGroup SupernovaButton;
        public RawImage MedalPlaceholder;
        public RawImage MedalPlaceholderShadow;
        public Text Ability;
        public Text AbilityDescription;

        #endregion

        #region Colors

        public Color NormalColor;
        public Color SelectedColor;
        public Color MultiplierOn;
        public Color MultiplierOff;

        #endregion



        #region Private 

        private readonly string url = "https://medalviewer.blob.core.windows.net/images/";

        private Transform prevParent;
        private float prevScale;

        private float elapsedTime = 0.0f;

        private bool isTransition = false;

        //private bool isDisplaying = false;
        private bool isDisplayingSupernova = false;
        private bool isDisplayingSkills = false;
        private bool isDisplayingTraits = false;

        private Color32 visible = new Color(1f, 1f, 1f, 1f);
        private Color32 invisible = new Color(0f, 0f, 0f, 0f);
        
        //private string initialTraitSlotText = "N/A";

        #endregion
            

        #region Assign Attributes

        private void AssignIcons(MedalDisplay medalDisplay)
        {
            PSMAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_PSM + "_Gem") as Texture2D;
            URAttribute.color = medalDisplay.Attribute_UR == "Upright" ? Upright : Reversed;

            Tier.texture = Resources.Load("Tier/Black/" + medalDisplay.Tier) as Texture2D;
            Target.texture = Resources.Load("Target/" + medalDisplay.Target) as Texture2D;
            Gauges.texture = Resources.Load("Gauges/" + medalDisplay.Gauge) as Texture2D;
        }

        private void AssignMedalName(MedalDisplay medalDisplay)
        {
            MedalName.text = medalDisplay.Name;
        }

        private void AssignContent(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            AssignPlayerEnemy(medalAbility);
            AssignEffects(medalAbility);

            AssignSkill();
            AssignTraits(medalDisplay);
            AssignStats(medalDisplay);
            //AssignSlots(medalDisplay);

            AssignVars(medalDisplay, medalAbility);
            AssignTotal();
        }

        #region AssignContent

        private void AssignMedalInfo(MedalDisplay medalDisplay)
        {
            var imageUrl = url + medalDisplay.ImageURL;
            StartCoroutine(LoadImage(imageUrl, MedalPlaceholder.gameObject));
            
            Ability.text = medalDisplay.Ability;
            AbilityDescription.text = medalDisplay.AbilityDescription;

            if (medalDisplay.IsSupernova)
            {
                SupernovaButton.interactable = true;
                SupernovaButton.blocksRaycasts = true;
                SupernovaButton.alpha = 1;
            }
        }

        private void AssignPlayerEnemy(MedalAbility medalAbility)
        {
            var strPlayerCounter = 0;
            var defPlayerCounter = 0;
            var strEnemyCounter = 0;
            var defEnemyCounter = 0;

            var counterSTR = 0;
            var counterDEF = 0;

            foreach (var strDef in medalAbility.CombatImages)
            {
                foreach (var raiseLower in strDef.Value)
                {
                    if (raiseLower.Key == "Raises")
                    {
                        if (strDef.Key == "STR")
                        {
                            foreach (var strRaise in raiseLower.Value)
                            {
                                PlayerAttack[strPlayerCounter].texture = strRaise;
                                PlayerAttack[strPlayerCounter].color = visible;
                                PlayerAttackMults[strPlayerCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defRaise in raiseLower.Value)
                            {
                                PlayerDefense[defPlayerCounter].texture = defRaise;
                                PlayerDefense[defPlayerCounter].color = visible;
                                PlayerDefenseMults[defPlayerCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                    else if (raiseLower.Key == "Lowers")
                    {
                        if (strDef.Key == "STR")
                        {
                            foreach (var strLower in raiseLower.Value)
                            {
                                EnemyAttack[strEnemyCounter].texture = strLower;
                                EnemyAttack[strEnemyCounter].color = visible;
                                EnemyAttackMults[strEnemyCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defLower in raiseLower.Value)
                            {
                                EnemyDefense[defEnemyCounter].texture = defLower;
                                EnemyDefense[defEnemyCounter].color = visible;
                                EnemyDefenseMults[defEnemyCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                }
            }

            // TODO Account for player saps
            
            if (CheckPlayer())
                Player.SetCanvasGroupActive();

            if (CheckEnemy())
                Enemy.SetCanvasGroupActive();
        }

        private void AssignEffects(MedalAbility medalAbility)
        {
            var effectCounter = 0;

            foreach (var effect in medalAbility.MiscImages)
            {
                if (effect.Value == null)
                    continue;

                EffectImages[effectCounter].enabled = true;
                EffectImages[effectCounter++].texture = effect.Value;
            }

            if (CheckEffects())
                Effects.SetCanvasGroupActive();
        }

        private void AssignSkill()
        {
            SkillImage.enabled = true;
        }

        private void AssignTraits(MedalDisplay medalDisplay)
        {
            //yield return null;
            for (int i = 0; i < medalDisplay.TraitSlots; ++i)
            {
                TraitSlots[i].enabled = true;
                //TraitSlotsText[i].enabled = true;
            }
        }

        private void AssignStats(MedalDisplay medalDisplay/*, MedalAbility medalAbility*/)
        {
            Defense.text = medalDisplay.BaseDefense.ToString();
            Strength.text = medalDisplay.BaseDefense.ToString();

            DefenseSlider.minValue = medalDisplay.BaseDefense;
            DefenseSlider.maxValue = medalDisplay.MaxDefense;
            StrengthSlider.minValue = medalDisplay.BaseStrength;
            StrengthSlider.maxValue = medalDisplay.MaxStrength;

            DefenseSlider.value = medalDisplay.BaseDefense;
            StrengthSlider.value = medalDisplay.BaseStrength;

            BaseMultiplierLow = medalDisplay.BaseMultiplierLow;
            BaseMultiplierHigh = medalDisplay.BaseMultiplierHigh;
            MaxMultiplierLow = medalDisplay.MaxMultiplierLow;
            MaxMultiplierHigh = medalDisplay.MaxMultiplierHigh;
            GuiltMultiplierLow = medalDisplay.GuiltMultiplierLow;
            GuiltMultiplierHigh = medalDisplay.GuiltMultiplierHigh;
            
            if(Effects.alpha != 0)
            {
                SwapMultiplier.GetComponent<Image>().enabled = true;
                SwapMultiplier.enabled = true;
            }

            switch (medalDisplay.Star)
            {
                case 1:
                case 2:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;
                    
                    break;
                case 3:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 1; ++i)
                    {
                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 4:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 2; ++i)
                    {
                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 5:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 3; ++i)
                    {
                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 6:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 5; ++i)
                    {
                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }

                    GuiltButtons[0].enabled = true;
                    GuiltButtons[0].GetComponent<Image>().enabled = true;

                    GuiltSlider.minValue = GuiltByTier[medalDisplay.Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[medalDisplay.Tier - 1].Item2;

                    Guilt.alpha = 1;
                    Guilt.interactable = true;
                    Guilt.blocksRaycasts = true;

                    GuiltValue.text = GuiltSlider.minValue.ToString();
                    
                    break;
                case 7:
                    Multiplier.text = medalDisplay.MaxMultiplierLow;

                    for (int i = 0; i < 5; ++i)
                    {
                        var colors = MultiplierOrbs[i].colors;
                        colors.normalColor = SelectedColor;
                        MultiplierOrbs[i].colors = colors;

                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    GuiltButtons[1].enabled = true;
                    GuiltButtons[1].GetComponent<Image>().enabled = true;

                    GuiltSlider.minValue = GuiltByTier[medalDisplay.Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[medalDisplay.Tier - 1].Item2;

                    Guilt.alpha = 1;
                    Guilt.interactable = true;
                    Guilt.blocksRaycasts = true;

                    GuiltValue.text = GuiltSlider.minValue.ToString();
                    
                    break;
                default:
                    break;
            }
        }

        private void AssignVars(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            Deals.text = medalAbility.Deal != "" ? medalAbility.Deal : "1";
            SPABonus.text = medalAbility.SPBonus != "" ? medalAbility.SPBonus : "0";
        }

        private void AssignTotal()
        {
            UpdateTotal();
        }

        #endregion

        #endregion Assign Attributes

        #region Update Attributes

        public void UpdateSkill(GameObject value)
        {
            SkillImage.texture = value.GetComponent<RawImage>().texture;
            SkillText.text = value.name;

            Skill.text = SkillConversion[value.name];
            
            UpdateTotal();

            HideSkills();
        }

        public void UpdateTrait(GameObject value)
        {
            CurrentTraitSlot.texture = value.GetComponent<RawImage>().texture;

            var index = int.Parse(CurrentTraitSlot.name.Split('_')[1]);
            TraitValues[index].text = TraitConversion[value.name];

            UpdateTotal();

            HideTraits();
        }

        public void UpdateStrength()
        {
            Strength.text = StrengthSlider.value.ToString();

            if (StrengthSlider.value != 0)
            {
                UpdateTotal();
            }
        }

        public void UpdateAddedStrength(int value)
        {
            var strength = int.Parse(AddedStrength.text);
            if ((strength + value) >= 0 && (strength + value) <= 1000)
            {
                AddedStrength.text = (strength + value).ToString();
            }

            UpdateTotal();
        }

        public void UpdateDefense()
        {
            Defense.text = DefenseSlider.value.ToString();
        }

        public void UpdateAddedDefense(int value)
        {
            var defense = int.Parse(AddedDefense.text);
            if ((defense + value) >= 0 && (defense + value) <= 1000)
            {
                AddedDefense.text = (defense + value).ToString();
            }
        }

        public void UpdateMultiplier()
        {
            var colors = SwapMultiplier.colors;

            if (MultiplierIdentifier.text == "Base")
            {
                MultiplierIdentifier.text = "Max";

                colors.normalColor = MultiplierOn;
                colors.highlightedColor = MultiplierOff;

                if(GuiltButtons[0].enabled == true)
                {
                    if(MultiplierOrbs[4].enabled == true)
                    {
                        Multiplier.text = MaxMultiplierHigh.ToString();
                    }
                    else
                    {
                        Multiplier.text = BaseMultiplierHigh.ToString();
                    }
                }
                else
                {
                    Multiplier.text = GuiltMultiplierHigh.ToString();
                }
            }
            else
            {
                MultiplierIdentifier.text = "Base";
                
                colors.normalColor = MultiplierOff;
                colors.highlightedColor = MultiplierOn;

                if (GuiltButtons[0].enabled == true)
                {
                    if (MultiplierOrbs[4].enabled == true)
                    {
                        Multiplier.text = MaxMultiplierLow.ToString();
                    }
                    else
                    {
                        Multiplier.text = BaseMultiplierLow.ToString();
                    }
                }
                else
                {
                    Multiplier.text = GuiltMultiplierLow.ToString();
                }
            }
            
            SwapMultiplier.colors = colors;

            UpdateTotal();
        }

        public void UpdateMultiplierOrb(int orbIndex)
        {
            for(int i = orbIndex; i < MultiplierOrbs.Length; ++i)
            {
                var colors = MultiplierOrbs[i].colors;
                colors.normalColor = NormalColor;
                MultiplierOrbs[i].colors = colors;
            }

            for(int i = 0; i < orbIndex; ++i)
            {
                var colors = MultiplierOrbs[i].colors;
                colors.normalColor = SelectedColor;
                MultiplierOrbs[i].colors = colors;
            }

            UpdateTotal();
        }

        public void UpdateGuilt()
        {
            if(GuiltButtons[0].enabled == true)
            {
                foreach(var orb in MultiplierOrbs)
                {
                    var colors = orb.colors;
                    colors.normalColor = SelectedColor;
                    orb.colors = colors;
                }

                GuiltButtons[0].enabled = false;
                GuiltButtons[0].GetComponent<Image>().enabled = false;

                GuiltButtons[1].enabled = true;
                GuiltButtons[2].enabled = true;
                GuiltButtons[1].GetComponent<Image>().enabled = true;
                GuiltButtons[2].GetComponent<Image>().enabled = true;

                if (SwapMultiplier.colors.normalColor == SelectedColor)
                    Multiplier.text = GuiltMultiplierHigh;
                else
                    Multiplier.text = GuiltMultiplierLow;

                GuiltSlider.interactable = true;
            }
            else if (GuiltButtons[1].enabled == true || GuiltButtons[2].enabled == true)
            {
                GuiltButtons[0].enabled = true;
                GuiltButtons[0].GetComponent<Image>().enabled = true;

                GuiltButtons[1].enabled = false;
                GuiltButtons[2].enabled = false;
                GuiltButtons[1].GetComponent<Image>().enabled = false;
                GuiltButtons[2].GetComponent<Image>().enabled = false;

                if (SwapMultiplier.colors.normalColor == SelectedColor)
                    Multiplier.text = MaxMultiplierHigh;
                else
                    Multiplier.text = MaxMultiplierLow;

                GuiltSlider.interactable = false;
            }

            UpdateTotal();
        }

        public void UpdateGuiltSliderValue()
        {
            GuiltValue.text = $"{GuiltSlider.value.ToString()}%";

            //GuiltButtons[1].GetComponent<CanvasGroup>().alpha = 

            if (GuiltSlider.value != 0)
            {
                UpdateTotal();
            }
        }

        public void UpdateSPBonus(int value)
        {
            if ((CurrSPIndex + value) >= 0 && (CurrSPIndex + value) < SPABonusValues.Length)
            {
                SPABonus.text = SPABonusValues[(CurrSPIndex + value)];
                CurrSPIndex += value;
            }

            UpdateTotal();
        }

        public void UpdateTotal()
        {
            if (Strength.text.Length == 0 || Multiplier.text.Length == 0 || Deals.text.Length == 0 || SPABonus.text.Length == 0)
                return;

            var str = StrengthSlider.value;//int.Parse(Strength.text);
            var addedStr = int.Parse(AddedStrength.text);
            var mult = float.Parse(Multiplier.text);
            var guilt = GuiltSlider.value;
            var deals = int.Parse(Deals.text);
            var spBonus = int.Parse(SPABonus.text);
            var skill = Skill.text.Length > 0 ? float.Parse(Skill.text) : 0.0f;
            //var trait1 = 0.0f;
            //var trait2 = 0.0f;
            //var trait3 = 0.0f;
            //var trait4 = 0.0f;
            //var trait5 = 0.0f;
            
            var totalRaids = 0.0f;
            var extraAttack = 0.0f;

            var totalStr = str + addedStr;

            if (TraitValues[0].text.Length > 2)
            {
                if (TraitValues[0].text[TraitValues[0].text.Length - 1] == 'S')
                {
                    totalStr += 1000;/* = TraitValues[0].text.Contains("%") ? float.Parse($".{TraitValues[0].text.Substring(1, 2)}") : TraitValues[0].text.Length > 0 ? int.Parse(TraitValues[0].text.Substring(1, 4)) : 0;*/
                }
                else if(TraitValues[0].text[TraitValues[0].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if(TraitValues[0].text.Substring(TraitValues[0].text.Length - 2) == "EA")
                {
                    extraAttack = 0.40f;
                }
            }

            if (TraitValues[1].text.Length > 2)
            {
                if (TraitValues[1].text[TraitValues[1].text.Length - 1] == 'S')
                {
                    totalStr += 1000;/* = TraitValues[0].text.Contains("%") ? float.Parse($".{TraitValues[0].text.Substring(1, 2)}") : TraitValues[0].text.Length > 0 ? int.Parse(TraitValues[0].text.Substring(1, 4)) : 0;*/
                }
                else if (TraitValues[1].text[TraitValues[1].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if (TraitValues[1].text.Substring(TraitValues[1].text.Length - 2) == "EA")
                {
                    extraAttack = 0.40f;
                }
            }

            if (TraitValues[2].text.Length > 2)
            {
                if (TraitValues[2].text[TraitValues[2].text.Length - 1] == 'S')
                {
                    totalStr += 1000;/* = TraitValues[0].text.Contains("%") ? float.Parse($".{TraitValues[0].text.Substring(1, 2)}") : TraitValues[0].text.Length > 0 ? int.Parse(TraitValues[0].text.Substring(1, 4)) : 0;*/
                }
                else if (TraitValues[2].text[TraitValues[2].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if (TraitValues[2].text.Substring(TraitValues[2].text.Length - 2) == "EA")
                {
                    extraAttack = 0.40f;
                }
            }

            if (TraitValues[3].text.Length > 2)
            {
                if (TraitValues[3].text[TraitValues[3].text.Length - 1] == 'S')
                {
                    totalStr += 1000;/* = TraitValues[0].text.Contains("%") ? float.Parse($".{TraitValues[0].text.Substring(1, 2)}") : TraitValues[0].text.Length > 0 ? int.Parse(TraitValues[0].text.Substring(1, 4)) : 0;*/
                }
                else if (TraitValues[3].text[TraitValues[3].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if (TraitValues[3].text.Substring(TraitValues[3].text.Length - 2) == "EA")
                {
                    extraAttack = 0.40f;
                }
            }

            if (TraitValues[4].text.Length > 2)
            {
                if (TraitValues[4].text[TraitValues[4].text.Length - 1] == 'S')
                {
                    totalStr += 1000;/* = TraitValues[0].text.Contains("%") ? float.Parse($".{TraitValues[0].text.Substring(1, 2)}") : TraitValues[0].text.Length > 0 ? int.Parse(TraitValues[0].text.Substring(1, 4)) : 0;*/
                }
                else if (TraitValues[4].text[TraitValues[4].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if (TraitValues[4].text.Substring(TraitValues[4].text.Length - 2) == "EA")
                {
                    extraAttack = 0.40f;
                }
            }
            
            //if (trait1 == 1000.0f)
            //    totalStr += (int)trait1;
            //if (trait2 == 1000.0f)
            //    totalStr += (int)trait2;
            //if (trait3 == 1000.0f)
            //    totalStr += (int)trait3;
            //if (trait4 == 1000.0f)
            //    totalStr += (int)trait4;
            //if (trait5 == 1000.0f)
            //    totalStr += (int)trait5;
            
            var totalMult = guilt != 0.0f && spBonus != 0 ? mult * ((guilt + spBonus + 100) / 100) : 
                            guilt != 0.0f ? mult * ((guilt + 100) / 100) :
                            spBonus != 0 ? mult * ((spBonus + 100) / 100) : 
                            mult;

            var totalMultTimesStrength = totalStr * totalMult;


            var total = skill != 0 && totalRaids != 0.0f && extraAttack != 0.0f ? deals * skill * (totalRaids + extraAttack + 1.0f) * totalMultTimesStrength :
                        skill != 0 && extraAttack != 0.0f ? deals * skill * (extraAttack + 1.0f) * totalMultTimesStrength :
                        totalRaids != 0.0f && extraAttack != 0.0f ? deals * (totalRaids + extraAttack + 1.0f) * totalMultTimesStrength :
                        skill != 0 ? deals * skill * totalMultTimesStrength :
                        totalRaids != 0.0f ? deals * (totalRaids + 1.0f) * totalMultTimesStrength :
                        extraAttack != 0.0f ? deals * (extraAttack + 1.0f) * totalMultTimesStrength :
                        deals * totalMultTimesStrength;

            FinalDamageOutput.text = String.Format("{0:#,#.##}", (int)Math.Ceiling(total));
        }
        
        #endregion

        #region Reset Attributes

        public void Reset()
        {
            ResetIcons();
            ResetMedalName();
            ResetMedalInfo();
            ResetContent();
        }

        private void ResetIcons()
        {
            PSMAttribute.texture = null;
            URAttribute.color = invisible;

            Tier.texture = null;
            Target.texture = null;
            Gauges.texture = null;
        }

        private void ResetMedalName()
        {
            MedalName.text = "PLACEHOLDER";
        }

        private void ResetMedalInfo()
        {
            MedalPlaceholder.texture = Resources.Load("Placeholder") as Texture2D;
            MedalPlaceholderShadow.texture = MedalPlaceholder.texture;
            
            Ability.text = "ABILITY";
            AbilityDescription.text = "ABILITY DESCRIPTION";

            SupernovaButton.interactable = false;
            SupernovaButton.blocksRaycasts = false;
            SupernovaButton.alpha = 0;
        }

        private void ResetContent()
        {
            ResetPlayerEnemy();
            ResetEffects();

            ResetSkill();
            ResetTraits();
            ResetStats();
            //ResetSlots();

            ResetVars();
            ResetTotal();
        }

        #region ResetContent

        public void ResetPlayerEnemy()
        {
            PlayerTurns.text = "";
            EnemyTurns.text = "";

            // Player
            foreach (var aB in PlayerAttack)
                aB.color = invisible;
            foreach (var aBM in PlayerAttackMults)
                aBM.text = "";

            foreach (var dB in PlayerDefense)
                dB.color = invisible;
            foreach (var dBM in PlayerDefenseMults)
                dBM.text = "";

            // Enemy
            foreach (var aS in EnemyAttack)
                aS.color = invisible;
            foreach (var aSM in EnemyAttackMults)
                aSM.text = "";

            foreach (var dS in EnemyDefense)
                dS.color = invisible;
            foreach (var dSM in EnemyDefenseMults)
                dSM.text = "";

            Player.SetCanvasGroupInactive();
            Enemy.SetCanvasGroupInactive();
        }

        public void ResetEffects()
        {
            foreach (var effect in EffectImages)
            {
                effect.texture = null;
                effect.enabled = false;
            }

            //foreach (var effectText in EffectTexts)
            //{
            //    effectText.text = "";
            //}

            Effects.SetCanvasGroupInactive();
        }

        public void ResetSkill()
        {
            SkillImage.texture = InitialImage.texture;
            SkillImage.enabled = false;

            SkillText.text = "";
        }

        public void ResetTraits()
        {
            for (int i = 0; i < TraitSlots.Length; ++i)
            {
                TraitSlots[i].texture = InitialImage.texture;
                TraitSlots[i].enabled = false;
            }
        }
        
        public void ResetStats()
        {
            Multiplier.text = "";
            Defense.text = "";
            Strength.text = "";
            
            var multColors = SwapMultiplier.colors;
            multColors.normalColor = MultiplierOff;
            SwapMultiplier.colors = multColors;
            SwapMultiplier.GetComponent<Image>().enabled = false;
            SwapMultiplier.enabled = false;

            DefenseSlider.value = 0;
            StrengthSlider.value = 0;
           
            MultiplierIdentifier.text = "Base";
            AddedDefense.text = "0";
            AddedStrength.text = "0";

            foreach (var orb in MultiplierOrbs)
            {
                var colors = orb.colors;
                colors.normalColor = NormalColor;
                orb.colors = colors;

                orb.enabled = false;
                orb.GetComponent<Image>().enabled = false;
            }
            
            foreach(var guilt in GuiltButtons)
            {
                guilt.GetComponent<Image>().enabled = false;
            }
            Guilt.alpha = 0;
            Guilt.interactable = false;
            Guilt.blocksRaycasts = false;
            GuiltSlider.value = 0;
            GuiltValue.text = "0%";
        }

        public void ResetVars()
        {
            Deals.text = "";
            SPABonus.text = "";
            Skill.text = "";

            foreach (var trait in TraitValues)
                trait.text = "";
        }

        public void ResetTotal()
        {
            FinalDamageOutput.text = "";
        }

        #endregion

        #endregion Reset Attributes

        #region Display Methods

        public void HandleDisplay(GameObject clickedOn)
        {
            if (clickedOn.transform.childCount > 1)
            {
                //DisplaySublistOfMedals(clickedOn);
            }
            else
            {
                StartCoroutine(Display(clickedOn.transform.GetChild(0).gameObject));
            }
        }
        
        public void HideCurrentMedal()
        {
            isTransition = true;
            //isDisplaying = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalHighlight));

            //TraitManager.HideToolBox();

            //Reset();
        }

        public void ShowSupernova()
        {
            isTransition = true;
            isDisplayingSupernova = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(MedalSupernova));
        }

        public void ShowSkills()
        {
            isTransition = true;
            isDisplayingSkills = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(MedalSkills));
        }

        public void ShowTraits(RawImage value)
        {
            isTransition = true;
            isDisplayingTraits = true;
            elapsedTime = 0.0f;

            CurrentTraitSlot = value;

            StartCoroutine(ShowDisplay(MedalTraits));
        }

        public void HideSupernova()
        {
            print("Hide Supernova");
            isTransition = true;
            isDisplayingSupernova = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalSupernova));
        }

        public void HideSkills()
        {
            print("Hide Skills");
            isTransition = true;
            isDisplayingSkills = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalSkills));
        }

        public void HideTraits()
        {
            print("Hide Traits");
            isTransition = true;
            isDisplayingTraits = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalTraits));
        }

        #endregion Display Methods

        #region Checks

        private bool CheckPlayer()
        {
            foreach (var aB in PlayerAttack)
            {
                if (aB.color == visible)
                    return true;
            }

            foreach (var dB in PlayerDefense)
            {
                if (dB.color == visible)
                    return true;
            }

            return false;
        }

        private bool CheckEnemy()
        {
            foreach (var aS in EnemyAttack)
            {
                if (aS.color == visible)
                    return true;
            }

            foreach (var dS in EnemyDefense)
            {
                if (dS.color == visible)
                    return true;
            }

            return false;
        }

        private bool CheckEffects()
        {
            foreach (var eff in EffectImages)
            {
                if (eff.enabled == true)
                    return true;
            }

            return false;
        }

        #endregion
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(this.isDisplayingSkills || this.isDisplayingTraits)
                {
                    if (this.isDisplayingSkills)
                        HideSkills();
                    else
                        HideTraits();
                }
                else if(this.isDisplayingSupernova)
                {
                    HideSupernova();
                }
                else
                {
                    HideCurrentMedal();
                }
            }
        }

        void Awake()
        {
            //background_group = GameObject.FindGameObjectWithTag("Background").GetComponent<CanvasGroup>();
            //		MedalSublistGroup = GameObject.FindGameObjectWithTag("MedalSublistGroup").GetComponent<CanvasGroup>();
            //MedalSublist = GameObject.FindGameObjectWithTag("MedalSublist");

            //medalCreator = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalCreator>();

            Reset();
        }

        public IEnumerator Display(GameObject medalObject)
        {
            yield return null;
            Loading.StartLoading();
            Reset();

            MedalDisplay medalDisplay = medalObject.GetComponent<MedalDisplay>();
            MedalAbility medalAbility = new MedalAbility();

            try
            {
                medalAbility = MedalAbilityParser.Parser(medalDisplay.AbilityDescription);
            }
            catch
            {
                print(medalDisplay.Name.Split(':')[0] + " " + medalDisplay.AbilityDescription);
            }

            #region Display Assignment

            AssignIcons(medalDisplay);
            AssignMedalName(medalDisplay);
            AssignMedalInfo(medalDisplay);

            if (medalAbility != null)
            {
                medalAbility.SetUpDisplayAbility();

                AssignContent(medalDisplay, medalAbility);
            }


            #endregion

            isTransition = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(MedalHighlight));
        }

        IEnumerator LoadImage(string imageUrl, GameObject medalObject)
        {
            Debug.Log(imageUrl);
            //yield return 0;
            UnityWebRequest image = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return image.SendWebRequest();
            if (image.isNetworkError || image.isHttpError)
                Debug.Log(image.error);
            else
            {
                medalObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)image.downloadHandler).texture;

                MedalPlaceholderShadow.texture = MedalPlaceholder.texture;
                print("Finished");
            }

            Loading.FinishLoading();
        }

        public IEnumerator ShowDisplay(CanvasGroup canvasGroup)
        {
            elapsedTime = 0;
            while (isTransition)
            {
                if (!Loading.IsLoading)
                {
                    elapsedTime += Time.deltaTime;
                    //print("Displaying");
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, elapsedTime);
                    canvasGroup.interactable = true;
                    canvasGroup.blocksRaycasts = true;

                    if (canvasGroup.alpha >= 1.0f)
                    {
                        elapsedTime = 0.0f;

                        //isDisplayVar = true;
                        isTransition = false;
                    }
                }
                yield return null;
            }
        }

        public IEnumerator HideDisplay(CanvasGroup canvasGroup)
        {
            while (isTransition)
            {
                elapsedTime += Time.deltaTime;
                //print("Hiding");
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, elapsedTime);
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;

                if (canvasGroup.alpha <= 0.0f)
                {
                    elapsedTime = 0.0f;

                    //isDisplaying = false;
                    isTransition = false;
                }
                yield return null;
            }
        }
    }
}

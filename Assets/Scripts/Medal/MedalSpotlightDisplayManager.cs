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
        MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();
        public Loading Loading;

        public CanvasGroup MedalHighlight;
        public CanvasGroup MedalSupernova;
        public CanvasGroup MedalTraits;
        public CanvasGroup MedalSkills;
        public CanvasGroup PetTraits;

        public CanvasGroup Player;
        public CanvasGroup Enemy;
        public CanvasGroup Effects;
        public CanvasGroup Guilt;

        public CanvasGroup SupernovaPlayer;
        public CanvasGroup SupernovaEnemy;
        public CanvasGroup SupernovaEffects;

        #region Icons

        public Text PetPointsBase;
        public Text PetPointsMax;
        public RawImage PSMAttribute;
        public RawImage URAttribute;
        public RawImage Tier;
        public RawImage Target;
        public RawImage Gauges;
        
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

        #region Pet Trait
        
        public RawImage PetTraitSlot;

        public Dictionary<string, string> PetTraitConversion = new Dictionary<string, string>
        {
            { "Ground Enemy DEF -20%", "-20%G" }, { "Aerial Enemy DEF -20%", "-20%A" },
            { "Damage in Raids +10%", "+10%R" }, { "Damage in Raids +20%", "+20%R" }, { "Damage in Raids +30%", "+30%R" }, { "Damage in Raids +100%", "+100%R" },
            { "Extra Attack: 120% Power", "+120%EA"},
            { "STR +50", "+50S"}, { "STR +100", "+100S"}, { "STR +200", "+200S"}, { "STR +400", "+400S"}, { "STR +500", "+500S"}, { "STR +2000", "+2000S"},
            { "DEF +200", "+200D" }, { "DEF +400", "+400D" }, { "DEF +600", "+600D" }, { "DEF +800", "+800D" }, { "DEF +1000", "+1000D" },
            { "Null", "" }
        };

        #endregion

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
        public RawImage SPABonus;
        public Slider SPABonusSlider;
        public Text Skill;
        public Text PetTraitValue;
        public Text[] TraitValues;

        //public int CurrSPIndex = 0;
        private List<string> SPABonusValues = new List<string>
        {
            "0", "15", "20", "30", "40", "60", "70", "80", "90",
            "100", "110", "120", "130", "140", "150", "160", "170", "180", "190",
            "200", "210", "250", "260"
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

        #region Supernova

        public Text SupernovaName;
        public Text SupernovaDescription;
        public RawImage SupernovaTarget;
        public Text SupernovaDamage;

        #region Player

        public Text SupernovaPlayerTurns;

        public RawImage[] SupernovaPlayerAttack;
        public Text[] SupernovaPlayerAttackMults;
        public RawImage[] SupernovaPlayerDefense;
        public Text[] SupernovaPlayerDefenseMults;

        #endregion

        #region Enemy

        public Text SupernovaEnemyTurns;

        public RawImage[] SupernovaEnemyAttack;
        public Text[] SupernovaEnemyAttackMults;
        public RawImage[] SupernovaEnemyDefense;
        public Text[] SupernovaEnemyDefenseMults;

        #endregion

        #region Effects

        public RawImage[] SupernovaEffectImages;
        //public Text[] SupernovaEffectTexts;

        #endregion

        #endregion


        #region Colors

        public Color NormalColor;
        public Color SelectedColor;
        public Color MultiplierOn;
        public Color MultiplierOff;

        #endregion
        
        #region Private 

        private readonly string url = "https://medalviewer.blob.core.windows.net/images/";

        private GameObject currSublistMedal;

        private Transform prevParent;
        private float prevScale;

        private float elapsedTime = 0.0f;

        private bool isTransition = false;

        private bool isDisplayingMedal = false;
        private bool isDisplayingSublist = false;
        private bool isDisplayingSupernova = false;
        private bool isDisplayingSkills = false;
        private bool isDisplayingTraits = false;
        private bool isDisplayingPetTraits = false;

        private Color32 visible = new Color(1f, 1f, 1f, 1f);
        private Color32 invisible = new Color(0f, 0f, 0f, 0f);
        
        //private string initialTraitSlotText = "N/A";

        #endregion
            

        #region Assign Attributes

        private void AssignIcons(MedalDisplay medalDisplay)
        {
            PetPointsBase.text = medalDisplay.BasePetPoints.ToString();
            PetPointsMax.text = medalDisplay.MaxPetPoints.ToString();

            PSMAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_PSM + "_Gem") as Texture2D;
            URAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_UR + "_Gem") as Texture2D;

            Tier.texture = Resources.Load("Tier/Black/" + medalDisplay.Tier) as Texture2D;
            Target.texture = Resources.Load("Target/" + medalDisplay.Target) as Texture2D;
            Gauges.texture = Resources.Load("Gauges/Uses/GU" + medalDisplay.Gauge) as Texture2D;
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
            AssignPetTrait();
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

        private void AssignSupernova(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            SupernovaName.text = medalDisplay.SupernovaName;
            SupernovaDescription.text = medalDisplay.SupernovaDescription;
            SupernovaDamage.text = medalDisplay.SupernovaDamage;

            SupernovaTarget.texture = Resources.Load($"Target/{medalDisplay.SupernovaTarget}") as Texture2D;

            AssignSupernovaPlayerEnemy(medalAbility);
            AssignSupernovaEffects(medalAbility);
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

        private void AssignPetTrait()
        {
            PetTraitSlot.enabled = true;
        }

        private void AssignStats(MedalDisplay medalDisplay/*, MedalAbility medalAbility*/)
        {
            Defense.text = medalDisplay.MaxDefense.ToString();
            Strength.text = medalDisplay.MaxStrength.ToString();

            DefenseSlider.minValue = medalDisplay.BaseDefense;
            DefenseSlider.maxValue = medalDisplay.MaxDefense == 0 ? medalDisplay.BaseDefense : medalDisplay.MaxDefense;
            StrengthSlider.minValue = medalDisplay.BaseStrength;
            StrengthSlider.maxValue = medalDisplay.MaxStrength == 0 ? medalDisplay.BaseStrength : medalDisplay.MaxStrength;

            DefenseSlider.value = medalDisplay.MaxDefense;
            StrengthSlider.value = medalDisplay.MaxStrength;

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

                // Change to Max 
                UpdateMultiplier();
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
                        var colors = MultiplierOrbs[i].colors;
                        colors.normalColor = SelectedColor;
                        MultiplierOrbs[i].colors = colors;

                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 4:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 2; ++i)
                    {
                        var colors = MultiplierOrbs[i].colors;
                        colors.normalColor = SelectedColor;
                        MultiplierOrbs[i].colors = colors;

                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 5:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 3; ++i)
                    {
                        var colors = MultiplierOrbs[i].colors;
                        colors.normalColor = SelectedColor;
                        MultiplierOrbs[i].colors = colors;

                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }
                    
                    break;
                case 6:
                    Multiplier.text = medalDisplay.BaseMultiplierLow;

                    for (int i = 0; i < 5; ++i)
                    {
                        var colors = MultiplierOrbs[i].colors;
                        colors.normalColor = SelectedColor;
                        MultiplierOrbs[i].colors = colors;

                        MultiplierOrbs[i].enabled = true;
                        MultiplierOrbs[i].GetComponent<Image>().enabled = true;
                    }

                    GuiltButtons[2].enabled = true;
                    GuiltButtons[2].GetComponent<RawImage>().enabled = true;
                    
                    GuiltButtons[0].GetComponent<RawImage>().texture = Resources.Load($"Tier/Inactive-Guilt/{medalDisplay.Tier}") as Texture2D;
                    GuiltButtons[1].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-White/{medalDisplay.Tier}") as Texture2D;
                    GuiltButtons[2].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-Black/{medalDisplay.Tier}") as Texture2D;

                    GuiltSlider.minValue = GuiltByTier[medalDisplay.Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[medalDisplay.Tier - 1].Item2;

                    GuiltSlider.value = GuiltSlider.maxValue;

                    Guilt.alpha = 1;
                    Guilt.interactable = true;
                    Guilt.blocksRaycasts = true;

                    GuiltValue.text = GuiltSlider.maxValue.ToString();
                    
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
                    
                    GuiltButtons[2].enabled = true;
                    GuiltButtons[2].GetComponent<RawImage>().enabled = true;
                    
                    GuiltButtons[0].GetComponent<RawImage>().texture = Resources.Load($"Tier/Inactive-Guilt/{medalDisplay.Tier}") as Texture2D;
                    GuiltButtons[1].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-White/{medalDisplay.Tier}") as Texture2D;
                    GuiltButtons[2].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-Black/{medalDisplay.Tier}") as Texture2D;

                    GuiltSlider.minValue = GuiltByTier[medalDisplay.Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[medalDisplay.Tier - 1].Item2;

                    GuiltSlider.value = GuiltSlider.maxValue;

                    Guilt.alpha = 1;
                    Guilt.interactable = true;
                    Guilt.blocksRaycasts = true;

                    GuiltValue.text = GuiltSlider.maxValue.ToString();
                    
                    break;
                default:
                    break;
            }
        }

        private void AssignVars(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            Deals.text = medalAbility.Deal != "" ? medalAbility.Deal : "1";
            SPABonusSlider.value = medalAbility.SPBonus != "" ? SPABonusValues.IndexOf(medalAbility.SPBonus) : 0;
            SPABonus.texture = Resources.Load($"SPATKBonus/Alt/{SPABonusValues[(int)SPABonusSlider.value]}A") as Texture2D;
        }

        private void AssignTotal()
        {
            UpdateTotal();
        }

        private void AssignSupernovaPlayerEnemy(MedalAbility medalAbility)
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
                                SupernovaPlayerAttack[strPlayerCounter].texture = strRaise;
                                SupernovaPlayerAttack[strPlayerCounter].color = visible;
                                SupernovaPlayerAttackMults[strPlayerCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defRaise in raiseLower.Value)
                            {
                                SupernovaPlayerDefense[defPlayerCounter].texture = defRaise;
                                SupernovaPlayerDefense[defPlayerCounter].color = visible;
                                SupernovaPlayerDefenseMults[defPlayerCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                    else if (raiseLower.Key == "Lowers")
                    {
                        if (strDef.Key == "STR")
                        {
                            foreach (var strLower in raiseLower.Value)
                            {
                                SupernovaEnemyAttack[strEnemyCounter].texture = strLower;
                                SupernovaEnemyAttack[strEnemyCounter].color = visible;
                                SupernovaEnemyAttackMults[strEnemyCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defLower in raiseLower.Value)
                            {
                                SupernovaEnemyDefense[defEnemyCounter].texture = defLower;
                                SupernovaEnemyDefense[defEnemyCounter].color = visible;
                                SupernovaEnemyDefenseMults[defEnemyCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                }
            }

            // TODO Account for player saps

            if (CheckSupernovaPlayer())
                SupernovaPlayer.SetCanvasGroupActive();

            if (CheckSupernovaEnemy())
                SupernovaEnemy.SetCanvasGroupActive();
        }

        private void AssignSupernovaEffects(MedalAbility medalAbility)
        {
            var effectCounter = 0;

            foreach (var effect in medalAbility.MiscImages)
            {
                if (effect.Value == null)
                    continue;

                SupernovaEffectImages[effectCounter].enabled = true;
                SupernovaEffectImages[effectCounter++].texture = effect.Value;
            }

            if (CheckSupernovaEffects())
                SupernovaEffects.SetCanvasGroupActive();
        }

        #endregion

        #endregion Assign Attributes

        #region Update Attributes

        public void UpdateSkill(GameObject value)
        {
            SkillImage.texture = value.GetComponent<RawImage>().texture;
            SkillText.text = value.name == "Null" ? "" : value.name;

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

        public void UpdatePetTrait(GameObject value)
        {
            PetTraitSlot.texture = value.GetComponent<RawImage>().texture;
            
            PetTraitValue.text = PetTraitConversion[value.name];

            UpdateTotal();

            HidePetTraits();
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
                        Multiplier.text = $"x{MaxMultiplierHigh}";
                    }
                    else
                    {
                        Multiplier.text = $"x{BaseMultiplierHigh}";
                    }
                }
                else
                {
                    Multiplier.text = $"x{GuiltMultiplierHigh}";
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
                        Multiplier.text = $"x{MaxMultiplierLow}";
                    }
                    else
                    {
                        Multiplier.text = $"x{BaseMultiplierLow}";
                    }
                }
                else
                {
                    Multiplier.text = $"x{GuiltMultiplierLow}";
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

        public void UpdateSPBonus()
        {
            SPABonus.texture = Resources.Load($"SPATKBonus/Alt/{SPABonusValues[(int)SPABonusSlider.value]}A") as Texture2D;

            UpdateTotal();
        }

        public void UpdateTotal()
        {
            if (Strength.text.Length == 0 || Multiplier.text.Length == 0 || Deals.text.Length == 0)
                return;

            var str = StrengthSlider.value;//int.Parse(Strength.text);
            var addedStr = int.Parse(AddedStrength.text);
            var mult = float.Parse(Multiplier.text);
            var guilt = GuiltSlider.value;
            var deals = int.Parse(Deals.text);
            var spBonus = int.Parse(SPABonusValues[(int)SPABonusSlider.value]);
            var skill = Skill.text.Length > 0 ? float.Parse(Skill.text) : 0.0f;
            
            var totalRaids = 0.0f;
            var extraAttack = 0.0f;

            var totalStr = str + addedStr;

            // TODO Add pet trait slot value

            if (TraitValues[0].text.Length > 2)
            {
                if (TraitValues[0].text[TraitValues[0].text.Length - 1] == 'S')
                {
                    totalStr += 1000;
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
                    totalStr += 1000;
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
                    totalStr += 1000;
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
                    totalStr += 1000;
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
                    totalStr += 1000;
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
            
            var totalMult = guilt != 0.0f && spBonus != 0 ? mult * ((guilt / 100) + 1.0f) * (((float)spBonus / 100) + 1.0f) :
                            guilt != 0.0f ? mult * ((guilt / 100) + 1.0f) :
                            spBonus != 0 ? mult * (((float)spBonus / 100) + 1.0f) :
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
            ResetSupernova();
        }

        private void ResetIcons()
        {
            PetPointsBase.text = "--";
            PetPointsMax.text = "--";

            PSMAttribute.texture = null;
            URAttribute.texture = null;

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
            ResetPetTrait();
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

        public void ResetPetTrait()
        {
            PetTraitSlot.texture = InitialImage.texture;
            PetTraitSlot.enabled = false;
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
                guilt.GetComponent<RawImage>().enabled = false;
                guilt.GetComponent<RawImage>().texture = null;
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
            SPABonus.texture = null;
            SPABonusSlider.value = 0;
            Skill.text = "";
            PetTraitValue.text = "";

            foreach (var trait in TraitValues)
                trait.text = "";
        }

        public void ResetTotal()
        {
            FinalDamageOutput.text = "";
        }

        #endregion

        private void ResetSupernova()
        {
            SupernovaName.text = "";
            SupernovaDescription.text = "";
            SupernovaDamage.text = "";
            SupernovaTarget.texture = null;

            ResetSupernovaPlayerEnemy();
            ResetSupernovaEffects();
        }

        #region Supernova

        public void ResetSupernovaPlayerEnemy()
        {
            SupernovaPlayerTurns.text = "";
            SupernovaEnemyTurns.text = "";

            // Player
            foreach (var aB in SupernovaPlayerAttack)
                aB.color = invisible;
            foreach (var aBM in SupernovaPlayerAttackMults)
                aBM.text = "";

            foreach (var dB in SupernovaPlayerDefense)
                dB.color = invisible;
            foreach (var dBM in SupernovaPlayerDefenseMults)
                dBM.text = "";

            // Enemy
            foreach (var aS in SupernovaEnemyAttack)
                aS.color = invisible;
            foreach (var aSM in SupernovaEnemyAttackMults)
                aSM.text = "";

            foreach (var dS in SupernovaEnemyDefense)
                dS.color = invisible;
            foreach (var dSM in SupernovaEnemyDefenseMults)
                dSM.text = "";

            SupernovaPlayer.SetCanvasGroupInactive();
            SupernovaEnemy.SetCanvasGroupInactive();
        }

        public void ResetSupernovaEffects()
        {
            foreach (var effect in SupernovaEffectImages)
            {
                effect.texture = null;
                effect.enabled = false;
            }

            //foreach (var effectText in EffectTexts)
            //{
            //    effectText.text = "";
            //}

            SupernovaEffects.SetCanvasGroupInactive();
        }

        #endregion

        #endregion Reset Attributes

        #region Display Methods

        public void HandleDisplay(GameObject clickedOn)
        {
            MedalCycleLogic.Instance.StopCycleMedals();
            
            if (clickedOn.GetComponentInChildren<GridLayoutGroup>().transform.childCount > 1)
            {
                DisplaySublistOfMedals(clickedOn);
            }
            else
            {
                StartCoroutine(Display(clickedOn.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject));
            }
        }
        
        public void HideCurrentMedal()
        {
            isDisplayingMedal = false;

            HideSkills();
            HideTraits();
            HidePetTraits();
            HideSupernova();

            isTransition = true;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalHighlight));
            
            MedalCycleLogic.Instance.StartCycleMedals();
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
            HidePetTraits();
            HideTraits();

            isTransition = true;
            isDisplayingSkills = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(MedalSkills));
        }

        public void ShowTraits(RawImage value)
        {
            HidePetTraits();
            HideSkills();

            isTransition = true;
            isDisplayingTraits = true;
            elapsedTime = 0.0f;

            CurrentTraitSlot = value;
            
            StartCoroutine(ShowDisplay(MedalTraits));
        }

        public void ShowPetTraits()
        {
            HideSkills();
            HideTraits();

            isTransition = true;
            isDisplayingPetTraits = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(PetTraits));
        }

        public void HideSupernova()
        {
            if (MedalSupernova.alpha == 0)
                return;

            isTransition = true;
            isDisplayingSupernova = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalSupernova));
        }

        public void HideSkills()
        {
            if (MedalSkills.alpha == 0)
                return;

            isTransition = true;
            isDisplayingSkills = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalSkills));
        }

        public void HideTraits()
        {
            if (MedalTraits.alpha == 0)
                return;

            isTransition = true;
            isDisplayingTraits = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(MedalTraits));
        }

        public void HidePetTraits()
        {
            if (PetTraits.alpha == 0)
                return;

            isTransition = true;
            isDisplayingTraits = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay(PetTraits));
        }

        #endregion Display Methods

        #region Checks

        #region Content

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

        #region Supernova

        private bool CheckSupernovaPlayer()
        {
            foreach (var aB in SupernovaPlayerAttack)
            {
                if (aB.color == visible)
                    return true;
            }

            foreach (var dB in SupernovaPlayerDefense)
            {
                if (dB.color == visible)
                    return true;
            }

            return false;
        }

        private bool CheckSupernovaEnemy()
        {
            foreach (var aS in SupernovaEnemyAttack)
            {
                if (aS.color == visible)
                    return true;
            }

            foreach (var dS in SupernovaEnemyDefense)
            {
                if (dS.color == visible)
                    return true;
            }

            return false;
        }

        private bool CheckSupernovaEffects()
        {
            foreach (var eff in SupernovaEffectImages)
            {
                if (eff.enabled == true)
                    return true;
            }

            return false;
        }

        #endregion

        #endregion

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isDisplayingMedal)
                {
                    if (this.isDisplayingSkills || this.isDisplayingTraits || this.isDisplayingPetTraits)
                    {
                        HideSkills();
                        HidePetTraits();
                        HideTraits();
                    }
                    else if (this.isDisplayingSupernova)
                    {
                        HideSupernova();
                    }
                    else
                    {
                        HideCurrentMedal();
                    }
                }
                else if(isDisplayingSublist)
                {
                    this.HideSublistOfMedals(true);
                }
            }
        }

        void Awake()
        {
            Reset();
        }

        public IEnumerator Display(GameObject medalObject)
        {
            yield return null;
            Loading.StartLoading();
            Reset();

            isDisplayingMedal = true;
            if(currSublistMedal != null)
                this.HideSublistOfMedals();

            MedalDisplay medalDisplay = medalObject.GetComponent<MedalDisplay>();
            MedalAbility medalAbility = new MedalAbility();
            MedalAbility medalAbilitySupernova = new MedalAbility();

            try
            {
                medalAbility = MedalAbilityParser.Parser(medalDisplay.AbilityDescription);

                if(medalDisplay.IsSupernova)
                {
                    medalAbilitySupernova = MedalAbilityParser.Parser(medalDisplay.SupernovaDescription);
                }
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

            if(medalAbilitySupernova != null)
            {
                medalAbilitySupernova.SetUpDisplayAbility();

                AssignSupernova(medalDisplay, medalAbilitySupernova);
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
                //print("Finished");
            }

            Loading.FinishLoading();
        }

        public IEnumerator ShowDisplay(CanvasGroup canvasGroup)
        {
            //print("Test");
            StopCoroutine(HideDisplay(canvasGroup));
            elapsedTime = 0;
            //print(isTransition);
            while (isTransition)
            {
                //print("Transition");
                if (!Loading.IsLoading)
                {
                    //print("Tranisitioning");
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
            StopCoroutine(ShowDisplay(canvasGroup));
            elapsedTime = 0.0f;

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

        public void DisplaySublistOfMedals(GameObject clickedOn)
        {
            isDisplayingSublist = true;

            currSublistMedal = clickedOn;

            clickedOn.GetComponentInChildren<CanvasGroup>().alpha = 1;
            clickedOn.GetComponentInChildren<CanvasGroup>().blocksRaycasts = true;
            clickedOn.GetComponentInChildren<CanvasGroup>().interactable = true;
        }

        public void HideSublistOfMedals(bool closed = false)
        {
            isDisplayingSublist = false;

            currSublistMedal.GetComponentInChildren<CanvasGroup>().alpha = 0;
            currSublistMedal.GetComponentInChildren<CanvasGroup>().blocksRaycasts = false;
            currSublistMedal.GetComponentInChildren<CanvasGroup>().interactable = false;

            currSublistMedal = null;

            if (closed)
            {
                MedalCycleLogic.Instance.StartCycleMedals();
            }
        }
    }
}

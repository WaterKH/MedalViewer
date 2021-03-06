﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    // TODO Potential idea - weigh the pros/ cons of segregating this manager into
    // pieces that would get called on each of the sections that are segregrated into
    // regions right now.
    public class MedalSpotlightDisplayManager : MonoBehaviour
    {
        //MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();
        public LoadManager LoadManager;
        //public SearchManager SearchManager;
        public DamoEasterEgg DamoEasterEgg;
        //public MedalManager MedalManager;
        public MedalGraphViewManager MedalGraphViewManager;

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

        #region Slots

        public Dictionary<string, List<KeybladeMultiplierSlot>> MultiplierSlots = new Dictionary<string, List<KeybladeMultiplierSlot>>();
        //public List<KeybladeMultiplierSlot> Slots;

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
            Tuple.Create(200, 280),
            Tuple.Create(280, 320)
        };

        private string BaseMultiplierLow;
        private string BaseMultiplierHigh;
        private string MaxMultiplierLow;
        private string MaxMultiplierHigh;
        private string GuiltMultiplierLow;
        private string GuiltMultiplierHigh;

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
            "200", "210", "250", "260", "280"
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

        #region Multiplier Colors

        public Color NormalColor;
        public Color SelectedColor;
        public Color MultiplierOn;
        public Color MultiplierOnHighlight;
        public Color MultiplierOff;
        public Color MultiplierOffHighlight;

        #endregion

        #region Private 

        private readonly string url = "https://medalviewer.blob.core.windows.net/images/";

        private GameObject SlotHolder;
        //private Vector2 initialSlotPosition;
        //private Vector2 initialSlotSize;
        //private GameObject currSublistMedal;

        private Transform prevParent;
        private float prevScale;

        private float elapsedTime = 0.0f;

        private bool isTransition = false;

        //private bool isDisplayingMedal = false;
        //private bool isDisplayingSublist = false;
        private bool isDisplayingSupernova = false;
        private bool isDisplayingSkills = false;
        private bool isDisplayingTraits = false;
        private bool isDisplayingPetTraits = false;

        private Color32 visible = new Color(1f, 1f, 1f, 1f);
        private Color32 invisible = new Color(0f, 0f, 0f, 0f);

        //private string initialTraitSlotText = "N/A";

        private readonly string getMultiplierSlotsPHP = "https://mvphp.azurewebsites.net/getMultiplierSlots.php";

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
            Gauges.texture = Resources.Load("Gauges/" + medalDisplay.Gauge) as Texture2D;
        }

        private void AssignMedalName(MedalDisplay medalDisplay)
        {
            MedalName.text = medalDisplay.Name.Replace("é", "e").Replace("ï", "i");
        }

        private void AssignContent(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            AssignPlayerEnemy(medalAbility);
            AssignEffects(medalAbility);

            AssignSkill();
            AssignTraits(medalDisplay);
            AssignPetTrait();
            AssignStats(medalDisplay, medalAbility);
            AssignSlots(medalDisplay);

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
            var ability = medalAbility.STR.FirstOrDefault() != null ? medalAbility.STR.FirstOrDefault() :
                          medalAbility.DEF.FirstOrDefault() != null ? medalAbility.DEF.FirstOrDefault() : null;

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
                    if (strDef.Key == "STR")
                    {
                        foreach (var str in raiseLower.Value)
                        {
                            if (medalAbility.STR[counterSTR].IsPlayerAffected)
                            {
                                this.HelperAssignPlayerEnemy(ref PlayerAttack, ref PlayerAttackMults, ref strPlayerCounter, ref counterSTR, medalAbility.STR, str);
                            }
                            else if(medalAbility.STR[counterSTR].IsEnemyAffected)
                            {
                                this.HelperAssignPlayerEnemy(ref EnemyAttack, ref EnemyAttackMults, ref strEnemyCounter, ref counterSTR, medalAbility.STR, str);
                            }
                        }
                    }
                    else if (strDef.Key == "DEF")
                    {
                        foreach (var def in raiseLower.Value)
                        {
                            if (medalAbility.DEF[counterDEF].IsPlayerAffected)
                            {
                                this.HelperAssignPlayerEnemy(ref PlayerDefense, ref PlayerDefenseMults, ref defPlayerCounter, ref counterDEF, medalAbility.DEF, def);
                            }
                            else if (medalAbility.DEF[counterDEF].IsEnemyAffected)
                            {
                                this.HelperAssignPlayerEnemy(ref EnemyDefense, ref EnemyDefenseMults, ref defEnemyCounter, ref counterDEF, medalAbility.DEF, def);
                            }
                        }
                    }
                }
            }

            if (CheckPlayer())
            {
                Player.SetCanvasGroupInactive();

                if (ability != null)
                    PlayerTurns.text = ability.Duration;
            }

            if (CheckEnemy())
            {
                Enemy.SetCanvasGroupInactive();

                if (ability != null)
                    EnemyTurns.text = ability.Duration;
            }
        }

        private void HelperAssignPlayerEnemy(ref RawImage[] images, ref Text[] multipliers, ref int imageIndex, ref int multiplierIndex, List<MedalCombatAbility> strDef, Texture2D image)
        {
            images[imageIndex].texture = image;
            images[imageIndex].color = visible;
            multipliers[imageIndex++].text = "x" + strDef[multiplierIndex++].Tier;
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

            foreach(var increase in medalAbility.IncreaseImages)
            {
                if (increase.Value == null)
                    continue;

                EffectImages[effectCounter].enabled = true;
                EffectImages[effectCounter++].texture = increase.Value;
            }

            if (CheckEffects())
                Effects.SetCanvasGroupInactive();
        }

        // TODO Beta - This will only pick the highest multipliers with the correct PSM
        // TODO Account for PSM buffs (further up the keyblade) and Effects (multiplier higher on different slots)
        private void AssignSlots(MedalDisplay medalDisplay)
        {
            //var slotCount = 0;

            foreach(var key in this.MultiplierSlots.Keys)
            {
                var value = this.MultiplierSlots[key].OrderByDescending(x => x.Multiplier).FirstOrDefault(x => x.PSM == medalDisplay.Attribute_PSM && 
                                                                                                                 (x.UR == medalDisplay.Attribute_UR || string.IsNullOrEmpty(x.UR)));

                if(value != null)
                {
                    var slot = Instantiate(Resources.Load("Slot") as GameObject);
                    slot.transform.SetParent(SlotHolder.transform, false);

                    slot.GetComponent<SlotDisplay>().AssignSlot(value);

                    //slotCount++;
                }
            }

            var slots = SlotHolder.GetComponentsInChildren<SlotDisplay>().OrderByDescending(x => double.Parse(x.Multiplier.text.Substring(1))).ToList();
            for(int i = 0; i < slots.Count; ++i)
            {
                slots[i].transform.SetSiblingIndex(i);
            }

            // Resize
            var maxY = (SlotHolder.transform.childCount - 1) * (SlotHolder.GetComponent<GridLayoutGroup>().cellSize.y + SlotHolder.GetComponent<GridLayoutGroup>().spacing.y);
            SlotHolder.GetComponent<RectTransform>().offsetMin = new Vector2(SlotHolder.GetComponent<RectTransform>().offsetMax.x, -maxY);

            //initialSlotPosition = SlotHolder.GetComponent<RectTransform>().position;
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

        private void AssignStats(MedalDisplay medalDisplay, MedalAbility medalAbility)
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

            var Tier = medalDisplay.Tier;

            // ? TODO This doesn't make sense, we may not have an Inflict or 
            if (Effects.alpha != 1 && (medalAbility.Inflicts != "" || medalAbility.DamagePlus != ""))
            {
                var image = medalAbility.Inflicts != "" ? medalAbility.MiscImages["INFL"] :
                            medalAbility.MiscImages["DAMAGE+"];

                SwapMultiplier.GetComponent<RawImage>().enabled = true;
                SwapMultiplier.GetComponent<RawImage>().texture = image;
                SwapMultiplier.enabled = true;
                SwapMultiplier.transform.parent.GetComponent<Image>().enabled = true;
                MultiplierIdentifier.enabled = true;

                // Change to Max 
                UpdateMultiplier();
            }

            switch (medalDisplay.Star)
            {
                case 1:
                case 2:
                    Multiplier.text = "x" + (BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

                    break;
                case 3:
                    Multiplier.text = "x" + (MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

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
                    Multiplier.text = "x" + (MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

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
                    Multiplier.text = "x" + (MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

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
                    Multiplier.text = "x" + (GuiltMultiplierHigh != "0" ? GuiltMultiplierHigh :
                                            GuiltMultiplierLow != "0" ? GuiltMultiplierLow :
                                            MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

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

                    GuiltButtons[0].GetComponent<RawImage>().texture = Resources.Load($"Tier/Inactive-Guilt/{Tier}") as Texture2D;
                    GuiltButtons[1].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-White/{Tier}") as Texture2D;
                    GuiltButtons[2].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-Black/{Tier}") as Texture2D;

                    GuiltSlider.minValue = GuiltByTier[Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[Tier - 1].Item2;

                    GuiltSlider.value = GuiltSlider.maxValue;

                    Guilt.SetCanvasGroupActive();

                    GuiltValue.text = GuiltSlider.maxValue.ToString();

                    break;
                case 7:
                    Multiplier.text = "x" + (GuiltMultiplierHigh != "" && GuiltMultiplierHigh != "0" ? GuiltMultiplierHigh :
                                            GuiltMultiplierLow != "" ? GuiltMultiplierLow :
                                            MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow);

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

                    GuiltButtons[0].GetComponent<RawImage>().texture = Resources.Load($"Tier/Inactive-Guilt/{Tier}") as Texture2D;
                    GuiltButtons[1].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-White/{Tier}") as Texture2D;
                    GuiltButtons[2].GetComponent<RawImage>().texture = Resources.Load($"Tier/Active-Black/{Tier}") as Texture2D;

                    GuiltSlider.minValue = GuiltByTier[Tier - 1].Item1;
                    GuiltSlider.maxValue = GuiltByTier[Tier - 1].Item2;

                    GuiltSlider.value = GuiltSlider.maxValue;

                    Guilt.SetCanvasGroupActive();

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
            var ability = medalAbility.STR.FirstOrDefault() != null ? medalAbility.STR.FirstOrDefault() :
                medalAbility.DEF.FirstOrDefault() != null ? medalAbility.DEF.FirstOrDefault() : null;

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
            {
                SupernovaPlayer.SetCanvasGroupInactive();
                if (ability != null)
                    SupernovaPlayerTurns.text = ability.Duration;
            }

            if (CheckSupernovaEnemy())
            {
                SupernovaEnemy.SetCanvasGroupInactive();
                if (ability != null)
                    SupernovaEnemyTurns.text = ability.Duration;
            }
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
                SupernovaEffects.SetCanvasGroupInactive();
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
                colors.highlightedColor = MultiplierOnHighlight;

                if (GuiltButtons[0].enabled == true)
                {
                    if (MultiplierOrbs[4].enabled == true)
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
                colors.highlightedColor = MultiplierOffHighlight;

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
            for (int i = orbIndex; i < MultiplierOrbs.Length; ++i)
            {
                var colors = MultiplierOrbs[i].colors;
                colors.normalColor = NormalColor;
                MultiplierOrbs[i].colors = colors;
            }

            for (int i = 0; i < orbIndex; ++i)
            {
                var colors = MultiplierOrbs[i].colors;
                colors.normalColor = SelectedColor;
                MultiplierOrbs[i].colors = colors;
            }

            UpdateTotal();
        }

        public void UpdateGuilt()
        {
            if (GuiltButtons[0].enabled == true)
            {
                foreach (var orb in MultiplierOrbs)
                {
                    var colors = orb.colors;
                    colors.normalColor = SelectedColor;
                    orb.colors = colors;
                }

                GuiltButtons[0].enabled = false;
                GuiltButtons[0].GetComponent<RawImage>().enabled = false;

                GuiltButtons[1].enabled = true;
                GuiltButtons[2].enabled = true;
                GuiltButtons[1].GetComponent<RawImage>().enabled = true;
                GuiltButtons[2].GetComponent<RawImage>().enabled = true;

                if (SwapMultiplier.colors.normalColor == MultiplierOn)
                    Multiplier.text = $"x{GuiltMultiplierHigh}";
                else
                    Multiplier.text = $"x{GuiltMultiplierLow}";

                Guilt.SetCanvasGroupActive();
                //GuiltSlider.interactable = true;
            }
            else if (GuiltButtons[1].enabled == true || GuiltButtons[2].enabled == true)
            {
                GuiltButtons[0].enabled = true;
                GuiltButtons[0].GetComponent<RawImage>().enabled = true;

                GuiltButtons[1].enabled = false;
                GuiltButtons[2].enabled = false;
                GuiltButtons[1].GetComponent<RawImage>().enabled = false;
                GuiltButtons[2].GetComponent<RawImage>().enabled = false;

                // TODO Fix this later because we may have to go back to base, and not max Multiplier
                if (SwapMultiplier.colors.normalColor == MultiplierOn)
                    Multiplier.text = $"x{MaxMultiplierHigh}";
                else
                    Multiplier.text = $"x{MaxMultiplierLow}";

                Guilt.SetCanvasGroupInactive();
                //GuiltSlider.interactable = false;
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
            var mult = float.Parse(Multiplier.text.Substring(1));
            var guilt = GuiltSlider.value;
            var deals = int.Parse(Deals.text);
            var spBonus = int.Parse(SPABonusValues[(int)SPABonusSlider.value]);
            var skill = Skill.text.Length > 0 ? float.Parse(Skill.text) : 0.0f;

            var totalRaids = 0.0f;
            var extraAttack = 0.0f;

            var totalStr = str + addedStr;

            if (TraitValues[0].text.Length > 2)
            {
                if (TraitValues[0].text[TraitValues[0].text.Length - 1] == 'S')
                {
                    totalStr += 1000;
                }
                else if (TraitValues[0].text[TraitValues[0].text.Length - 1] == 'R')
                {
                    totalRaids += 0.40f;
                }
                else if (TraitValues[0].text.Substring(TraitValues[0].text.Length - 2) == "EA")
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

            if (PetTraitValue.text.Length > 2)
            {
                if (PetTraitValue.text[PetTraitValue.text.Length - 1] == 'S')
                {
                    totalStr += int.Parse(PetTraitValue.text.Substring(1, PetTraitValue.text.Length - 2));// 1000;
                }
                else if (PetTraitValue.text[PetTraitValue.text.Length - 1] == 'R')
                {
                    totalRaids += float.Parse(PetTraitValue.text.Substring(1, PetTraitValue.text.Length - 3));// 0.40f;
                }
                else if (PetTraitValue.text.Substring(PetTraitValue.text.Length - 2) == "EA")
                {
                    var temp = float.Parse(PetTraitValue.text.Substring(1, PetTraitValue.text.Length - 4));
                    extraAttack = temp > extraAttack ? temp : extraAttack; //0.40f;
                }
            }

            var totalMult = guilt != 0.0f && spBonus != 0 ? mult * ((guilt / 100) + 1.0f) * (((float)spBonus / 100) + 1.0f) :
                            guilt != 0.0f ? mult * ((guilt / 100) + 1.0f) :
                            spBonus != 0 ? mult * (((float)spBonus / 100) + 1.0f) :
                            mult;

            var totalMultTimesStrength = totalStr * totalMult;

            var total = skill != 0 && totalRaids != 0.0f && extraAttack != 0.0f ? deals * skill * (((totalRaids / 100) + 1.0f) + ((extraAttack / 100) + 1.0f)) * totalMultTimesStrength :
                        skill != 0 && extraAttack != 0.0f ? deals * skill * ((extraAttack / 100) + 1.0f) * totalMultTimesStrength :
                        skill != 0 && totalRaids != 0.0f ? deals * skill * ((totalRaids / 100) + 1.0f) * totalMultTimesStrength :
                        totalRaids != 0.0f && extraAttack != 0.0f ? deals * (((totalRaids / 100) + 1.0f) + ((extraAttack / 100) + 1.0f)) * totalMultTimesStrength :
                        skill != 0 ? deals * skill * totalMultTimesStrength :
                        totalRaids != 0.0f ? deals * ((totalRaids / 100) + 1.0f) * totalMultTimesStrength :
                        extraAttack != 0.0f ? deals * ((extraAttack / 100) + 1.0f) * totalMultTimesStrength :
                        deals * totalMultTimesStrength;

            FinalDamageOutput.text = String.Format("{0:#,#.##}", (int)Math.Ceiling(total));
        }

        #endregion

        #region Reset Attributes

        public void ResetDisplay()
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
            ResetSlots();

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

            Player.SetCanvasGroupActive();
            Enemy.SetCanvasGroupActive();
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

            Effects.SetCanvasGroupActive();
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
            SwapMultiplier.GetComponent<RawImage>().enabled = false;
            SwapMultiplier.enabled = false;
            SwapMultiplier.transform.parent.GetComponent<Image>().enabled = false;

            DefenseSlider.value = 0;
            StrengthSlider.value = 0;

            MultiplierIdentifier.text = "Base";
            MultiplierIdentifier.enabled = false;
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

            foreach (var guilt in GuiltButtons)
            {
                guilt.GetComponent<RawImage>().enabled = false;
                guilt.GetComponent<RawImage>().texture = null;
                guilt.enabled = false;
            }
            Guilt.alpha = 0;
            Guilt.interactable = false;
            Guilt.blocksRaycasts = false;
            GuiltSlider.value = 0;
            GuiltValue.text = "0%";
        }

        public void ResetSlots()
        {
            //SlotHolder.GetComponent<RectTransform>().offsetMin = initialSlotSize;
            //SlotHolder.GetComponent<RectTransform>().position = initialSlotPosition;
            foreach (var go in SlotHolder.GetComponentsInChildren<SlotDisplay>())
            {
                Destroy(go.gameObject);
            }
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

            SupernovaPlayer.SetCanvasGroupActive();
            SupernovaEnemy.SetCanvasGroupActive();
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

            SupernovaEffects.SetCanvasGroupActive();
        }

        #endregion

        #endregion Reset Attributes

        #region Display Methods

        public void HideCurrentMedal()
        {
            MedalGraphViewManager.IsDisplayingMedal = false;

            HideSkills();
            HideTraits();
            HidePetTraits();
            HideSupernova();

            isTransition = true;
            elapsedTime = 0.0f;

            StartCoroutine(this.HideDisplay(MedalHighlight));

            // TODO If you click on a search item, it should close the search window/ Handle it over there
            //if (!SearchManager.IsDisplayingSearch)
            //{
            //    MedalCycleLogic.Instance.StartCycleMedals();
            //}
        }

        public void ShowSupernova()
        {
            isTransition = true;
            isDisplayingSupernova = true;
            elapsedTime = 0.0f;

            StartCoroutine(this.ShowDisplay(MedalSupernova));
        }

        public void ShowSkills()
        {
            HidePetTraits();
            HideTraits();

            isTransition = true;
            isDisplayingSkills = true;
            elapsedTime = 0.0f;

            StartCoroutine(this.ShowDisplay(MedalSkills));
        }

        public void ShowTraits(RawImage value)
        {
            HidePetTraits();
            HideSkills();

            isTransition = true;
            isDisplayingTraits = true;
            elapsedTime = 0.0f;

            CurrentTraitSlot = value;

            StartCoroutine(this.ShowDisplay(MedalTraits));
        }

        public void ShowPetTraits()
        {
            HideSkills();
            HideTraits();

            isTransition = true;
            isDisplayingPetTraits = true;
            elapsedTime = 0.0f;

            StartCoroutine(this.ShowDisplay(PetTraits));
        }

        public void HideSupernova()
        {
            if (MedalSupernova.alpha == 0)
                return;

            isTransition = true;
            isDisplayingSupernova = false;
            elapsedTime = 0.0f;

            StartCoroutine(this.HideDisplay(MedalSupernova));
        }

        public void HideSkills()
        {
            if (MedalSkills.alpha == 0)
                return;

            isTransition = true;
            isDisplayingSkills = false;
            elapsedTime = 0.0f;

            StartCoroutine(this.HideDisplay(MedalSkills));
        }

        public void HideTraits()
        {
            if (MedalTraits.alpha == 0)
                return;

            isTransition = true;
            isDisplayingTraits = false;
            elapsedTime = 0.0f;

            StartCoroutine(this.HideDisplay(MedalTraits));
        }

        public void HidePetTraits()
        {
            if (PetTraits.alpha == 0)
                return;

            isTransition = true;
            isDisplayingTraits = false;
            elapsedTime = 0.0f;

            StartCoroutine(this.HideDisplay(PetTraits));
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
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            {
                if (MedalGraphViewManager.IsDisplayingMedal)
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
                        DamoEasterEgg.DesummonDamo();
                    }
                }
            }
        }

        void Awake()
        {
            SlotHolder = GameObject.FindGameObjectWithTag("SlotHolder");
            //initialSlotPosition = SlotHolder.GetComponent<RectTransform>().position;
            //initialSlotSize = SlotHolder.GetComponent<RectTransform>().offsetMin;
            ResetDisplay();

            StartCoroutine(this.GetMultiplierSlotsPHP());
        }

        public IEnumerator Display(GameObject medalObject, MedalDisplay medal = null)
        {
            while (LoadManager.IsLoading)
            {
                yield return null;
            }
            LoadManager.StartLoading();
            ResetDisplay();

            MedalDisplay medalDisplay = null;

            if (medalObject != null)
                medalDisplay = medalObject.GetComponent<MedalDisplay>();
            else if (medal != null)
                medalDisplay = medal;

            MedalAbility medalAbility = null;
            MedalAbility medalAbilitySupernova = null;

            try
            {
                medalAbility = MedalAbilityParser.Instance.Parser(medalDisplay.AbilityDescription);

                if (medalDisplay.IsSupernova)
                {
                    medalAbilitySupernova = MedalAbilityParser.Instance.Parser(medalDisplay.SupernovaDescription);
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

            if (medalAbilitySupernova != null)
            {
                medalAbilitySupernova.SetUpDisplayAbility();

                AssignSupernova(medalDisplay, medalAbilitySupernova);
            }

            #endregion

            isTransition = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay(MedalHighlight));
            DamoEasterEgg.SummonDamo();
        }

        IEnumerator LoadImage(string imageUrl, GameObject medalObject)
        {
            //Debug.Log(imageUrl);
            //yield return 0;
            UnityWebRequest image = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return image.SendWebRequest();
            if (image.isNetworkError || image.isHttpError)
                Debug.Log("ERROR: " + image.error);
            else
            {
                medalObject.GetComponent<RawImage>().texture = ((DownloadHandlerTexture)image.downloadHandler).texture;

                MedalPlaceholderShadow.texture = MedalPlaceholder.texture;
                //print("Finished");
            }

            LoadManager.FinishLoading();
        }

        public IEnumerator ShowDisplay(CanvasGroup canvasGroup)
        {
            //print("Test");
            StopCoroutine(HideDisplay(canvasGroup));
            elapsedTime = 0.0f;
            //print(isTransition);
            while (isTransition)
            {
                //print("Transition");
                if (!LoadManager.IsLoading)
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

        public IEnumerator GetMultiplierSlotsPHP()
        {
            var query = "SELECT KSM.[Id], KS.SlotNumber, AL.PSM, AL.UR, K.[Name], [KeybladeLevel], [Multiplier] FROM[dbo].[KeybladeSlotMultiplier] KSM, Keyblade K, KeybladeSlot KS, AttributeLookup AL Where KSM.KeybladeId = K.Id AND KSM.SlotId = KS.Id AND KS.AttributeId = AL.Id";
            WWWForm form = new WWWForm();
            form.AddField("sqlQuery", query);

            using (UnityWebRequest www = UnityWebRequest.Post(getMultiplierSlotsPHP, form))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log("ERROR:: " + www.error);
                }
                else
                {
                    //Debug.Log(www.downloadHandler.text);
                    var rows = www.downloadHandler.text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var row in rows)
                    {
                        var splitRow = row.Split(new char[] { '|' }, StringSplitOptions.None);

                        var keybladeMultiplierSlot = new KeybladeMultiplierSlot
                        {
                            Id = string.IsNullOrEmpty(splitRow[0]) ? -1 : int.Parse(splitRow[0]),
                            Name = string.IsNullOrEmpty(splitRow[1]) ? "" : splitRow[1],
                            SlotNumber = string.IsNullOrEmpty(splitRow[2]) ? -1 : int.Parse(splitRow[2]),
                            PSM = string.IsNullOrEmpty(splitRow[3]) ? "" : splitRow[3],
                            UR = string.IsNullOrEmpty(splitRow[4]) ? "" : splitRow[4],
                            KeybladeLevel = string.IsNullOrEmpty(splitRow[5]) ? -1 : double.Parse(splitRow[5]),
                            Multiplier = string.IsNullOrEmpty(splitRow[6]) ? -1 : double.Parse(splitRow[6])
                        };

                        if (!this.MultiplierSlots.ContainsKey(keybladeMultiplierSlot.Name))
                            this.MultiplierSlots.Add(keybladeMultiplierSlot.Name, new List<KeybladeMultiplierSlot>());

                        this.MultiplierSlots[keybladeMultiplierSlot.Name].Add(keybladeMultiplierSlot);
                    }
                }
            }
        }
    }
}

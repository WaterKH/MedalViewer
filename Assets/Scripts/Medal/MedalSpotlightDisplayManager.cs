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

        //public MedalCreator medalCreator;
        public TraitManager TraitManager;
        MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();
        public Loading Loading;

        public CanvasGroup MedalHighlight;

        #region Base

        public Text MedalName;
        public RawImage MedalPlaceholder;
        public RawImage MedalPlaceholderShadow;
        public Text Ability;
        public Text AbilityDescription;

        public RawImage Tier;
        public RawImage Target;
        public RawImage Gauges;
        public RawImage PSMAttribute;
        public RawImage URAttribute;
        public CanvasGroup Supernova;

        public Color32 Reversed;// = new Color(171, 0, 255, 255);
        public Color32 Upright;// = new Color(255f, 255f, 0f, 255f);

        #endregion Base

        #region Traits

        public CanvasGroup Traits;
        public RawImage InitialImage;
        public RawImage[] TraitSlots;
        public Text[] TraitSlotsText;

        #endregion Traits

        #region Boosts and Saps

        public CanvasGroup Boosts;
        public CanvasGroup Saps;

        public Text BoostsTurns;
        public Text SapsTurns;

        public RawImage[] AttackBoosts;
        public Text[] AttackBoostMults;
        public RawImage[] DefenseBoosts;
        public Text[] DefenseBoostMults;

        public RawImage[] AttackSaps;
        public Text[] AttackSapMults;
        public RawImage[] DefenseSaps;
        public Text[] DefenseSapMults;

        #endregion Boosts and Saps

        #region Effects

        public CanvasGroup Effects;
        public RawImage[] EffectImages;
        public Text[] EffectTexts;

        #endregion

        #region Stats

        public Text Multiplier;
        public Text Defense;
        public Text Strength;

        public Slider GuiltSlider;
        public Toggle BaseMultiplier;
        public Toggle MaxMultiplier;
        public Toggle MaxGuiltMultiplier;
        public Toggle Boosted;

        public Button SwapMultiplier; // Probably not needed 

        #endregion

        #region Skill

        public RawImage SkillImage;
        public Text SkillText;

        #endregion

        #region CalculationVariables

        public Text CalculatedStrength;
        public Text NumberOfHits;
        public Text SpecialAttackBonus;
        public Slider SpecialAttackBonusSlider;
        public Text SkillVariable;
        public Text TraitAddition;

        public Text FinalDamageOutput;

        public SPATKBonusManager SPATKBonusManager; // Needed?

        #endregion

        #region Private 

        //private int startIndexPlayer = 8;
        //private int startIndexEnemy = 6;
        //private string PlayerEffectInitial;
        //private string EnemyEffectInitial;

        //private CanvasGroup background_group;
        //private GameObject MedalSublist;
        //private CanvasGroup MedalSublistGroup;

        private Transform prevParent;
        private float prevScale;

        private float elapsedTime = 0.0f;

        private bool isTransition = false;
        private bool isDisplaying = false;

        private Color32 visible = new Color(1f, 1f, 1f, 1f);
        private Color32 invisible = new Color(0f, 0f, 0f, 0f);

        private string initialTraitSlotText = "No Trait Selected";

        #endregion

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
            //MedalCycleLogic.Instance.StopCycleMedals();
            Reset();

            MedalDisplay medalDisplay = medalObject.GetComponent<MedalDisplay>();
            MedalAbility medalAbility = new MedalAbility();
            //var medalAbilityID = 0;


            try
            {
                medalAbility = MedalAbilityParser.Parser(medalDisplay.AbilityDescription);
                //medalAbilityID = medalDisplay.Id;

                //         if (medalCreator.medalAbilityImages.ContainsKey(medalAbilityID))
                //         {
                //             medalAbility = medalCreator.medalAbilityImages[medalAbilityID];
                //         }
            }
            catch
            {
                print(medalDisplay.Name.Split(':')[0] + " " + medalDisplay.AbilityDescription);
            }

            #region Display Assignment

            //StartCoroutine(AssignBase(medalObject, medalDisplay));
            //StartCoroutine(AssignTraits(medalDisplay));

            AssignBase(medalObject, medalDisplay);
            AssignTraits(medalDisplay);

            if (medalAbility != null)
            {
                medalAbility.SetUpDisplayAbility();

                //StartCoroutine(AssignBoostsSaps(medalAbility));
                //StartCoroutine(AssignEffects(medalAbility));

                //StartCoroutine(AssignStats(medalDisplay, medalAbility));

                AssignBoostsSaps(medalAbility);
                AssignEffects(medalAbility);

                AssignStats(medalDisplay, medalAbility);
            }


            #endregion

            //if(MedalSublistGroup.alpha >= 0.0f)
            //{
            //	HideSublistOfMedals();
            //}
            isTransition = true;
            elapsedTime = 0.0f;

            StartCoroutine(ShowDisplay());
            //Loading.FinishLoading();
        }

        #region Assign Attributes

        public void AssignBase(GameObject medalObject, MedalDisplay medalDisplay)
        {
            //yield return null;
            //MedalPlaceholder.texture = medalObject.GetComponent<RawImage>().texture;
            var imageUrl = url + medalDisplay.ImageURL;
            StartCoroutine(LoadImage(imageUrl, MedalPlaceholder.gameObject));

            MedalName.text = medalDisplay.Name;
            PSMAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_PSM + "_Gem") as Texture2D;
            URAttribute.color = medalDisplay.Attribute_UR == "Upright" ? Upright : Reversed;

            Tier.texture = Resources.Load("Tier/Black/" + medalDisplay.Tier) as Texture2D;
            Target.texture = Resources.Load("Target/" + medalDisplay.Target) as Texture2D;
            Gauges.texture = Resources.Load("Gauges/" + medalDisplay.Gauge) as Texture2D;

            Ability.text = medalDisplay.Ability;
            AbilityDescription.text = medalDisplay.AbilityDescription;

            if (medalDisplay.IsSupernova)
            {
                Supernova.interactable = true;
                Supernova.blocksRaycasts = true;
                Supernova.alpha = 1;
            }
        }

        public void AssignTraits(MedalDisplay medalDisplay)
        {
            //yield return null;
            for (int i = 0; i < medalDisplay.TraitSlots; ++i)
            {
                TraitSlots[i].enabled = true;
                TraitSlotsText[i].enabled = true;
            }
        }

        public void AssignBoostsSaps(MedalAbility medalAbility)
        {
            //yield return null;
            var strBoostCounter = 0;
            var defBoostCounter = 0;
            var strSapCounter = 0;
            var defSapCounter = 0;

            var counterSTR = 0;
            var counterDEF = 0;
            //var sapCounterSTR = 0;
            //var sapCounterDEF = 0;

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
                                AttackBoosts[strBoostCounter].texture = strRaise;
                                AttackBoosts[strBoostCounter].color = visible;
                                AttackBoostMults[strBoostCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defRaise in raiseLower.Value)
                            {
                                DefenseBoosts[defBoostCounter].texture = defRaise;
                                DefenseBoosts[defBoostCounter].color = visible;
                                DefenseBoostMults[defBoostCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                    else if (raiseLower.Key == "Lowers")
                    {
                        if (strDef.Key == "STR")
                        {
                            foreach (var strLower in raiseLower.Value)
                            {
                                AttackSaps[strSapCounter].texture = strLower;
                                AttackSaps[strSapCounter].color = visible;
                                AttackSapMults[strSapCounter++].text = "x" + medalAbility.STR[counterSTR++].Tier;
                            }
                        }
                        else if (strDef.Key == "DEF")
                        {
                            foreach (var defLower in raiseLower.Value)
                            {
                                DefenseSaps[defSapCounter].texture = defLower;
                                DefenseSaps[defSapCounter].color = visible;
                                DefenseSapMults[defSapCounter++].text = "x" + medalAbility.DEF[counterDEF++].Tier;
                            }
                        }
                    }
                }
            }

            // TODO Account for player saps

            // If none of either are filled in, dis/able it
            // TODO Should we make this smarter? 
            if (CheckBoosts())
                Boosts.SetCanvasGroupActive();

            if (CheckSaps())
                Saps.SetCanvasGroupActive();
        }

        public void AssignSkill()
        {
            // TODO If we save user medals, maybe pull from that database? But for right now, we don't need one
            //yield return null;
        }

        public void AssignEffects(MedalAbility medalAbility)
        {
            //yield return null;
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

        public void AssignStats(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            //yield return null;
            Multiplier.text = medalDisplay.BaseMultiplier.Split('-')[0];
            Defense.text = medalDisplay.MaxDefense.ToString();
            Strength.text = medalDisplay.MaxDefense.ToString();

            //switch (medalDisplay.Star)
            //{
            //    case 1:
            //    case 2:
            //        MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.BaseMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

            //        EqualTexts[0].text = "=";
            //        MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

            //        BaseMultiplier.color = visible;
            //        break;
            //    case 3:
            //        MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
            //        MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

            //        EqualTexts[0].text = "=";
            //        MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[1].text = "=";
            //        MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

            //        BaseMultiplier.color = visible;
            //        MaxMultipliers[0].color = visible;
            //        break;
            //    case 4:
            //        MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
            //        MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

            //        EqualTexts[0].text = "=";
            //        MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[1].text = "=";
            //        MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

            //        BaseMultiplier.color = visible;
            //        MaxMultipliers[1].color = visible;
            //        break;
            //    case 5:
            //        MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
            //        MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

            //        EqualTexts[0].text = "=";
            //        MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[1].text = "=";
            //        MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

            //        BaseMultiplier.color = visible;
            //        MaxMultipliers[2].color = visible;
            //        break;
            //    case 6:
            //        MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
            //        MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");
            //        MultiplierTexts[2].text = medalDisplay.GuiltMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;
            //        SPATKBonusManager.CurrTier = medalDisplay.Tier;

            //        EqualTexts[0].text = "=";
            //        MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[1].text = "=";
            //        MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[2].text = "=";
            //        MultiplierWithStrengthTexts[2].text = Parser.ParseMultiplierWithStrength(medalDisplay.GuiltMultiplier, medalDisplay.MaxStrength);

            //        BaseMultiplier.color = visible;
            //        MaxMultipliers[3].color = visible;
            //        GuiltMultiplier.color = visible;

            //        MultiplierTier.color = visible;
            //        MultiplierTier.texture = Resources.Load("Tier/Tier_" + medalDisplay.Tier) as Texture2D;
            //        break;
            //    case 7:
            //        MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");
            //        MultiplierTexts[2].text = medalDisplay.GuiltMultiplier.Replace("-", "~");

            //        SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
            //        SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;
            //        SPATKBonusManager.CurrTier = medalDisplay.Tier;

            //        EqualTexts[1].text = "=";
            //        MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

            //        EqualTexts[2].text = "=";
            //        MultiplierWithStrengthTexts[2].text = Parser.ParseMultiplierWithStrength(medalDisplay.GuiltMultiplier, medalDisplay.MaxStrength);

            //        MaxMultipliers[3].color = visible;
            //        GuiltMultiplier.color = visible;

            //        MultiplierTier.color = visible;
            //        MultiplierTier.texture = Resources.Load("Tier/Tier_" + medalDisplay.Tier) as Texture2D;
            //        break;
            //    default:
            //        break;
            //}

            //SPATKBonusManager.UpdateSPATKBonus();
        }

        #endregion Assign Attributes

        #region Reset Attributes

        public void Reset()
        {
            ResetBase();
            ResetTraits();
            ResetBoostsSaps();
            ResetSkill();
            ResetEffects();
            ResetStats();
        }

        public void ResetBase()
        {
            MedalPlaceholder.texture = Resources.Load("Placeholder") as Texture2D;
            MedalPlaceholderShadow.texture = MedalPlaceholder.texture;

            MedalName.text = "PLACEHOLDER";
            PSMAttribute.texture = null;
            URAttribute.color = invisible;

            Tier.texture = null;
            Target.texture = null;
            Gauges.texture = null;

            Ability.text = "ABILITY";
            AbilityDescription.text = "ABILITY DESCRIPTION";

            Supernova.interactable = false;
            Supernova.blocksRaycasts = false;
            Supernova.alpha = 0;
        }

        public void ResetTraits()
        {
            for (int i = 0; i < TraitSlots.Length; ++i)
            {
                TraitSlots[i].texture = InitialImage.texture;
                TraitSlots[i].enabled = false;

                TraitSlotsText[i].enabled = false;
                TraitSlotsText[i].text = initialTraitSlotText;
            }
        }

        public void ResetBoostsSaps()
        {
            BoostsTurns.text = "";
            SapsTurns.text = "";

            // Boosts
            foreach (var aB in AttackBoosts)
                aB.color = invisible;
            foreach (var aBM in AttackBoostMults)
                aBM.text = "";

            foreach (var dB in DefenseBoosts)
                dB.color = invisible;
            foreach (var dBM in DefenseBoostMults)
                dBM.text = "";

            // Saps
            foreach (var aS in AttackSaps)
                aS.color = invisible;
            foreach (var aSM in AttackSapMults)
                aSM.text = "";

            foreach (var dS in DefenseSaps)
                dS.color = invisible;
            foreach (var dSM in DefenseSapMults)
                dSM.text = "";

            Boosts.SetCanvasGroupInactive();
            Saps.SetCanvasGroupInactive();
        }

        public void ResetSkill()
        {
            SkillImage.texture = null;
            SkillImage.enabled = false;

            SkillText.text = "";
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

        public void ResetStats()
        {
            Multiplier.text = "";
            Defense.text = "";
            Strength.text = "";

            //BaseMultiplier.isOn = true;
            //MaxMultiplier.isOn = false;
            //MaxGuiltMultiplier.isOn = false;
            //Boosted.isOn = false;

            //GuiltSlider.value = 0;
            //GuiltSlider.minValue = 0;
            //GuiltSlider.maxValue = 1;
        }

        public void ResetCalculations()
        {
            CalculatedStrength.text = "";
            NumberOfHits.text = "";
            SpecialAttackBonus.text = "";
            SkillVariable.text = "";
            TraitAddition.text = "";
            FinalDamageOutput.text = "";

            SpecialAttackBonusSlider.value = 0;
        }

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

        //public void DisplaySublistOfMedals(GameObject clickedOn)
        //{
        //    MedalCycleLogic.Instance.StopCycleMedals();

        //    var medalList = clickedOn.GetComponentsInChildren<Transform>();
        //    this.prevParent = medalList[0];

        //    for (int i = 1; i < medalList.Length; ++i)
        //    {
        //        var medal = medalList[i];

        //        this.prevScale = medal.transform.localScale.x;
        //        medal.GetComponent<RawImage>().color = visible;

        //        medal.SetParent(MedalSublist.transform);
        //        medal.transform.localScale = new Vector3(1f, 1f, 1f);
        //        medal.GetComponent<RawImage>().raycastTarget = true;
        //    }

        //    //MedalSublistGroup.alpha = 1;
        //    //MedalSublistGroup.interactable = true;
        //    //MedalSublistGroup.blocksRaycasts = true;
        //}

        //public void HideSublistOfMedals()
        //{
        //    //MedalSublistGroup.alpha = 0;
        //    //MedalSublistGroup.interactable = false;
        //    //MedalSublistGroup.blocksRaycasts = false;

        //    var medalList = MedalSublist.GetComponentsInChildren<Transform>();

        //    for (int i = 1; i < medalList.Length; ++i)
        //    {
        //        var medal = medalList[i];

        //        medal.GetComponent<RawImage>().raycastTarget = false;

        //        medal.SetParent(this.prevParent);
        //        medal.transform.localScale = new Vector3(this.prevScale, this.prevScale, this.prevScale);
        //        medal.GetComponent<RawImage>().color = invisible;
        //    }

        //}

        #region Calculation Methods

        public void BaseMultiplierClick()
        {

        }

        public void MaxMultiplierClick()
        {

        }

        public void MaxGuiltMultiplierClick()
        {

        }

        public void EffectMultiplierClick()
        {

        }

        public void BoostedClick()
        {

        }

        public void FinalDamageOutputCalculator(MedalAbility medalAbility)
        {

        }

        #endregion

        public void HideCurrentMedal()
        {
            isTransition = true;
            isDisplaying = false;
            elapsedTime = 0.0f;

            StartCoroutine(HideDisplay());

            //TraitManager.HideToolBox();

            //Reset();
        }

        #endregion Display Methods

        #region Checks

        public bool CheckBoosts()
        {
            foreach (var aB in AttackBoosts)
            {
                if (aB.color == visible)
                    return true;
            }

            foreach (var dB in DefenseBoosts)
            {
                if (dB.color == visible)
                    return true;
            }

            return false;
        }

        public bool CheckSaps()
        {
            foreach (var aS in AttackSaps)
            {
                if (aS.color == visible)
                    return true;
            }

            foreach (var dS in DefenseSaps)
            {
                if (dS.color == visible)
                    return true;
            }

            return false;
        }

        public bool CheckEffects()
        {
            foreach (var eff in EffectImages)
            {
                if (eff.color == visible)
                    return true;
            }

            return false;
        }

        #endregion

        private readonly string url = "https://medalviewer.blob.core.windows.net/images/";

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

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //print(isDisplaying);
                if (/*MedalSublistGroup.alpha >= 0.0f ||*/ isDisplaying)
                {
                    //if (!MedalCycleLogic.Instance.firstPass)
                    //{
                    //    HideSublistOfMedals();
                    //}
                    if (TraitManager.IsDisplayingTrait)
                    {
                        TraitManager.HideToolBox();
                    }
                    else if (isDisplaying)
                    {
                        HideCurrentMedal();
                    }
                }
                //MedalCycleLogic.Instance.CycleMedals();
            }


        }

        IEnumerator ShowDisplay()
        {
            while (isTransition)
            {
                elapsedTime += Time.deltaTime;

                if (!Loading.IsLoading)
                {
                    MedalHighlight.alpha = Mathf.Lerp(MedalHighlight.alpha, 1, elapsedTime);
                    MedalHighlight.interactable = true;
                    MedalHighlight.blocksRaycasts = true;

                    if (MedalHighlight.alpha >= 1.0f)
                    {
                        elapsedTime = 0.0f;

                        isDisplaying = true;
                        isTransition = false;
                    }
                }
                yield return null;
            }
        }

        IEnumerator HideDisplay()
        {
            while (isTransition)
            {
                elapsedTime += Time.deltaTime;
                //print(isTransition);
                MedalHighlight.alpha = Mathf.Lerp(MedalHighlight.alpha, 0, elapsedTime);
                MedalHighlight.interactable = false;
                MedalHighlight.blocksRaycasts = false;

                if (MedalHighlight.alpha <= 0.0f)
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

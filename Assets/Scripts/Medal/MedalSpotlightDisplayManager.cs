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

        #endregion

        #region Traits

        //public CanvasGroup Traits;
        public RawImage[] TraitSlots;
        //public Text[] TraitSlotsText;
        
        public RawImage InitialImage;

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

        #endregion
        
        #region Vars

        public Text Deals;
        public Text SPABonus;
        public Text Skill;
        public Text[] TraitValues;

        public SPATKBonusManager SPATKBonusManager; // Needed?

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
            // TODO If we save user medals, maybe pull from that database? But for right now, we don't need one
            //yield return null;
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

        private void AssignVars(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            Deals.text = medalAbility.Deal;
            SPABonus.text = medalAbility.SPBonus ?? "15";
        }

        private void AssignTotal()
        {

        }

        #endregion 

        #endregion Assign Attributes

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
            SkillImage.texture = null;
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
            
            // TODO Add rest of vars
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

        public void ShowTraits()
        {
            isTransition = true;
            isDisplayingTraits = true;
            elapsedTime = 0.0f;

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

        public bool CheckPlayer()
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

        public bool CheckEnemy()
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

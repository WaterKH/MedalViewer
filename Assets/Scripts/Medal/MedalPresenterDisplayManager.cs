using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace MedalViewer.Medal
{
    public class MedalPresenterDisplayManager : MonoBehaviour
    {
        MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();
        //public SearchManager SearchManager;
        public Loading Loading;

        public CanvasGroup MedalHighlight;
        public CanvasGroup ButtonCanvasGroup;
        //public CanvasGroup MedalSupernova;

        //public CanvasGroup Player;
        //public CanvasGroup Enemy;
        //public CanvasGroup Effects;

        //public CanvasGroup SupernovaPlayer;
        //public CanvasGroup SupernovaEnemy;
        //public CanvasGroup SupernovaEffects;

        #region Icons

        public TextMeshProUGUI PetPointsBase;
        public TextMeshProUGUI PetPointsMax;
        public RawImage PSMAttribute;
        public RawImage URAttribute;
        public RawImage Tier;
        public RawImage Target;
        public RawImage Gauges;
        
        #endregion

        #region MedalName

        public TextMeshProUGUI MedalName;

        #endregion

        #region Content
        
        #region Player
        
        //public TextMeshProUGUI PlayerTurns;

        public RawImage[] PlayerAttack;
        public TextMeshProUGUI[] PlayerAttackMults;
        public RawImage[] PlayerDefense;
        public TextMeshProUGUI[] PlayerDefenseMults;
        
        #endregion

        #region Enemy
        
        //public TextMeshProUGUI EnemyTurns;

        public RawImage[] EnemyAttack;
        public TextMeshProUGUI[] EnemyAttackMults;
        public RawImage[] EnemyDefense;
        public TextMeshProUGUI[] EnemyDefenseMults;

        #endregion
        
        #region Effects

        public RawImage[] EffectImages;
        //public TextMeshProUGUI[] EffectTexts;

        #endregion

        #region Stats

        public TextMeshProUGUI Multiplier;

        #endregion

        #endregion

        #region MedalInfo

        public CanvasGroup SupernovaButton;
        public RawImage MedalPlaceholder;
        //public RawImage MedalPlaceholderShadow;

        #endregion

        #region Supernova

        public RawImage SupernovaTarget;
        public TextMeshProUGUI SupernovaDamage;

        #region Player

        public Text SupernovaPlayerTurns;

        public RawImage[] SupernovaPlayerAttack;
        public TextMeshProUGUI[] SupernovaPlayerAttackMults;
        public RawImage[] SupernovaPlayerDefense;
        public TextMeshProUGUI[] SupernovaPlayerDefenseMults;

        #endregion

        #region Enemy

        public Text SupernovaEnemyTurns;

        public RawImage[] SupernovaEnemyAttack;
        public TextMeshProUGUI[] SupernovaEnemyAttackMults;
        public RawImage[] SupernovaEnemyDefense;
        public TextMeshProUGUI[] SupernovaEnemyDefenseMults;

        #endregion

        #region Effects

        public RawImage[] SupernovaEffectImages;
        //public Text[] SupernovaEffectTexts;

        #endregion

        #endregion
        

        #region Private 

        private readonly string url = "https://medalviewer.blob.core.windows.net/images/";

        //private GameObject currSublistMedal;

        private Transform prevParent;
        private float prevScale;

        private float elapsedTime = 0.0f;

        private bool isTransition = false;

        private bool isDisplayingMedal = false;
        private bool isDisplayingSublist = false;
        private bool isDisplayingSupernova = false;
        //private bool isDisplayingSkills = false;
        //private bool isDisplayingTraits = false;
        //private bool isDisplayingPetTraits = false;

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

            AssignStats(medalDisplay, medalAbility);
            //AssignSlots(medalDisplay);
        }

        #region AssignContent

        private void AssignMedalInfo(MedalDisplay medalDisplay)
        {
            var imageUrl = url + medalDisplay.ImageURL;
            StartCoroutine(LoadImage(imageUrl, MedalPlaceholder.gameObject));
            
            //Ability.text = medalDisplay.Ability;
            //AbilityDescription.text = medalDisplay.AbilityDescription;

            if (medalDisplay.IsSupernova)
            {
                SupernovaButton.interactable = true;
                SupernovaButton.blocksRaycasts = true;
                SupernovaButton.alpha = 1;
            }
        }

        private void AssignSupernova(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            //SupernovaName.text = medalDisplay.SupernovaName;
            //SupernovaDescription.text = medalDisplay.SupernovaDescription;
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
            {
                //Player.SetCanvasGroupInactive();

                //if (ability != null)
                //    PlayerTurns.text = ability.Duration;
            }

            if (CheckEnemy())
            {
                //Enemy.SetCanvasGroupInactive();

                //if (ability != null)
                //    EnemyTurns.text = ability.Duration;
            }
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

            //if (CheckEffects())
                //Effects.SetCanvasGroupInactive();
        }

        private void AssignStats(MedalDisplay medalDisplay, MedalAbility medalAbility)
        {
            var BaseMultiplierLow = medalDisplay.BaseMultiplierLow;
            var BaseMultiplierHigh = medalDisplay.BaseMultiplierHigh;
            var MaxMultiplierLow = medalDisplay.MaxMultiplierLow;
            var MaxMultiplierHigh = medalDisplay.MaxMultiplierHigh;
            var GuiltMultiplierLow = medalDisplay.GuiltMultiplierLow;
            var GuiltMultiplierHigh = medalDisplay.GuiltMultiplierHigh;

            var Tier = medalDisplay.Tier;
            
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
                    
                    break;
                case 4:
                    Multiplier.text = "x" + (MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

                    break;
                case 5:
                    Multiplier.text = "x" + (MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

                    break;
                case 6:
                    Multiplier.text = "x" + (GuiltMultiplierHigh != "" ? GuiltMultiplierHigh :
                                            GuiltMultiplierLow != "" ? GuiltMultiplierLow :
                                            MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow != "" ? MaxMultiplierLow :
                                            BaseMultiplierHigh != "" ? BaseMultiplierHigh :
                                            BaseMultiplierLow);

                    break;
                case 7:
                    Multiplier.text = "x" + (GuiltMultiplierHigh != "" && GuiltMultiplierHigh != "0" ? GuiltMultiplierHigh :
                                            GuiltMultiplierLow != "" ? GuiltMultiplierLow : 
                                            MaxMultiplierHigh != "" ? MaxMultiplierHigh :
                                            MaxMultiplierLow);

                    break;
                default:
                    break;
            }
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
                //SupernovaPlayer.SetCanvasGroupInactive();
                if (ability != null)
                    SupernovaPlayerTurns.text = ability.Duration;
            }

            if (CheckSupernovaEnemy())
            {
                //SupernovaEnemy.SetCanvasGroupInactive();
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

            //if (CheckSupernovaEffects())
                //SupernovaEffects.SetCanvasGroupInactive();
        }

        #endregion

        #endregion Assign Attributes

        #region Reset Attributes

        public void ResetDisplay()
        {
            ResetIcons();
            ResetMedalName();
            ResetMedalInfo();
            ResetContent();
            //ResetSupernova();
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
            //MedalPlaceholderShadow.texture = MedalPlaceholder.texture;
            
            SupernovaButton.interactable = false;
            SupernovaButton.blocksRaycasts = false;
            SupernovaButton.alpha = 0;
        }

        private void ResetContent()
        {
            ResetPlayerEnemy();
            ResetEffects();

            ResetStats();
            //ResetSlots();
        }

        #region ResetContent

        public void ResetPlayerEnemy()
        {
            //PlayerTurns.text = "";
            //EnemyTurns.text = "";

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

            //Player.SetCanvasGroupActive();
            //Enemy.SetCanvasGroupActive();
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

            //Effects.SetCanvasGroupActive();
        }

        public void ResetStats()
        {
            Multiplier.text = "";
        }

        #endregion

        private void ResetSupernova()
        {
            //SupernovaName.text = "";
            //SupernovaDescription.text = "";
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

            //SupernovaPlayer.SetCanvasGroupActive();
            //SupernovaEnemy.SetCanvasGroupActive();
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

            //SupernovaEffects.SetCanvasGroupActive();
        }

        #endregion

        #endregion Reset Attributes

        #region Display Methods


        //public void HandleDisplay(GameObject clickedOn)
        //{
        //    if (clickedOn.GetComponentInChildren<GridLayoutGroup>().transform.childCount > 1)
        //    {
        //        DisplaySublistOfMedals(clickedOn);
        //    }
        //    else
        //    {
        //        StartCoroutine(Display(clickedOn.GetComponentInChildren<GridLayoutGroup>().transform.GetChild(0).gameObject));
        //    }
        //}
        
        public void HideCurrentMedal()
        {
            isDisplayingMedal = false;

            //HideSupernova();

            isTransition = true;
            elapsedTime = 0.0f;

            //StartCoroutine(HideDisplay(MedalHighlight));
            ResetDisplay();

            MedalHighlight.SetCanvasGroupInactive();
            ButtonCanvasGroup.SetCanvasGroupActive();
        }

        public void ShowSupernova()
        {
            isTransition = true;
            isDisplayingSupernova = true;
            elapsedTime = 0.0f;

            //StartCoroutine(ShowDisplay(MedalSupernova));
        }

        public void HideSupernova()
        {
            //if (MedalSupernova.alpha == 0)
            //    return;

            isTransition = true;
            isDisplayingSupernova = false;
            elapsedTime = 0.0f;

            //StartCoroutine(HideDisplay(MedalSupernova));
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
            //if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.C))
            //{
            //    if (isDisplayingMedal)
            //    {
            //        if (this.isDisplayingSupernova)
            //        {
            //            HideSupernova();
            //        }
            //        else
            //        {
            //            HideCurrentMedal();
            //        }
            //    }
            //    else if(isDisplayingSublist)
            //    {
            //        this.HideSublistOfMedals(true);
            //    }
            //}
        }

        void Awake()
        {
            Loading = GameObject.FindGameObjectWithTag("Loading").GetComponent<Loading>();

            ResetDisplay();
        }

        public IEnumerator Display(GameObject medalObject, MedalDisplay medal = null)
        {
            while (Loading.IsLoading)
            {
                yield return null;
            }
            Loading.StartLoading();
            ResetDisplay();

            isDisplayingMedal = true;

            MedalDisplay medalDisplay = null;

            if (medalObject != null)
                medalDisplay = medalObject.GetComponent<MedalDisplay>();
            else if (medal != null)
                medalDisplay = medal;

            MedalAbility medalAbility = null;
            MedalAbility medalAbilitySupernova = null;

            try
            {
                medalAbility = MedalAbilityParser.Parser(medalDisplay.AbilityDescription);

                if (medalDisplay.IsSupernova)
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

                //AssignSupernova(medalDisplay, medalAbilitySupernova);
            }

            #endregion

            isTransition = true;
            elapsedTime = 0.0f;

            //StartCoroutine(ShowDisplay(MedalHighlight));
            MedalHighlight.SetCanvasGroupActive();
            ButtonCanvasGroup.SetCanvasGroupInactive();
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

                //MedalPlaceholderShadow.texture = MedalPlaceholder.texture;
                //print("Finished");
            }

            Loading.FinishLoading();
        }

        //public IEnumerator ShowDisplay(CanvasGroup canvasGroup)
        //{
        //    //print("Test");
        //    StopCoroutine(HideDisplay(canvasGroup));
        //    elapsedTime = 0.0f;
        //    //print(isTransition);
        //    while (isTransition)
        //    {
        //        //print("Transition");
        //        if (!Loading.IsLoading)
        //        {
        //            //print("Tranisitioning");
        //            elapsedTime += Time.deltaTime;
        //            //print("Displaying");
        //            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, elapsedTime);
        //            canvasGroup.interactable = true;
        //            canvasGroup.blocksRaycasts = true;

        //            if (canvasGroup.alpha >= 1.0f)
        //            {
        //                elapsedTime = 0.0f;

        //                //isDisplayVar = true;
        //                isTransition = false;
        //            }
        //        }
        //        yield return null;
        //    }
        //}

        //public IEnumerator HideDisplay(CanvasGroup canvasGroup)
        //{
        //    StopCoroutine(ShowDisplay(canvasGroup));
        //    elapsedTime = 0.0f;

        //    while (isTransition)
        //    {
        //        elapsedTime += Time.deltaTime;
        //        //print("Hiding");
        //        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, elapsedTime);
        //        canvasGroup.interactable = false;
        //        canvasGroup.blocksRaycasts = false;

        //        if (canvasGroup.alpha <= 0.0f)
        //        {
        //            elapsedTime = 0.0f;

        //            //isDisplaying = false;
        //            isTransition = false;
        //        }
        //        yield return null;
        //    }
        //}

        //public void DisplaySublistOfMedals(GameObject clickedOn)
        //{
        //    isDisplayingSublist = true;

        //    if (Globals.CurrSublistMedal != null)
        //        HideSublistOfMedals();

        //    Globals.CurrSublistMedal = clickedOn;

        //    var canvasGroup = clickedOn.GetComponentsInChildren<CanvasGroup>().First(x => x.name == "SublistContent");

        //    canvasGroup.SetCanvasGroupActive();
        //}

        //public void HideSublistOfMedals(bool closed = false)
        //{
        //    isDisplayingSublist = false;
        //}
    }
}

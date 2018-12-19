using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MedalSpotlightDisplayManager : MonoBehaviour {

	public MedalCreator medalCreator;
    public TraitManager TraitManager;
    MedalAbilityParser MedalAbilityParser = new MedalAbilityParser();

    #region Base

    public Text MedalName;
    public RawImage PSMAttribute;
    public RawImage URAttribute;
	public RawImage MedalPlaceholder;
    public RawImage MedalPlaceholderShadow;
    public Text Ability;
    public Text AbilityDescription;

	public RawImage Tier;
	public RawImage Target;
	public RawImage Gauges;

    #endregion Base

    #region Traits

    public RawImage InitialImage;
    public RawImage[] TraitSlots;
    public Text[] TraitSlotsText;

    #endregion Traits

    #region Boosts and Saps

    public GameObject PlayerParent;
    public GameObject EnemyParent;

    public Text PlayerEffect;
	public Text EnemyEffect;

    public RawImage[] AttackBoosts;
	public Text[] AttackBoostMults;
	public RawImage[] DefenseBoosts;
	public Text[] DefenseBoostMults;

    public RawImage GaugeBoost;
    public RawImage SPBBoost;
    public RawImage HPBoost;

    public RawImage[] AttackSaps;
	public Text[] AttackSapsMults;
	public RawImage[] DefenseSaps;
	public Text[] DefenseSapsMults;

    public RawImage AttackCounterSap;
    public RawImage TurnCounterSap;
    public Text AttackTurnDivider;
    public RawImage DamageSap;
    public Text DamageText;

    #endregion Boosts and Saps

    #region Strength/Defense/Multipliers

    public Text BaseDefense;
    public Text BaseStrength;
    public Text MaxDefense;
    public Text MaxStrength;

    public RawImage MultiplierTier;
	public RawImage BaseMultiplier;
    public RawImage[] MaxMultipliers;
    public RawImage GuiltMultiplier;

	public Text[] MultiplierTexts;
    public Text[] EqualTexts;
    public Text[] MultiplierWithStrengthTexts;

    public SPATKBonusManager SPATKBonusManager;

    #endregion Strength/Defense/Multipliers

    #region Private 

    private int startIndexPlayer = 8;
    private int startIndexEnemy = 6;
    private string PlayerEffectInitial;
    private string EnemyEffectInitial;

    private CanvasGroup background_group;
	private GameObject MedalSublist;
	//private CanvasGroup MedalSublistGroup;

	private Transform prevParent;
	private float prevScale;

	private float elapsedTime = 0.0f;

	private bool isTransition = false;
	private bool isDisplaying = false;

	private Color32 visible = new Color (1f, 1f, 1f, 1f);
	private Color32 invisible = new Color (0f, 0f, 0f, 0f);

    private string initialTraitSlotText = "No Trait Selected";

    #endregion

    void Awake()
	{
		background_group = GameObject.FindGameObjectWithTag("Background").GetComponent<CanvasGroup>();
//		MedalSublistGroup = GameObject.FindGameObjectWithTag("MedalSublistGroup").GetComponent<CanvasGroup>();
		MedalSublist = GameObject.FindGameObjectWithTag("MedalSublist");

		medalCreator = GameObject.FindGameObjectWithTag("ScriptHolder").GetComponent<MedalCreator>();

		PlayerEffectInitial = PlayerEffect.text.Substring(0, startIndexPlayer);
		EnemyEffectInitial = EnemyEffect.text.Substring(0, startIndexEnemy);

        Reset();
    }

	public void Display(GameObject medalObject)
	{
        //MedalCycleLogic.Instance.StopCycleMedals();
        Reset();

        MedalDisplay medalDisplay = medalObject.GetComponent<MedalDisplay>();
        MedalAbility medalAbility = new MedalAbility();
        //var medalAbilityID = 0;

        isTransition = true;
        isDisplaying = true;
        elapsedTime = 0.0f;

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

        this.AssignBase(medalObject, medalDisplay);
        this.AssignTraits(medalDisplay);
		
        if(medalAbility != null)
		{
            medalAbility.SetUpDisplayAbility();

            this.AssignBoostsSaps(medalAbility);
		}

		this.AssignStrengthDefenseMultipliers(medalDisplay, medalAbility);

		#endregion

		//if(MedalSublistGroup.alpha >= 0.0f)
		//{
		//	HideSublistOfMedals();
		//}
	}

    #region Assign Attributes

    public void AssignBase(GameObject medalObject, MedalDisplay medalDisplay)
    {
        MedalPlaceholder.texture = medalObject.GetComponent<RawImage>().texture;
        MedalPlaceholderShadow.texture = MedalPlaceholder.texture;

        MedalName.text = medalDisplay.Name;
        PSMAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_PSM + "_Gem") as Texture2D;
        URAttribute.texture = Resources.Load("Gems/" + medalDisplay.Attribute_UR + "_Gem") as Texture2D;

        Tier.texture = Resources.Load("Tier/Tier_" + medalDisplay.Tier) as Texture2D;
        Target.texture = Resources.Load("Target/" + medalDisplay.Target) as Texture2D;
        Gauges.texture = Resources.Load("Gauges/" + medalDisplay.Gauge) as Texture2D;

        Ability.text = medalDisplay.Ability;
        AbilityDescription.text = medalDisplay.AbilityDescription;
    }

    public void AssignTraits(MedalDisplay medalDisplay)
    {
        for (int i = 0; i < medalDisplay.TraitSlots; ++i)
        {
            TraitSlots[i].enabled = true;
            TraitSlotsText[i].enabled = true;
        }
    }

	public void AssignBoostsSaps(MedalAbility medalAbility)
	{
        if (medalAbility.STR.Count > 0)
        {
            PlayerEffect.text += medalAbility.STR[0].Amount.ToUpper();
            EnemyEffect.text += medalAbility.STR[0].Amount.ToUpper();
        }
        else if (medalAbility.DEF.Count > 0)
        {
            PlayerEffect.text += medalAbility.DEF[0].Amount.ToUpper();
            EnemyEffect.text += medalAbility.DEF[0].Amount.ToUpper();
        }

        var strBoosts = medalAbility.CombatImages["STR"]["Raises"];
	    var boostCounterSTR = 0;
	    var boostCounterDEF = 0;

		for(int i = 0; i < strBoosts.Count; ++i)
		{
			AttackBoosts[i].texture = strBoosts[i];
			AttackBoosts[i].color = visible;
			AttackBoostMults[i].text = "x" + medalAbility.STR[boostCounterSTR++].Tier;
		}

		if(medalAbility.CombatImages["DEF"]["PlayerLowers"].Count > 0)
		{
			var defBoosts = medalAbility.CombatImages["DEF"]["PlayerLowers"];

			for(int i = 0; i < defBoosts.Count; ++i)
			{
				DefenseBoosts[i].texture = defBoosts[i];
				DefenseBoosts[i].color = visible;
				DefenseBoostMults[i].text = "x" + medalAbility.STR[boostCounterDEF++].Tier;
			}
		}
		else
		{
			var defBoosts = medalAbility.CombatImages["DEF"]["Raises"];

			for(int i = 0; i < defBoosts.Count; ++i)
			{
				DefenseBoosts[i].texture = defBoosts[i];
				DefenseBoosts[i].color = visible;
				DefenseBoostMults[i].text = "x" + medalAbility.DEF[boostCounterDEF++].Tier;
			}
		}

        GaugeBoost.texture = Resources.Load("Gauges/" + medalAbility.GAUGE) as Texture2D;
        SPBBoost.texture = Resources.Load("SPBonus/" + medalAbility.SPBONUS) as Texture2D;
        HPBoost.texture = Resources.Load("Heal/" + medalAbility.HEAL) as Texture2D;

        if (GaugeBoost.texture != null)
        {
            GaugeBoost.enabled = true;
        }

        if (SPBBoost.texture != null)
        {
            SPBBoost.enabled = true;
        }

        if (HPBoost.texture != null)
        {
            HPBoost.enabled = true;
        }

        var strSaps = medalAbility.CombatImages["STR"]["Lowers"];

		for(int i = 0; i < strSaps.Count; ++i)
		{
			AttackSaps[i].texture = strSaps[i];
			AttackSaps[i].color = visible;
			AttackSapsMults[i].text = "x" + medalAbility.STR[boostCounterSTR++].Tier;
		}

		var defSaps = medalAbility.CombatImages["DEF"]["Lowers"];

		for(int i = 0; i < defSaps.Count; ++i)
		{
			DefenseSaps[i].texture = defSaps[i];
			DefenseSaps[i].color = visible;
			DefenseSapsMults[i].text = "x" + medalAbility.DEF[boostCounterDEF++].Tier;
		}

        AttackCounterSap.texture = medalAbility.MiscImages["COUNT"];
        TurnCounterSap.texture = medalAbility.MiscImages["COUNT"];
        DamageSap.texture = Resources.Load("Damage/" + medalAbility.DAMAGE) as Texture2D;

        if(AttackCounterSap.texture != null)
        {
            AttackCounterSap.enabled = true;
            AttackTurnDivider.enabled = true;
        }

        if (TurnCounterSap.texture != null)
        {
            TurnCounterSap.enabled = true;
            AttackTurnDivider.enabled = true;
        }

        if (DamageSap.texture != null)
        {
            DamageSap.enabled = true;
            DamageText.enabled = true;
        }

        // If none of either are filled in, dis/able it
        PlayerParent.SetActive(CheckPlayerValues());
        EnemyParent.SetActive(CheckEnemyValues());
    } // TODO Rewrite this garbage - Make it variable and loop through the dictionary itself, not look at predefined results

	public void AssignStrengthDefenseMultipliers(MedalDisplay medalDisplay, MedalAbility medalAbility = null)
    {
        BaseDefense.text = medalDisplay.BaseDefense.ToString();
        MaxDefense.text = medalDisplay.MaxDefense.ToString();
        BaseStrength.text = medalDisplay.BaseStrength.ToString();
        MaxStrength.text = medalDisplay.MaxStrength.ToString();

        //MultiplierTexts[0].text = medalDisplay.BaseMultiplier;
		//MultiplierTexts[1].text = medalDisplay.MaxMultiplier;
		//MultiplierTexts[2].text = medalDisplay.GuiltMultiplier;

        switch(medalDisplay.Star)
        {
            case 1:
            case 2:
                MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");

                SPATKBonusManager.CurrMultiplier = medalDisplay.BaseMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

                EqualTexts[0].text = "=";
                MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

                BaseMultiplier.color = visible;
                break;
            case 3:
                MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
                MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

                SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

                EqualTexts[0].text = "=";
                MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

                EqualTexts[1].text = "=";
                MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

                BaseMultiplier.color = visible;
                MaxMultipliers[0].color = visible;
                break;
            case 4:
                MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
                MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

                SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

                EqualTexts[0].text = "=";
                MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

                EqualTexts[1].text = "=";
                MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

                BaseMultiplier.color = visible;
                MaxMultipliers[1].color = visible;
                break;
            case 5:
                MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
                MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");

                SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;

                EqualTexts[0].text = "=";
                MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

                EqualTexts[1].text = "=";
                MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

                BaseMultiplier.color = visible;
                MaxMultipliers[2].color = visible;
                break;
            case 6:
                MultiplierTexts[0].text = medalDisplay.BaseMultiplier.Replace("-", "~");
                MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");
                MultiplierTexts[2].text = medalDisplay.GuiltMultiplier.Replace("-", "~");
                
                SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;
                SPATKBonusManager.CurrTier = medalDisplay.Tier;

                EqualTexts[0].text = "=";
                MultiplierWithStrengthTexts[0].text = Parser.ParseMultiplierWithStrength(medalDisplay.BaseMultiplier, medalDisplay.MaxStrength);

                EqualTexts[1].text = "=";
                MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

                EqualTexts[2].text = "=";
                MultiplierWithStrengthTexts[2].text = Parser.ParseMultiplierWithStrength(medalDisplay.GuiltMultiplier, medalDisplay.MaxStrength);

                BaseMultiplier.color = visible;
                MaxMultipliers[3].color = visible;
                GuiltMultiplier.color = visible;

                MultiplierTier.color = visible;
                MultiplierTier.texture = Resources.Load("Tier/Tier_" + medalDisplay.Tier) as Texture2D;
                break;
            case 7:
                MultiplierTexts[1].text = medalDisplay.MaxMultiplier.Replace("-", "~");
                MultiplierTexts[2].text = medalDisplay.GuiltMultiplier.Replace("-", "~");
               
                SPATKBonusManager.CurrMultiplier = medalDisplay.MaxMultiplier;
                SPATKBonusManager.CurrMaxStrength = medalDisplay.MaxStrength;
                SPATKBonusManager.CurrTier = medalDisplay.Tier;

                EqualTexts[1].text = "=";
                MultiplierWithStrengthTexts[1].text = Parser.ParseMultiplierWithStrength(medalDisplay.MaxMultiplier, medalDisplay.MaxStrength);

                EqualTexts[2].text = "=";
                MultiplierWithStrengthTexts[2].text = Parser.ParseMultiplierWithStrength(medalDisplay.GuiltMultiplier, medalDisplay.MaxStrength);

                MaxMultipliers[3].color = visible;
                GuiltMultiplier.color = visible;

                MultiplierTier.color = visible;
                MultiplierTier.texture = Resources.Load("Tier/Tier_" + medalDisplay.Tier) as Texture2D;
                break;
            default:
                break;
        }

        SPATKBonusManager.UpdateSPATKBonus();
    }

    #endregion Assign Attributes

    #region Reset Attributes

    public void Reset()
    {
        ResetBase();
        ResetTraits();
        ResetBoostsSaps();
        ResetStrengthDefenseMultipliers();
    }

    public void ResetBase()
    {
        MedalPlaceholder.texture = Resources.Load("Medals/noctis") as Texture2D;
        MedalPlaceholderShadow.texture = MedalPlaceholder.texture;

        MedalName.text = "PLACEHOLDER";
        PSMAttribute.texture = null;
        URAttribute.texture = null;

        Tier.texture = null;
        Target.texture = null;
        Gauges.texture = null;

        Ability.text = "ABILITY";
        AbilityDescription.text = "ABILITY DESCRIPTION";
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
        PlayerEffect.text = PlayerEffectInitial;
        EnemyEffect.text = EnemyEffectInitial;

        foreach (var aB in AttackBoosts)
            aB.color = invisible;
        foreach (var aBM in AttackBoostMults)
            aBM.text = "";

        foreach (var dB in DefenseBoosts)
            dB.color = invisible;
        foreach (var dBM in DefenseBoostMults)
            dBM.text = "";

        GaugeBoost.enabled = false;
        GaugeBoost.texture = null;

        SPBBoost.enabled = false;
        SPBBoost.texture = null;

        HPBoost.enabled = false;
        HPBoost.texture = null;

        foreach (var aS in AttackSaps)
            aS.color = invisible;
        foreach (var aSM in AttackSapsMults)
            aSM.text = "";

        foreach (var dS in DefenseSaps)
            dS.color = invisible;
        foreach (var dSM in DefenseSapsMults)
            dSM.text = "";

        AttackCounterSap.enabled = false;
        AttackCounterSap.texture = null;

        TurnCounterSap.enabled = false;
        TurnCounterSap.texture = null;

        AttackTurnDivider.enabled = false;

        DamageSap.enabled = false;
        DamageSap.texture = null;

        DamageText.enabled = false;
    }

    public void ResetStrengthDefenseMultipliers()
    {
        BaseMultiplier.color = invisible;
        MaxMultipliers.ToList().ForEach(x => x.color = invisible);
        GuiltMultiplier.color = invisible;
        MultiplierTier.color = invisible;

        EqualTexts.ToList().ForEach(x => x.text = "");

        MultiplierWithStrengthTexts.ToList().ForEach(x => x.text = "");

        foreach (var mT in MultiplierTexts)
            mT.text = "";

        SPATKBonusManager.BonusMultiplierText.text = "";

        BaseDefense.text = "NA";
        MaxDefense.text = "NA";
        BaseStrength.text = "NA";
        MaxStrength.text = "NA";
    }

    #endregion Reset Attributes

    #region Display Methods

    public void HandleDisplay(GameObject clickedOn)
    {
        if (clickedOn.transform.childCount > 1)
        {
            DisplaySublistOfMedals(clickedOn);
        }
        else
        {
            Display(clickedOn.transform.GetChild(0).gameObject);
        }
    }

    public void DisplaySublistOfMedals(GameObject clickedOn)
    {
        MedalCycleLogic.Instance.StopCycleMedals();

        var medalList = clickedOn.GetComponentsInChildren<Transform>();
        this.prevParent = medalList[0];

        for (int i = 1; i < medalList.Length; ++i)
        {
            var medal = medalList[i];

            this.prevScale = medal.transform.localScale.x;
            medal.GetComponent<RawImage>().color = visible;

            medal.SetParent(MedalSublist.transform);
            medal.transform.localScale = new Vector3(1f, 1f, 1f);
            medal.GetComponent<RawImage>().raycastTarget = true;
        }

        //MedalSublistGroup.alpha = 1;
        //MedalSublistGroup.interactable = true;
        //MedalSublistGroup.blocksRaycasts = true;
    }

    public void HideSublistOfMedals()
    {
        //MedalSublistGroup.alpha = 0;
        //MedalSublistGroup.interactable = false;
        //MedalSublistGroup.blocksRaycasts = false;

        var medalList = MedalSublist.GetComponentsInChildren<Transform>();

        for (int i = 1; i < medalList.Length; ++i)
        {
            var medal = medalList[i];

            medal.GetComponent<RawImage>().raycastTarget = false;

            medal.SetParent(this.prevParent);
            medal.transform.localScale = new Vector3(this.prevScale, this.prevScale, this.prevScale);
            medal.GetComponent<RawImage>().color = invisible;
        }

    }

    public void HideCurrentMedal()
    {
        isTransition = true;
        isDisplaying = false;
        elapsedTime = 0.0f;

        TraitManager.HideToolBox();

        //Reset();
    }

    #endregion Display Methods

    public bool CheckPlayerValues()
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

        if (GaugeBoost.texture != null || SPBBoost.texture != null || HPBoost.texture != null)
        {
            return true;
        }

        return false;        
    }

    public bool CheckEnemyValues()
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

        if (AttackCounterSap.texture != null || TurnCounterSap.texture != null || DamageSap.texture != null)
        {
            return true;
        }

        return false;
    }

    void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
            if (/*MedalSublistGroup.alpha >= 0.0f ||*/ isDisplaying)
            {
                //if (!MedalCycleLogic.Instance.firstPass)
                //{
                //    HideSublistOfMedals();
                //}
                if(TraitManager.IsDisplayingTrait)
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

		if(isTransition)
		{
			elapsedTime += Time.deltaTime;
			// If we haven't moved, we are at our initial position, else we are moving back
			if(isDisplaying)
			{
				background_group.alpha = Mathf.Lerp(background_group.alpha, 1, elapsedTime);
                background_group.interactable = true;
                background_group.blocksRaycasts = true;

                if (background_group.alpha >= 1.0f)
				{
					elapsedTime = 0.0f;
					isTransition = false;
				}
			}
			else
			{
				background_group.alpha = Mathf.Lerp(background_group.alpha, 0, elapsedTime);
                background_group.interactable = false;
                background_group.blocksRaycasts = false;

                if (background_group.alpha <= 0.0f)
				{
					elapsedTime = 0.0f;
					isTransition = false;
                }
			}
		}
	}
}

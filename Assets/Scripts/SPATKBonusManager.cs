using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SPATKBonusManager : MonoBehaviour {

    public RawImage BonusMultiplier;
    public Button[] BonusMultiplierButtons;
    public Text BonusMultiplierText;
    public Text BonusMultiplierTextValue;
    public int BonusMultiplierValue = 20;

    public string CurrMultiplier;
    public int CurrMaxStrength;
    public int CurrTier;

    private int MaxBonus = 120;
    private int MinBonus = 20;

    public void IncreaseSPATKBonus()
    {
        BonusMultiplierValue += 10;

        if(BonusMultiplierValue == 50)
        {
            BonusMultiplierValue += 10;
        }

        BonusMultiplier.texture = Resources.Load("SPATKBonus/" + BonusMultiplierValue) as Texture2D;

        if(BonusMultiplierValue == MaxBonus)
        {
            BonusMultiplierButtons[1].interactable = false;
        }
        else if(!BonusMultiplierButtons[0].interactable)
        {
            BonusMultiplierButtons[0].interactable = true;
        }

        this.UpdateSPATKBonus();
    }

    public void DecreaseSPATKBonus()
    {
        BonusMultiplierValue -= 10;

        if (BonusMultiplierValue == 50)
        {
            BonusMultiplierValue -= 10;
        }

        BonusMultiplier.texture = Resources.Load("SPATKBonus/" + BonusMultiplierValue) as Texture2D;

        if (BonusMultiplierValue == MinBonus)
        {
            BonusMultiplierButtons[0].interactable = false;
        }
        else if (!BonusMultiplierButtons[1].interactable)
        {
            BonusMultiplierButtons[1].interactable = true;
        }

        this.UpdateSPATKBonus();
    }

    public void UpdateSPATKBonus()
    {
        BonusMultiplierText.text = Parser.ParseMultiplier(CurrMultiplier, BonusMultiplierValue, CurrTier);

        //Debug.Log(BonusMultiplierText.text + " " + CurrMaxStrength);
        BonusMultiplierTextValue.text =
                     Parser.ParseMultiplierWithStrength(BonusMultiplierText.text, CurrMaxStrength);
    }
}

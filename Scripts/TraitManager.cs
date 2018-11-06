using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitManager : MonoBehaviour {

    public RawImage[] Slots;
    public Text[] SlotsText;

    public CanvasGroup SlotGroup;
    public GameObject SlotGameObject;

    public bool IsDisplayingTrait;

    private int currIndex;
    private Dictionary<string, string> slotStrings = new Dictionary<string, string>
    {
        {"Ground", "Ground DEF -60%"}, {"Aerial", "Aerial DEF -60%"},
        {"Raids", "Raids +40%"}, {"ExtraAttack", "Extra Attack: +40%"},
        {"STR1000", "STR +1000"}, {"DEF2000", "DEF +2000"}
    };
	
    public void ClickedSlot(GameObject slot)
    {
        currIndex = int.Parse(slot.name.Split('_')[1]) - 1;

        DisplayToolBox();
    }

    public void ClickedTrait(GameObject trait)
    {
        Slots[currIndex].texture = trait.GetComponent<RawImage>().texture;
        SlotsText[currIndex].text = slotStrings[trait.name];

        HideToolBox();
    }

    public void DisplayToolBox()
    {
        SlotGameObject.GetComponent<RectTransform>().position = Input.mousePosition;

        SlotGroup.alpha = 1;
        SlotGroup.interactable = true;
        SlotGroup.blocksRaycasts = true;

        IsDisplayingTrait = true;
    }

    public void HideToolBox()
    {
        SlotGroup.alpha = 0;
        SlotGroup.interactable = false;
        SlotGroup.blocksRaycasts = false;

        IsDisplayingTrait = false;
    }

    // Update is called once per frame
    void Update () 
    {
		
	}
}

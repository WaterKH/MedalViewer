using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MedalViewer.Medal;

namespace MedalViewer
{
    public class SlotDisplay : MonoBehaviour
    {

        public Text KeybladeName;
        public Text SlotNumber;
        public Text Multiplier;

        public RawImage Keyblade;
        public RawImage OrbPSM;
        public RawImage SlotUR;

        public void AssignSlot(KeybladeMultiplierSlot keybladeSlot)
        {
            KeybladeName.text = keybladeSlot.Name;
            SlotNumber.text = keybladeSlot.SlotNumber.ToString();
            if(keybladeSlot.Multiplier.ToString().Contains("."))
                Multiplier.text = "x" + keybladeSlot.Multiplier.ToString().PadRight(4, '0');
            else
                Multiplier.text = "x" + keybladeSlot.Multiplier.ToString() + ".".PadRight(3, '0');

            var keybladeName = keybladeSlot.Name.Replace(" ", "").Equals("BadGuyBreaker") ? keybladeSlot.Name.Replace(" ", "") : keybladeSlot.Name.Replace(" ", "") + "3";
            Keyblade.texture = Resources.Load("Slots/Keyblades/" + keybladeName) as Texture2D;
            OrbPSM.texture = Resources.Load("Gems/" + keybladeSlot.PSM + "_Gem") as Texture2D;
            if(!string.IsNullOrEmpty(keybladeSlot.UR))
                SlotUR.texture = Resources.Load("Slots/" + keybladeSlot.UR) as Texture2D;
            else
                SlotUR.texture = Resources.Load("Slots/Normal") as Texture2D;
        }
    }
}

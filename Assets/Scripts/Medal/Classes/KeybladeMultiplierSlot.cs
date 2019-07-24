using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MedalViewer.Medal
{
    public class KeybladeMultiplierSlot
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public int SlotNumber { get; set; }
        public string PSM { get; set; }
        public string UR { get; set; }
        public double KeybladeLevel { get; set; }
        public double Multiplier { get; set; }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medal {

    public int Id { get; set; }
	public string Name { get; set; }
    public string ImageURL { get; set; }
    public int Star { get; set; }
    public string Class { get; set; }
    public string Type { get; set; }
    public string Attribute_PSM { get; set; }
    public string Attribute_UR { get; set; }

    public string Discriminator { get; set; }

    public int BaseAttack { get; set; }
    public int MaxAttack { get; set; }
    public int BaseDefense { get; set; }
    public int MaxDefense { get; set; }
    public int TraitSlots { get; set; }
    public int BasePetPoints { get; set; }
    public int MaxPetPoints { get; set; }
    public string Ability { get; set; }
    public string AbilityDescription { get; set; }
    public string Target { get; set; }
    public int Gauge { get; set; }
    public string BaseMultiplier { get; set; }
    public string MaxMultiplier { get; set; }
    public string GuiltMultiplier { get; set; }
    public string SubslotMultiplier { get; set; }
    public int Tier { get; set; }
    public string SupernovaName { get; set; }
    public string SupernovaDescription { get; set; }
    public string SupernovaDamage { get; set; }
    public string SupernovaTarget { get; set; }

    public string Effect { get; set; }
    public string Effect_Description { get; set; }
}

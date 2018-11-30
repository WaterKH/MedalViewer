using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class MedalAbilityParser : MonoBehaviour
{

    public Regex dealMultipleRegex = new Regex("Deals (\\d+) [\\w+\\s*]? hits");
    public Regex dealOneRegex = new Regex("Deals an? [\\w+\\s*] hit");
    public Regex raiseLowerRegex = new Regex(@"^(?:Lowers|Raises)(?: target(?:s')?(?:'s)?)? (\w+\W?\w+ (?:by \d+ tier\w? )?&*\s?)*for \d+ (?:turn(?:s)?|attack(?:s))");
    public Regex lowerRegex = new Regex(@"Lowers targets' (\w+) by (\d+) tiers for (\d+) (\w+)");
    public Regex updatedRaiseLowerRegex = new Regex(@"(\d+) (\w+): ([.+,*]+)");
    public Regex innerUpdatedRaiseLowerRegex = new Regex("(\\w)[ target\\w+]? (\\w+) by (\\d+)");

    public Regex inflictFixedRegex = new Regex("Inflicts[ a]? fixed");
    // 358
    public Regex inflictMoreRegex = new Regex("Inflicts more damage ([\\w+\\s*]+)");
    
    // 320
    public Regex inflictTheHigherLowerBiggerSmallerRegex = new Regex("the ([\\w+]?) ([the slot number]?[your HP]?[your party]?)");
    public Regex inflictTheMoreLess = new Regex("the ([more]?[less]?) ([special]?[SP]?[skills]?[Lux]?[turns]?[gauges used]?[gauges are full]?[enemies]?)");
    public Regex inflictTheMoreGauges = new Regex("the more gauges ([\\w+\\s*])");
    public Regex inflictToRegex = new Regex("to (\\w+) targets");

    // 11
    public Regex inflictWithRegex = new Regex("with ([\\d+]) enemy left");

    // 27
    public Regex inflictedInRegex = new Regex("in slot (\\d+)");
    public Regex inflictedIfRegex = new Regex("if the target is ([\\w+])");

    public Regex recoverRegex = new Regex("([\\w+]) recovers HP");
    public Regex hpRecoverRegex = new Regex("HP recovery LV (\\d+)");
    public Regex hpMaxRegex = new Regex("HP MAX");

    public Regex fillRegex = new Regex("Fills (\\d+) gauges");
    public Regex gaugeRegex = new Regex("Gauge \\+(\\d+)");

    public Regex curesRegex = new Regex("Cures [own]? status ailments", RegexOptions.IgnoreCase);
    

    public MedalAbility Parser(string abilityDescription)
    {
        var returnMedal = new MedalAbility();

        var parts = abilityDescription.Split('.');

        foreach (var item in parts)
        {

        }

        return returnMedal;
    }
}

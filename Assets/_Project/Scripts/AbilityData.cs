using UnityEngine;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Scriptable Objects/AbilityData")]
public class AbilityData : ScriptableObject
{

    [Header("Ability Attributes")]

    public Sprite abilityIcon;
    public string abilityName;
    public enum AbilityType { Physical, Magic};
    public AbilityType abilityType;
    [TextArea(3, 6)] public string abilityDescription;

    [Header("Ability Effects")]

    public GameObject abilityVfx;
    public enum AbilityEffect{ Damage, Heal};
    public AbilityEffect abilityEffect;
    public enum PowerType { Flat, Percent}; // % max de la cible
    public PowerType powerType;
    public int power;
    public int duration;
    public enum AbilityCostResource { Mana, Hp}
    public AbilityCostResource abilityCostResource;
    public enum AbilityCostType { Flat, Percent};
    public AbilityCostType abilityCostType;
    public int abilityCost;
    [Range(0, 100)] public int accuracy;
    public enum TargetSide { Ennemies, Allies, Self};
    public TargetSide targetSide;
    public enum TargetMode { Single, All};
    public TargetMode targetMode;


    public string FormattedDescription()
    {
        string value = power.ToString();
        string cost = abilityCost.ToString();
        string accuracyindice = accuracy.ToString();

        if (powerType == PowerType.Percent)
            value = value + "%";
        if (abilityCostType == AbilityCostType.Percent)
            cost = cost + "%";

        accuracyindice = accuracyindice + "%";

        // Power
        if (targetSide == TargetSide.Allies || targetSide == TargetSide.Self)
            value = "<color=#5EF461>" + value + "</color>"; //couleur vert heal
        else
            value = "<color=#FFD86B>" + value + "</color>"; //couleur doré

        value = "<b>" + value + "</b>"; // gras
        value = "<size=120%>" + value + "</size>"; // plus grand

        //Cost
        cost = "<color=#18E4F4>" + cost + "</color>"; //couleur bleu mana
        cost = "<b>" + cost + "</b>";
        cost = "<size=120%>" + cost + "</size>";

        //Accuracy
        accuracyindice = "<color=#FFD86B>" + accuracyindice + "</color>"; //couleur doré
        accuracyindice = "<b>" + accuracyindice + "</b>";
        accuracyindice = "<size=120%>" + accuracyindice + "</size>";

        return abilityDescription
            .Replace("{power}", value)
            .Replace("{cost}", cost)
            .Replace("{accuracy}", accuracyindice);
    }






}

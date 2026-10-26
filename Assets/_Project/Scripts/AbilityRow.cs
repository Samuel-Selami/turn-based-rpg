using TMPro;
using UnityEngine;

public class AbilityRow : MonoBehaviour
{
    [SerializeField] TMP_Text abilityName;
    [SerializeField] TMP_Text abilityManaCostText;
    [SerializeField] GameObject highlight;

    public void Fill(AbilityData ability)
    {
        abilityName.text = ability.abilityName;
        abilityManaCostText.text = ability.abilityCost.ToString();
    }
    
    public void SetHighlighted(bool on)
    {
        highlight.SetActive(on);
    }
}

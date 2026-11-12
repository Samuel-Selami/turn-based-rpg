using UnityEngine;
using UnityEngine.UI;
using static BattleInput;

public class ButtonPrompt : MonoBehaviour
{
    [Header("Image")]

    [SerializeField] Image confirmIcon;
    [SerializeField] Image backIcon;

    [Header("--------- Icon Sprite ---------")]

    [Header("Keyboard")]
    [SerializeField] Sprite keyboardConfirm;
    [SerializeField] Sprite keyboardBack;
    [Header("Xbox")]
    [SerializeField] Sprite xboxConfirm;
    [SerializeField] Sprite xboxBack;
    [Header("PS")]
    [SerializeField] Sprite psConfirm;
    [SerializeField] Sprite psBack;
    [Header("Generic Gamepad")]
    [SerializeField] Sprite genericConfirm;
    [SerializeField] Sprite genericBack;

    PadType lastPad;

    void Start()
    {
        
    }

    void Update()
    {
        PadType pad = BattleInput.CurrentPad();
        if (pad == lastPad) return;
        lastPad = pad;

        switch (pad)
        {
            case PadType.Keyboard:
                SetIcons(keyboardConfirm, keyboardBack);
                break;

            case PadType.Xbox:
                SetIcons(xboxConfirm, xboxBack);
                break;

            case PadType.Playstation:
                SetIcons(psConfirm, psBack);
                break;

            case PadType.Generic:
                SetIcons(genericConfirm, genericBack);
                break;
        }
    }

    void SetIcons(Sprite confirm, Sprite back)
    {
        if (confirmIcon != null) confirmIcon.sprite = confirm;
        if (backIcon != null) backIcon.sprite = back;
    }
}

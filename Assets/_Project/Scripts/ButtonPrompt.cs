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
                confirmIcon.sprite = keyboardConfirm;
                backIcon.sprite = keyboardBack;
                break;

            case PadType.Xbox:
                confirmIcon.sprite = xboxConfirm;
                backIcon.sprite = xboxBack;
                break;

            case PadType.Playstation:
                confirmIcon.sprite = psConfirm;
                backIcon.sprite = psBack;
                break;

            case PadType.Generic:
                confirmIcon.sprite= genericConfirm;
                backIcon.sprite= genericBack;
                break;
        }
    }
}

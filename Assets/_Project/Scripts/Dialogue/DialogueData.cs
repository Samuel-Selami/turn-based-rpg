using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    public Sprite portrait;
    public bool isplayerSide;
    [TextArea(3, 6)] public string text;
}

[CreateAssetMenu(fileName = "DialogueData", menuName = "Scriptable Objects/DialogueData")]

public class DialogueData : ScriptableObject
{
    public List<DialogueLine> lines;
}

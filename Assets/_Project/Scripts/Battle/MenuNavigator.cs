using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TitleScreenSelection
{
    public RectTransform button;
    public GameObject highlightBorder;
    public GameObject highlightStarLeft;
    public GameObject highlightStarRight;


    public void SetHighlighted(bool on, float highlightScale)
    {
        highlightBorder.SetActive(on);
        highlightStarLeft.SetActive(on);
        highlightStarRight.SetActive(on);

        float scale = 1f;
        if (on)
            scale = highlightScale;
        button.localScale = Vector3.one * scale; // scale up
    }
}
public class MenuNavigator : MonoBehaviour
{
    [SerializeField] List<TitleScreenSelection> entries;
    [SerializeField] RectTransform mainMenuSelectionArrow;
    [SerializeField] float arrowSpeed = 1f;
    public int index;
    Vector3 arrowTarget;
    [SerializeField] float highlightScale = 1.2f;


    void Start()
    {
        Highlight(0);
    }

    void Highlight(int i)
    {
        for (int j = 0; j < entries.Count; j++)
            entries[j].SetHighlighted(false, highlightScale);
        entries[i].SetHighlighted(true, highlightScale);

        if (mainMenuSelectionArrow != null)
        {
            arrowTarget = mainMenuSelectionArrow.position;
            arrowTarget.y = entries[i].button.position.y;
        }
    }

    void Update()
    {
        if (entries.Count == 0) return;

        int v = BattleInput.Vertical();
        if (v != 0)
        {
            index = (index + v + entries.Count) % entries.Count;
            Highlight(index);
        }
        if (mainMenuSelectionArrow != null)
            mainMenuSelectionArrow.position = Vector3.Lerp(mainMenuSelectionArrow.position, arrowTarget, arrowSpeed * Time.deltaTime);
    }
}

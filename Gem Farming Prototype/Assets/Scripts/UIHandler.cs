using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : Singleton<UIHandler>
{
    [SerializeField] TMP_Text goldText;
    [SerializeField] CanvasGroup collectedGemsButtonCG;
    [SerializeField] CanvasGroup collectedGemsCG;
    [SerializeField] ScrollViewSlot scrollViewSlotPrefab;
    List<ScrollViewSlot> scrollViewSlots = new List<ScrollViewSlot>();

    public void InitUI()
    {
        UpdateGoldText(PlayerPrefsHandler.Instance.gold);
        CreateScrollViewLayout();
    }

    #region GoldRelatedStuff
    public void AnimateGoldText(int to)
    {
        string currentTextValue = goldText.text;
        int from = string.IsNullOrEmpty(currentTextValue) ? 0 : int.Parse(currentTextValue);

        DOVirtual.Int(from, to, .1f, (x) => UpdateGoldText(x));
    }

    void UpdateGoldText(int value)
    {
        goldText.text = $"{value}";
    }

    #endregion

    #region ScrollView
    void CreateScrollViewLayout()
    {
        ScrollRect scroll = collectedGemsCG.GetComponent<ScrollRect>();
        for (int i = 0; i < GameManager.Instance.gems.Count; i++)
        {
            ScrollViewSlot temp = Instantiate(scrollViewSlotPrefab, scroll.content);
            temp.InitSlot(GameManager.Instance.gems[i], i);
            scrollViewSlots.Add(temp);
        }
    }

    public void UpdateCollectedCount(int index, int newCount)
    {
        scrollViewSlots[index].UpdateCollectedCount(newCount);
    }

    #endregion

    #region ButtonCallbacks

    public void SetCollectedGemsCG(bool value)
    {
        SetCGActiveness(collectedGemsCG, value);
        SetCGActiveness(collectedGemsButtonCG, !value);
    }

    #endregion

    #region Utils

    void SetCGActiveness(CanvasGroup cg, bool value)
    {
        cg.alpha = value ? 1f : 0f;
        cg.blocksRaycasts = value;
    }

    #endregion
}

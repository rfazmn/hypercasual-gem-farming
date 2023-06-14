using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewSlot : MonoBehaviour
{
    [SerializeField] Image gemImage;
    [SerializeField] TMP_Text typeText;
    [SerializeField] TMP_Text collectedCountText;

    public void InitSlot(GemSO gemSO, int index)
    {
        gemImage.sprite = gemSO.icon;
        typeText.text = gemSO.gemName;
        collectedCountText.text = $"{PlayerPrefsHandler.Instance.collectedGems[index]}";
    }

    public void UpdateCollectedCount(int newCount)
    {
        collectedCountText.text = $"{newCount}";
    }
}

using System.Collections;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] float sellingInterval = .1f;

    void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SellGems());
    }

    void OnTriggerExit(Collider other)
    {
        StopAllCoroutines();
    }

    IEnumerator SellGems()
    {
        float timer = sellingInterval;
        while(GameManager.Instance.gemStack.Count > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = sellingInterval;
                GameManager.Instance.RemoveStackOffsetOnGemSold();
                GemController topOfTheStack = GameManager.Instance.PopGemStack();
                PlayerPrefsHandler.Instance.IncreaseGold(topOfTheStack.CalculateSellPrice());
                PlayerPrefsHandler.Instance.IncreaseCollectedGems(topOfTheStack.gemIndex);
                UIHandler.Instance.UpdateCollectedCount(topOfTheStack.gemIndex, PlayerPrefsHandler.Instance.collectedGems[topOfTheStack.gemIndex]);
                topOfTheStack.OnGemSold();
            }

            yield return null;
        }
    }
}

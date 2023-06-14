using DG.Tweening;
using UnityEngine;

public class GemController : MonoBehaviour
{
    public int gemIndex;
    float scaleValue;
    GemSpawner currentSpawner;
    bool collected = false;

    void Update()
    {
        if (transform.localScale.x < 1f && !collected)
        {
            scaleValue = transform.localScale.x + Time.deltaTime / GameManager.Instance.gemGrowthTime;
            scaleValue = Mathf.Clamp(scaleValue, 0f, 1f);
            transform.localScale = Vector3.one * scaleValue;
        }
    }

    public void InitGem(Vector3 spawnPos, GemSpawner spawner)
    {
        currentSpawner = spawner;
        spawnPos.y = 1f;
        transform.position = spawnPos;
        transform.localScale = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void OnGemSold()
    {
        collected = false;
        transform.SetParent(GameManager.Instance.gemParent);
        gameObject.SetActive(false);
        scaleValue = 0f;
    }

    public int CalculateSellPrice()
    {
        int price = GameManager.Instance.gems[gemIndex].defaultPrice + (int)(scaleValue * 100f);
        return price;
    }

    void OnTriggerStay(Collider other)
    {
        if (collected)
            return;

        if (transform.localScale.x >= GameManager.Instance.minGemStackSize)
        {
            currentSpawner.SpawnGem(transform.position);
            transform.SetParent(other.transform);
            MoveToTarget(GameManager.Instance.GetNewGemStackOffset(transform.localScale.x));
            GameManager.Instance.PushGemStack(this);
            collected = true;
        }
    }

    void MoveToTarget(Vector3 localGemOffset)
    {
        float distance = Vector3.Distance(localGemOffset, transform.position);
        transform.DOLocalMove(localGemOffset, distance * GameManager.Instance.gemUnitMoveSpeed);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [Header("Game Settings")]
    [Space]
    [Header("Gem")]
    public float minGemStackSize = .25f;
    public float gemUnitMoveSpeed = .1f;
    [Tooltip("Total time of gem size from 0 to 1")]
    public float gemGrowthTime = 5f;

    [Space]
    public Transform gemParent;
    public List<GemSO> gems;

    public List<GemController> gemPool;
    public List<GemController> gemStack;

    //first offset of stacked gem
    [SerializeField] Vector3 gemStackOffset = new Vector3(0f, 1f, -1.1f);
    Vector3 gemStackPosition;
    float gemUnitLength = .775f;

    public void PushGemStack(GemController gemController)
    {
        gemStack.Add(gemController);
    }

    public GemController PopGemStack()
    {
        int lastIndex = gemStack.Count - 1;
        GemController gem = gemStack[lastIndex];
        gemStack.RemoveAt(lastIndex);
        return gem;
    }

    #region GemUtils

    public GemController GetAvailableGem()
    {
        GemController gem = null;
        for (int i = 0; i < gemPool.Count; i++)
        {
            if (!gemPool[i].gameObject.activeSelf)
            {
                gem = gemPool[i];
                break;
            }
        }

        if (gem == null)
        {
            gem = Instantiate(GetRandomGem(), gemParent).GetComponent<GemController>();
            gemPool.Add(gem);
        }

        return gem;
    }

    public GameObject GetRandomGem()
    {
        int gemIndex = Random.Range(0, gems.Count);
        return gems[gemIndex].gemPrefab;
    }

    public Vector3 GetNewGemStackOffset(float currentGemScaleValue)
    {
        if (gemStack.Count == 0)
        {
            gemStackPosition = gemStackOffset;
            return gemStackPosition;
        }

        Transform lastStackedGem = gemStack[gemStack.Count - 1].transform;
        float gemUnitLength = .775f;
        //gem unit length * half of last stacked gem scale + gem unit length * half of current collected gem scale
        //Due to the origin point of the gem object this solution may result in a bit of spaces between gems
        float newYOffset =  (gemUnitLength * lastStackedGem.localScale.x * .5f) + (gemUnitLength * currentGemScaleValue * .5f);
        gemStackPosition.y += newYOffset;
        return gemStackPosition;
    }

    public void RemoveStackOffsetOnGemSold()
    {
        if (gemStack.Count < 2)
            return;

        Transform lastStackedGem = gemStack[gemStack.Count - 1].transform;
        Transform secondToLastGem = gemStack[gemStack.Count - 2].transform;

        float offset = (gemUnitLength * lastStackedGem.localScale.x * .5f) + (gemUnitLength * secondToLastGem.localScale.x * .5f);
        gemStackPosition.y -= offset;
    }

    #endregion
}

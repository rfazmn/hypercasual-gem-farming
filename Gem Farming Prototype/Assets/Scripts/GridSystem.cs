using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] Vector2Int gridSize;
    [SerializeField] GameObject gridGroundPrefab;
    [SerializeField] GameObject gemSpawnerPrefab;

    [Tooltip("Overrides generation from the editor")]
    [SerializeField] bool generateRuntime;

    [Tooltip("If this value is checked, enter the vector values below ('CustomPosition'), otherwise ignore")]
    [SerializeField] bool customSpawn;
    [SerializeField] Vector3 customPosition;

    [Tooltip("Total offset from the edge of the grid to the gems")]
    [SerializeField] float gridScaleOffset = .025f;
    [SerializeField] float gemSpawnerUnitScale = .1f;
    [SerializeField] float offsetBetweenSpawners = .1f;

    void Start()
    {
        if (!generateRuntime)
            return;

        GenerateGrid();
    }

    [ContextMenu("Generate Grid")]
    void GenerateGridEditor()
    {
        if (generateRuntime)
            return;

        GenerateGrid();
    }

    [ContextMenu("Destroy Grid")]
    void DestroyGrid()
    {
        GetComponent<GemSpawner>().ClearSpawners();
        ClearChilds();
    }

    void GenerateGrid()
    {
        ClearChilds();

        if (gridSize.x == 0 || gridSize.y == 0)
        {
            Debug.Log("Grid values can not be 0");
            return;
        }

        GemSpawner gemSpawner = GetComponent<GemSpawner>();
        gemSpawner.ClearSpawners();
        gemSpawner.ableToSpawn = !generateRuntime;
        //0.1f / 1f
        float scaleAndLengthRatio = .1f;

        GameObject gridGround = Instantiate(gridGroundPrefab, transform);
        Vector3 gridSpawnPos = customSpawn ? customPosition : transform.position;
        gridSpawnPos.y = gridGround.transform.position.y;
        gridGround.transform.position = gridSpawnPos;
        gridGround.transform.localScale = new Vector3(gridSize.x * gemSpawnerUnitScale + gridScaleOffset + (gridSize.x - 1) * offsetBetweenSpawners * scaleAndLengthRatio, 1f, gridSize.y * gemSpawnerUnitScale + gridScaleOffset + (gridSize.y - 1) * offsetBetweenSpawners * scaleAndLengthRatio);

        //corresponding length in world space
        float unitLength = gemSpawnerUnitScale * 10f;
        float halfOfUnitLength = unitLength * .5f;
        float increaseBy = unitLength + offsetBetweenSpawners;
        float halfOfOffsetBetweenGems = offsetBetweenSpawners * .5f;

        float startPosX = gridSize.x * .5f * -unitLength + halfOfUnitLength - (gridSize.x - 1) * halfOfOffsetBetweenGems;

        for (int x = 0; x < gridSize.x; x++)
        {
            float startPosY = gridSize.y * .5f * -unitLength + halfOfUnitLength - (gridSize.y - 1) * halfOfOffsetBetweenGems;

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject gemSpawnerGO = Instantiate(gemSpawnerPrefab);
                gemSpawnerGO.transform.localScale = Vector3.one * gemSpawnerUnitScale;
                gemSpawnerGO.transform.position = gridGround.transform.position + new Vector3(startPosX, gemSpawnerGO.transform.position.y, startPosY);
                gemSpawnerGO.transform.SetParent(gridGround.transform, true);
                gemSpawner.AddSpawnersToList(gemSpawnerGO);

                startPosY += increaseBy;
            }
            startPosX += increaseBy;
        }

        gemSpawner.SpawnGemBatched(generateRuntime);
    }

    void ClearChilds()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
}

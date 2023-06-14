using UnityEngine;

[CreateAssetMenu(menuName ="Gem/New Gem",fileName ="New Gem")]
public class GemSO : ScriptableObject
{
    public string gemName;
    public int defaultPrice;
    public Sprite icon;
    public GameObject gemPrefab;
}

using UnityEngine;

[CreateAssetMenu(fileName = "CreatureObject", menuName = "Scriptable Objects/CreatureObject")]
public class CreatureObject : ScriptableObject
{
    public CreatureType type;
    public string creatureName;

    public Vector2 minMaxScale = Vector2.one;
    public Vector2 minMaxWeight = Vector2.one; //Kg
    public Vector2 minMaxValue = Vector2.one;

    public CreatureVariationEntry[] variations;
    
}


[System.Serializable]
public class CreatureVariationEntry
{
    public int tickets;
    public GameObject prefab;
    public float valueMultiplier;
    public Rarity rarity;
}

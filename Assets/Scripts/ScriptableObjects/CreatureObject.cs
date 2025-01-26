using UnityEngine;

[CreateAssetMenu(fileName = "CreatureObject", menuName = "Scriptable Objects/CreatureObject")]
public class CreatureObject : ScriptableObject
{
    public CreatureType type;
    public string creatureName;

    public Vector2 minMaxScale = Vector2.one;
    public Vector2 minMaxWeight = Vector2.one; //Kg
    public Vector2 minMaxValue = Vector2.one;

    public int[] tickets = new int[1] {1000}; //odds
    public GameObject[] prefabVariations = new GameObject[1]; //0 must be default
    public float[] valueMultiplier = new float[1] {1};
    public Rarity[] rarity = new Rarity[1] {Rarity.COMMON};
    
}

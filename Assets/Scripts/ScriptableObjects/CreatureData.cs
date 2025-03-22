using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Scriptable Objects/CreatureData")]
public class CreatureData : ScriptableObject
{

    public void Init(CreatureType type, string name, int variation, float scale, float weight, int value, Rarity rarity)
    {
        this.type = type;
        this.name = name;
        this.weight = weight;
        this.scale = scale;
        this.value = value;
        this.prefabIndex = variation;
        this.rarity = rarity;
    }

    public static CreatureData CreateInstance(CreatureType type, string name, int variation, float scale, float weight, int value, Rarity rarity)
    {
        var data = ScriptableObject.CreateInstance<CreatureData>();
        data.Init(type, name, variation, scale, weight, value, rarity);
        return data;
    }

    public CreatureType type;
    public string creatureName;

    public float scale;
    public float weight;
    public int value;

    public int prefabIndex;

    public Rarity rarity;
}

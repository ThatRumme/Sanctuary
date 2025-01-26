using UnityEngine;

[CreateAssetMenu(fileName = "ItemObject", menuName = "Scriptable Objects/ItemObject")]
class ItemObject : ScriptableObject {

    public ItemType type;
    public string itemName;

    public float weight; //kg
    public GameObject[] prefab;
}

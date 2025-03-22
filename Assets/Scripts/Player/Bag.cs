using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{

    private Dictionary<ItemType, int> inventory = new();
    private List<CreatureData> creatures = new();

    private float weight = 0f;
    public float Weight { get { return weight; } }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void AddCreature(CreatureData creature)
    {
        creatures.Add(creature);
        weight += creature.weight;
        EventManager.OnCreaturesValueUpdated(GetCreaturesValue());
    }

    public void RemoveCreature(CreatureData creature)
    {
        creatures.Remove(creature);
        weight += creature.weight;
        EventManager.OnCreaturesValueUpdated(GetCreaturesValue());
    }

    public void AddItem(ItemType itemType, int amount)
    {
        ItemObject item = ConfigUtil.Instance.GetItemObjectOfType(itemType);
        int currentAmount = inventory[itemType];
        int newAmount = amount + currentAmount;
        inventory[itemType] = newAmount;
        weight += amount * item.weight;
    }

    public void RemoveItem(ItemType itemType, int amount)
    {
        ItemObject item = ConfigUtil.Instance.GetItemObjectOfType(itemType);
        int currentAmount = inventory[itemType];
        int newAmount = currentAmount - amount;
        inventory[itemType] = newAmount;
        weight -= amount * item.weight;
    }


    public int GetCreaturesValue()
    {
        int totalValue = 0;
        for(int i = 0; i < creatures.Count; i++)
        {
            totalValue += creatures[i].value;
        }
        return totalValue;
    }
}

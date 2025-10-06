using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

class ConfigUtil : Singleton<ConfigUtil>
{

    private Dictionary<ItemType, ItemObject> itemObjects;
    private Dictionary<CreatureType, CreatureObject> creatureObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
      
        itemObjects = new Dictionary<ItemType, ItemObject>();
        creatureObjects = new Dictionary<CreatureType, CreatureObject>();

        var itemGuids = AssetDatabase.FindAssets("t:" + typeof(ItemObject).Name);
        for (int i = 0; i < itemGuids.Length; i++) //probably could get optimized
        {
            string path = AssetDatabase.GUIDToAssetPath(itemGuids[i]);
            var a = AssetDatabase.LoadAssetAtPath(path, typeof(ItemObject)) as ItemObject;
            itemObjects[a.type] = a;
        }

        var creatureGuids = AssetDatabase.FindAssets("t:" + typeof(CreatureObject).Name);
        for (int i = 0; i < creatureGuids.Length; i++) //probably could get optimized
        {
            string path = AssetDatabase.GUIDToAssetPath(creatureGuids[i]);
            var a = AssetDatabase.LoadAssetAtPath(path, typeof(CreatureObject)) as CreatureObject;
            creatureObjects[a.type] = a;
        }
    }

    public ItemObject GetItemObjectOfType(ItemType type)
    {
        return itemObjects[type];
    }

    public int GetTotalTickets(CreatureVariationEntry[] entries)
    {
        int totalTickets = 0;
        for(int i = 0;i < entries.Length;i++)
        {
            totalTickets += entries[i].tickets;
        }
        return totalTickets;
    }

    public int RollVariation(CreatureObject creature)
    {
        int ticket = UnityEngine.Random.Range(0, GetTotalTickets(creature.variations));

        int count = 0;
        int variation = -1;

        while(count < ticket)
        {
            count += creature.variations[variation+1].tickets;
            variation++;
        }

        return variation;
    }

    public CreatureObject GetRandomCreature()
    {
        return creatureObjects[(CreatureType)Enum.ToObject(typeof(CreatureType), UnityEngine.Random.Range(1, Enum.GetNames(typeof(CreatureType)).Length-1))];
    }

    public CreatureData CreateRandomCreatureData(CreatureObject randomCreature)
    {

        int variation = RollVariation(randomCreature);

        float statRoll = UnityEngine.Random.Range(0f, 1f);

        float scale = Mathf.Lerp(randomCreature.minMaxScale.x, randomCreature.minMaxScale.y, statRoll);
        float weight = Mathf.Lerp(randomCreature.minMaxWeight.x, randomCreature.minMaxWeight.y, statRoll);
        int value = (int)Math.Round(Mathf.Lerp(randomCreature.minMaxValue.x, randomCreature.minMaxValue.y, statRoll) * randomCreature.variations[variation].valueMultiplier);

        Rarity rarity = randomCreature.variations[variation].rarity;

        CreatureData creatureData = CreatureData.CreateInstance(randomCreature.type, randomCreature.name, variation, scale, weight, value, rarity);
        
        return creatureData;

    }
}

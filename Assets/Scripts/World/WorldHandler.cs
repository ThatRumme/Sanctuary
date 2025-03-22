using UnityEngine;

public class WorldHandler : MonoBehaviour
{

    private Transform[] spawnPoints;

    private bool isInitializing = true;

    public Transform playerSpawn;

    private void Awake()
    {
        GameManager.Instance.worldHandler = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeWorld();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void InitializeWorld()
    {
        GameManager.Instance.player.transform.position = playerSpawn.position;
        GameManager.Instance.player.transform.rotation = playerSpawn.rotation;

        FindSpawnPoints();
        SpawnCreatures();
        SpawnItems();

        isInitializing = false;

    }

    void FindSpawnPoints()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SpawnPoint");

        spawnPoints = new Transform[objs.Length];
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            spawnPoints[i] = objs[i].transform;
        }
    }

    void SpawnCreatures()
    {
        for(int i = 0;i < spawnPoints.Length;i++)
        {
            SpawnCreatureOnSpawnPoint(spawnPoints[i].position);
        }
    }


    void SpawnCreatureOnSpawnPoint(Vector3 spawnPos)
    {

        CreatureObject creatureObject = ConfigUtil.Instance.GetRandomCreature();

        CreatureData data = ConfigUtil.Instance.CreateRandomCreatureData(creatureObject);

        GameObject creature = Instantiate(creatureObject.variations[data.prefabIndex].prefab);
        creature.transform.position = spawnPos + new Vector3(0, 0.5f, 0);
        creature.GetComponent<Creature>().Setup(data);

    }

    void SpawnItems()
    {

    }


    public void OnExtraction()
    {

    }
}

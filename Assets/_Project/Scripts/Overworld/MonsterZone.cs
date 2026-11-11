using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


[System.Serializable]
public class SpawnEntry
{
    public GameObject monsterPrefab;
    public float weight;

}

public class MonsterZone : MonoBehaviour
{
    public GameFlow gameFlow;
    [Header("Monster Spawn")]
    [SerializeField] List<SpawnEntry> spawnEntries;
    [SerializeField] int spawnCount = 5;
    [SerializeField] float minSpawnDistance = 3f;
    List<Vector3> usedPoints = new List<Vector3>();
    BoxCollider zone;
    [Header("Sprite Aggro")]
    public GameObject questionMarkPrefab;
    public GameObject exclamationMarkPrefab;

    private void Awake()
    {
        zone = GetComponent<BoxCollider>();
    }

    void Start()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 point = FreePointInZone();
            usedPoints.Add(point);

            GameObject monster = Instantiate(PickMonster(), point, Quaternion.identity);
            monster.GetComponent<OverworldMonster>().zone = this;
        }
    }

    bool IsFarEnough(Vector3 candidate)
    {
        for (int i = 0; i < usedPoints.Count; i++)
        {
            if (Vector3.Distance(candidate, usedPoints[i]) < minSpawnDistance)
                return false;
        }
        return true;
    }

    Vector3 FreePointInZone()
    {
        for (int attempt = 0; attempt < 30; attempt++)
        {
            Vector3 candidate = RandomPointInZone();

            if (IsFarEnough(candidate))
                return candidate;
        }
        Debug.LogWarning("Zone trop petite : impossible de placer ce monstre à " + minSpawnDistance + " m des autres.");
        return RandomPointInZone();
    }
    GameObject PickMonster()
    {
        float totalWeight = 0f;


        for (int i = 0; i < spawnEntries.Count; i++)
        {
            totalWeight += spawnEntries[i].weight;
        }

        float roll = Random.Range(0f, totalWeight);

        for (int i = 0; i < spawnEntries.Count; i++)
        {
            roll -= spawnEntries[i].weight;
            if (roll <= 0f)
                return spawnEntries[i].monsterPrefab;
        }

        return spawnEntries[0].monsterPrefab;
    }
    public Vector3 RandomPointInZone()
    {
        float randomPointX = Random.Range(-zone.size.x / 2f, zone.size.x / 2f);
        float randomPointZ = Random.Range(-zone.size.z / 2f, zone.size.z / 2f);


        Vector3 local = new Vector3(randomPointX, 0f, randomPointZ) + zone.center;
        Vector3 randomPointFinal = transform.TransformPoint(local);

        return SnapToGround(randomPointFinal);
    }

    public Vector3 SnapToGround(Vector3 point)
    {
        Vector3 high = point + Vector3.up * 50f;

        if (Physics.Raycast(high, Vector3.down, out RaycastHit hit, 100f, ~0, QueryTriggerInteraction.Ignore))
            return hit.point;

        return point;
    }



}

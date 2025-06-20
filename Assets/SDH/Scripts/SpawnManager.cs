using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEditor;

[Serializable]
public class SpawnArea
{
    public string areaName;
    public Vector3 centerPosition;
    public float radius;
    public List<MonsterSpawnData> spawnableMonsters;
    public int maxMonsterCount;
    public float spawnInterval;
    public bool isActive = true;

    [HideInInspector]
    public List<GameObject> currentMonsters = new List<GameObject>();
}

[Serializable]
public class MonsterSpawnData
{
    public GameObject monsterPrefab;
    public int spawnWeight = 1;
}


public class SpawnManager : MonoBehaviour
{
    [Header("스폰 설정")]
    public List<SpawnArea> spawnAreas = new List<SpawnArea>();
    public LayerMask groundLayer = 1;
    public float playerCheckDistance = 50f;
    public int maxTotalMonsters = 100;
    public float updateInterval = 2f;

    private Transform player;
    private Dictionary<SpawnArea, Coroutine> spawnCoroutines = new Dictionary<SpawnArea, Coroutine>();
    private int currentTotalMonsters = 0;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        foreach (var area in spawnAreas)
        {
            if (area.isActive)
            {
                var coroutine = StartCoroutine(ManageSpawnArea(area));
                spawnCoroutines[area] = coroutine;
            }
        }
    }


    IEnumerator ManageSpawnArea(SpawnArea area)
    {
        while (area.isActive)
        {
            yield return new WaitForSeconds(area.spawnInterval);
            CleanupDeadMonsters(area);
            if (ShouldSpawnMonster(area))
            {
                SpawnMonsterInArea(area);
            }
        }
    }


    bool ShouldSpawnMonster(SpawnArea area)
    {
        if (currentTotalMonsters >= maxTotalMonsters)
            return false;

        if (area.currentMonsters.Count >= area.maxMonsterCount)
            return false;

        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(area.centerPosition, player.position);
            if (distanceToPlayer < playerCheckDistance)
                return false;
        }

        return true;
    }


    void SpawnMonsterInArea(SpawnArea area)
    {
        Vector3 spawnPosition = GetRandomSpawnPosition(area);
        if (spawnPosition == Vector3.zero) return;

        MonsterSpawnData selectedMonster = SelectRandomMonster(area.spawnableMonsters);
        if (selectedMonster == null) return;

        GameObject monster = Instantiate(selectedMonster.monsterPrefab, spawnPosition, Quaternion.identity);

        area.currentMonsters.Add(monster);
        currentTotalMonsters++;
    }


    Vector3 GetRandomSpawnPosition(SpawnArea area)
    {
        for (int attempts = 0; attempts < 10; attempts++)
        {
            Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * area.radius;
            Vector3 randomPosition = area.centerPosition + new Vector3(randomCircle.x, 0, randomCircle.y);

            if (Physics.Raycast(randomPosition + Vector3.up * 10f, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            {
                return hit.point;
            }
        }

        return Vector3.zero; // 적절한 위치를 찾지 못함
    }

    MonsterSpawnData SelectRandomMonster(List<MonsterSpawnData> monsters)
    {
        if (monsters.Count == 0) return null;

        int totalWeight = 0;
        foreach (var monster in monsters)
        {
            totalWeight += monster.spawnWeight;
        }

        int randomValue = UnityEngine.Random.Range(0, totalWeight);
        int currentWeight = 0;

        foreach (var monster in monsters)
        {
            currentWeight += monster.spawnWeight;
            if (randomValue < currentWeight)
            {
                return monster;
            }
        }

        return monsters[0];
    }

    void CleanupDeadMonsters(SpawnArea area)
    {
        for (int i = area.currentMonsters.Count - 1; i >= 0; i--)
        {
            if (area.currentMonsters[i] == null)
            {
                area.currentMonsters.RemoveAt(i);
                currentTotalMonsters--;
            }
        }
    }


    public void SetAreaActive(string areaName, bool active)
    {
        var area = spawnAreas.Find(a => a.areaName == areaName);
        if (area != null)
        {
            area.isActive = active;

            if (active && !spawnCoroutines.ContainsKey(area))
            {
                spawnCoroutines[area] = StartCoroutine(ManageSpawnArea(area));
            }
            else if (!active && spawnCoroutines.ContainsKey(area))
            {
                StopCoroutine(spawnCoroutines[area]);
                spawnCoroutines.Remove(area);
            }
        }
    }
}

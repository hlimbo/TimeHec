using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMore : MonoBehaviour {

    public GameObject targetPrefab;
    public float checkFrequency = 1.0f;//check every 1 second
    [SerializeField]
    private int spawnCount;
    [SerializeField]
    private List<Vector2> spawnPositions;
    private void Awake()
    {
        StartCoroutine(SpawnTargets());
        spawnCount = transform.childCount;
        spawnPositions = new List<Vector2>();
        for(int i = 0;i < spawnCount;++i)
        {
            spawnPositions.Add(transform.GetChild(i).position);
        }
    }

    IEnumerator SpawnTargets()
    {
        while(true)
        {
            if(transform.childCount == 0)
            {
                foreach (Vector2 spawnPosition in spawnPositions)
                    Instantiate<GameObject>(targetPrefab, spawnPosition, Quaternion.identity, this.transform);
            }
            yield return new WaitForSecondsRealtime(checkFrequency);
        }
    }


}

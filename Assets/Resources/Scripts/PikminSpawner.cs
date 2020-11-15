using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PikminSpawner : MonoBehaviour
{
    [SerializeField] private int spawnNum = 1;
    [SerializeField] private float spawnRadius = 0;

    public List<Pikmin> SpawnPikmin(Pikmin pikmin, int spawnerIndex)
    {
        List<Pikmin> pikminList = new List<Pikmin>();

        int pikminIndex = spawnerIndex * spawnNum;
        for(int i = 0; i < spawnNum; i++)
        {
            Pikmin newPikmin = Instantiate(pikmin);
            Vector3 pos = transform.position + (Random.insideUnitSphere * spawnRadius);
            newPikmin.transform.position = pos;
            newPikmin.GetComponent<NavMeshAgent>().enabled = true;
            newPikmin.PikminID = pikminIndex++;
            pikminList.Add(newPikmin);
        }

        return pikminList;
    }
}

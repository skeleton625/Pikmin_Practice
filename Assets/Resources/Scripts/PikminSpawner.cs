using UnityEngine;
using UnityEngine.AI;

public class PikminSpawner : MonoBehaviour
{
    [SerializeField] private int spawnNum = 1;
    [SerializeField] private float spawnRadius = 0;

    public void SpawnPikmin(Pikmin pikmin)
    {
        for(int i = 0; i < spawnNum; i++)
        {
            Pikmin newPikmin = Instantiate(pikmin);
            Vector3 pos = transform.position + (Random.insideUnitSphere * spawnRadius);
            newPikmin.transform.position = pos;
            newPikmin.GetComponent<NavMeshAgent>().enabled = true;
        }
    }
}

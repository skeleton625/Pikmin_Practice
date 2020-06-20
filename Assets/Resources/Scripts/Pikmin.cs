using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pikmin : MonoBehaviour
{
    [SerializeField] private Transform target;

    public enum State { Idle, Fallow, Interact };

    private NavMeshAgent agent;
    private Vector3 prepos;
    private State state = default;
    private Coroutine updateTarget = default;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        prepos = target.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ControlObject"))
        {
            state = State.Fallow;

            if (updateTarget != null)
                StopCoroutine(updateTarget);
            updateTarget = StartCoroutine(UpdateTarget());

            IEnumerator UpdateTarget()
            {
                while (true)
                {
                    if (prepos != target.position)
                    {
                        prepos = target.position;
                        agent.SetDestination(prepos);
                    }
                    yield return null;
                }
            }
        }
    }
}

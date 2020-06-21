using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Pikmin : MonoBehaviour
{
    private Transform target = default;

    public enum State { Idle, Follow, Interact };

    private NavMeshAgent agent;
    private Vector3 prepos;
    private State state = default;
    private Coroutine updateTarget = default;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find("Follow_Position").transform;
        prepos = target.position;
    }

    private void Start()
    {
        agent.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ControlObject"))
        {
            state = State.Follow;

            if (updateTarget != null)
                StopCoroutine(updateTarget);
            updateTarget = StartCoroutine(UpdateTarget());

            IEnumerator UpdateTarget()
            {
                agent.SetDestination(prepos);
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

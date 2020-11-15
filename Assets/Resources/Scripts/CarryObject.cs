using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarryObject : InteractiveObject
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform Destination;
    private NavMeshAgent agent = null;
    private Coroutine carryingCoroutine = null;
    private float originalAgnetSpeed;

    public override void Initialize()
    {
        base.Initialize();
        agent = GetComponent<NavMeshAgent>();
        originalAgnetSpeed = agent.speed;
    }

    public override void Interact()
    {
        if (agent.enabled)
            agent.isStopped = false;
        else
            agent.enabled = true;
        if (carryingCoroutine != null)
            StopCoroutine(carryingCoroutine);
        StartCoroutine(GetInPosition());

        IEnumerator GetInPosition()
        {
            agent.SetDestination(Destination.position);
            yield return new WaitUntil(() => agent.IsDone());
            agent.enabled = false;
        }
    }

    public override void StopInteract()
    {
        if(agent.enabled)
            agent.isStopped = true;
        if (carryingCoroutine != null)
            StopCoroutine(carryingCoroutine);
    }

    private void Update()
    {
        if (fractionObject != null)
            fractionObject.transform.position = mainCamera.WorldToScreenPoint(transform.position + uiOffset);
    }
}

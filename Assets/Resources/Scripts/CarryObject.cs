﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CarryObject : InteractiveObject
{
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
        if (carryingCoroutine != null)
            StopCoroutine(carryingCoroutine);

        agent.enabled = true;
        carryingCoroutine = StartCoroutine(GetInPosition());

        IEnumerator GetInPosition()
        {
            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryObjectManager : MonoBehaviour
{
    [Header("Source Component"), Space(10)]
    [SerializeField] private Transform Source = null;

    [Header("Destination Component"), Space(10)]
    [SerializeField] private Transform Dsetination = null;

    [Header("Carry Object Component"), Space(10)]
    [SerializeField] private Transform SmallCarryObject = null;
    [SerializeField] private float CarryObjectScale = 0f;
    [SerializeField] private float SpawnRadius = 0f;

    public void FinishCarryObject(CarryObject carryObject)
    {
        carryObject.transform.localScale = new Vector3(CarryObjectScale, CarryObjectScale, CarryObjectScale);

        var position = Source.position + Random.insideUnitSphere * SpawnRadius;
        position.y = 0;

        carryObject.Initialize();
        carryObject.transform.position = position;
    }
}

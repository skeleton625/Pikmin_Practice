using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikminController : MonoBehaviour
{
    [HideInInspector] public Vector3 HitPoint = Vector3.zero;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform ControlObject;

    private Camera camera = default;
    private int colliderLayer;

    private void Awake()
    {
        camera = Camera.main;
        colliderLayer = -257;
    }

    private void Update()
    {
        UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, colliderLayer))
        {
            HitPoint = hit.point;
            ControlObject.position = HitPoint + targetOffset;
            ControlObject.up = Vector3.Lerp(ControlObject.up, hit.normal, .3f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PikminManager : MonoBehaviour
{
    [SerializeField] private Transform visualCylinder;
    private PikminController controller;

    private void Awake()
    {
        controller = GetComponent<PikminController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
            SetWhistleCylinder(true);

        if (Input.GetMouseButtonUp(0))
            SetWhistleCylinder(false);
    }

    public void SetWhistleCylinder(bool on)
    {
        if(on)
        {
            visualCylinder.localScale = Vector3.zero;
            visualCylinder.DOScaleX(4, .4f);
            visualCylinder.DOScaleZ(4, .4f);
            visualCylinder.DOScaleY(2, .3f).SetDelay(.4f);
        }
        else
        {
            visualCylinder.DOKill();
            visualCylinder.DOScaleX(0, .15f);
            visualCylinder.DOScaleZ(0, .15f);
            visualCylinder.DOScaleY(0, .05f);
        }
    }
}

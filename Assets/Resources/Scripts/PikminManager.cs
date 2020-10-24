using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


[System.Serializable] public class PikminEvent : UnityEvent<int> { }
[System.Serializable] public class PlayerEvent : UnityEvent<Vector3> { } 
public class PikminManager : MonoBehaviour
{
    [SerializeField] private Transform pikminThrowPosition;
    [SerializeField] private SphereCollider cylinderCollider;
    [SerializeField] private Transform visualCylinder;
    [SerializeField] private Pikmin pikminPrefab;

    private PikminController controller;
    private List<Pikmin> pikminList;

    public static PikminManager instance;

    private void Awake()
    {
        instance = this;

        InitializePikmin();
    }

    private void InitializePikmin()
    {
        pikminList = new List<Pikmin>();
        controller = GetComponent<PikminController>();

        PikminSpawner[] spawners = FindObjectsOfType(typeof(PikminSpawner)) as PikminSpawner[];
        foreach (PikminSpawner spawner in spawners)
            spawner.SpawnPikmin(pikminPrefab);
    }

    public void GetPikmin(Pikmin pikmin)
    {
        pikminList.Add(pikmin);
    }

    public void SetWhistleCylinder(bool on)
    {
        if (on)
        {
            cylinderCollider.enabled = true;
            visualCylinder.localScale = Vector3.zero;
            visualCylinder.DOScaleX(4, .4f);
            visualCylinder.DOScaleZ(4, .4f);
            visualCylinder.DOScaleY(0.7f, .3f).SetDelay(.2f);
        }
        else
        {
            cylinderCollider.enabled = false;
            visualCylinder.DOKill();
            visualCylinder.DOScaleX(0, .15f);
            visualCylinder.DOScaleZ(0, .15f);
            visualCylinder.DOScaleY(0, .05f);
        }
    }

    public void ControlPikmin()
    {
        if (Input.GetMouseButtonDown(1))
            SetWhistleCylinder(true);
        if (Input.GetMouseButtonUp(1))
            SetWhistleCylinder(false);

        if (Input.GetMouseButtonDown(0))
        {
            if (pikminList.Count < 1)
                return;

            for(int i = 0; i < pikminList.Count; i++)
            {
                if(pikminList[i].agent.IsDone())
                {
                    Pikmin pikmin = pikminList[i];
                    pikminList.RemoveAt(i);
                    pikmin.agent.enabled = false;
                    pikmin.transform.DOMove(pikminThrowPosition.position, .05f);
                    pikmin.Throw(visualCylinder.position, .5f, .05f);
                    break;
                }
            }
            
            /* Pikmin 던지는 소리, 움직이는 소리 필요 */
        }
    }
}

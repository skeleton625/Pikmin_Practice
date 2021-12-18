using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarryRespawner : MonoBehaviour
{
    [Header("Carry Object Respawn Radius"), Space(10)]
    [SerializeField] private float RespawnRadius = 10f;

    private List<CarryObject> respawnObjects = null;
    private Vector3[] dirs = new Vector3[4]
        { new Vector3(1, 0, 1 ), new Vector3(-1, 0, 1 ), 
          new Vector3(-1, 0, -1 ), new Vector3(1, 0, -1 ) };

    private void Awake()
    {
        respawnObjects = new List<CarryObject>();
    }

    private void Update()
    {
        if (respawnObjects.Count > 0)
        {
            var carryObject = respawnObjects[0];
            var position = transform.position;
            position.y = 100;

            if (Physics.Raycast(position, - Vector3.up, out RaycastHit center, 200, -1) &&
                center.transform.CompareTag("Terrain"))
            {
                var isPossible = true;
                var width = carryObject.transform.localPosition.x / 2;
                for (int i = 0; i < 4; ++i)
                {
                    if (!(Physics.Raycast(position + dirs[i] * width, - Vector3.up, out RaycastHit hit, 200, -1) &&
                        hit.transform.CompareTag("Terrain")))
                    {
                        isPossible = false;
                        break;
                    }
                }

                if (isPossible)
                {
                    carryObject.transform.position = center.point;
                    DOTween.Sequence().Join(carryObject.Visual.DOScale(1, 1.3f).SetEase(Ease.InQuint));
                    respawnObjects.RemoveAt(0);
                }
            }
        }
    }

    public void Push(CarryObject transform)
    {
        respawnObjects.Add(transform);
    }
}

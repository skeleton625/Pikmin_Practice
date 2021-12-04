using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam = null;
    [SerializeField] private float CameraRotValue = 90;
    [SerializeField] private float RotTime = .5f;

    private CinemachineOrbitalTransposer transposer = null;

    private void Start()
    {
        transposer = virtualCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void SetCameraAxis(float x)
    {
        transposer.m_XAxis.Value = x;
    }

    public void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            DOVirtual.Float(transposer.m_XAxis.Value, transposer.m_XAxis.Value - CameraRotValue, RotTime, SetCameraAxis).SetEase(Ease.OutSine);
        if (Input.GetKeyDown(KeyCode.E))
            DOVirtual.Float(transposer.m_XAxis.Value, transposer.m_XAxis.Value + CameraRotValue, RotTime, SetCameraAxis).SetEase(Ease.OutSine);
    }
}

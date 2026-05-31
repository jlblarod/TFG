using UnityEngine;
using Unity.Cinemachine;

public class CameraSetup : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            GetComponent<CinemachineCamera>().Target.TrackingTarget = player.transform;
        }
    }
}
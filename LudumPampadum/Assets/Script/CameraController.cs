using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Parameters")]
    [SerializeField]
    float smoothCamera = 2;
    [SerializeField]
    Vector3 cameraOffset = Vector3.zero;

    [Space]
    [SerializeField]
    Vector2 clampX = Vector2.zero;
    [SerializeField]
    Vector2 clampZ = Vector2.zero;

    [Header("Focus")]
    [SerializeField]
    Transform focusTarget; // Focus de base pour le gameplay
    [SerializeField]
    Transform focusTargetLock; // Focus de target pour le gameplay
    [SerializeField]
    Transform focusPriority; // Focus pour les cinés


    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        if (focusTarget == null)
            focusTarget = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Update()
    {
        FocusOnTarget(focusTarget.position);
    }

    public void FocusOnTarget(Vector3 targetPos)
    {
        if (focusPriority != null)
            targetPos = focusPriority.position;
        if (focusTargetLock != null)
            targetPos = targetPos + ((focusTargetLock.position - focusTarget.position) / 2);

        if (focusPriority == null)
        {
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, clampX.x, clampX.y), targetPos.y, Mathf.Clamp(targetPos.z, clampZ.x, clampZ.y));
            transform.position = Vector3.SmoothDamp(transform.position, targetPos + cameraOffset, ref velocity, smoothCamera);
        }
        else
        {
            targetPos = new Vector3(Mathf.Clamp(targetPos.x, clampX.x, clampX.y), targetPos.y - 5f, Mathf.Clamp(targetPos.z, clampZ.x, clampZ.y) + 10);
            transform.position = Vector3.SmoothDamp(transform.position, targetPos + cameraOffset, ref velocity, 0.5f);
        }

    }

    public void SetLocked(Transform lockTransform)
    {
        focusTargetLock = lockTransform;
    }

    public void SetFocusPriority(Transform focusTransform)
    {
        focusPriority = focusTransform;
    }


}
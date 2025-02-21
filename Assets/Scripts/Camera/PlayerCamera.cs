using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerCamera : MonoBehaviour
{
    [System.Serializable]
    private enum CameraFollowMode {
        Fixed,
        Delay,
        Edge
    }
    [SerializeField]
    private Transform followTarget = null;
    protected Vector3 FollowTargetPosition { get => followTarget.position; }

    void Update()
    {
        this.UpdateCamera();
    }

    public abstract void UpdateCamera();
    

}

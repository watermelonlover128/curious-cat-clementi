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
    private Transform followTarget = null;
    protected Vector3 FollowTargetPosition { get => this.followTarget.position; }

    void Update()
    {
        this.UpdateCamera();
    }

    public abstract void UpdateCamera();

    public void SetTarget(Transform target) {
        this.followTarget = target;
        this.transform.position = new Vector3(this.FollowTargetPosition.x,
                                    this.FollowTargetPosition.y,
                                    this.transform.position.z);
    }
    

}

using System.Collections;
using UnityEngine;

public class PlayerCameraFixed : PlayerCamera {
    public override void UpdateCamera()
    {
        this.transform.position = new Vector3(this.FollowTargetPosition.x,
                                    this.FollowTargetPosition.y,
                                    this.transform.position.z); 
    }
}
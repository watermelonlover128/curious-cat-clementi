using System.Collections;
using UnityEngine;

public class PlayerCameraDelayed : PlayerCamera
{
    [SerializeField]
    [Range(0.0f, 20.0f)]
    private float rangeOffset = 0.0f;
    [SerializeField]
    [Range(0.1f, 5.0f)]
    private float followSpeed = 1.0f;

    private bool isFollowing = false;
    public override void UpdateCamera()
    {
        Vector3 targetPos = this.FollowTargetPosition;
        targetPos.z = 0;
        Vector3 camPos = this.transform.position;
        camPos.z = 0;
        if (isFollowing && Vector3.Distance(targetPos, camPos) == 0) {
            isFollowing = false;
            return;
        }
        if (!isFollowing && Vector3.Distance(targetPos, camPos) < rangeOffset) 
            return;
        isFollowing = true;
        float cam_x = Mathf.Lerp(camPos.x, targetPos.x, followSpeed * Time.deltaTime);
        float cam_y = Mathf.Lerp(camPos.y, targetPos.y, followSpeed * Time.deltaTime);
        this.transform.position = new Vector3(cam_x, cam_y, this.transform.position.z);
    }
}
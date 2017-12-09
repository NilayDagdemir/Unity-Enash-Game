using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform TargetPos;

    void Update()
    {
        Vector3 cameraPos = new Vector3(TargetPos.position.x, TargetPos.position.y, transform.position.z);
        transform.position = cameraPos;
    }
}

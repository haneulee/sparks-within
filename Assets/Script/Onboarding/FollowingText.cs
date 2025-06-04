using UnityEngine;

public class FollowingText : MonoBehaviour
{
    public Transform targetCamera;

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            transform.LookAt(transform.position + targetCamera.forward);
        }
    }
}

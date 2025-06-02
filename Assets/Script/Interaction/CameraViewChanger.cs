using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    public Transform xrRig; // XR Rig의 Transform
    public Transform playerCamera; // 카메라 Transform (참조용)

    void Start()
    {
        if (xrRig == null)
        {
            // XR Rig 찾기
            xrRig = GameObject.Find("XR Origin")?.transform;
            if (xrRig == null)
            {
                Debug.LogError("❌ XR Rig not found! Please assign it manually.");
            }
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }
    }

    public void MoveToCube(GameObject cube)
    {
        if (cube != null && xrRig != null)
        {
            // XR Rig의 위치를 큐브 위치로 이동
            // Y축을 제외한 X, Z축의 offset만 유지
            Vector3 offset = xrRig.position - playerCamera.position;
            offset.y = 0; // Y축 offset 제거
            xrRig.position = cube.transform.position + offset;
            
            Debug.Log($"✅ XR Rig moved to cube: {cube.name}");
        }
        else
        {
            Debug.LogError("❌ Cannot move: cube or XR Rig is null!");
        }
    }
} 
using UnityEngine;

public class CameraViewChanger : MonoBehaviour
{
    public Transform xrRig; // XR Rig의 Transform
    public Transform playerCamera; // 카메라 Transform (참조용)
    private Transform targetCube; // 따라다닐 큐브의 Transform
    private Vector3 offset; // 카메라와 큐브 사이의 상대적 위치 차이
    private bool isFollowing = false; // 큐브를 따라다니는 중인지 여부
    
    [Header("Smooth Following Settings")]
    [SerializeField] private float smoothSpeed = 5f; // 부드러운 움직임을 위한 속도
    [SerializeField] private float maxDistance = 10f; // 최대 이동 거리 제한

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

    void LateUpdate()
    {
        if (isFollowing && targetCube != null)
        {
            // 큐브의 위치에 offset을 적용하여 XR Rig의 위치 업데이트
            xrRig.position = targetCube.position + offset;
        }
    }

    public void MoveToCube(GameObject cube)
    {
        if (cube != null && xrRig != null)
        {
            targetCube = cube.transform;
            // 초기 offset 계산 (Y축 제외)
            offset = xrRig.position - playerCamera.position;
            offset.y = 0;
            
            // 초기 위치 설정
            xrRig.position = targetCube.position + offset;
            isFollowing = true;
            
            Debug.Log($"✅ XR Rig following cube: {cube.name}");
        }
        else
        {
            Debug.LogError("❌ Cannot move: cube or XR Rig is null!");
        }
    }

    public void StopFollowing()
    {
        isFollowing = false;
        targetCube = null;
        Debug.Log("✅ Stopped following cube");
    }
} 
using UnityEngine;

public class Look : MonoBehaviour
{
    public GameObject ringPrefab;             // <- 프리팹 연결할 필드
    public Transform playerCamera;            // <- 카메라 연결
    public float ringSpeed = 1f;
    public float ringSpawnOffset = 0.5f;
    public bool effectTriggered = false;

    private GameObject activeRing;
    private bool ringLaunched = false;
    
    private floatingobject floatScript;

   void Start()
    {
        floatScript = GetComponent<floatingobject>();

        // floatingobject 스크립트에 playerCamera 전달
        if (floatScript != null && playerCamera != null)
        {
            floatScript.playerCamera = playerCamera;
        }
    }

    public void TriggerRingEffect()
    {
        if (ringLaunched || ringPrefab == null) return;
        if (floatScript != null)
            floatScript.isFrozen = true;

        ringLaunched = true;

        Vector3 spawnPos = transform.position +
            (playerCamera.position - transform.position).normalized * ringSpawnOffset;

        activeRing = Instantiate(ringPrefab, spawnPos, Quaternion.identity);
        activeRing.AddComponent<RingMover>().Initialize(playerCamera, ringSpeed);
    }
}

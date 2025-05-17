using UnityEngine;

public class Look : MonoBehaviour
{
    public GameObject ringPrefab;
    public Transform playerCamera;
    public float ringSpeed = 1f;
    public float ringSpawnOffset = 0.5f;
    public bool effectTriggered = false;

    private GameObject activeRing;
    private bool ringLaunched = false;
    
    private floatingobject floatScript;

   void Start()
    {
        floatScript = GetComponent<floatingobject>();

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
        RingMover mover = activeRing.AddComponent<RingMover>();
        mover.Initialize(playerCamera, ringSpeed, this.gameObject);
    }
}

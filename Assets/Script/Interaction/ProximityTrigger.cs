using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform playerCamera;
    public float handTouchThreshold = 0.05f;
    public float cubeAlignmentThreshold = 0.95f;

    private bool triggered = false;
    private CameraViewChanger cameraViewChanger;
    private LookController lookController;

    void Start()
    {
        cameraViewChanger = FindObjectOfType<CameraViewChanger>();
        lookController = FindObjectOfType<LookController>();

        if (cameraViewChanger == null)
            Debug.LogWarning("⚠️ CameraViewChanger component not found in scene!");
        if (lookController == null)
            Debug.LogWarning("⚠️ LookController not found!");
    }

    void Update()
    {
        if (leftHand == null || rightHand == null || lookController == null) return;

        float handDistance = Vector3.Distance(leftHand.position, rightHand.position);
        if (handDistance >= handTouchThreshold)
        {
            triggered = false;
            return;
        }

        GameObject target = lookController.currentLookTarget;
        // Vector3 handCenter = (leftHand.position + rightHand.position) * 0.5f;
        // Vector3 toCube = (transform.position - handCenter).normalized;
        // Vector3 handsForward = ((leftHand.forward + rightHand.forward) * 0.5f).normalized;
        // float dot = Vector3.Dot(toCube, handsForward);

        if (target && !triggered)
        {
            TriggerEffect();
            triggered = true;
        }
    }

    void TriggerEffect()
    {
        var profile = GetComponent<SoundProfile>();
        if (profile == null)
        {
            Debug.LogWarning("⚠️ SoundProfile not found on cube.");
            return;
        }

        // 🔄 Skybox는 항상 바꿈
        var skyboxChanger = FindObjectOfType<SkyboxColorChanger>();
        if (skyboxChanger != null && skyboxChanger.skyboxMaterial != null)
        {
            skyboxChanger.targetTopColor = profile.topColor;
            skyboxChanger.targetBottomColor = profile.bottomColor;
            skyboxChanger.ChangeSkyboxColor();
            Debug.Log($"🌈 Skybox changed due to interaction with: {profile.beingName}");
        }

        // 🏢 빌딩 색상도 변경
        var buildingChanger = FindObjectOfType<BuildingColorChanger>();
        if (buildingChanger != null && buildingChanger.buildingMaterial != null)
        {
            buildingChanger.targetTopColor = profile.topColor;
            buildingChanger.targetBottomColor = profile.bottomColor;
            buildingChanger.ChangeBuildingColor();
            Debug.Log($"🏢 Building color changed due to interaction with: {profile.beingName}");
        }

        // 👀 카메라가 큐브로 즉시 이동
        if (cameraViewChanger != null)
        {
            cameraViewChanger.MoveToCube(gameObject);
            Debug.Log($"👀 Camera instantly moved to cube: {profile.beingName}");
        }

        // ❌ 이미 수집된 경우 소리나 등록은 안함
        if (SoundMemoryManager.Instance != null)
        {
            if (SoundMemoryManager.Instance.HasBeenCollected(profile.beingName))
            {
                Debug.Log($"🔁 {profile.beingName} already collected. Skipping sound.");
                return;
            }

            Debug.Log($"🎵 Playing sound: {profile.beingName}");
            SoundMemoryManager.Instance.AddSound(profile);
        }
        else
        {
            Debug.LogError("❌ SoundMemoryManager.Instance is NULL!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var profile = GetComponent<SoundProfile>();
            if (profile == null)
            {
                Debug.LogWarning("⚠️ SoundProfile not found on cube.");
                return;
            }

            // 🔄 Skybox는 항상 바꿈
            var skyboxChanger = FindObjectOfType<SkyboxColorChanger>();
            if (skyboxChanger != null && skyboxChanger.skyboxMaterial != null)
            {
                skyboxChanger.targetTopColor = profile.topColor;
                skyboxChanger.targetBottomColor = profile.bottomColor;
                skyboxChanger.ChangeSkyboxColor();
                Debug.Log($"🌈 Skybox changed due to interaction with: {profile.beingName}");
            }

            // 🏢 빌딩 색상도 변경
            var buildingChanger = FindObjectOfType<BuildingColorChanger>();
            if (buildingChanger != null && buildingChanger.buildingMaterial != null)
            {
                buildingChanger.targetTopColor = profile.topColor;
                buildingChanger.targetBottomColor = profile.bottomColor;
                buildingChanger.ChangeBuildingColor();
                Debug.Log($"🏢 Building color changed due to interaction with: {profile.beingName}");
            }

            // ❌ 이미 수집된 경우 소리나 등록은 안함
            if (SoundMemoryManager.Instance != null)
            {
                if (SoundMemoryManager.Instance.HasBeenCollected(profile.beingName))
                {
                    Debug.Log($"🔁 {profile.beingName} already collected. Skipping sound.");
                    return;
                }

                Debug.Log($"🎵 Playing sound: {profile.beingName}");
                SoundMemoryManager.Instance.AddSound(profile);
            }
            else
            {
                Debug.LogError("❌ SoundMemoryManager.Instance is NULL!");
            }
        }
    }
}

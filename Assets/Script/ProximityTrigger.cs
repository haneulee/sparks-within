using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Transform playerCamera;
    public float handTouchThreshold = 0.05f;
    public float cubeAlignmentThreshold = 0.95f;

    private bool triggered = false;

    void Update()
    {
        if (leftHand == null || rightHand == null) return;

        float handDistance = Vector3.Distance(leftHand.position, rightHand.position);
        if (handDistance >= handTouchThreshold)
        {
            triggered = false;
            return;
        }

        Vector3 handCenter = (leftHand.position + rightHand.position) * 0.5f;
        Vector3 toCube = (transform.position - handCenter).normalized;
        Vector3 handsForward = ((leftHand.forward + rightHand.forward) * 0.5f).normalized;

        float dot = Vector3.Dot(toCube, handsForward);

        Debug.Log($"dotHand: {dot:F3}, handDistance: {handDistance:F3}");

        if (dot > cubeAlignmentThreshold && !triggered)
        {
            Debug.Log("✔️ Hands are touching and pointing toward the cube!");

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
        var changer = FindObjectOfType<SkyboxColorChanger>();
        if (changer != null && changer.skyboxMaterial != null)
        {
            changer.targetTopColor = profile.topColor;
            changer.targetBottomColor = profile.bottomColor;
            changer.ChangeSkyboxColor();
            Debug.Log($"🌈 Skybox changed due to interaction with: {profile.beingName}");
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

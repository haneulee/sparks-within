using UnityEngine;
using System.Collections.Generic;

public class LookController : MonoBehaviour
{
    public Transform playerCamera;
    public List<GameObject> cubes;
    public float lookThreshold = 0.96f; // 약 10도 이내

    void Update()
    {
        GameObject bestCandidate = null;
        float bestDot = -1f;

        foreach (var cube in cubes)
        {
            Vector3 dirToCube = (cube.transform.position - playerCamera.position).normalized;
            float dot = Vector3.Dot(playerCamera.forward, dirToCube);
            AudioSource audio = cube.GetComponent<AudioSource>();
            SoundProfile profile = cube.GetComponent<SoundProfile>();

            // 수집 여부 확인
            bool alreadyCollected = profile != null &&
                                    SoundMemoryManager.Instance != null &&
                                    SoundMemoryManager.Instance.HasBeenCollected(profile.beingName);

            if (dot > lookThreshold && dot > bestDot)
            {
                bestDot = dot;
                bestCandidate = cube;

                // ❗ 수집되지 않은 큐브만 소리 재생
                if (!audio.isPlaying && !alreadyCollected)
                    audio.Play();
            }
            else
            {
                if (audio.isPlaying)
                    audio.Stop();
            }
        }
    }
}

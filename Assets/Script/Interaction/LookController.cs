using UnityEngine;
using System.Collections.Generic;

public class LookController : MonoBehaviour
{
    public Transform playerCamera;
    public List<GameObject> cubes;
    public float lookThreshold = 0.96f; // 약 10도 이내
    public float defaultVolume = 1.5f; // 기본 볼륨을 1.5배로 설정
    public float spatialBlend = 0.5f; // 3D 효과 정도 (0: 2D, 1: 3D)
    public float maxDistance = 100f; // 최대 거리
    public GameObject currentLookTarget { get; private set; }

    void Start()
    {
        // 모든 큐브의 AudioSource 설정 초기화
        foreach (var cube in cubes)
        {
            AudioSource audio = cube.GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.spatialBlend = spatialBlend;
                audio.maxDistance = maxDistance;
                audio.rolloffMode = AudioRolloffMode.Linear;
            }
        }
    }

    void Update()
    {
        currentLookTarget = null; // 👈 매 프레임 초기화
        GameObject bestCandidate = null;
        float bestDot = -1f;

        foreach (var cube in cubes)
        {
            Vector3 dirToCube = (cube.transform.position - playerCamera.position).normalized;
            float dot = Vector3.Dot(playerCamera.forward, dirToCube);
            AudioSource audio = cube.GetComponent<AudioSource>();
            SoundProfile profile = cube.GetComponent<SoundProfile>();

            bool alreadyCollected = profile != null &&
                                    SoundMemoryManager.Instance != null &&
                                    SoundMemoryManager.Instance.HasBeenCollected(profile.beingName);

            if (dot > lookThreshold && dot > bestDot)
            {
                bestDot = dot;
                bestCandidate = cube;

                if (!audio.isPlaying && !alreadyCollected)
                {
                    audio.volume = defaultVolume;
                    audio.Play();
                }
            }
            else
            {
                if (audio.isPlaying)
                    audio.Stop();
            }
        }

        currentLookTarget = bestCandidate; // 👈 현재 바라보는 큐브 업데이트
    }

}

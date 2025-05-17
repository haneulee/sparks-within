using System.Collections.Generic;
using UnityEngine;

public class SoundMemoryManager : MonoBehaviour
{
    public static SoundMemoryManager Instance;
    private List<string> collectedNames = new();
    private List<AudioSource> activeSources = new();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddSound(SoundProfile profile)
    {
        if (profile == null || profile.soundClip == null)
        {
            Debug.LogWarning("⚠️ SoundProfile or soundClip is null — skipping");
            return;
        }

        if (collectedNames.Contains(profile.beingName)) return;

        collectedNames.Add(profile.beingName);

        GameObject go = new GameObject("Sound_" + profile.beingName);
        go.transform.parent = transform;

        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = profile.soundClip;
        src.loop = true;
        src.volume = 0.15f;
        src.spatialBlend = 0f;

        if (src.clip.loadState == AudioDataLoadState.Loaded)
        {
            src.Play();
            Debug.Log("✅ Playing sound: " + profile.beingName);
        }
        else
        {
            Debug.LogWarning("⚠️ Clip not fully loaded: " + profile.beingName);
        }

        activeSources.Add(src);
    }


}

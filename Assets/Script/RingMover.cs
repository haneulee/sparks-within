using UnityEngine;

public class RingMover : MonoBehaviour
{
    private Transform target;
    private float speed = 1f;
    private float destroyDistance = 1f;

    private GameObject sourceCube;

    public void Initialize(Transform targetTransform, float moveSpeed, GameObject fromCube)
    {
        target = targetTransform;
        speed = moveSpeed;
        sourceCube = fromCube;
    }

    void Update()
    {
        if (target == null) return;

        transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) < destroyDistance)
        {
            if (sourceCube != null)
            {
                var profile = sourceCube.GetComponent<SoundProfile>();
                if (profile != null)
                {
                    if (SoundMemoryManager.Instance == null)
                    {
                        Debug.LogError("❌ SoundMemoryManager.Instance is NULL!");
                    }
                    else
                    {
                        Debug.Log("✅ SoundMemoryManager.Instance is alive.");
                        SoundMemoryManager.Instance.AddSound(profile);
                    }

                    var audio = sourceCube.GetComponent<AudioSource>();
                    if (audio != null) audio.Stop();

                    Destroy(sourceCube);
                    Debug.Log("Destroyed cube: " + sourceCube.name);
                }
                else
                {
                    Debug.LogWarning("SoundProfile component is missing on " + sourceCube.name);
                }
            }
            else
            {
                Debug.LogWarning("sourceCube is NULL in RingMover");
            }

            Destroy(gameObject);
        }
    }

}

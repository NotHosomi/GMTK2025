using UnityEngine;

public class WakeTrail : MonoBehaviour
{
    public GameObject[] wakePrefabs;
    public float spawnInterval = 0.1f;
    public float trailLifespan = 0.5f;
    public float backwardOffset = 0.5f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            SpawnWake();
        }
    }

    void SpawnWake()
    {
        if (wakePrefabs.Length == 0) return;

        Vector3 backward = -transform.up.normalized * backwardOffset;
        Vector3 spawnPos = transform.position + backward;

        int index = Random.Range(0, wakePrefabs.Length);
        GameObject wake = Instantiate(wakePrefabs[index], spawnPos, transform.rotation);
        StartCoroutine(FadeAndDestroy(wake.GetComponent<SpriteRenderer>(), trailLifespan));
    }

    System.Collections.IEnumerator FadeAndDestroy(SpriteRenderer sr, float duration)
    {
        float t = 0f;
        Color original = sr.color;
        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / duration);
            sr.color = new Color(original.r, original.g, original.b, alpha);
            yield return null;
        }
        Destroy(sr.gameObject);
    }
}

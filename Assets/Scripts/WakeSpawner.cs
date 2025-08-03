using UnityEngine;

public class WakeSpawner : MonoBehaviour
{
    public GameObject wakePrefab;
    public Sprite[] wakeSprites;
    public float spawnOffsetBack = 1.2f;
    public float spawnOffsetDown = 0.2f;

    private Wake lastWake;

    void Update()
    {
        if (lastWake == null || lastWake.Equals(null) || lastWake.HalfFaded)
        {
            SpawnWake();
        }
    }

    void SpawnWake()
    {
        Vector2 offset = (-transform.up * spawnOffsetBack) + (-transform.right * spawnOffsetDown);
        Vector3 spawnPos = transform.position + (Vector3)offset;

        GameObject wake = Instantiate(wakePrefab, spawnPos, transform.rotation, transform);

        SpriteRenderer sr = wake.GetComponent<SpriteRenderer>();
        sr.sprite = wakeSprites[Random.Range(0, wakeSprites.Length)];

        lastWake = wake.GetComponent<Wake>();
    }

}

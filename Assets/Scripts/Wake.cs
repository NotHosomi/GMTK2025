using UnityEngine;

public class Wake : MonoBehaviour
{
    public float fadeDuration = 2f;
    private SpriteRenderer sr;
    private float elapsed = 0f;

    public bool HalfFaded => sr.color.a <= 0.5f;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        float alpha = Mathf.Clamp01(1f - (elapsed / fadeDuration));
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        if (alpha <= 0f) Destroy(gameObject);
    }
}

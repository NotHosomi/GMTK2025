using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    private Vector2 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float time = Time.time * frequency;
        Vector2 primary = direction.normalized * Mathf.Sin(time) * amplitude;

        Vector2 perpendicular = new Vector3(-direction.y, direction.x).normalized;
        Vector2 swirl = perpendicular * Mathf.Cos(time) * (amplitude * 0.5f);

        transform.localPosition = startPos + primary + swirl;
    }

}

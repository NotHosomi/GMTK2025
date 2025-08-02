using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    public static List<Crate> s_crates;
    SpriteRenderer sr;
    private void Awake()
    {
        s_crates.Add(this);
    }

    private void OnDestroy()
    {
        s_crates.Remove(this);
    }

    [SerializeField] List<Sprite> frames;
    const float frameTime = 1.0f / 12.0f;
    float animClock = 0;
    int frameIdx = 0;
    void Animate()
    {
        animClock -= Time.deltaTime;
        if (animClock < 0)
        {
            animClock += frameTime;
            frameIdx = (frameIdx + 1) % frames.Count;
            sr.sprite = frames[frameIdx];
        }
    }
}

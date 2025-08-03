using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    public static List<Crate> s_crates;
    SpriteRenderer sr;
    public bool isNetted = false;
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if(s_crates != null)
        {
            s_crates.Add(this);
        }
    }

    private void Update()
    {
        Animate();
    }

    private void OnDestroy()
    {
        s_crates.Remove(this);
    }

    [SerializeField] List<Sprite> frames;
    const float frameTime = 1.0f / 8.0f;
    float animClock = frameTime;
    int frameIdx = 4;
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

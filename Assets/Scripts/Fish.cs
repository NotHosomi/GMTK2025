using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public static List<Fish> s_fish;
    SpriteRenderer sr;
    public static Fish NewFish(Vector2 pos)
    {
        Fish fish = (GameObject.Instantiate(Resources.Load("Prefab/Fish")) as GameObject).GetComponent<Fish>();
        fish.transform.position = pos;
        return fish;
    }
    private void Awake()
    {
        s_fish.Add(this);
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnDestroy()
    {
        s_fish.Remove(this);
    }

    private void Update()
    {
        Animate();
    }

    [SerializeField] List<Sprite> frames;
    const float frameTime = 1.0f / 12.0f;
    float animClock = frameTime;
    int frameIdx = 3;
    void Animate()
    {
        animClock -= Time.deltaTime;
        if(animClock < 0)
        {
            animClock += frameTime;
            frameIdx = (frameIdx + 1) % frames.Count;
            sr.sprite = frames[frameIdx];
        }
    }
}

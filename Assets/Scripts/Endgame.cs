using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Endgame : MonoBehaviour
{
    [SerializeField] AnimationCurve pos;
    [SerializeField] AnimationCurve opacity;

    float time;
    bool running = false;
    public void Run()
    {
        running = true;
        SFX._i.PlaySound(SFX.E_Sfx.womp);
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!running) return;
        time += Time.deltaTime;
        PlayIntro();

        if (time > pos.keys[pos.keys.Length - 1].time)
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.transform.parent != null)
                {
                    if (hit.collider.name == "ExitBtn")
                    {
                        SceneManager.LoadScene("Menu");
                    }
                    else if (hit.collider.name == "RetryBtn")
                    {
                        SceneManager.LoadScene("GameScene");
                    }
                }
            }
        }
    }

    void PlayIntro()
    {
        transform.position = new Vector3(pos.Evaluate(time), 0, 0);
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color col = sr.color;
            col.a = opacity.Evaluate(time);
            sr.color = col;
        }
        for (int i = 0; i < transform.childCount; ++i)
        {
            TextMeshPro tmp = transform.GetChild(i).GetComponent<TextMeshPro>();
            if(tmp != null)
            {
                Color col = tmp.color;
                col.a = opacity.Evaluate(time);
                tmp.color = col;
            }
            sr = transform.GetChild(i).GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color col = sr.color;
                col.a = opacity.Evaluate(time);
                sr.color = col;
            }
        }
    }
}

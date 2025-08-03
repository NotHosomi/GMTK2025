using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class Loop : MonoBehaviour
{
    private float mergeDist = 0.01f;
    private float MoveSpeed = 1f;
    private float itemMoveSpeed = 0.5f;
    private List<Vector2> trailPoints;
    private List<GameObject> nettedItems;
    private List<float> moveSpeeds;
    private List<float> nettedItemSpeeds;
    private LineRenderer line;

    public static Loop NewLoop(Vector2 origin, List<Vector2> trail)
    {
        Loop lp = (GameObject.Instantiate(Resources.Load("Prefab/Loop")) as GameObject).GetComponent<Loop>();
        lp.transform.position = origin;
        lp.trailPoints = trail;
        lp.line = lp.GetComponent<LineRenderer>();
        return lp;
    }

    // Start is called before the first frame update
    void Start()
    {
        nettedItems = new List<GameObject>();
        line.loop = true;
        line.positionCount = trailPoints.Count;
        float greatestDist = 0;
        moveSpeeds = new List<float>();
        for (int i = 0; i < trailPoints.Count; ++i)
        {
            line.SetPosition(i, trailPoints[i]);

            if((trailPoints[i] - (Vector2)transform.position).sqrMagnitude > greatestDist * greatestDist)
            {
                greatestDist = (trailPoints[i] - (Vector2)transform.position).magnitude;
            }
        }
        for(int i = 0; i < trailPoints.Count; ++i)
        {
            moveSpeeds.Add(MoveSpeed * (trailPoints[i] - (Vector2)transform.position).magnitude);
        }

        nettedItemSpeeds = new List<float>();
        foreach(Enemy e in Enemy.s_Enemies)
        {
            if(InNet(e.transform.position))
            {
                Destroy(e);
                e.GetComponent<Rigidbody2D>().simulated = false;
                nettedItems.Add(e.gameObject);
            }
        }
        foreach (Crate c in Crate.s_crates)
        {
            if (InNet(c.transform.position))
            {
                c.isNetted = true;
                nettedItems.Add(c.gameObject);
            }
        }
        foreach (Fish f in Fish.s_fish)
        {
            if (InNet(f.transform.position))
            {
                f.isNetted = true;
                nettedItems.Add(f.gameObject);
            }
        }

        for (int i = 0; i < nettedItems.Count; ++i)
        {
            nettedItemSpeeds.Add(itemMoveSpeed * ((Vector2)nettedItems[i].transform.position - (Vector2)transform.position).magnitude);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < nettedItems.Count; ++i)
        {
            nettedItems[i].transform.position = moveObj(nettedItems[i].transform.position, nettedItemSpeeds[i]);
        }
        for(int i = 0; i < trailPoints.Count; ++i)
        {
            trailPoints[i] = moveObj(trailPoints[i], moveSpeeds[i]);
        }
        for(int i = trailPoints.Count - 2; i > 1; --i)
        {
            Vector2 delta = trailPoints[i] - trailPoints[i + 1];
            if (delta.sqrMagnitude < (mergeDist * mergeDist))
            {
                //trailPoints[i] += (delta / 2.0f);
                trailPoints.RemoveAt(i + 1);
                moveSpeeds.RemoveAt(i + 1);
            }
        }
        line.positionCount = trailPoints.Count;
        for(int i = 0; i < trailPoints.Count; ++i)
        {
            line.SetPosition(i, trailPoints[i]);
        }
        if(trailPoints.Count < 5)
        {
            // todo: show caught items
            foreach(GameObject obj in nettedItems)
            {
                if(obj.tag == "Enemy")
                {
                    TrailDrawer._i.LineLength += 5;
                }
                else if (obj.tag == "Crate")
                {
                    TrailDrawer._i.LineLength += 2;
                }
                else if (obj.tag == "Fish")
                {
                    Score._i.AddScore(obj.transform.localScale.x);
                }
                Destroy(obj);
            }
            Destroy(gameObject);
        }
    }

    bool InNet(Vector2 pos)
    {
        int crossings = 0;
        int count = trailPoints.Count;

        for (int i = 0; i < count; i++)
        {
            Vector2 a = trailPoints[i];
            Vector2 b = trailPoints[(i + 1) % count];

            // Edge cross check
            if (((a.y > pos.y) != (b.y > pos.y)) &&
                (pos.x < (b.x - a.x) * (pos.y - a.y) / (b.y - a.y) + a.x))
            {
                crossings++;
            }
        }

        // Inside if odd
        return (crossings % 2) == 1;
    }

    Vector2 moveObj(Vector2 currentPos, float speed)
    {
        Vector2 delta = (currentPos - (Vector2)transform.position);
        Vector2 movement = delta.normalized * Time.deltaTime * speed;
        if (movement.sqrMagnitude > delta.sqrMagnitude)
        {
            movement = delta;
        }
        return currentPos - movement;
    }
}


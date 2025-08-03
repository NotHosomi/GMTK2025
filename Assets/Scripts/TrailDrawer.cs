using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TrailDrawer : MonoBehaviour
{
    public static TrailDrawer _i;
    public float LineLength = 20;
    public float pointSpacing = 0.1f;
    public float minLoopDistance = 0.5f;
    private List<Vector2> trailPoints = new List<Vector2>();
    private LineRenderer line;

    bool reachedLimit = false;
    bool trimmed = false;
    bool pauseOnNext = false;

    void Awake()
    {
        _i = this;
        line = GetComponent<LineRenderer>();
        line.positionCount = 0;
        line.loop = false;
        trailPoints.Add(transform.position);
    }

    void Update()
    {
        Vector2 currentPos = transform.position;
        if (trailPoints.Count == 0 || Vector2.Distance(currentPos, trailPoints[trailPoints.Count - 1]) > pointSpacing)
        {
            trailPoints.Add(currentPos);
            if(trailPoints.Count * pointSpacing >= LineLength)
            { // at limit
                reachedLimit = true;
                trailPoints.RemoveAt(0);
                if (line.positionCount != trailPoints.Count)
                { // For some reason, if a loop is created while at the lenght limit, the line position count gets trimmed BEFORE the trim function occurs??
                    Debug.Log("[ERROR] V:" + trailPoints.Count + "\tL:" + line.positionCount);
                    line.positionCount = trailPoints.Count;
                }
                for (int i = 0; i < trailPoints.Count; ++i)
                {
                    line.SetPosition(i, trailPoints[i]);
                }
            }
            else
            { // growing
                line.positionCount = trailPoints.Count;
                line.SetPosition(trailPoints.Count - 1, currentPos);
            }
        }

        trimmed = false;
        DetectLoop(currentPos);
    }

    void DetectLoop(Vector2 currentPos)
    {
        int count = trailPoints.Count;
        if (count < 4) return;

        Vector2 a1 = trailPoints[trailPoints.Count - 2];
        Vector2 a2 = trailPoints[trailPoints.Count - 1];

        for (int i = 0; i < count - 3; i++)
        {
            Vector2 b1 = trailPoints[i];
            Vector2 b2 = trailPoints[i + 1];

            if (LinesIntersect(a1, a2, b1, b2))
            {
                Debug.Log("Loop Intersected");
                TrimTrail(i+1);
                return;
            }
        }
    }

    void ClearTrail()
    {
        trailPoints.Clear();
        line.positionCount = 0;
    }

    void TrimTrail(int Idx)
    {
        trimmed = true;
        int count = (trailPoints.Count - 2) - Idx;
        Loop.NewLoop(trailPoints[trailPoints.Count - 1], trailPoints.GetRange(Idx, count));
        trailPoints.RemoveRange(Idx + 1, count);
        line.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; ++i)
        {
            line.SetPosition(i, trailPoints[i]);
        }
        LineLength = trailPoints.Count * pointSpacing;
    }

    bool LinesIntersect(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
    {
        float denominator = (A.x - B.x) * (C.y - D.y) - (A.y - B.y) * (C.x - D.x);
        if (denominator == 0) return false;

        float t = ((A.x - C.x) * (C.y - D.y) - (A.y - C.y) * (C.x - D.x)) / denominator;
        float u = ((A.x - C.x) * (A.y - B.y) - (A.y - C.y) * (A.x - B.x)) / denominator;

        return t > 0 && t < 1 && u > 0 && u < 1;
    }


    // scrap
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Crate>() && !collision.gameObject.GetComponent<Crate>().isNetted)
        {
            Destroy(collision.gameObject);
            ++LineLength;
        }
        if (collision.gameObject.GetComponent<Enemy>())
        {
            collision.gameObject.GetComponent<Enemy>().OnHit();
            if (LineLength == 0)
            {
                // todo: endgame
            }
            LineLength -= 5;
            if (LineLength < 5)
            {
                LineLength = 0;
            }
            while (trailPoints.Count * pointSpacing > LineLength)
            {
                trailPoints.RemoveAt(0);
            }
            line.positionCount = trailPoints.Count;
            for (int i = 0; i < trailPoints.Count; ++i)
            {
                line.SetPosition(i, trailPoints[i]);
            }
        }
    }

    public List<Vector2> GetTrail()
    {
        return trailPoints;
    }
}

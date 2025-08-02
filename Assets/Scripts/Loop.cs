using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class Loop : MonoBehaviour
{
    private float mergeDist = 0.01f;
    private float MoveSpeed = 1f;
    private List<Vector2> trailPoints;
    private List<float> moveSpeeds;
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
            float normalisationFactor = (trailPoints[i] - (Vector2)transform.position).magnitude;// / greatestDist);
            moveSpeeds.Add(MoveSpeed * normalisationFactor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < trailPoints.Count; ++i)
        {
            Vector2 delta = (trailPoints[i] - (Vector2)transform.position);
            Vector2 movement = delta.normalized * Time.deltaTime * moveSpeeds[i];
            if(movement.sqrMagnitude > delta.sqrMagnitude)
            {
                movement = delta;
            }
            trailPoints[i] -= movement;
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
            // todo: show caught item
            Destroy(gameObject);
        }
    }
}

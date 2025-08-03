using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static List<Enemy> s_Enemies;
    public static int s_count = 0;

    public float turnSpeed = 180f;
    public float targetSpeed;
    public float lerpRate = 5f;
    public float speedHi = 10f;
    public float speedBase = 6f;
    public float speedLo = 4f;
    private Rigidbody2D rb;
    private float currentSpeed;
    private float turnInput;
    private float speedInput;

    Transform player;
    float heading;
    float moodClock = 0;
    float meanderClock = 0;
    bool turning = false; // meandering only
    enum E_Mood { attacking, meandering, fleeing };
    E_Mood mood = E_Mood.attacking;
    public static Enemy NewEnemy(Vector2 pos, float rot)
    {
        Debug.Log("New enemy");
        Enemy enemy = (GameObject.Instantiate(Resources.Load("Prefab/Enemy")) as GameObject).GetComponent<Enemy>();
        enemy.transform.position = pos;
        enemy.player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy.rb = enemy.GetComponent<Rigidbody2D>();
        enemy.rb.SetRotation(rot);
        return enemy;
    }

    // Start is called before the first frame update
    void Start()
    {
        s_Enemies.Add(this);
        ++s_count;
    }

    private void OnDestroy()
    {
        s_Enemies.Remove(this);
        --s_count;
    }

    // Update is called once per frame
    void Update()
    {
        MoodSelect();
        MoodAct();
    }

    void MoodSelect()
    {
        moodClock -= Time.deltaTime;
        if (moodClock < 0)
        {
            switch (mood)
            {
                case E_Mood.attacking:
                    mood = E_Mood.fleeing;
                    targetSpeed = speedHi;
                    moodClock = Random.Range(3.0f, 5.0f);
                    break;
                case E_Mood.meandering:
                case E_Mood.fleeing:
                    int opt = Random.Range(0, 4);
                    if (opt == 0)
                    {
                        mood = E_Mood.attacking;
                        moodClock = Random.Range(5.0f, 8.0f);
                    }
                    else
                    {
                        mood = E_Mood.meandering;
                        moodClock = Random.Range(3.0f, 5.0f);
                    }
                    if (mood == E_Mood.meandering)
                    {
                        targetSpeed = speedLo;
                    }
                    else
                    {
                        targetSpeed = speedHi;
                    }
                    break;
            }
        }
    }

    void MoodAct()
    {
        switch (mood)
        {
            case E_Mood.attacking:
                Attack();
                break;
            case E_Mood.fleeing:
                Flee();
                break;
            case E_Mood.meandering:
                Meander();
                break;
        }
    }

    void PickMood(List<E_Mood> options)
    {
        mood = options[Random.Range(0, options.Count)];
    }

    void Attack()
    {
        // in range?
        if((player.position - transform.position).magnitude < 1)
        {
            targetSpeed = speedBase;
        }
        else
        {
            float angle = AngleToPlayer();
            if (angle < 0.5 || angle > (2 * 3.145) - 0.5)
            {
                targetSpeed = speedHi;
            }
            else if (angle > (3.145 / 2) && angle < (1.5 * 3.145)) ;
            else
            {
                targetSpeed = speedLo;
            }
            TurnToward();
        }
    }

    void Flee()
    {
        TurnAway();
        if ((player.position - transform.position).magnitude < 16)
        {
            targetSpeed = speedBase;
        }
        else
        {
            targetSpeed = speedLo;
        }
    }

    void Meander()
    {
        meanderClock -= Time.deltaTime;
        if(meanderClock < 0)
        {
            turnInput = Random.Range(-1.0f, 1.0f);
            meanderClock = Random.Range(0.1f, 0.8f);
        }
    }

    void TurnToward()
    {
        if (AngleToPlayer() > 3.142)
        {
            turnInput = -1;
        }
        else
        {
            turnInput = 1;
        }
    }

    void TurnAway()
    {
        if (AngleToPlayer() > 3.142)
        {
            turnInput = 1;
        }
        else
        {
            turnInput = -1;
        }
    }

    void FixedUpdate()
    {
        CollisionAvoidance(TrailDrawer._i.GetTrail());

        float rotationAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation + rotationAmount);

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpRate * Time.fixedDeltaTime);


        rb.velocity = transform.up * currentSpeed;
    }

    float AngleToPlayer()
    {
        Vector2 heading = transform.up;
        Vector2 delta = player.position - transform.position;
        return Vector2.Angle(heading, delta);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, 4*transform.up + transform.position);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, player.position);

        Gizmos.color = debugLhit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugLRay);
        Gizmos.color = debugChit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugCRay);
        Gizmos.color = debugRhit ? Color.red : Color.blue;
        Gizmos.DrawLine(transform.position, debugRRay);
    }

    public void OnHit()
    {
        mood = E_Mood.fleeing;
        moodClock = Random.Range(4.0f, 6.0f);
    }


    Vector2 debugLRay;
    Vector2 debugCRay;
    Vector2 debugRRay;
    bool debugLhit;
    bool debugChit;
    bool debugRhit;
    // bounds check
    void CollisionAvoidance(List<Vector2> collider)
    {
        Vector2 leftQ = transform.rotation * new Vector2(-1f, 4);
        Vector2 centerQ = transform.rotation * new Vector2(0, 6);
        Vector2 rightQ = transform.rotation * new Vector2(1f, 4);
        leftQ += (Vector2)transform.position;
        rightQ += (Vector2)transform.position;
        centerQ += (Vector2)transform.position;
        debugLRay = leftQ;
        debugCRay = centerQ;
        debugRRay = rightQ;

        float L = 0;
        float R = 0;
        float C = 0;
        for (int i = 0; i < collider.Count - 1; ++i)
        {
            float temp = LineIntersection(transform.position, leftQ, collider[i], collider[i + 1]);
            if (temp > L) { L = temp; }
            temp = LineIntersection(transform.position, rightQ, collider[i], collider[i + 1]);
            if (temp > R) { R = temp; }
            temp = LineIntersection(transform.position, centerQ, collider[i], collider[i + 1]);
            if (temp > C) { C = temp; }
        }
        debugRhit = R != 0;
        debugLhit = L != 0;
        debugChit = C != 0 && R != 0 && L != 0;
        if (R == 0 && L == 0) { return; }
        if (R > 0 && L > 0)
        {
            if (C > 0)
            {
                turnInput = 2.5f;
                return;
            }
        }
        if (R >= L)
        {
            turnInput = R * 2 + 0.5f;
        }
        else
        {
            turnInput = -(L * 2 + 0.5f);
        }
    }
    float LineIntersection(Vector2 QueryOrigin, Vector2 QueryEnd, Vector2 LineStart, Vector2 LineEnd)
    {
        Vector2 A = QueryOrigin;
        Vector2 B = QueryEnd;
        Vector2 C = LineStart;
        Vector2 D = LineEnd;
        float denominator = (A.x - B.x) * (C.y - D.y) - (A.y - B.y) * (C.x - D.x);
        if (denominator == 0) return 0;

        float t = ((A.x - C.x) * (C.y - D.y) - (A.y - C.y) * (C.x - D.x)) / denominator;
        float u = ((A.x - C.x) * (A.y - B.y) - (A.y - C.y) * (A.x - B.x)) / denominator;

        if (!(t > 0 && t < 1 && u > 0 && u < 1))
        {
            return 0;
        }
        return 1 - t;
    }
}

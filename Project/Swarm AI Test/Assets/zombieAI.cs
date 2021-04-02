using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Pathfinding;

public class zombieAI : MonoBehaviour
{
    public GameObject point;
    public Rigidbody2D rb;

    public static int zombieCount;
    public int ID;
    public Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {
        point = transform.Find("Point").gameObject;
        StartCoroutine(path());
        seeker = gameObject.GetComponent<Seeker>();
        seeker.graphMask = 0;
        for (int i = 0; i < 6; ++i)
        {
            if (ID != i)
            {
                seeker.graphMask += 1 << i;
            }
        }
        gameObject.GetComponent<Seeker>().graphMask = seeker.graphMask;

        //        graph = AstarPath.active.Scan();
    }
    IEnumerator path()
    {
        while (true)
        {
            while (Vector2.Distance(transform.position, point.transform.position) <= 1.5f)
            {
                point.GetComponent<Point>().findNewPos();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}

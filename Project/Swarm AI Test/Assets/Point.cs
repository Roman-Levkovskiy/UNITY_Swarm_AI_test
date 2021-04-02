using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public GameObject targetPos;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Background")
        {
            targetPos = collision.gameObject;
        }
    }

    private void Start()
    {
        targetPos = gameObject;


        StartCoroutine(changePos());
    }

    public void Update()
    {
        transform.position = position;
        if(Vector2.Distance(transform.position, transform.parent.parent.position)>3.5f)
        {
            findNewPos();
        }
    }

    IEnumerator changePos()
    {
        while (true)
        {
            findNewPos();

            yield return new WaitForSeconds(5f);
        }
    }

    public GameObject AI;
    public Vector2 position;
    public void findNewPos()
    {
        if (transform.parent.name != "zombie_an_1")
        {
            AI = transform.parent.parent.gameObject;
            float angle = AI.GetComponent<SwarmIntelligance>().angle;

            if (AI.GetComponent<SwarmIntelligance>().moveDir != null)
            {
                transform.position = AI.GetComponent<SwarmIntelligance>().moveDir.transform.position;
            }

            float r = 3.5f + Random.Range(-1.4f, 1.4f);
            angle += Random.Range(-30f, 30f);
            position = new Vector2(transform.parent.parent.position.x + r * Mathf.Cos(angle - 90), transform.parent.parent.position.y + r * Mathf.Sin(angle - 90));

        }
        else
        {
            transform.position = new Vector2(-100, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Back : MonoBehaviour
{
    public GameObject up, down, left, right;
    public List<GameObject> allZombies;
    public List<GameObject> allPoints;
    public bool isUnderZombie;
    public bool isUnderPoint;
    public bool isSelected;
    public int zombieCount;
    public int number;
    private void Start()
    {
        allPoints = new List<GameObject>();
        allZombies = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            gameObject.layer = collision.gameObject.layer;
            allZombies.Add(collision.transform.Find("Point").gameObject);
            isUnderZombie = true;
            ++zombieCount;
        }
        if (collision.tag == "Point")
        {
            isUnderPoint = true;
            allPoints.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            --zombieCount;
            if (zombieCount == 0)
            {
                gameObject.layer = 8;
                isUnderZombie = false;
                gameObject.layer = 0;
            }
            allZombies.Remove(allZombies.Find(d => d == collision.transform.Find("Point").gameObject));
        }
        if (collision.tag == "Point")
        {
            allPoints.Remove(allPoints.Find(d => d == collision.gameObject));
            if(allPoints.Count <=0)
            {
                isUnderPoint = false;
            }
        }
    }
}

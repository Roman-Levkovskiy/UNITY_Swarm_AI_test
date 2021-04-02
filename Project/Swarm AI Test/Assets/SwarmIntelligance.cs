using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmIntelligance : MonoBehaviour
{
    public float x;
    public float y;
    public GameObject zombie;
    public GameObject moveDir;
    public Rigidbody2D rb;
    public List<GameObject> allZombies;
    GameController.cubeInfo[][] cubes;
    float[][] map;
    float[][] map2;
    public List<KeyValuePair<GameObject, Vector2>> background;
    public float degree;
    public float angle;
    public int max_i, min_i, max_j, min_j;
    public int zombieCount;
    public struct backCube
    {
        public float x;
        public float y;
        public float score;
        public GameObject cube;

        public backCube( float x, float y, float score, GameObject cube)
        {
            this.x = x;
            this.y = y;
            this.score = score;
            this.cube = cube;
        }

    }

    public backCube[][] back;

    void Start()
    {
        zombieAI.zombieCount = 0;
        zombieCount = 0;
        angle = 0;
        map = new float[73][];
        for (int i = 0; i < 73; ++i)
        {
            map[i] = new float[41];
        }

        map2 = map;

        moveDir = Instantiate(new GameObject(), Vector2.zero, new Quaternion());
        moveDir.transform.parent = gameObject.transform;
        moveDir.name = "MoveDir";
        allZombies = new List<GameObject>();
        background = new List<KeyValuePair<GameObject, Vector2>>();
        rb = GetComponent<Rigidbody2D>();

        for (int i = 0; i < 6; ++i)
        {
            bool canSpawn = false;
            Vector2 currentZombie = new Vector2();
            while (!canSpawn)
            {
                canSpawn = true;
                currentZombie = new Vector2(transform.position.x + Random.Range(-4f, 4f), transform.position.y + Random.Range(-4f, 4f));
                for (int j = 0; j<allZombies.Count; ++j)
                {
                    if(Vector2.Distance(currentZombie, allZombies[j].transform.position) < 1.8f)
                    {
                        canSpawn = false;
                    }
                }
            }
            allZombies.Add(Instantiate(zombie, currentZombie, new Quaternion()));
            allZombies[i].name += (i+1);
            allZombies[i].transform.parent = gameObject.transform;
            ++zombieCount;
        }

        cubes = GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>().cubes;

        for (int i = 0; i < allZombies.Count; ++i)
        {
            scale();
        }
    }
    void Update()
    {
        moveDir.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        movePosCheck();
        moveDir.transform.position = new Vector2(x, y);
        rb.velocity = (moveDir.transform.position-transform.position).normalized / 1.5f;

        //var q = Quaternion.LookRotation(moveDir.transform.position - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, q, 1000 * Time.deltaTime);

        Vector2 relativePos = moveDir.transform.position - transform.position;
        angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Background")
        {
            collision.GetComponent<Back>().isSelected = true;
            collision.GetComponent<MeshRenderer>().material.color = Color.red;
            background.Add(new KeyValuePair<GameObject, Vector2>(collision.gameObject, collision.transform.position));

            for(int i = 0; i<allZombies.Count; ++i)
            {
                scale();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Background")
        {
            collision.GetComponent<Back>().isSelected = false;
            collision.gameObject.transform.GetComponent<MeshRenderer>().material.color = Color.white;
            background.Remove(background.Find(x => x.Key == collision.gameObject));
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Background")
        {
            collision.gameObject.transform.GetComponent<MeshRenderer>().material.color = new Color(3.25f -  Vector2.Distance(collision.gameObject.transform.position, moveDir.transform.position)/ 4.25f, 0, 0);
        }
    }

    public void movePosCheck()
    {
        Vector2 A = transform.position;
        Vector2 B = moveDir.transform.position;

        var v = new Vector2(B.x - A.x, B.y - A.y);
        var l = (float)Mathf.Sqrt(v.x * v.x + v.y * v.y);
        v = new Vector2(v.x / l, v.y / l);
        var newEnd = new Vector2(B.x + v.x * 5f, B.y + v.y * 5f);

        float LL = Mathf.Sqrt((B.x - A.x) * (B.x - A.x) + (B.y - A.y) * (B.y - A.y));

        x = A.x + (B.x - A.x) * 5f / LL;
        y = A.y + (B.y - A.y) * 5f / LL;
    }

    public void calculatemap()
    {
        //map = map2;

        List<Vector2> zombies = new List<Vector2>();
        for (int i = 0; i < 73; ++i)
        {
            for (int j = 0; j < 41; ++j)
            {
                Debug.Log(i * 73 + j);
                if (GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>().allBack[i * 41 + j].GetComponent<Back>().isUnderZombie)
                {
                    zombies.Add(new Vector2((float)((i - 36) / 4), (float)((j - 20) / 4) ));
                }
            }
        }



        for(int i = 0; i<zombies.Count; ++i)
        {
            Debug.Log(GameObject.FindGameObjectWithTag("Controller").GetComponent<GameController>().transform.Find("Cube"+((zombies[i].x*4+36)*73+(zombies[i].y*4+20))).transform.position   );
        }
    }

    public void scale()
    {
        max_i = max_j = 0;
        min_i = min_j = 1000;    //Mathf.Infinity is float, can't use it here
        
        for( int i = 0; i < 73; ++i)
        {
            for(int j = 0; j<41; ++j)
            {
                if(cubes[i][j].cube.GetComponent<Back>().isSelected)
                {
                    if (i < min_i)
                    {
                        min_i = i;
                    }
                    if (j < min_j)
                    {
                        min_j = j;
                    }
                    if (i > max_i)
                    {
                        max_i = i;
                    }
                    if (j > max_j)
                    {
                        max_j = j;
                    }
                }
            }
        }

        if (max_i > min_i)
        {
            back = new backCube[max_i - min_i + 1][];

            for (int i = 0; i <= max_i - min_i; ++i)
            {
                back[i] = new backCube[max_j - min_j + 1];
            }


            for (int i = min_i; i <= max_i; ++i)
            {
                for (int j = min_j; j <= max_j; ++j)
                {
                    back[i - min_i][j - min_j] = new backCube((float)(i - 36) / 4f, (float)(j - 20) / 4f, 0, cubes[i][j].cube);
                }
            }

            int x = max_i - min_i;
            int y = max_j - min_j;
        }
    }
}

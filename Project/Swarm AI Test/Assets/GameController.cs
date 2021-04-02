using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cube;
    public List<GameObject> allBack;
    public struct cubeInfo
    {
        public float x;
        public float y;
        public GameObject cube;

        public cubeInfo(float x, float y, GameObject cube)
        {
            this.x = x;
            this.y = y;
            this.cube = cube;
        }
    }

    public cubeInfo[][] cubes;

    void Start()
    {
        allBack = new List<GameObject>();

        cubes = new cubeInfo[73][];
        for (int i = 0; i < 73; ++i)
        {
            cubes[i] = new cubeInfo[41];
        }

        for (float i = -9; i <= 9; i += 0.25f)
        {
            for (float j = -5; j <= 5; j += 0.25f)
            {
                allBack.Add(Instantiate(cube, new Vector3(i, j, 1), new Quaternion()));
                cubes[(int)(i * 4 + 36)][(int)(j * 4 + 20)] = new cubeInfo(i, j, allBack[allBack.Count-1]);
            }
        }
        for (int i = 0; i <= 2992; ++i)
        {
            if (i < 2952)
            {
                allBack[i].GetComponent<Back>().right = allBack[i +41];
            }
            if (i > 40)
            {
                allBack[i].GetComponent<Back>().left = allBack[i -41];
            }
            if (i % 41 != 0 && i > 0)
            {
                allBack[i].GetComponent<Back>().down = allBack[i - 1];
            }
            if (i % 41 -40 != 0 && i < 2992)
            {
                allBack[i].GetComponent<Back>().up = allBack[i + 1];
            }
            allBack[i].GetComponent<Back>().number = i;

            allBack[i].transform.parent = gameObject.transform;
            allBack[i].gameObject.name = "Cube"+allBack[i].GetComponent<Back>().number;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

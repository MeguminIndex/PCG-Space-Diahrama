using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GenerateShipBasic : MonoBehaviour {


    public GameObject baseGeomitry;

    

    public int numberBlocks;
    public int maxW;
    public int maxH;
    public int maxT;
    public int minW;
    public int minH;
    public int minT;

    public List<GameObject> shipBlocks;

	// Use this for initialization
	void Start () {
        shipBlocks = new List<GameObject>();


        StartCoroutine(Ship());



        //for(int i  =0; i < numberBlocks; i++ )
        //{

        //    GameObject block = Instantiate(baseGeomitry, nextPos, Quaternion.identity);

        //    block.name = i.ToString();

        //    int ax = Random.Range(0,6);

        //    int n = Random.Range(0,2);

        //    switch (ax)
        //    {
        //        //X
        //        case 0:
        //            {
        //                if (n == 0 && x != true)
        //                {
        //                    nextPos.x -= baseGeomitry.transform.lossyScale.x;
        //                    y = false;
        //                    z = false;
        //                }
        //                else if (x == true)
        //                {
        //                    nextPos.x += baseGeomitry.transform.lossyScale.x;
        //                    x = true;
        //                }
        //                break;
        //            }
        //        //Y
        //        case 1:
        //            {
        //                if (n == 0 && y != true)
        //                {
        //                    nextPos.y -= baseGeomitry.transform.lossyScale.y;
        //                    x = false;
        //                    z = false;
        //                }
        //                else if (y == true)
        //                {
        //                    nextPos.y += baseGeomitry.transform.lossyScale.y;
        //                    y = true;
        //                }
        //                break;
        //            }
        //        //Z
        //        case 2:
        //            {
        //                if (n == 0 && z != true)
        //                {
        //                    nextPos.z -= baseGeomitry.transform.lossyScale.z;
        //                    x = false;
        //                    y = false;
        //                }
        //                else if (z == true)
        //                {
        //                    nextPos.z += baseGeomitry.transform.lossyScale.z;
        //                    z = true;
        //                }
        //                break;
        //            }




        //    }


        //    shipBlocks.Add(block);

        //}





    }


    IEnumerator Ship()
    {


        Vector3 nextPos = Vector3.zero;
        bool x = false, y = false, z = false;

        int w, h, t;

        w = Random.Range(minW, maxH + 1);
        h = Random.Range(minH, maxH + 1);
        t = Random.Range(minT, maxT + 1);
        //for(int i =0; i < numberBlocks; i++)
        //{



        //}


        for (int wi = 1; wi < w + 1; wi++)
        {
            for (int hi = 1; hi < h + 1; hi++)
            {
                for (int ti = 1; ti < t+1; ti++)
                {

                    nextPos.z = baseGeomitry.transform.lossyScale.z * ti;

                    nextPos.x = baseGeomitry.transform.lossyScale.x * wi;

                    nextPos.y = baseGeomitry.transform.lossyScale.y * hi;

                    GameObject block = Instantiate(baseGeomitry, nextPos, Quaternion.identity);
                    block.name = "W: " + wi.ToString() + " H: " + hi.ToString() + " Z: " + ti.ToString();

                    //nextPos.z = baseGeomitry.transform.lossyScale.z * ti;

                    //nextPos.x = baseGeomitry.transform.lossyScale.x * wi;

                    //nextPos.y = baseGeomitry.transform.lossyScale.y * hi;

                    yield return new WaitForSeconds(0.1f);

                shipBlocks.Add(block);
                 }
                nextPos.y = baseGeomitry.transform.lossyScale.y * hi;
            }

           
            nextPos.x = 0;
            nextPos.y = 0;
            nextPos.z = 0;
        }


    }

    // Update is called once per frame
    void Update () {
		
	}
}

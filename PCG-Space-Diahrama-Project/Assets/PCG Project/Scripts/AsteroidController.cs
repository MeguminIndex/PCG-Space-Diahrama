using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public Vector3 spawnArea;
    public int numPoints;
    public float mod;
    public float density;

    public Vector3 sizeMin;
    public Vector3 sizeMax;

    public int astroidsBeforeVariation;



    [SerializeField]
    private GameObject[] astroidPrefabs;

    private int numstroids;
    private List<GameObject> astroids;

    private void Start()
    {
        InitAsteroids();
    }


    public void InitAsteroids()
    {
        astroids = new List<GameObject>();

        numstroids = (int)(spawnArea.sqrMagnitude * density);
        Debug.Log(numstroids);
        astroids.Capacity = numstroids;

        //generate spawn point areas (groups), asteriods tend to group up a bit so having several base points for them to gravitate around good
        Vector3[] points = new Vector3[numPoints];
        for(int i =0; i < numPoints; i++)
        {
            //generate these points randomly so the layout could be different
            points[i] = new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
                Random.Range(-spawnArea.y, spawnArea.y),
                Random.Range(-spawnArea.z, spawnArea.z));
        }
                                     
        for (int i =0; i <= numstroids; i++)
        {
            byte val = 0;
            if( i % astroidsBeforeVariation == 0)
            {
                val = (byte)Random.Range(0, astroidPrefabs.Length);
            }

          
            GameObject astroid = GameObject.Instantiate(astroidPrefabs[val]);

            float x = 0, y = 0, z = 0;

            x = Random.Range(sizeMin.x, sizeMax.x);
            y = Random.Range(sizeMin.y, sizeMax.y);
            z = Random.Range(sizeMin.z, sizeMax.z);



            astroid.transform.localScale = new Vector3(x,y,z);

            int pointInd = Random.Range(0, numPoints);
            
            //astroids position based on a random basepoint + a random offset from that point
            x = Random.Range(points[pointInd].x + (-spawnArea.x/ mod), points[pointInd].x +(spawnArea.x / mod));
            y = Random.Range(points[pointInd].y +(-spawnArea.y/ mod), points[pointInd].y +(spawnArea.y / mod));
            z = Random.Range(points[pointInd].z + (-spawnArea.z / mod), points[pointInd].z +(spawnArea.z / mod));

            //take into consideration ships ect at some point??


            Vector3 pos = new Vector3(x,y,z);
            Quaternion rot = Random.rotation;
            astroid.transform.position = pos;
            astroid.transform.rotation = rot;
            astroids.Add(astroid);

          

        }


        



    }



    private void OnApplicationQuit()
    {
        astroids.Clear();
        astroids = null;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    // public Vector3 spawnArea;

    public float maxAstoridSpawnDist;
    public float minAstroidSpawnDist;

   // public int numPoints;
   // public float mod;


    public float minDensity;
    public float maxDensity;
    float density;

    public Vector3 sizeMin;
    public Vector3 sizeMax;

    public int astroidsBeforeVariation;



    [SerializeField]
    private GameObject[] astroidPrefabs;

    private int numstroids;
    private List<GameObject> astroids;

    private void Start()
    {
        //InitAsteroids();

        InitPlanetAsteroids();

    }


    //public void InitAsteroids()
    //{
    //    astroids = new List<GameObject>();

    //    numstroids = (int)(spawnArea.sqrMagnitude * density);
    //    Debug.Log(numstroids);
    //    astroids.Capacity = numstroids;

    //    //generate spawn point areas (groups), asteriods tend to group up a bit so having several base points for them to gravitate around good
    //    Vector3[] points = new Vector3[numPoints];
    //    for(int i =0; i < numPoints; i++)
    //    {
    //        //generate these points randomly so the layout could be different
    //        points[i] = new Vector3(Random.Range(-spawnArea.x, spawnArea.x),
    //            Random.Range(-spawnArea.y, spawnArea.y),
    //            Random.Range(-spawnArea.z, spawnArea.z));
    //    }
                                     
    //    for (int i =0; i <= numstroids; i++)
    //    {
    //        byte val = 0;
    //        if( i % astroidsBeforeVariation == 0)
    //        {
    //            val = (byte)Random.Range(0, astroidPrefabs.Length);
    //        }

          
    //        GameObject astroid = GameObject.Instantiate(astroidPrefabs[val]);

    //        float x = 0, y = 0, z = 0;

    //        x = Random.Range(sizeMin.x, sizeMax.x);
    //        y = Random.Range(sizeMin.y, sizeMax.y);
    //        z = Random.Range(sizeMin.z, sizeMax.z);



    //        astroid.transform.localScale = new Vector3(x,y,z);

    //        int pointInd = Random.Range(0, numPoints);
            
    //        //astroids position based on a random basepoint + a random offset from that point
    //        x = Random.Range(points[pointInd].x + (-spawnArea.x/ mod), points[pointInd].x +(spawnArea.x / mod));
    //        y = Random.Range(points[pointInd].y +(-spawnArea.y/ mod), points[pointInd].y +(spawnArea.y / mod));
    //        z = Random.Range(points[pointInd].z + (-spawnArea.z / mod), points[pointInd].z +(spawnArea.z / mod));

    //        //take into consideration ships ect at some point??


    //        Vector3 pos = new Vector3(x,y,z);
    //        Quaternion rot = Random.rotation;
    //        astroid.transform.position = pos;
    //        astroid.transform.rotation = rot;
    //        astroids.Add(astroid);

          

    //    }


        



    //}


    void InitPlanetAsteroids()
    {

        astroids = new List<GameObject>();

        

        float volume = 4.0f / 3.0f * Mathf.PI * ((transform.lossyScale.x + maxAstoridSpawnDist) * Mathf.PI);
       // Debug.Log("Volume: " + volume.ToString());

        density = Random.Range(minDensity,maxDensity);

        numstroids = (int)(volume * density);
        Debug.Log("Number of astroids: " + numstroids.ToString());
        astroids.Capacity = numstroids;


        float radius = GetComponent<SphereCollider>().radius;

        for (int i = 0; i <= numstroids; i++)
        {

            byte val = 0;
            if (i % astroidsBeforeVariation == 0)
            {
                val = (byte)Random.Range(0, astroidPrefabs.Length);
            }


            GameObject astroid = GameObject.Instantiate(astroidPrefabs[val]);
            float astroidDist = Random.Range(minAstroidSpawnDist,maxAstoridSpawnDist);

           // Debug.Log("Astroid Dist: " + astroidDist.ToString());

            Vector3 pos2 = Random.onUnitSphere * (radius + astroidDist);
            Vector2 vec2 = Random.insideUnitCircle * astroidDist;

            Vector3 pos = new Vector3(vec2.x, 0, vec2.y);

            astroid.transform.position = transform.TransformPoint(pos2);


            //SCALE
            float x = 0, y = 0, z = 0;
            x = Random.Range(sizeMin.x, sizeMax.x);
            y = Random.Range(sizeMin.y, sizeMax.y);
            z = Random.Range(sizeMin.z, sizeMax.z);
            astroid.transform.localScale = new Vector3(x, y, z);

            //ROTATION          
            Quaternion rot = Random.rotation;
            //astroid.transform.position = pos;
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

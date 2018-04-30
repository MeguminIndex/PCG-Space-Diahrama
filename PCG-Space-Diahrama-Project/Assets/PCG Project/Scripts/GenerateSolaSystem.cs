using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSolaSystem : MonoBehaviour {

    public int seed;
    public bool useSeed;

    [Header("Star Properties")]
    public GameObject[] starTypesPrefabs;

    [Header("Planet Properties")]
    public int minNPlanets;
    public int maxNPlanets;
    public GameObject planetPrefab;
    public float maxPlanDist;
    public float minPlanDist;
    public float maxPlanSize;
    public float minPlanSize;
    public float planetmindistOffset;



    //PRIVATE VARIABLES
    GameObject sun;   
    private int nPlanets;
    private int starTypeID;

    List<Vector3> planetPos;
    /* used to marking positions first - 
    later use this maby to handle any checking to ensure planets not too close to each other */
    List<GameObject> planets;//refrence to planets spawned insola system.


 

    // Use this for initialization
    void Start()
    {
        GenerateSystem();
        Time.timeScale = 0;//pausing the time allows easy way to stop all movement in my scene, reduce physical load since nextr function is demanding

        StartCoroutine(UpdatePlenetTextures());
    }


    void GenerateSystem()
    {

        planetPos = new List<Vector3>();
        planets = new List<GameObject>();

        if (useSeed)
            Random.InitState(seed);

        nPlanets = Random.Range(minNPlanets, maxNPlanets);

        starTypeID = Random.Range(0, starTypesPrefabs.Length);

        sun = Instantiate(starTypesPrefabs[starTypeID]);

        sun.transform.position = transform.position;

        Debug.Log("N Planets: " + nPlanets.ToString());

        

        for (int i = 0, progress = 0; progress < nPlanets; progress++)
        {
            Vector3 pos = Vector3.zero;
            float randDIst = Random.Range(minPlanDist, maxPlanDist);
            pos = Random.insideUnitSphere * randDIst;

            //check planet is not too close

            bool spawn = true;

            for(int c =0; c < planetPos.Count; c++ )
            {
              //  Debug.Log(Vector3.Distance(pos, planetPos[c]));

                if (Vector3.Distance(pos, planetPos[c]) < maxPlanSize + (maxPlanSize *2))
                {
                  //  Debug.Log("Dont generate PLANET");
                    spawn = false;
                    break;
                }

            }
         //   Debug.Log("Planet SPawned");

            if (spawn == true)
            {
                planetPos.Add(pos);
                i++;
            }
        }






        for (int i = 0; i < planetPos.Count; i++)
        {
            GameObject spawned = GameObject.Instantiate(planetPrefab);
            spawned.transform.position = transform.TransformPoint(planetPos[i]);
            float size = Random.Range(minPlanSize,maxPlanSize);
            spawned.transform.localScale = new Vector3(size, size, size);


            planets.Add(spawned);
        }




        Debug.Log("Number of planets spawned: " + planets.Count);
    }



    IEnumerator UpdatePlenetTextures()
    {
        //hold time for generastion to conclude to help ensure everything is generated.
        yield return new WaitForSecondsRealtime(5);
        Debug.Log("Updating Planets Started");

        int size = planets.Count;
        for(int i=0; i < size; i++)
        {
            
            PlanetGenerator tmp = planets[i].GetComponent<PlanetGenerator>();

            if (tmp == null)
                i -= 1;
            else
                i += tmp.GenerateFullTexture();

            yield return new WaitForSecondsRealtime(2);


        }

        Debug.Log("Finished Updating Planets maps");

        Time.timeScale = 1;
    }

}

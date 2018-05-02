using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSolaSystem : MonoBehaviour {

    public SolaSystemData solaSysData;

    //public int seed;
    //public bool useSeed;

    //[Header("Star Properties")]
    //public GameObject[] starTypesPrefabs;

    //[Header("Planet Properties")]
    //public int minNPlanets;
    //public int maxNPlanets;
    //public GameObject planetPrefab;
    //public float maxPlanDist;
    //public float minPlanDist;
    //public float maxPlanSize;
    //public float minPlanSize;
    //public float planetmindistOffset;



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

        //StartCoroutine(UpdatePlenetTextures());

        StartCoroutine(DisableTimeDuration(5));
    }


    void GenerateSystem()
    {

        planetPos = new List<Vector3>();
        planets = new List<GameObject>();

        if (solaSysData.useSeed)
            Random.InitState(solaSysData.seed);

        nPlanets = Random.Range(solaSysData.minNPlanets, solaSysData.maxNPlanets);

        starTypeID = Random.Range(0, solaSysData.starTypesPrefabs.Length);

        sun = Instantiate(solaSysData.starTypesPrefabs[starTypeID]);

        sun.transform.position = transform.position;

      //  Debug.Log("N Planets: " + nPlanets.ToString());

        

        for (int i = 0, progress = 0; progress < nPlanets; progress++)
        {
            Vector3 pos = Vector3.zero;
            float randDIst = Random.Range(solaSysData.minPlanDist, solaSysData.maxPlanDist);
            pos = Random.onUnitSphere * randDIst;

           

            //check planet is not too close

            bool spawn = true;

            for(int c =0; c < planetPos.Count; c++ )
            {
              //  Debug.Log(Vector3.Distance(pos, planetPos[c]));

                if (Vector3.Distance(pos, planetPos[c]) < solaSysData.maxPlanSize + (solaSysData.maxPlanSize * 2))
                {
                  //  Debug.Log("Dont generate PLANET");
                    spawn = false;
                    break;
                }

            }
       

            if (spawn == true)
            {
                planetPos.Add(pos);
                i++;
            }
        }






        for (int i = 0; i < planetPos.Count; i++)
        {
            GameObject spawned = GameObject.Instantiate(solaSysData.planetPrefab);

            //Debug.Log("Planet Distance from sun: " + planetPos[i]);
            spawned.transform.position = transform.TransformPoint(planetPos[i]);
            float size = Random.Range(solaSysData.minPlanSize, solaSysData.maxPlanSize);
            spawned.transform.localScale = new Vector3(size, size, size);


            planets.Add(spawned);
        }




        Debug.Log("Number of planets spawned: " + planets.Count);
    }

    IEnumerator DisableTimeDuration(float duration )
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }

    IEnumerator UpdatePlenetTextures()
    {
        //hold time for generastion to conclude to help ensure everything is generated.
        yield return new WaitForSecondsRealtime(5);
        Debug.Log("Updating Planets Started");

        int size = planets.Count;

        int wcount = 0;

        for(int i=0; i < size; i++ , wcount++)
        {
            
            PlanetGenerator tmp = planets[i].GetComponent<PlanetGenerator>();

            if (tmp == null)
            {
                i -= 1;
                //yield return new WaitForSecondsRealtime(0.5f);
            }
            else
                i += tmp.GenerateFullTexture();

            if (wcount > 4)
            {
                //yield return new WaitForSecondsRealtime(0.5f);
                wcount = 0;
            }

        }

        Debug.Log("Finished Calling Planets updates");

        Time.timeScale = 1;
    }


    void GenerateBattleAftermath()
    {



    }

}

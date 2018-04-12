using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateSolaSystem : MonoBehaviour {

    public int seed;
    public bool useSeed;

    public int minNPlanets;
    public int maxNPlanets;
    public GameObject planetPrefab;

    public GameObject[] starTypesPrefabs;

    

    GameObject sun;
    

    private int nPlanets;
    public float maxPlanDist;
    public float minPlanDist;


    private int starTypeID;

    List<Vector3> planetPos;
    /* used to marking positions first - 
    later use this maby to handle any checking to ensure planets not too close to each other */
    List<GameObject> planets;//refrence to planets spawned insola system.


 

    // Use this for initialization
    void Start()
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

        for (int i = 0; i < nPlanets; i++)
        {
            Vector3 pos = Vector3.zero;

            float randDIst = Random.Range(minPlanDist,maxPlanDist);

            pos = Random.insideUnitSphere * randDIst;

            //Debug.Log(pos);
            planetPos.Add(pos);
        }


        for (int i = 0; i < nPlanets; i++)
        {
            GameObject spawned = GameObject.Instantiate(planetPrefab);
            spawned.transform.position = transform.TransformPoint(planetPos[i]);
            
            planets.Add(spawned);
        }


    }



}

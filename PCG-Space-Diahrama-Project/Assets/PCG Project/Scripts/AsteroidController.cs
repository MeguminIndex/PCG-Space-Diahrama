using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour {

    public Vector3 spawnArea;
    public float density;
    private int numstroids;
    private List<GameObject> astroids;

    [SerializeField]
    private GameObject[] astroidPrefabs;

	// Use this for initialization
	void Start () {
        astroids = new List<GameObject>();

        InitAsteroids();

    }
	
	

    void InitAsteroids()
    {

        numstroids = (int)(spawnArea.magnitude * density);

        for (int i =0; i < numstroids; i++)
        {
            GameObject astroid = GameObject.Instantiate(astroidPrefabs[0]);

            float x =0, y=0,z=0;

            x = Random.Range(-spawnArea.x, spawnArea.x);
            y = Random.Range(-spawnArea.y, spawnArea.y);
            z = Random.Range(-spawnArea.z,spawnArea.z);

            Vector3 pos = new Vector3(x,y,z);
            Quaternion rot = Random.rotation;
            astroid.transform.position = pos;
            astroid.transform.rotation = rot;
            astroids.Add(astroid);

        }



    }



}

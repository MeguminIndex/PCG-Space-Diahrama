using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AsteroidController))]
public class PlanetGenerator : MonoBehaviour
{

    MeshRenderer rend;
    Material mat;

    AsteroidController asteroidC;

    public float maxRedWeightDist;
    public float maxBlueWeight;
    // public float 
    public float rotationSpeed = 5.0f;



    public GameObject salvagingDronePref;


    List<GameObject> salvageDrones;
    bool[] moveDrone;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<MeshRenderer>();//grab render
        asteroidC = GetComponent<AsteroidController>();

        InitPlanet();

        asteroidC.StartAstoridProcess();

        InitSpace();

    }

    void InitPlanet()
    {
      
        //create new mat based off base
        mat = new Material(rend.material);

        //create random colour
        Color tmpCol = Random.ColorHSV(0f, 1f, 1f, 1f, 0.0f, 1f);//make it so colour is reder when closer to a sun?

        //tmpCol = Color.black;


        float closestSunDist = float.MaxValue;
        float tmpDIst;
        GameObject[] suns = GameObject.FindGameObjectsWithTag("Sun");
        for (int i = 0; i < suns.Length; i++)
        {
            tmpDIst = Vector3.Distance(transform.position, suns[i].transform.position);
            if (tmpDIst < closestSunDist)
                closestSunDist = tmpDIst;

        }



        tmpCol.r = Mathf.InverseLerp(maxRedWeightDist, 0.0f, closestSunDist);
        tmpCol.b = Mathf.InverseLerp(maxRedWeightDist / 2, maxBlueWeight, closestSunDist);

        // tmpCol.a = 1;


        //  Debug.Log("Planet Colour: " + tmpCol.ToString());

        mat.color = tmpCol;//update mat colour
        rend.material = mat;//update material

    }

    void InitSpace()
    {
        

        if (asteroidC.GetNumAstroids() > 20)
        {

          salvageDrones = new List<GameObject>();    

            int numDrones = (int)((float)(asteroidC.GetNumAstroids()/30) * GetComponent<SphereCollider>().radius);

            Debug.Log("Number of Drones: " + numDrones + "Number of Astroids: " + asteroidC.GetNumAstroids());

            for (int i = 0; i < numDrones; i++)
            {
                Vector3 dronePos = transform.TransformPoint( Random.onUnitSphere * (GetComponent<SphereCollider>().radius + 1.56f));

                GameObject tmp = Instantiate(salvagingDronePref, dronePos, Quaternion.identity);

                salvageDrones.Add(tmp);
            }

            moveDrone = new bool[salvageDrones.Count];
            Debug.Log(moveDrone.Length);
            for(int i =0; i < moveDrone.Length; i++)
            {
                moveDrone[i] = true;
            }
        }


    }


    private void Update()
    {

        transform.Rotate((new Vector3(1, 1, 0) * rotationSpeed) * Time.deltaTime);

        if(salvageDrones != null)
        for(int i =0; i < salvageDrones.Count; i++)
        {

               
                
                if(moveDrone[i] == true)
                {
                    StartCoroutine(CheckToMoveDrone(i));
                   
                }

           
        }



    }

    IEnumerator CheckToMoveDrone(int index)
    {
        moveDrone[index] = false;
        Debug.Log("Moving Drone: " + index);
        yield return StartCoroutine(MoveDrone(index)); 

         moveDrone[index] = true;
    }

    IEnumerator MoveDrone(int index)
    {

        Vector3 endPosition = asteroidC.GetRandomAstroidPosition();
        Rigidbody rb = salvageDrones[index].GetComponent<Rigidbody>();

        bool destination = false;

        while (destination == false)
        {
            rb.MovePosition(Vector3.MoveTowards(salvageDrones[index].transform.position, endPosition, 0.5f));


            //if (salvageDrones[index].transform.position == endPosition)
            //    destination = true;

            if (Vector3.Distance(rb.transform.position,endPosition) <= 0.8f)
                destination = true;
            else
                yield return null;



        }

        // the drone waits for a period of time as it deals with its buisness
        yield return new WaitForSeconds(Random.Range(1, 4));

    }

}
	


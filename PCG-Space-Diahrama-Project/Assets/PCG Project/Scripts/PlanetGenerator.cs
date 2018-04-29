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
        //update value for this drone to ensure coroutine is not ran again.
        moveDrone[index] = false;
       // Debug.Log("Moving Drone: " + index);

        //call the coroutine for moving the drone
        yield return StartCoroutine(MoveDrone(index)); 

        //update value so a new destination can be set
         moveDrone[index] = true;
    }

    IEnumerator MoveDrone(int index)
    {
        //position for the drone to head to
        Vector3 endPosition = asteroidC.GetRandomAstroidPosition();

        //grab refrence of drones rigidbody
        Rigidbody rb = salvageDrones[index].GetComponent<Rigidbody>();

        
        bool destination = false;
        //while the drone has not reached its destination loop
        while (destination == false)
        {
            //move the drone

            Vector3 nexPositionStep = Vector3.MoveTowards(salvageDrones[index].transform.position, endPosition, 10.0f * Time.deltaTime);

            Vector3 direction  = (endPosition - salvageDrones[index].transform.position);

            float disPlanDrone = Vector3.Distance(salvageDrones[index].transform.position, transform.position);
           // Debug.Log("Distance between drone and Planet: " + disPlanDrone);
            if (disPlanDrone <= transform.lossyScale.x + 1.0f)
            {
               
                Vector3 dir = transform.position - salvageDrones[index].transform.position;

                direction = -dir ;// * (200.0f /*/ (disPlanDrone - transform.lossyScale.x) */);
            //    Debug.Log(direction);
            }

            salvageDrones[index].transform.LookAt(salvageDrones[index].transform.position - direction);

            //rb.MovePosition(nexPositionStep);
            // rb.velocity = (endPosition - salvageDrones[index].transform.position) * Time.deltaTime * 10.0f;
            //   rb.velocity = (direction *0.5f )  * Time.deltaTime * 7.0f;
            rb.AddForce((direction * 0.5f) * Time.deltaTime * 25.0f);
            //  Debug.Log("Velocity: " + rb.velocity);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 6);


            if (Vector3.Distance(rb.transform.position,endPosition) <= 0.6f)
                destination = true;
            else
                yield return null;


           

        }

        // the drone waits for a period of time as it deals with its buisness
        yield return new WaitForSeconds(Random.Range(1, 2));

    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x + 1.0f);
    }

}
	


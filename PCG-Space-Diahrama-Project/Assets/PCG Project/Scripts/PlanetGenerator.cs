using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AsteroidController))]
public class PlanetGenerator : MonoBehaviour
{

    public bool useTestSeed;
    public int testSeed;//seed used in the testing stage of single planets
    MeshRenderer rend;
    Material mat;

    AsteroidController asteroidC;
    PlanetTextureMapping planMapping;

    [SerializeField]
    private SolaSystemData sSystemData;

    public float maxRedWeightDist;
   // public float maxBlueWeight;
    public float rotationSpeed = 5.0f;



    public GameObject[] salvagingDronePref;

    private int droneTypeIndex = -1;

    [SerializeField]
    private Material[] droneOneMaterialOptions;

    private Material material = null;
    List<GameObject> salvageDrones;
    bool[] moveDrone;

    [Tooltip("0-100")]
    public int civilisationChance;
    bool hasCivilisation = false;


    

    void Start()
    {
        if(useTestSeed == true)
        Random.InitState(testSeed);
        

        rend = GetComponent<MeshRenderer>();//grab render
        asteroidC = GetComponent<AsteroidController>();
        planMapping = GetComponent<PlanetTextureMapping>();

        InitPlanet();

        asteroidC.StartAstoridProcess();

        
        int v = Random.Range(0,101);
        Debug.Log("Has Civilisation chance value " + v);
        if (v <= civilisationChance)
            hasCivilisation = true;

        InitSpace();


    }




    void InitPlanet()
    {
      
        //create new mat based off base
        mat = new Material(rend.material);

        //create random colour
        Color tmpCol = Random.ColorHSV(0f, 1f, 1f, 1f, 0.0f, 1f);//initial random colour

        Debug.Log("Base Planet Colour: " + tmpCol);

        float closestSunDist = float.MaxValue;//cap this float at max value so the first sun in list will always be smaller in distance
        float tmpDIst;
        GameObject[] suns = GameObject.FindGameObjectsWithTag("Sun");

        //check distance for everysun
        for (int i = 0; i < suns.Length; i++)
        {   //grab distance between planet and current sun
            tmpDIst = Vector3.Distance(transform.position, suns[i].transform.position);
            if (tmpDIst < closestSunDist)//if the distance is smaller than the current closest
                closestSunDist = tmpDIst;//update value

        }

    

        tmpCol.r = Mathf.InverseLerp(maxRedWeightDist, sSystemData.minPlanDist/2, closestSunDist);
        tmpCol.b = Mathf.InverseLerp(maxRedWeightDist / 2, sSystemData.maxPlanDist, closestSunDist);

        // tmpCol.a = 1;


        //  Debug.Log("Planet Colour: " + tmpCol.ToString());

        mat.color = tmpCol;//update mat colour
        rend.material = mat;//update material

    }

    void InitSpace()
    {
        
        if (hasCivilisation == false)
            return;




        if (asteroidC.GetNumAstroids() > 80)
        {

          salvageDrones = new List<GameObject>();    

            int numDrones = (int)((float)(asteroidC.GetNumAstroids()/60) * GetComponent<SphereCollider>().radius);

            Debug.Log("Number of Drones: " + numDrones + " Number of Astroids: " + asteroidC.GetNumAstroids());
            
            droneTypeIndex = Random.Range(0, salvagingDronePref.Length);
            Debug.Log("Drone Type: " + droneTypeIndex);

            switch (droneTypeIndex)
            {
                case 0:
                    material = droneOneMaterialOptions[Random.Range(0, droneOneMaterialOptions.Length)];
                    break;

            }


            for (int i = 0; i < numDrones; i++)
            {
                Vector3 dronePos = transform.TransformPoint( Random.onUnitSphere * (GetComponent<SphereCollider>().radius + 1.56f));

                GameObject tmp = Instantiate(salvagingDronePref[droneTypeIndex], dronePos, Quaternion.identity);

                if(material !=null)
                tmp.GetComponent<MeshRenderer>().material = material;

                salvageDrones.Add(tmp);
            }

            moveDrone = new bool[salvageDrones.Count];
           
            for(int i =0; i < moveDrone.Length; i++)
            {
                moveDrone[i] = true;
            }
        }





    }


    private void Update()
    {

        transform.Rotate((new Vector3(1, 1, 0) * rotationSpeed) * Time.deltaTime);

        if (salvageDrones != null)
        {

            for (int i = 0; i < salvageDrones.Count; i++)
            {


                if (moveDrone[i] == true)
                {
                    StartCoroutine(CheckToMoveDrone(i));

                }


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
        int asteroidIndex = asteroidC.GetRandomAstroidIndex();

        Vector3 endPosition = asteroidC.GetAstroidPosition(asteroidIndex);

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

           // salvageDrones[index].transform.LookAt(salvageDrones[index].transform.position - direction);

           // Quaternion.LookRotation(salvageDrones[index].transform.position - direction);


            salvageDrones[index].transform.rotation = Quaternion.RotateTowards(salvageDrones[index].transform.rotation, Quaternion.LookRotation(-direction),90*Time.deltaTime);

            //rb.MovePosition(nexPositionStep);
            // rb.velocity = (endPosition - salvageDrones[index].transform.position) * Time.deltaTime * 10.0f;
            //   rb.velocity = (direction *0.5f )  * Time.deltaTime * 7.0f;
            rb.AddForce((direction * 0.5f) * Time.deltaTime * 25.0f);
            //  Debug.Log("Velocity: " + rb.velocity);

            rb.velocity = Vector3.ClampMagnitude(rb.velocity, 4);


            if (Vector3.Distance(rb.transform.position, endPosition) <= 3.0f)
            {
                destination = true;

                rb.velocity = Vector3.zero;
            }
            else
                yield return null;

            endPosition = asteroidC.GetAstroidPosition(asteroidIndex);

        }
        

        // the drone waits for a period of time as it deals with its buisness
        yield return new WaitForSeconds(Random.Range(1, 2));

    }

    IEnumerator WaitToGenerateMaps()
    {
        yield return new WaitForSeconds(Random.Range(10,60));
        planMapping.Generate();

    }


    public int GenerateFullTexture()
    {
        if (planMapping == null)
            return -1;

        planMapping.Generate();

        return 0;
    }

    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, transform.lossyScale.x + 1.0f);
    //}

}
	


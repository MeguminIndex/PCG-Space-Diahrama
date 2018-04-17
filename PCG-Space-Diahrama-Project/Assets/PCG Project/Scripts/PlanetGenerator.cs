using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

    MeshRenderer rend;
    Material mat;

    public float maxRedWeightDist;
    public float maxBlueWeight;
   // public float 
   


    // Use this for initialization
    void Start()
    {
        rend = GetComponent<MeshRenderer>();//grab render

        //create new mat based off base
        mat = new Material(rend.material);

        //create random colour
        Color tmpCol= Random.ColorHSV(0f, 1f, 1f, 1f, 0.0f, 1f);//make it so colour is reder when closer to a sun?

        //tmpCol = Color.black;


        float closestSunDist = float.MaxValue;
        float tmpDIst;
        GameObject[] suns = GameObject.FindGameObjectsWithTag("Sun");
        for(int i =0; i < suns.Length; i++)
        {
            tmpDIst = Vector3.Distance(transform.position, suns[i].transform.position);
            if (tmpDIst < closestSunDist)
                closestSunDist = tmpDIst;
             
        }


      
        tmpCol.r = Mathf.InverseLerp(maxRedWeightDist, 0.0f, closestSunDist);
        tmpCol.b = Mathf.InverseLerp(maxRedWeightDist, maxBlueWeight, closestSunDist);
      
        tmpCol.a = 1;
      

        Debug.Log("Planet Colour: " + tmpCol.ToString());

        mat.color = tmpCol;//update mat colour
        rend.material = mat;//update material

    }

}
	


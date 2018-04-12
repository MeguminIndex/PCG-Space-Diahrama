using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{

    MeshRenderer rend;
    Material mat;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<MeshRenderer>();//grab render

        //create new mat based off base
        mat = new Material(rend.material);

        //create random colour
        Color tmpCol = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);//make it so colour is reder when closer to a sun?
        Debug.Log("Planet Colour: " + tmpCol.ToString());

        mat.color = tmpCol;//update mat colour
        rend.material = mat;//update material

    }

}
	


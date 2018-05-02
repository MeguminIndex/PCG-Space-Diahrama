using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour {

    public SolaSystemData SystemData;

    [SerializeField]
    private Material standard;
    [SerializeField]
    private Material ice;

    [SerializeField]
    private MeshRenderer[] meshes;

	// Use this for initialization
	void Start () {



        float mPlanDIst = SystemData.maxPlanDist;

        float v = (mPlanDIst / 100) * 30;
        if (v > 400)
            v = 400;
        mPlanDIst -= v;


        try
        {
            if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Sun").transform.position) > mPlanDIst)
            {
                for (int i = 0; i < meshes.Length; i++)
                    meshes[i].material = ice;
            }
            else
            {
                for (int i = 0; i < meshes.Length; i++)
                    meshes[i].material = standard;
            }
        }
		catch(Exception e)
        {
            for (int i = 0; i < meshes.Length; i++)
                meshes[i].material = standard;
        }
	}

    

}

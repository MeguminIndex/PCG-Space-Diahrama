using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SolaSystemData", menuName = "PCG-Diahrama/Data/SolaSystem", order = 1)]
public class SolaSystemData : ScriptableObject {


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



}

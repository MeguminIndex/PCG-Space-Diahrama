using System.Collections;
using System.Collections.Generic;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;
using System.IO;
using System;
using UnityEngine;
using System.Threading;

public enum NoiseType
{
    Perlin,
    Billow,
    RidgedMultifractal,
    Voronoi,
    Mix,
    Practice
};

public class PlanetTextureMapping : MonoBehaviour
{
    private Noise2D m_noiseMap = null;
    //public Texture2D[] m_textures = new Texture2D[4];

    Texture2D normalMap;
    Texture2D heighMap;
    Texture2D terrainMap;

    public int resolution = 64;

    public bool randomNoiseType;
    public NoiseType noise = NoiseType.Voronoi;
    //public float zoom = 1f;
    //public float offset = 0f;
    public float turbulence = 0f;
    public int perlinOctaves = 6;


    //used for spherical mapping
    float _west = -180;
    float _east = 180;
    float _north = -90;
    float _south = 90;



    Thread t1;

    
    public void Start()
    {
        seed = UnityEngine.Random.Range(0, 100);


        if(randomNoiseType == true)
        noise = (NoiseType)UnityEngine.Random.Range(0,6);

        // Generate();
       // Debug.Log("Noise Type: " + noise);
    }

    //public void OnGUI()
    //{
    //    int y = 0;
    //    foreach (string i in System.Enum.GetNames(typeof(NoiseType)))
    //    {
    //        if (GUI.Button(new Rect(0, y, 100, 20), i))
    //        {
    //            noise = (NoiseType)Enum.Parse(typeof(NoiseType), i);
    //            Generate();
    //        }
    //        y += 20;
    //    }


    //    frequency = double.Parse(GUI.TextField(new Rect(0, 120, 100, 20), frequency.ToString()));
    //    displacement = double.Parse(GUI.TextField(new Rect(0, 140, 100, 20), displacement.ToString()));
    //    resolution = int.Parse(GUI.TextField(new Rect(0, 160, 100, 20), resolution.ToString()));


    //    perlinOctaves = int.Parse(GUI.TextField(new Rect(0, 180, 100, 20), perlinOctaves.ToString()));
    //    turbulence = float.Parse(GUI.TextField(new Rect(0, 200, 100, 20), turbulence.ToString()));
    //    //zoom = float.Parse(GUI.TextField(new Rect(0, 220, 100, 20), zoom.ToString()));

    //}

    public double displacement = 4;
    public double frequency = 2;
    public int seed = 0;


    private void backgroundProcessing()
    {
        // Create the module network
        ModuleBase moduleBase;
        switch (noise)
        {
            case NoiseType.Billow:
                moduleBase = new Billow();
                break;

            case NoiseType.RidgedMultifractal:
                moduleBase = new RidgedMultifractal();
                break;

            case NoiseType.Voronoi:
                moduleBase = new Voronoi(frequency, displacement, seed, false);

                break;

            case NoiseType.Mix:
                Perlin perlin = new Perlin();
                var rigged = new RidgedMultifractal();
                moduleBase = new Add(perlin, rigged);
                break;

            case NoiseType.Practice:
                var bill = new Billow();
                bill.Frequency = frequency;
                moduleBase = new Turbulence(turbulence / 10, bill);
                break;



            default:
                var defPerlin = new Perlin();
                defPerlin.OctaveCount = perlinOctaves;
                moduleBase = defPerlin;

                break;

        }

        // Initialize the noise map
        this.m_noiseMap = new Noise2D(resolution, resolution, moduleBase);

        m_noiseMap.GenerateSpherical(_north, _south, _west, _east);

    }




    private IEnumerator CheckBackgroundThread()
    {
        t1 = new Thread(backgroundProcessing) { Name = "backgroundProcessing" };
        t1.Start();

        while(t1.IsAlive)
        {
            yield return null;
        }

        heighMap = this.m_noiseMap.GetTexture(GradientPresets.Grayscale);
        terrainMap = this.m_noiseMap.GetTexture(GradientPresets.Terrain);
        normalMap = this.m_noiseMap.GetNormalMap(3.0f);

        yield return null;

        //heighMap = this.m_noiseMap.GetTexture(GradientPresets.Grayscale);
        heighMap.Apply();
        //terrainMap = this.m_noiseMap.GetTexture(GradientPresets.Terrain);
        terrainMap.Apply();
        //normalMap = this.m_noiseMap.GetNormalMap(3.0f);
        normalMap.Apply();

        Renderer rend = GetComponent<Renderer>();
        rend.material.mainTexture = terrainMap;

        rend.material.EnableKeyword("_ParallaxMap");
        rend.material.SetTexture("_ParallaxMap", heighMap);

        rend.material.EnableKeyword("_BumpMap");
        rend.material.SetTexture("_BumpMap", normalMap);


        //Debug.Log($"Planet: {gameObject.name} Maps generated");

        normalMap = null;
        heighMap = null;
        terrainMap = null;
        m_noiseMap = null;

    }

    public void Generate()
    {
        StartCoroutine(CheckBackgroundThread());
       
    }


    //IEnumerator CoGen()
    //{
    //    // Create the module network
    //    ModuleBase moduleBase;
    //    switch (noise)
    //    {
    //        case NoiseType.Billow:
    //            moduleBase = new Billow();
    //            break;

    //        case NoiseType.RidgedMultifractal:
    //            moduleBase = new RidgedMultifractal();
    //            break;

    //        //case NoiseType.Voronoi:
    //        //    seed = UnityEngine.Random.Range(0, 100);
    //        //    moduleBase = new Voronoi(frequency, displacement, seed, false);

    //        //    break;

    //        //case NoiseType.Mix:
    //        //    Perlin perlin = new Perlin();
    //        //    var rigged = new RidgedMultifractal();
    //        //    moduleBase = new Add(perlin, rigged);
    //        //    break;

    //        case NoiseType.Practice:
    //            var bill = new Billow();
    //            bill.Frequency = frequency;
    //            moduleBase = new Turbulence(turbulence / 10, bill);


    //            break;



    //        default:
    //            var defPerlin = new Perlin();
    //            defPerlin.OctaveCount = perlinOctaves;
    //            moduleBase = defPerlin;

    //            break;

    //    }

    //    // Initialize the noise map
    //    this.m_noiseMap = new Noise2D(resolution, resolution, moduleBase);

    //    yield return null;

    //    m_noiseMap.GenerateSpherical(_north, _south, _west, _east);


    //    heighMap = this.m_noiseMap.GetTexture(GradientPresets.Grayscale);
    //    heighMap.Apply();

    //    yield return null;

    //    terrainMap = this.m_noiseMap.GetTexture(GradientPresets.Terrain);
    //    terrainMap.Apply();

    //    yield return null;

    //    normalMap = this.m_noiseMap.GetNormalMap(3.0f);
    //    normalMap.Apply();

    //    yield return null;

    //    //display on plane

    //    Renderer rend = GetComponent<Renderer>();
    //    rend.material.mainTexture = terrainMap;

    //    rend.material.EnableKeyword("_ParallaxMap");
    //    rend.material.SetTexture("_ParallaxMap", heighMap);

    //    rend.material.EnableKeyword("_BumpMap");
    //    rend.material.SetTexture("_BumpMap", normalMap);


    //    Debug.Log($"Planet: {gameObject.name} Maps generated");

    //    normalMap = null;
    //    heighMap = null;
    //    terrainMap = null;


    //}


}
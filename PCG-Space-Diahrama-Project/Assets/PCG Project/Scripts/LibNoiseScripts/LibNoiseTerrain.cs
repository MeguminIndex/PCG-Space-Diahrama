using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using LibNoise;
using LibNoise.Generator;

public class LibNoiseTerrain : MonoBehaviour {

    [SerializeField]
    Gradient _gradient = GradientPresets.RGB;

    [SerializeField]
    float _left = 2;

    [SerializeField]
    float _right = 6;

    [SerializeField]
    float _top = 1;

    [SerializeField]
    float _bottom = 5;

    [SerializeField]
    int _octaveCount = 1;

    [SerializeField]
    float _frecuency = 1;

    [SerializeField]
    float _persistence = 0.5f;


    public Texture2D image;

    void Start()
    {
        var perlin = new Perlin();

        perlin.OctaveCount = _octaveCount;
        perlin.Frequency = _frecuency;
        perlin.Persistence = _persistence;

        perlin.Seed = Random.Range(0, 100);
        // Unlike on the base LibNoise tutorial, we don't have a separate heightMap target
        // to set - we will instead build it after.  We also initialize the resulting size
        // on the constructor instead of passing a separate destination size.
        var heightMapBuilder = new Noise2D(256, 256, perlin);
        heightMapBuilder.GeneratePlanar(_left, _right, _top, _bottom);

        // Get the image
         image = heightMapBuilder.GetTexture(_gradient);

        // Set it. It may appear inverted from the example on the LibNoise site depending 
        // on the angle at which the object is rotated/viewed.

        Renderer rend = GetComponent<Renderer>();
        //rend.material.mainTexture = image;

        //rend.material.EnableKeyword("_ParallaxMap");
        //rend.material.SetTexture("_ParallaxMap", image);

        rend.material.EnableKeyword("_BumpMap");
        rend.material.SetTexture("_BumpMap", image);

        
        // We don't do the light changes for the texture, since that's beyond the scope of 
        // this port
    }
}

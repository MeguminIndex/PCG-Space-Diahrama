using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class GridScript : MonoBehaviour
{

    public int xSize, ySize;

    public Vector3[] vertices;

    //our refrence to mesh component
    private Mesh mesh;

    private void Awake()
    {
        Generate();
    }



    //slow down process
    private void Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(xSize + 1) * (ySize + 1)];//vector for verts
        Vector2[] uv = new Vector2[vertices.Length];//temp place to store our uvs before we pass to mesh componenet

        //tangents for normal maps
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                //   yield return wait;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;
        int[] triangles = new int[xSize * ySize * 6];

        //triangles[0] = 0;
        //triangles[1] = xSize + 1;
        //triangles[2] = 1;
        //triangles[3] = 1;
        //triangles[4] = xSize + 1;
        //triangles[5] = xSize + 2;

        //OR 

        //triangles[0] = 0;
        //triangles[3] = triangles[2] = 1;
        //triangles[4] = triangles[1] = xSize + 1;
        //triangles[5] = xSize + 2;


        for(int i =0, vi =0, y=0; y < ySize; y++, vi++)
        for (int x = 0; x < xSize; x++, i += 6, vi++)
        {
            triangles[i] = vi;
            triangles[i + 3] = triangles[i + 2] = vi + 1;
            triangles[i + 4] = triangles[i + 1] = vi + xSize + 1;
            triangles[i + 5] = vi + xSize + 2;
            mesh.triangles = triangles;

                //yield return wait;
        }


        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }


    //visulise the verticies in editor by using OnDrawGizmoes
    private void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(transform.TransformPoint(vertices[i]), 0.1f);
        }
    }

}
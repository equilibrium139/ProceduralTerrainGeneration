using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] int width = 100;
    [SerializeField] int length = 100;
    [SerializeField] float height = 2.0f;
    [SerializeField] float scale = 20.0f;
    //[SerializeField] bool pits = false;
    //[SerializeField] bool peaks = false;
    [SerializeField] bool hillSlope = false;
    List<Vector3> terrainVertices;
    List<int> terrainTriangles;
    Mesh terrainMesh;

    private void Awake()
    {
        terrainVertices = RandomGridPoints();
        terrainTriangles = CreateGridTriangles();
        terrainMesh = new Mesh();
        terrainMesh.Clear();
        terrainMesh.vertices = terrainVertices.ToArray();
        terrainMesh.triangles = terrainTriangles.ToArray();
        terrainMesh.RecalculateNormals();
        var meshComponent = GetComponent<MeshFilter>();
        meshComponent.mesh = terrainMesh;
    }

    List<Vector3> RandomGridPoints()
    {
        List<Vector3> points = new List<Vector3>();
        float offset = Random.Range(100.0f, 200.0f);

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //points.Add(new Vector3(x, RandomHeight2(x, z, offset, pits, peaks) * height, z));
                if (hillSlope )
                {
                    points.Add(new Vector3(x, RandomHeight(x, z, offset) * height * ((float)(x*z)/(width*length)), z));
                    Debug.Log(((float)(x + z) / (width + length))+ " x="+ x+ " z="+ z);
                }
                else
                {
                    points.Add(new Vector3(x, RandomHeight(x, z, offset) * height, z));
                }
            }
        }

        /*
        if (pits)
        {
            //int z = Random.Range(0, length);
            for (int z = 0; z < length; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    points.Add(new Vector3(x, RandomHeight2(x, z, offset, pits) * height , z));
                }
            }
        }
        else
        {
            for (int z = 0; z < length; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    points.Add(new Vector3(x, RandomHeight(x, z, offset) * height, z));
                }
            }
        }
        */
        /*
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                points.Add(new Vector3(x, RandomHeight(x, z, offset) * height, z));
            }
        }
        */

        return points;
    }

    float RandomHeight(int x, int z, float offset)
    {
        float xCoord = (float)x / width * scale;
        float zCoord = (float)z / width * scale;

        xCoord += offset;
        zCoord += offset;

        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    float RandomHeight2(int x, int z, float offset, bool pits, bool peaks)
    {
        float xCoord = (float)x / width * scale;
        float zCoord = (float)z / width * scale;

        xCoord += offset;
        zCoord += offset;

        if (pits)
        {
            return Mathf.PerlinNoise(xCoord, zCoord) * -1;
        }
        else if (peaks)
        {
            float peakOffset = Random.Range(400.0f, 600.0f);
            bool apply = (Random.value > 0.5f);
            if (apply)
            {
                xCoord += peakOffset;
                zCoord += peakOffset;
            }
        }
        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    List<int> CreateGridTriangles()
    {
        List<int> triangles = new List<int>();

        for (int z = 0; z < length - 1; z++)
        {
            for (int x = 0; x < width - 1; x++)
            {
                int bottomLeft = x + z * length;
                int bottomRight = bottomLeft + 1;
                int topLeft = x + (z + 1) * length;
                int topRight = topLeft + 1;

                triangles.Add(bottomLeft);
                triangles.Add(topRight);
                triangles.Add(bottomRight);
                triangles.Add(bottomLeft);
                triangles.Add(topLeft);
                triangles.Add(topRight);
            }
        }

        return triangles;
    }
}

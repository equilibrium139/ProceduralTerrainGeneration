using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] int width = 10;
    [SerializeField] int length = 10;
    [SerializeField] float spacing = 1.0f;
    [SerializeField] float height = 2.0f;

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

        float offset = Random.RandomRange(100.0f, 200.0f);


        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                points.Add(new Vector3(x, RandomHeight(x, z, offset) * height, z));
            }
        }

        return points;
    }

    float RandomHeight(int x, int z, float offset)
    {
        float xCoord = (float)x / width;
        float zCoord = (float)z / width;

        xCoord += offset;
        zCoord += offset;

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

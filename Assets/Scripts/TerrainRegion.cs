using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRegion : MonoBehaviour
{
    public Vector2Int bottomLeft;
    public int width, length;
    Mesh mesh;
    List<Vector3> vertices;
    List<int> triangles;

    public void GenerateMesh(List<TerrainRegion> neighbors, bool hillSlope, float height, float scale)
    {
        vertices = RandomGridPoints(hillSlope, height, scale);

        foreach (TerrainRegion neighbor in neighbors)
        {
            // Left neighbor
            if (neighbor.bottomLeft.x < bottomLeft.x)
            {
                // Copy right border vertices
                for (int i = 0; i < length; i++)
                {
                    vertices[i * width] = neighbor.vertices[i * width + width - 1];
                }
            }
            // Right neighbor
            else if (neighbor.bottomLeft.x > bottomLeft.x)
            {
                // Copy left border vertices
                for (int i = 0; i < length; i++)
                {
                    var v1 = vertices[i * width + width - 1];
                    var v2 = neighbor.vertices[i * width];
                    if (v1.x != v2.x || v1.z != v2.z)
                    {
                        Debug.Log("Oh shit");
                    }
                    vertices[i * width + width - 1] = neighbor.vertices[i * width];
                }
            }
            // Up neighbor
            else if (neighbor.bottomLeft.y > bottomLeft.y)
            {
                // Copy bottom border vertices
                for (int i = 0; i < width; i++)
                {
                    vertices[(length - 1) * width + i] = neighbor.vertices[i];
                }
            }
            // Down neighbor
            else if (neighbor.bottomLeft.y < bottomLeft.y)
            {
                // Copy top border vertices
                for (int i = 0; i < width; i++)
                {
                    vertices[i] = neighbor.vertices[(length - 1) * width + i];
                }
            }
        }

        triangles = CreateGridTriangles();

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
        var meshComponent = GetComponent<MeshFilter>();
        meshComponent.mesh = mesh;
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

    float RandomHeight(int x, int z, float offset, float scale)
    {
        float xCoord = (float)x / width * scale;
        float zCoord = (float)z / length * scale;

        xCoord += offset;
        zCoord += offset;

        return Mathf.PerlinNoise(xCoord, zCoord);
    }

    List<Vector3> RandomGridPoints(bool hillSlope, float height, float scale)
    {
        List<Vector3> points = new List<Vector3>();
        float offset = Random.Range(100.0f, 200.0f);

        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                //points.Add(new Vector3(x, RandomHeight2(x, z, offset, pits, peaks) * height, z));
                if (hillSlope)
                {
                    points.Add(new Vector3(x + bottomLeft.x, RandomHeight(x, z, offset, scale) * height * ((float)(x * z) / (width * length)), z + bottomLeft.y));
                    Debug.Log(((float)(x + z) / (width + length)) + " x=" + x + " z=" + z);
                }
                else
                {
                    points.Add(new Vector3(x + bottomLeft.x, RandomHeight(x, z, offset, scale) * height, z + bottomLeft.y));
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

    public bool PointInRegion(Vector3 point)
    {
        return point.x >= bottomLeft.x && point.x <= bottomLeft.x + width
            && point.z >= bottomLeft.y && point.z <= bottomLeft.y + length;
    }
}

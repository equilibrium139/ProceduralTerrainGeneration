using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTerrain : MonoBehaviour
{
    [SerializeField] int width = 50;
    [SerializeField] int length = 50;
    [SerializeField] float height = 2.0f;
    [SerializeField] float scale = 20.0f;
    //[SerializeField] bool pits = false;
    //[SerializeField] bool peaks = false;
    [SerializeField] bool hillSlope = false;
    List<TerrainRegion> regions;

    private void Start()
    {
        GenerateInitialRegions();
    }

    private void GenerateInitialRegions()
    {
        GameObject region1 = new GameObject();
        region1.name = "Bottom left";
        region1.transform.parent = transform;
        var region1Component = region1.AddComponent<TerrainRegion>();
        region1.AddComponent<MeshFilter>();
        region1.AddComponent<MeshRenderer>();
        region1Component.bottomLeft = new Vector2Int(0, 0);
        region1Component.width = width;
        region1Component.length = length;
        region1Component.GenerateMesh(new List<TerrainRegion>(), hillSlope, height, scale);

        GameObject region2 = new GameObject();
        region2.transform.parent = transform;
        region2.name = "Bottom right";
        var region2Component = region2.AddComponent<TerrainRegion>();
        region2.AddComponent<MeshFilter>();
        region2.AddComponent<MeshRenderer>();
        region2Component.bottomLeft = new Vector2Int(width, 0);
        region2Component.width = width;
        region2Component.length = length;
        region2Component.GenerateMesh(new List<TerrainRegion> { region1Component }, hillSlope, height, scale);

        GameObject region3 = new GameObject();
        region3.transform.parent = transform;
        region3.name = "Top Right";
        var region3Component = region3.AddComponent<TerrainRegion>();
        region3.AddComponent<MeshFilter>();
        region3.AddComponent<MeshRenderer>();
        region3Component.bottomLeft = new Vector2Int(width, length);
        region3Component.width = width;
        region3Component.length = length;
        region3Component.GenerateMesh(new List<TerrainRegion> { region2Component }, hillSlope, height, scale);

        GameObject region4 = new GameObject();
        region4.name = "Top left";
        region4.transform.parent = transform;
        var region4Component = region4.AddComponent<TerrainRegion>();
        region4.AddComponent<MeshFilter>();
        region4.AddComponent<MeshRenderer>();
        region4Component.bottomLeft = new Vector2Int(0, length);
        region4Component.width = width;
        region4Component.length = length;
        region4Component.GenerateMesh(new List<TerrainRegion> { region1Component, region3Component }, hillSlope, height, scale);
    }
}

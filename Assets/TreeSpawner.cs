using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [Range(0,1000000)]
    public int treeAmount = 5;

    public int blockSize = 50;

    public bool removeOldTrees = true;

    public int treeToSpawn = 8;

    public IEnumerator TreeSpawn() 
    {
        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;

        if (removeOldTrees) 
        {
            //Delete all trees
            print("Removing Trees");
            yield return null;
            TreeInstance[] empty = new TreeInstance[0];
            terrainData.treeInstances = empty;
            terrain.Flush();
            print("Trees Removed");
            yield return null;
        }

        print("Spawning new trees...");
        yield return null;

        //Spawn new Trees
        float x;
        float z;
        TreeInstance treeInstance;
        for (int treeNumBlock = 0; treeNumBlock < treeAmount; treeNumBlock+=blockSize)
        {
            var minOuter = Mathf.Min(treeAmount, treeNumBlock + blockSize);
            for (int treeNum = treeNumBlock; treeNum < minOuter; treeNum++) 
            {
                x = Random.value;
                z = Random.value;
                treeInstance = new TreeInstance
                {
                    prototypeIndex = treeToSpawn,
                    color = Color.white,
                    heightScale = 1,
                    widthScale = 1,
                    position = (Vector3.right * x) + (Vector3.up * terrain.terrainData.GetInterpolatedHeight(x, z)) + (Vector3.forward * z)
                };
                terrain.AddTreeInstance(treeInstance);

                //print(string.Format("tree {0} placed", treeNum));
                //yield return null;
                //print("Updating Terrain...");
                //yield return null;
                //terrain.Flush();
                //print("Terrain Updated!");
                //yield return null;
            }
            //print("Block Complete!");
            //yield return null;
        }

        print("Tree Spawning Complete!");
        yield return null;
    }

    public IEnumerator TreeSpawnDetailObjects() 
    {
        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;

        //if (removeOldTrees)
        //{
        //    //Delete all trees
        //    print("Removing Trees");
        //    yield return null;
        //    TreeInstance[] empty = new TreeInstance[0];
        //    terrainData.treeInstances = empty;
        //    terrain.Flush();
        //    print("Trees Removed");
        //    yield return null;
        //}

        print("Spawning new trees...");
        yield return null;


        int width = terrainData.detailWidth;
        int depth = terrainData.detailHeight;

        int[,] map = new int [terrainData.detailWidth, terrainData.detailHeight];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                map[x, y] = 0;//(int)(Random.value * 256);
            }
        }

        //for (int treeNumBlock = 0; treeNumBlock < width; treeNumBlock += blockSize)
        //{
        //    var minOuter = Mathf.Min(width, treeNumBlock + blockSize);
        //    for (int x = treeNumBlock; x < minOuter; x++)
        //    {
        //        for (int i = 0; i < length; i++)
        //        {

        //        }
        //        var value = Random.value;


        //    }
        //    //print("Block Complete!");
        //    //yield return null;
        //}

        terrainData.SetDetailLayer(0, 0, treeToSpawn, map);
        print("Tree Spawning Complete!");
        yield return null;
    }

    [ContextMenu("Spawn Trees")]
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(TreeSpawn());
        StartCoroutine(TreeSpawnDetailObjects());
    }
}

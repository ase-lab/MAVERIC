using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terrain))]
public class TreeSpawner : MonoBehaviour
{
    #region Deprecated Properties
    ////[Range(0,1000000)]
    ////public int treeAmount = 5;

    //[Range(0,1)]
    //public float treeDensity = 0;

    //public int blockSize = 50;

    //public bool removeOldTrees = true;
    #endregion

    //Under details in Terrain component, the list is left to right top to bottom zero indexed.
    public int treeToSpawn = 8;

    public IEnumerator TreeSpawnDetailObjects() 
    {
        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;

        print("Spawning new trees...");
        yield return null;

        int width = terrainData.detailWidth;
        int depth = terrainData.detailHeight; //so not to be confused with upwards height.

        int[,] map = new int [width, depth];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < depth; y++)
            {
                map[x, y] = (int)(Mathf.PerlinNoise(x*.5f, y*.5f) * 2f);// (int)(Random.Range(0,2)); //The density of this detail at x,y. 0 being non 256 being full.
            }
        }

        terrainData.SetDetailLayer(0, 0, treeToSpawn, map);
        print("Tree Spawning Complete!");
        yield return null;
    }

    [ContextMenu("Spawn Trees")] //Allows you to run again without restarting
    void Start()
    {
        //Methods are coroutines to prevent Unity from crashing
        //This way we can let a frame go by to update dev on how tree spawning is progressing
        //and stop the editor from freezing (very nice quality of life). 

        //Originally trees were spawned as trees, but we can't take advantage of procedural instancing like that.
        //StartCoroutine(TreeSpawn());
        StartCoroutine(TreeSpawnDetailObjects());
    }


    //public IEnumerator TreeSpawn() 
    //{
    //    // Get the attached terrain component
    //    Terrain terrain = GetComponent<Terrain>();

    //    // Get a reference to the terrain data
    //    TerrainData terrainData = terrain.terrainData;

    //    if (removeOldTrees) 
    //    {
    //        //Delete all trees
    //        print("Removing Trees");
    //        yield return null;
    //        TreeInstance[] empty = new TreeInstance[0];
    //        terrainData.treeInstances = empty;
    //        terrain.Flush();
    //        print("Trees Removed");
    //        yield return null;
    //    }

    //    print("Spawning new trees...");
    //    yield return null;

    //    //Spawn new Trees
    //    float x;
    //    float z;
    //    TreeInstance treeInstance;
    //    for (int treeNumBlock = 0; treeNumBlock < treeAmount; treeNumBlock+=blockSize)
    //    {
    //        var minOuter = Mathf.Min(treeAmount, treeNumBlock + blockSize);
    //        for (int treeNum = treeNumBlock; treeNum < minOuter; treeNum++) 
    //        {
    //            x = Random.value;
    //            z = Random.value;
    //            treeInstance = new TreeInstance
    //            {
    //                prototypeIndex = treeToSpawn,
    //                color = Color.white,
    //                heightScale = 1,
    //                widthScale = 1,
    //                position = (Vector3.right * x) + (Vector3.up * terrain.terrainData.GetInterpolatedHeight(x, z)) + (Vector3.forward * z)
    //            };
    //            terrain.AddTreeInstance(treeInstance);

    //            //print(string.Format("tree {0} placed", treeNum));
    //            //yield return null;
    //            //print("Updating Terrain...");
    //            //yield return null;
    //            //terrain.Flush();
    //            //print("Terrain Updated!");
    //            //yield return null;
    //        }
    //        //print("Block Complete!");
    //        //yield return null;
    //    }

    //    print("Tree Spawning Complete!");
    //    yield return null;
    //}
}

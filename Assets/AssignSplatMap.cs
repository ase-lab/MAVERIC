﻿using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array

[RequireComponent(typeof(Terrain))]
// This script textures the terrain
public class AssignSplatMap : MonoBehaviour
{
    [ContextMenu("Update Terrain")]  //Allows you to run again without restarting
    void Start()
    {
        // Get the attached terrain component
        Terrain terrain = GetComponent<Terrain>();

        // Get a reference to the terrain data
        TerrainData terrainData = terrain.terrainData;

        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];

        for (int y = 0; y < terrainData.alphamapHeight; y++)
        {
            for (int x = 0; x < terrainData.alphamapWidth; x++)
            {
                // Normalise x/y coordinates to range 0-1 
                float y_01 = (float)y / (float)terrainData.alphamapHeight;
                float x_01 = (float)x / (float)terrainData.alphamapWidth;

                // Note: 'heightmapHeight' is depracated it asks to update to the float 'heightmapResolution' instead. 
                // It asks you to update 'heightmapWidth' to 'heightmapResolution' also. 
                // This does not make sense as height and width are not neccesarily the same.
                // Therefore do not update API when opening the project.

                // Sample the height at this location (note GetHeight expects int coordinates corresponding to locations in the heightmap array)
                float height = terrainData.GetHeight(Mathf.RoundToInt(y_01 * terrainData.heightmapHeight), Mathf.RoundToInt(x_01 * terrainData.heightmapWidth));

                // Calculate the normal of the terrain (note this is in normalised coordinates relative to the overall terrain dimensions)
                Vector3 normal = terrainData.GetInterpolatedNormal(y_01, x_01);

                // Calculate the steepness of the terrain
                float steepness = terrainData.GetSteepness(y_01, x_01);

                // Setup an array to record the mix of texture weights at this point
                float[] splatWeights = new float[terrainData.alphamapLayers];

                // CHANGE THE RULES BELOW TO SET THE WEIGHTS OF EACH TEXTURE ON WHATEVER RULES YOU WANT

                //You'll note a lot of perlin noise, many textures with different scales and noise are used to hide texture tiling.

                //This expression makes a smooth fade from 1 to 0 around when height hits ~2600m
                var heightFadeOut = (1f - (1 / (1 + Mathf.Exp(-0.005f * (height - 2600)))));

                // Aerial_Grass_Rocks uses perlin noise and does not appear on higher terrain
                splatWeights[0] = Mathf.PerlinNoise(y_01 * 250f + 167, x_01 * 250f + 140f) * heightFadeOut;

                // Ground 37 uses perlin noise and does not appear on higher terrain
                splatWeights[1] = Mathf.PerlinNoise(y_01 * 231f - 26f, x_01 * 231f - 26f) * heightFadeOut;

                // Rocks stronger on steeper terrain
                // Note "steepness" is unbounded, so we "normalise" it by dividing by the extent of heightmap height and scale factor
                splatWeights[2] = (Mathf.Clamp01(steepness * steepness / (terrainData.heightmapHeight / 5.0f))) * .2f * heightFadeOut;

                // Mossy Ground increases with height but only on surfaces facing positive Z axis. it's weighted 10% since this texture is so strong
                splatWeights[3] = (height/3600f * (Mathf.Clamp01(normal.z))) * .1f * Mathf.PerlinNoise(y_01 * 340f, x_01 * 340f) * heightFadeOut;

                //Mulchy ground uses perlin noise and does not appear on higher terrain
                splatWeights[4] = Mathf.PerlinNoise(y_01 * 200f, x_01 * 200f) * heightFadeOut;// Mathf.PerlinNoise(y_01/60f, x_01/60f);

                //Snow appears around 2050m or higher. The trees are set to do the same.
                splatWeights[5] = 1/(.9f+Mathf.Exp(-0.005f*(height-2050)));

                //Dirt and light grass uses perlin noise and does not appear on higher terrain
                splatWeights[6] = Mathf.PerlinNoise(y_01 * 290f + 100f, x_01 * 290f + 50f) * heightFadeOut;

                //light dry Dirt and grass patches uses perlin noise and does not appear on higher terrain
                splatWeights[7] = Mathf.PerlinNoise(y_01 * 192f + 500f, x_01 * 192f + 500f) * heightFadeOut;

                // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                float z = splatWeights.Sum();

                // Loop through each terrain texture
                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    // Normalize so that sum of all texture weights = 1
                    splatWeights[i] /= z;

                    // Assign this point to the splatmap array
                    splatmapData[x, y, i] = splatWeights[i];
                }
            }
        }

        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}
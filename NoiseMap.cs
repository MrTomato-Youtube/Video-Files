﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{
    public int size, scale;
    private Renderer Renderer;
    private Color[] Col;
    private Texture2D tex;
    public float FalloffDensity;
    private float[] fall, perlin;

    private void Start()
    {
        Renderer = this.gameObject.GetComponent<Renderer>();
        Col = new Color[size * size];
        fall = new float[size * size];
        perlin = new float[size * size];
        tex = new Texture2D(size, size);
        Renderer.material.mainTexture = tex;
        GenerateFallOffMap();
        GeneratePerlinNoiseMap();
        Invoke("GenerateFinalIsland", 1);


    }

    void GenerateFallOffMap()
    {
        Vector2 center = new Vector2(size / 2, size / 2);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center) / FalloffDensity;
                fall[x * size + y] = dist;
                //Col[x * size + y] = new Color(dist, dist, dist);
            }
        }
        //tex.SetPixels(Col);
        //tex.Apply();
    }

    void GeneratePerlinNoiseMap()
    {
        float x = 0.0f;
        while(x < size)
        {
            float y = 0.0f;
            while(y < size)
            {
                float xCoord = x / size * scale;
                float yCoord = y / size * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                perlin[(int)x * size + (int)y] = sample;
                //Col[(int)x * size + (int)y] = new Color(sample, sample, sample);
                y++;
            }
            x++;
        }
        //tex.SetPixels(Col);
        //tex.Apply();
    }

    void GenerateFinalIsland()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float sample = perlin[x * size + y] - fall[x * size + y];
                Col[x * size + y] = new Color(sample, sample, sample) * UnityEngine.Random.ColorHSV();
            }
        }
        tex.SetPixels(Col);
        tex.Apply();
    }
}

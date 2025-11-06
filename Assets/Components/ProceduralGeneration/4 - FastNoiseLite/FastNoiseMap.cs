using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Fast Noise Map")]
    public class FastNoiseMap : ProceduralGenerationMethod
    {
        [Header("Global Data")]
        [SerializeField] private FastNoiseLite.NoiseType noiseType;
        [SerializeField, UnityEngine.Range(0.001f, 1f)] private float frequency = 0.001f;
        [SerializeField, UnityEngine.Range(0.001f, 1f)] private float amplitude = 0.001f;

        [Header("Fractal Data")]
        [SerializeField] private FastNoiseLite.FractalType fractalType = FastNoiseLite.FractalType.None;
        [SerializeField] private int Octaves;
        [SerializeField] private float Lacunarity = 0.001f;

        [Header("Tile Depth")]
        [SerializeField, UnityEngine.Range( -1f, 1f)] private float WaterHeight = -0.33f;
        [SerializeField, UnityEngine.Range(-1f, 1f)] private float SandHeight = 0.33f;
        [SerializeField, UnityEngine.Range(-1f, 1f)] private float GrassHeight = 1f;

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            FastNoiseLite noise = new FastNoiseLite(RandomService.Seed);

            InitializeNoise(noise);

            // Height Map
            float[,] noiseData = new float[Grid.Width, Grid.Lenght];

            for (int x = 0; x < Grid.Lenght; x++)
            {
                for (int y = 0; y < Grid.Width; y++)
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    noiseData[x, y] = noise.GetNoise(x, y);

                    if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                    {
                        float height = noise.GetNoise(x, y);

                        string tileName;

                        if (height <= WaterHeight)
                        {
                            tileName = WATER_TILE_NAME;
                        }
                        else if (height > WaterHeight && height <= SandHeight)
                        {
                            tileName = SAND_TILE_NAME;
                        }
                        else
                        {
                            tileName = GRASS_TILE_NAME;
                        }

                        AddTileToCell(cell, tileName, true);
                    }
                }
            }
        }

        void InitializeNoise(FastNoiseLite noise)
        {
            //Setting Global Data
            noise.SetNoiseType(noiseType);
            noise.SetFrequency(frequency);

            //Setting Fractal
            noise.SetFractalType(fractalType);
            noise.SetFractalOctaves(Octaves);
            noise.SetFractalLacunarity(Lacunarity);
        }

        void CreateTile(int x, int y, string type)
        {
            if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
            {
                AddTileToCell(cell, type, true);
            }
        }

        float GetNoiseData(int x, int y, FastNoiseLite noise)
        {
            var amp = noise.GetNoise(x, y) * amplitude;

            return Math.Clamp(amp, -1f, 1f);
        }
    }
}
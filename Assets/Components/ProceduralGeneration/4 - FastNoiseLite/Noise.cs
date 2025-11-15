using Cysharp.Threading.Tasks;
using Components.ProceduralGeneration;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using VTools.Grid;

[CreateAssetMenu(menuName = "Procedural Generation Method/Noise")]
public class Noise : ProceduralGenerationMethod
{
    [Header ("Noise Parameters")]
    [SerializeField, Range(0.01f, 0.2f), Tooltip("Noise frequency")] private float _frequency = 0.1f;
    [SerializeField, Range(0, 100), Tooltip("Noise Density")] private int _noiseDensity = 60;
    [SerializeField, Tooltip("Noise type")] private FastNoiseLite.NoiseType _noiseType = FastNoiseLite.NoiseType.OpenSimplex2S;
    [SerializeField, Tooltip("Noise rotation type")] private FastNoiseLite.RotationType3D _rotationType = FastNoiseLite.RotationType3D.ImproveXZPlanes;
    [SerializeField, Tooltip("Noise cellular distance function")] private FastNoiseLite.CellularDistanceFunction _cellularDistance = FastNoiseLite.CellularDistanceFunction.Euclidean;
    [SerializeField, Range(0.0f, 6.0f), Tooltip("Noise cell jitter")] private float _cellJitter = 2.0f;
    [SerializeField, Range(0.0f, 6.0f), Tooltip("Noise pingpongStrength")] private float _pingpongStrength = 2.0f;
    [SerializeField, Range(0, 6), Tooltip("Noise amplitude")] private float _amplitude = 0.5f;

    [Header ("Fractal Parameters")]
    [SerializeField, Range(1, 5), Tooltip("Noise fractal Octave")] private int _fracOctave = 3;
    [SerializeField, Range(0, 20), Tooltip("Noise fractal Lacunarity")] private int _fracLacunarity = 3;
    [SerializeField, Range(0, 3), Tooltip("Noise fractal Gain")] private int _fracGain = 3;
    [SerializeField, Range(-3, 6), Tooltip("Noise fractal WeightedStrength")] private int _fracWeightedStrength = 3;

    [Header ("Quantity Parameters")]
    [SerializeField, Range(-1.0f, 1.0f), Tooltip("water proportion")] private float _waterQuantity = 0.3f;
    [SerializeField, Range(-1.0f, 1.0f), Tooltip("sand proportion")] private float _sandQuantity = 0.3f;
    [SerializeField, Range(-1.0f, 1.0f), Tooltip("Grass proportion")] private float _grassQuantity = 0.3f;
    //[SerializeField, Range(-1.0f, 1.0f), Tooltip("sand proportion")] private float _treeQuantity = 0.3f;

    private FastNoiseLite noiseLite;
    private Dictionary<Vector2Int, string> cellsByTileName = new Dictionary<Vector2Int, string>();
    float[,] noiseData;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        noiseData = new float[Grid.Width, Grid.Lenght];

        InitNoise();

        for (int x = 0; x < Grid.Lenght; x++)
        {
            for (int y = 0; y < Grid.Width; y++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                {
                    float height = noiseLite.GetNoise(x, y);

                    string tileName;

                    if (height <= _waterQuantity)
                    {
                        tileName = WATER_TILE_NAME;
                    }
                    else if (height > _waterQuantity && height <= _sandQuantity)
                    {
                        tileName = SAND_TILE_NAME;
                    }
                    else
                    {
                        tileName = GRASS_TILE_NAME;
                    }

                    AddTileToCell(cell, tileName, false);
                }
            }

            await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
        }
        
    }
    private void InitNoise()
    {
        RandomSeed();
        noiseLite = new FastNoiseLite();
        noiseLite.SetNoiseType(_noiseType);
        noiseLite.SetRotationType3D(_rotationType);       
        noiseLite.SetFrequency(_frequency);
        noiseLite.SetFractalType(FastNoiseLite.FractalType.Ridged);
        noiseLite.SetFractalOctaves(_fracOctave);
        noiseLite.SetFractalLacunarity(_fracLacunarity);
        noiseLite.SetFractalGain(_fracGain);
        noiseLite.SetFractalWeightedStrength(_fracWeightedStrength);
        noiseLite.SetCellularDistanceFunction(_cellularDistance);
        noiseLite.SetCellularJitter(_cellJitter);
        noiseLite.SetFractalPingPongStrength(_pingpongStrength);
    }

    private void RandomSeed()
    {
        int r_seed = RandomService.Range(0, 10000);

        noiseLite.SetSeed(r_seed);
    }

    private float GetNoiseData(FastNoiseLite noise, int x, int y)
    {
        var amplifiedNoise = noise.GetNoise(x, y) * _amplitude;

        return Mathf.Clamp(amplifiedNoise, -1.0f, 1.0f);
    }

    private float Get01NoiseData(FastNoiseLite noise, int x, int y)
    {
        return (GetNoiseData(noise, x, y) + 1.0f) / 2.0f;
    }

    private void MakeMap(KeyValuePair<Vector2Int, string> cell)
    {
        if (Grid.TryGetCellByCoordinates(cell.Key.x, cell.Key.y, out Cell actualCell))
        {
            AddTileToCell(actualCell, cell.Value, false);
        }
    }
    
}

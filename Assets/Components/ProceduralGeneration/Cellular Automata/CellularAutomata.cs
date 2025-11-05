using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
    public class CellularAutomata : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _noiseDensity = 50;

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            GeneratePixel();

            for (int i = 0; i < _maxSteps; i++) 
            {
                cancellationToken.ThrowIfCancellationRequested();

                ////Ste i de l'algo
                //if (Grid.TryGetCellByCoordinates(10, 10, out Cell cell))
                //{
                //    if (cell.GridObject.Template.Name == GRASS_TILE_NAME)
                //    {
                //        //C'est de l'herbe en 10 10

                //    }
                //}

                

                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken:  cancellationToken);
            }
        }

        private void GeneratePixel()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                    {
                        string type = (RandomService.Range(0, 100 + 1) <= _noiseDensity) ? WATER_TILE_NAME : GRASS_TILE_NAME;

                        AddTileToCell(cell, type, true);
                    }

                }
            }
        }

        private void DetectTypeCell()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                    {
                        

                    }

                }
            }
        }


    }
}
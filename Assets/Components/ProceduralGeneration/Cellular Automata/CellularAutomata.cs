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

        private static readonly (int dx, int dy)[] directions = new (int, int)[]
        {
            (0, 0),   // center
            (1, 0),   // right
            (-1, 0),  // left
            (0, 1),   // up
            (0, -1),  // down
            (1, 1),   // top-right
            (-1, 1),  // top-left
            (1, -1),  // bottom-right
            (-1, -1), // bottom-left
        };

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            GeneratePixel();

            for (int i = 0; i < _maxSteps; i++) 
            {
                cancellationToken.ThrowIfCancellationRequested();

                ChangeTypeCell();

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

        private string DetectTypeCell(int x, int y, Cell cell)
        {
            int grassCounter = 0;
            int waterCounter = 0;

            //Center
            if (Grid.TryGetCellByCoordinates(x, y, out Cell centerCell))
            {
                if (centerCell.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (centerCell.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Right
            if (Grid.TryGetCellByCoordinates(x, y + 1, out Cell cellUp))
            {
                if (cellUp.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellUp.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Left
            if (Grid.TryGetCellByCoordinates(x, y - 1, out Cell cellDown))
            {
                if (cellDown.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellDown.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Top
            if (Grid.TryGetCellByCoordinates(x + 1, y, out Cell cellRight))
            {
                if (cellRight.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellRight.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Bottom
            if (Grid.TryGetCellByCoordinates(x - 1, y, out Cell cellLeft))
            {
                if (cellLeft.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellLeft.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }

            //Top-Right
            if (Grid.TryGetCellByCoordinates(x + 1, y + 1, out Cell cellTopRight))
            {
                if (cellTopRight.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellTopRight.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Top-Left
            if (Grid.TryGetCellByCoordinates(x - 1, y + 1, out Cell cellTopLeft))
            {
                if (cellTopLeft.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellTopLeft.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Bottom-Right
            if (Grid.TryGetCellByCoordinates(x + 1, y - 1, out Cell cellBottomRight))
            {
                if (cellBottomRight.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellBottomRight.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }
            //Bottom-Left
            if (Grid.TryGetCellByCoordinates(x - 1, y - 1, out Cell cellBottomLeft))
            {
                if (cellBottomLeft.GridObject.Template.Name == GRASS_TILE_NAME)
                {
                    grassCounter++;
                }
                if (cellBottomLeft.GridObject.Template.Name == WATER_TILE_NAME)
                {
                    waterCounter++;
                }
            }

            //Decide type of cell
            if (grassCounter > waterCounter)
            {
                    return GRASS_TILE_NAME;
            }
            else
            {
                return WATER_TILE_NAME;
            }
        }

        private void ChangeTypeCell()
        {
            for (int y = 0; y < 64; y++)
            {
                for (int x = 0; x < 64; x++)
                {
                    // Récupérer la cellule avant de détecter son type
                    if (Grid.TryGetCellByCoordinates(x, y, out Cell cell))
                    {
                        // Détecter le type de la cellule
                        string detectedType = DetectTypeCell(x, y, cell);

                        // Changer le type de la cellule selon le résultat
                        if (detectedType == GRASS_TILE_NAME)
                        {
                            AddTileToCell(cell, GRASS_TILE_NAME, true);
                        }
                        else if (detectedType == WATER_TILE_NAME)
                        {
                            AddTileToCell(cell, WATER_TILE_NAME, true);
                        }
                    }
                }
            }                    
        }

        private string DetectTypeCell2(int x, int y, Cell cell)
        {
            int grassCounter = 0;
            int waterCounter = 0;

            foreach (var (dx, dy) in directions)
            {
                if (Grid.TryGetCellByCoordinates(x + dx, y + dy, out Cell neighborCell))
                {
                    string name = neighborCell.GridObject.Template.Name;

                    if (name == GRASS_TILE_NAME)
                    {
                        grassCounter++;
                    }
                    else if (name == WATER_TILE_NAME)
                    {
                        waterCounter++;
                    }
                }

            }
            return grassCounter > waterCounter ? GRASS_TILE_NAME : WATER_TILE_NAME;
        }

        private void VerifTypeOfCell(Cell cell)
        {
            if (cell == null)
            {
                return;
            }
            if (cell.GridObject.Template.Name == GRASS_TILE_NAME)
            {

            }
            if (cell.GridObject.Template.Name == WATER_TILE_NAME)
            {

            }
        }
    
    
    }
}
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
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms = 10;

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            List<RectInt> rects = new List<RectInt>();

            for (int i = 0; i < _maxSteps; i++)
            {
                int x = RandomService.Range(0, 63);
                int y = RandomService.Range(0, 63);
                int rWidth = RandomService.Range(5, 10);
                int rHeight = RandomService.Range(5, 10);

                RectInt room = new RectInt(x, y, rWidth, rHeight);

                // Verify if the room can be placed
                //Debug.Log($"Attempting to place room at {room.position} with size {room.size}");

                if (!VerifyIfDoesntOverlap(room, rects))
                {
                    rects.Add(room);

                    this.PlaceRoom(room);

                }
                else
                {
                    i--;
                }

                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
            }

            for (int i = 0; i < rects.Count; ++i)
            {
                Debug.Log(i + " = " + rects[i]);
            }

            AscendingOrder(rects);

            // Final ground building.
            BuildGround();
        }

        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");

            // Instantiate ground blocks
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int z = 0; z < Grid.Lenght; z++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                    {
                        //Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }

                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }

        private void PlaceRoom(RectInt room)
        {
            for (int ix = room.xMin; ix < room.xMax; ix++)
            {
                for (int iy = room.yMin; iy < room.yMax; iy++)
                {
                    if (Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                    {
                        AddTileToCell(cell, ROOM_TILE_NAME, true);
                    }
                }
            }
        }

        private bool VerifyIfDoesntOverlap(RectInt room, List<RectInt> list)
        {
            if (list.Count == 0)
                return false;

            for (int i = 0; i < list.Count; i++)
            {
                if (room.Overlaps(list[i]))
                {
                    //Debug.Log($"Room at {room.position} with size {room.size} overlaps with existing room at {list[i].position} with size {list[i].size}");
                    return true;
                }
            }

            return false;
        }

        //TO DO : Finish this method to sort rooms in ascending order based on their xMin value
        private void AscendingOrder(List<RectInt> list)
        {
            List<RectInt> sortedList = new List<RectInt>();

            for (int i = 0; i < list.Count; i++)
            {
                RectInt smallest = list[i];

                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[j].xMin < smallest.xMin)
                    {
                        RectInt x = list[i];
                        smallest = list[j];
                        list[j] = x;
                    }
                }

                sortedList.Add(smallest);
            }

            Debug.Log("Sorted List:");

            for (int i = 0; i < list.Count; ++i) 
            {
                Debug.Log(i + " = " + list[i]);
            }

        }

    }
}
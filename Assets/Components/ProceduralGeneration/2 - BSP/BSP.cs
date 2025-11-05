using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using VTools.Grid;
using VTools.RandomService;
using VTools.ScriptableObjectDatabase;

[CreateAssetMenu(menuName = "Procedural Generation Method/BSP Placement")]
public class BSP : ProceduralGenerationMethod
{
    [Header("Room Parameters")]
    [SerializeField] private int _maxRooms = 2;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        //RandomService rs;
        //Node mainParent = new Node();

        RectInt baseRect = new RectInt(0, 0, 64, 64);

        Split(baseRect);        

        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);

        BuildGround();
    }

    private void PlaceFirstRoom(RectInt room)
    {
        for (int ix = room.xMin; ix < room.xMax; ix++)
        {
            for (int iy = room.yMin; iy < room.yMax; iy++)
            {
                if (Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                {
                    AddTileToCell(cell, WATER_TILE_NAME, true);
                }
            }
        }
    }

    private void PlaceSecondRoom(RectInt room)
    {
        for (int ix = room.xMin; ix < room.xMax; ix++)
        {
            for (int iy = room.yMin; iy < room.yMax; iy++)
            {
                if (Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                {
                    AddTileToCell(cell, ROCK_TILE_NAME, true);
                }
            }
        }
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

    private void Split(RectInt rect)
    {
        bool x = RandomService.Chance(0.5f);

        if (x == true) //Horizontal
        {
            int Ycut = SplitHorizontal(rect);

            RectInt topRect = new RectInt(
                rect.xMin,
                rect.yMin + Ycut,
                rect.width,
                rect.height - Ycut);
            
            RectInt downRect = new RectInt(rect.xMin, rect.yMin, rect.width, Ycut);

            Debug.Log("toprect: " + topRect);
            Debug.Log("downrect: " + downRect);

            if(topRect.width < 10 || downRect.width < 10)
            {
                return;
            }

            Split(topRect);
            Split(downRect);
        }
        else //Vertical
        {
            int Xcut = SplitVertical(rect);

            RectInt leftRect = new RectInt(
                rect.xMin,
                rect.yMin,
                Xcut,
                rect.height);

            RectInt rightRect = new RectInt(rect.xMin + Xcut, rect.yMin, Xcut, rect.height);

            Debug.Log("Left rect: " + leftRect);
            Debug.Log("Right rect: " + rightRect);

            PlaceFirstRoom(leftRect);
            PlaceSecondRoom(rightRect);
        }
    }

    private int SplitVertical(RectInt rect)
    {
        int random = RandomService.Range(0 + rect.width / 3, (rect.width / 3) * 2);
        return random;
    }

    private int SplitHorizontal(RectInt rect)
    {
        int random = RandomService.Range(0 + rect.height / 3, (rect.height / 3) * 2);
        return random;
    }

}

using Components.ProceduralGeneration;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[CreateAssetMenu(menuName = "Procedural Generation Method/BSP Placement")]
public class BSP : ProceduralGenerationMethod
{
    [Header("Room Parameters")]
    [SerializeField] private int _maxRooms = 10;

    protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
    {
        RectInt baseRect = new RectInt(0,0,63,63);

        Split(baseRect);

        await UniTask.Delay(GridGenerator.StepDelay, cancellationToken: cancellationToken);
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

    private void Split(RectInt rect)
    {
        int x = RandomService.Range(0, 1);

        if (x == 0) //Horizontal
        {
            SplitHorizontal(rect);

        }
        else if (x == 1) //Vertical
        {
            SplitVertical(rect);
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

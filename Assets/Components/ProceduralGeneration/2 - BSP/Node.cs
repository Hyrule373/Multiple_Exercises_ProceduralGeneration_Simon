using UnityEngine;
using VTools.RandomService;

public class Node
{
    public Node Child1, Child2;
    private RandomService _randomService;
    private BSP _bsp;

    RectInt RectInt;

    public void InitializeNode(BSP bsp, RandomService randomService)
    {
        _randomService = randomService;
        _bsp = bsp;
    }

    private void GenerateRoom()
    {

    }

    public void Split()
    {
        bool horizontal = _randomService.Chance(0.5f);

        //Node firstChild = new Node(_randomService);
        //Node secondChild = new Node(_randomService);
    }

}

//Création du root tree 
using UnityEngine;
using VTools.RandomService;

public class Node
{
    public Node Child1, Child2;
    private RandomService _randomService;

    RectInt RectInt;

    public Node(RandomService randomService)
    {
        _randomService = randomService;
    }

    public void Split()
    {
        bool horizontal = _randomService.Chance(0.5f);

        Node firstChild = new Node(_randomService);
        Node secondChild = new Node(_randomService);
    }

}

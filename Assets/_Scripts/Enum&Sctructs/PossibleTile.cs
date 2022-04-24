using System;

[Serializable]
public struct PossibleTile
{
    public PossibleTile(BaseTile tile, int cost)
    {
        this.tile = tile;
        actionCost = cost;
    }

    public BaseTile tile;
    public int actionCost;
}

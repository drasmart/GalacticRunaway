using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class BattleField
    {
        public HexCoords2Int size = new HexCoords2Int(1, 1);
        public HexCoords2Int offset = new HexCoords2Int(0, 0);

        public List<BattleSide> sides = new List<BattleSide>();
        public List<LootBox> lootBoxes = new List<LootBox>();
        public List<LandObstacle> obstacles = new List<LandObstacle>();
    }
}

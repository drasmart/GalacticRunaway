using HexGrid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [System.Serializable]
    public class BattleFieldMutation
    {
        public readonly BattleFieldElementType elementType;
        public readonly BattleFieldElement element;
        public readonly int side;
        public readonly LandUnit unit;
        public readonly LandObstacle obstacle;
        public readonly LootBox lootBox;
        public readonly HexCoords2Int oldCoords;

        public BattleFieldMutation(LandUnit unit, HexCoords2Int oldCoords, int side)
        {
            element = unit;
            elementType = BattleFieldElementType.Unit;
            this.unit = unit;
            this.side = side;
            this.oldCoords = oldCoords;
        }
        public BattleFieldMutation(LandObstacle obstacle)
        {
            element = obstacle;
            elementType = BattleFieldElementType.Obstacle;
            this.obstacle = obstacle;
            oldCoords = obstacle.coords;
        }
        public BattleFieldMutation(LootBox lootBox)
        {
            element = lootBox;
            elementType = BattleFieldElementType.LootBox;
            this.lootBox = lootBox;
            oldCoords = lootBox.coords;
        }

        public override string ToString()
        {
            var msg = "BattleFieldMutation(";
            switch (elementType)
            {
                case BattleFieldElementType.Unit:
                    msg += "unit: " + unit.ToString();
                    msg += ", oldCoords: " + oldCoords.ToString();
                    msg += ", side: " + side.ToString();
                    break;
                case BattleFieldElementType.LootBox:
                    msg += "lootBox: " + lootBox.ToString();
                    break;
                case BattleFieldElementType.Obstacle:
                    msg += "obstacle: " + obstacle.ToString();
                    break;
            }
            msg += ")";
            return msg;
        }
    }
}

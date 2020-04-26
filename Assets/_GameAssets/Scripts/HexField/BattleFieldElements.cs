using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [CreateAssetMenu(menuName = "HexField/Land/BattleField Elements")]
    public class BattleFieldElements : ScriptableObject
    {
        public List<GameObject> sideUnits = new List<GameObject>();
        public GameObject obstacle = null;
        public GameObject pickup = null;
    }
}

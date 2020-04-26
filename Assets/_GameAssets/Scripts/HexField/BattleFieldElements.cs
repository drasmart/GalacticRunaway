using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexField
{
    [CreateAssetMenu(menuName = "HexField/Land/BattleField Elements")]
    public class BattleFieldElements : ScriptableObject
    {
        public List<GameObject> sideUnits = new List<GameObject>();
        public GameObject obstacle = null;
        [FormerlySerializedAs("pickup")]
        public GameObject lootBox = null;
    }
}

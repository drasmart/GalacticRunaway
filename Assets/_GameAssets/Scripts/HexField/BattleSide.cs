using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    [Serializable]
    public class BattleSide
    {
        public List<LandUnit> units = new List<LandUnit>();
    }
}

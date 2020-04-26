using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexField
{
    public interface Destructable
    {
        Tuple<int, int> hpRange { get; }
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UIElements;

namespace MatrixModels.Editor
{
    [CustomPropertyDrawer(typeof(BoolMatrix))]
    public class BoolMatrixDrawer : BaseMatrixDrawer { }
}

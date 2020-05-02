using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEngine.UIElements;

namespace MatrixModels.Editor
{
    [CustomPropertyDrawer(typeof(IntMatrix))]
    public class IntMatrixDrawer : BaseMatrixDrawer { }
}

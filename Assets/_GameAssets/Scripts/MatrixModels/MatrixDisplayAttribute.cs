using System;

namespace MatrixModels
{
    [AttributeUsage(AttributeTargets.Field)]
    public class MatrixDisplayAttribute: Attribute
    {
        public bool InvertX = false;
        public bool YUp = true;
        public bool Hexagonal = false;
    }
}
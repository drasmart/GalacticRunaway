using UnityEngine;
using System.Collections;

namespace MatrixModels
{
    using ValueType = System.Int32;

    [System.Serializable]
    public class IntMatrix
    {
        [SerializeField]
        [Min(1)]
        private int rows = 1;

        [SerializeField]
        [Min(1)]
        private int columns = 1;

        [SerializeField]
        private Row[] matrix = { new Row(1) };

        public int Columns {
            get { return columns; }
            set {
                if (value < 1)
                {
                    return;
                }
                columns = value;
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    matrix[i].Resize(columns);
                }
            }
        }

        public int Rows {
            get { return rows; }
            set {
                if (value < 1)
                {
                    return;
                }
                rows = value;
                Row[] oldMatrix = matrix;
                matrix = new Row[rows];
                for (int j = 0; j < rows; j++)
                {
                    if (oldMatrix == null && oldMatrix.Length <= j)
                    {
                        matrix[j] = new Row(columns);
                    }
                    else
                    {
                        matrix[j] = oldMatrix[j];
                    }
                }
            }
        }

        private ValueType this[int row, int column] {
            get { return matrix[row][column]; }
            set { matrix[row][column] = value; }
        }

        public ValueType this[Vector2Int coords] {
            get { return this[coords.y, coords.x]; }
            set { this[coords.y, coords.x] = value; }
        }

        public Vector2Int Size {
            get { return new Vector2Int(columns, rows); }
            set { Columns = value.y; Rows = value.x; }
        }

        public ValueType[,] ToArray()
        {
            var result = new ValueType[columns, rows];
            for (int x = 0; x < columns; x++)
                for (int y = 0; y < rows; y++)
                    result[x, y] = matrix[y][x];
            return result;
        }

        [System.Serializable]
        public class Row
        {
            [SerializeField]
            private ValueType[] cells = new ValueType[] { default(ValueType) };

            public ValueType this[int i] {
                get { return cells[i]; }
                set { cells[i] = value; }
            }

            public Row(int n)
            {
                Resize(n);
            }

            public void Resize(int n)
            {
                if (n == cells.Length)
                {
                    return;
                }
                ValueType[] oldCells = cells;
                cells = new ValueType[n];
                for (int i = 0; i < n; i++)
                {
                    cells[i] = ((oldCells != null && i < oldCells.Length) ? oldCells[i] : default(ValueType));
                }
            }
        }
    }
}

using System.Collections.Generic;

public class Helper
{
    private Dictionary<int, int> _rows;
    private Dictionary<int, int> _cols;
    private Dictionary<int, int> _sqrs;

    private Dictionary<int, List<int>> _rowValues;
    private Dictionary<int, List<int>> _colValues;
    private Dictionary<int, List<int>> _sqrValues;

    private Dictionary<int, Dictionary<int, int>> _tranform2DTo1D;
    private Dictionary<int, int[]> _tranform1DTo2D;


    private bool _isConfigured;
    public void Configurations()
    {
        if (_isConfigured)
        {
            return;
        }
        _isConfigured = true;


        _tranform2DTo1D = new Dictionary<int, Dictionary<int, int>>();
        _tranform1DTo2D = new Dictionary<int, int[]>();
        for (int i = 0; i < 9; i++)
        {
            _tranform2DTo1D.Add(i, new Dictionary<int, int>());
        }

        _rows = new Dictionary<int, int>();
        _cols = new Dictionary<int, int>();
        _sqrs = new Dictionary<int, int>();


        int[][] ss = new int[][]{
               new int[] {0, 1, 2, 9, 10, 11, 18, 19, 20},
               new int[] {3, 4, 5, 12, 13, 14, 21, 22, 23},
               new int[] {6, 7, 8, 15, 16, 17, 24, 25, 26},
               new int[] {27, 28, 29, 36, 37, 38, 45, 46, 47},
               new int[] {30, 31, 32, 39, 40, 41, 48, 49, 50},
               new int[] {33, 34, 35, 42, 43, 44, 51, 52, 53},
               new int[] {54, 55, 56, 63, 64, 65, 72, 73, 74},
               new int[]  {57, 58, 59, 66, 67, 68, 75, 76, 77},
               new int[] {60, 61, 62, 69, 70, 71, 78, 79, 80}
            };

        _rowValues = new Dictionary<int, List<int>>();
        _colValues = new Dictionary<int, List<int>>();
        _sqrValues = new Dictionary<int, List<int>>();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _sqrs[ss[i][j]] = i;
                int index = ss[i][j];
                int r = _rows[i] = GetRow(index);
                int c = _cols[j] = GetCol(index);
                int s = i;

                if (!_rowValues.ContainsKey(r)) { _rowValues[r] = new List<int>(); }
                _rowValues[r].Add(index);
                if (!_colValues.ContainsKey(c)) { _colValues[c] = new List<int>(); }
                _colValues[c].Add(index);
                if (!_sqrValues.ContainsKey(s)) { _sqrValues[s] = new List<int>(); }
                _sqrValues[s].Add(index);

            }
        }


    }


    public int GetSqr(int index)
    {
        return _sqrs[index];
    }

    public int GetRow(int index)
    {
        return Index1DTo2D(index)[0];
    }
    public int GetCol(int index)
    {
        return Index1DTo2D(index)[1];
    }

    public List<int> GetRowIndexes(int index)
    {
        return _rowValues[GetRow(index)];
    }

    public List<int> GetColIndexes(int index)
    {
        return _colValues[GetCol(index)];
    }

    public List<int> GetSqrIndexes(int index)
    {
        return _sqrValues[GetSqr(index)];
    }


    public bool IsValidGame(int[] board)
    {
        Checker rowChecker = new Checker();
        Checker[] colCheckers = new Checker[9];
        Checker[] sqrCheckers = new Checker[9];
        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            rowChecker.Clear();
            for (int j = 0; j < 9; j++)
            {
                if (!rowChecker.Add(board[index]))
                {
                    //Debug.Log(i + " " + j + " " + index);
                    return false;
                }

                if (!colCheckers[j].Add(board[index]))
                {
                    //Debug.Log(i + " " + j);
                    return false;
                }

                if (!sqrCheckers[GetSqr(index)].Add(board[index]))
                {
                    //Debug.Log(i + " " + j);
                    return false;
                }
                index++;
            }
        }

        return true;
    }


    public bool IsValid(int[] board, int position, int val)
    {
        int newC = val;

        // Check rows
        foreach (int j in GetRowIndexes(position))
        {
            if (board[j] == newC)
            {
                return false;
            }
        }

        // Check squares
        foreach (int j in GetSqrIndexes(position))
        {
            if (board[j] == newC)
            {
                return false;
            }
        }

        // Check cols
        foreach (int j in GetColIndexes(position))
        {
            if (board[j] == newC)
            {
                return false;
            }
        }


        return true;
    }



    public int Index2DTo1D(int row, int col)
    {
        if (_tranform2DTo1D[row].ContainsKey(col))
        {
            return _tranform2DTo1D[row][col];
        }
        else
        {
            int value = row * 9 + col;
            _tranform2DTo1D[row][col] = value;
            _tranform1DTo2D[value] = new int[2];
            _tranform1DTo2D[value][0] = row;
            _tranform1DTo2D[value][1] = col;
            return value;
        }
    }

    public int[] Index1DTo2D(int value)
    {

        if (_tranform1DTo2D.ContainsKey(value))
        {
            return _tranform1DTo2D[value];
        }
        else
        {
            int row = value / 9;
            int col = value % 9;
            _tranform2DTo1D[row][col] = value;
            _tranform1DTo2D[value] = new int[2];
            _tranform1DTo2D[value][0] = row;
            _tranform1DTo2D[value][1] = col;
            return _tranform1DTo2D[value];
        }
    }


    private struct Checker
    {
        private int _seenNumbers;

        public bool Add(int val)
        {
            if (val == 0) return true;

            int mask = 1 << val;

            if ((_seenNumbers & mask) != 0) return false;

            _seenNumbers |= mask;
            return true;
        }

        public void Clear()
        {
            _seenNumbers = 0;
        }
    }


}

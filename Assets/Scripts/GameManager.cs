using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject [] slots;


    private Slot[] _slots = new Slot[81];


    public Slot[] Slots { get => _slots; set => _slots = value; }
    public static Slot SelectedSlot { get => _selectedSlot; set => _selectedSlot = value; }


    private void Start()
    {
        Helper.Configurations();

        for (int i = 0; i < slots.Length; i++)
        {

            int x = i;
            slots[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSlot(x);});

            _slots[i] = slots[i].AddComponent<Slot>();
            _slots[i].Create();

        }

    }


    public static bool IsWorked;

    private void Update()
    {
        if (IsWorked)
        {
            return;
        }

        if (_selectedSlot != null)
        {

            if (Input.GetKeyDown("1") || Input.GetKeyDown(KeyCode.Keypad1))
            {
                _selectedSlot.Value = 1;
            }else if (Input.GetKeyDown("2") || Input.GetKeyDown(KeyCode.Keypad2))
            {
                _selectedSlot.Value = 2;
            }
            else if (Input.GetKeyDown("3") || Input.GetKeyDown(KeyCode.Keypad3))
            {
                _selectedSlot.Value = 3;
            }
            else if (Input.GetKeyDown("4") || Input.GetKeyDown(KeyCode.Keypad4))
            {
                _selectedSlot.Value = 4;
            }
            else if (Input.GetKeyDown("5") || Input.GetKeyDown(KeyCode.Keypad5))
            {
                _selectedSlot.Value = 5;
            }
            else if (Input.GetKeyDown("6") || Input.GetKeyDown(KeyCode.Keypad6))
            {
                _selectedSlot.Value = 6;
            }
            else if (Input.GetKeyDown("7") || Input.GetKeyDown(KeyCode.Keypad7))
            {
                _selectedSlot.Value = 7;
            }
            else if (Input.GetKeyDown("8") || Input.GetKeyDown(KeyCode.Keypad8))
            {
                _selectedSlot.Value = 8;
            }
            else if (Input.GetKeyDown("9") || Input.GetKeyDown(KeyCode.Keypad9))
            {
                _selectedSlot.Value = 9;
            }
            else if(Input.GetKeyDown(KeyCode.Delete))
            {
                _selectedSlot.Value = 0;
            }
        }

    }


    private static Slot _selectedSlot = null;

    public void OnClickSlot(int slot)
    {
        if (IsWorked)
            return;
        //Debug.Log(slot);
        _selectedSlot = _slots[slot];

    }


    public void OnClickResetButton()
    {

        for (int i = 0; i < slots.Length; i++)
        {

            _slots[i].Create();

        }

        IsWorked = false;
    }


    

    public void OnClickSolveButton()
    {
        if (IsWorked)
        {
            return;
        }


        StringBuilder initialString = new StringBuilder();
        // Create String
        for (int i = 0; i < _slots.Length; i++)
        {
            initialString.Append(_slots[i].Value);
        }

        // Check the game is valid
        Debug.Log(initialString);
        GameManager.initialString = initialString.ToString();

        bool rst = Helper.IsValidGame(GameManager.initialString);
        if (!rst)
        {
            Debug.Log("not valid");
            StartCoroutine(ShowTextRoutine());
            OnClickResetButton();
            return;
        }

        finalStr = "";
        StartCoroutine(SolveRoutine());


    }


    private IEnumerator SolveRoutine()
    {
        IsWorked = true;

        Thread t1 = new Thread(new ThreadStart(Solver1));
        Thread t2 = new Thread(new ThreadStart(Solver2));
        Thread t3 = new Thread(new ThreadStart(Solver3));
        Thread t4 = new Thread(new ThreadStart(Solver4));

        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();

        isFounded = false;

        while (string.IsNullOrEmpty(finalStr))
        {
            yield return null;
        }


        for (int i = 0; i < 81; i++)
        {
            _slots[i].Value = (finalStr[i] - 48);
        }

        IsWorked = false;

        t1.Abort();
        t2.Abort();
        t3.Abort();
        t4.Abort();

        Debug.Log(finalStr);
    }


    private static string initialString;



    private static int[] array1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9};
    private static void Solver1()
    {
        System.Random rnd = new System.Random();
        array1 = array1.OrderBy(x => rnd.Next()).ToArray();
        Recursive(new StringBuilder(initialString), 0, array1,1);
    }


    private static int[] array2 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static void Solver2()
    {
        System.Random rnd = new System.Random();
        array2 = array2.OrderBy(x => rnd.Next()).ToArray();
        Recursive(new StringBuilder(initialString), 0, array2,2);
    }

    
    private static int[] array3 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static void Solver3()
    {
        System.Random rnd = new System.Random();
        array3 = array3.OrderBy(x => rnd.Next()).ToArray();
        RecursiveBack(new StringBuilder(initialString), 80, array3,3);

    }

    private static int[] array4 = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    private static void Solver4()
    {
        System.Random rnd = new System.Random();
        array4 = array4.OrderBy(x => rnd.Next()).ToArray();
        RecursiveBack(new StringBuilder(initialString), 80, array4,4);

    }

    private static readonly object lockObj = new object();
    private static bool isFounded = false;
    private static void SetFinalString(string str , int founder)
    {
        lock (lockObj)
        {
            if (!isFounded)
            {
                isFounded = true;
                finalStr = str;
                Debug.Log("Thread : " + founder);
            }

        }
    }

    private static bool IsValid(StringBuilder str, int position, int val)
    {

        char newC = ((char)(val + 48));


        // Check rows
        foreach (int j in Helper.GetRowIndexes(position))
        {
            if (str[j] == newC)
            {
                return false;
            }
        }

        // Check squares
        foreach (int j in Helper.GetSqrIndexes(position))
        {
            if (str[j] == newC)
            {
                return false;
            }
        }

        // Check cols
        foreach (int j in Helper.GetColIndexes(position))
        {
            if (str[j] == newC)
            {
                return false;
            }
        }


        return true;


    }



    private static string finalStr;
    private static bool Recursive(StringBuilder str,int position,int[] array,int t)
    {
        //Debug.Log(str +" "+size);

        if(position >= 81)
        {
            SetFinalString(str.ToString(),t);
            return true;
        }


        if(str[position] != '0')
        {
            return Recursive(str, position+1, array,t);
        }

        for (int i = 0; i < array.Length; i++)
        {
            if (IsValid(str,position,array[i]))
            {
                str[position] = (char)(array[i] + 48);
                if (Recursive(str, position + 1, array,t))
                {
                    return true;
                }
            }
        }
        str[position] = '0';

        return false;

    }

    private static bool RecursiveBack(StringBuilder str, int position, int[] array, int t)
    {
        //Debug.Log(str +" "+size);



        if (position < 0)
        {
            SetFinalString(str.ToString(),t);
            return true;
        }


        if (str[position] != '0')
        {
            return RecursiveBack(str, position - 1, array, t);
        }

        for (int i = 0; i < array.Length; i++)
        {
            if (IsValid(str, position, array[i]))
            {
                str[position] = (char)(array[i] + 48);
                if (RecursiveBack(str, position - 1, array, t))
                {
                    return true;
                }
            }
        }
        str[position] = '0';

        return false;

    }



    [SerializeField]
    private GameObject text;

    private IEnumerator ShowTextRoutine()
    {

        text.SetActive(true);
        yield return new WaitForSeconds(1f);
        text.SetActive(false);

    }




    private class Helper
    {
        private static Dictionary<int, int> rows;
        private static Dictionary<int, int> cols;
        private static Dictionary<int, int> sqrs;

        private static Dictionary<int, List<int>> rowValues;
        private static Dictionary<int, List<int>> colValues;
        private static Dictionary<int, List<int>> sqrValues;


        private static bool isConfigured;
        public static void Configurations()
        {
            if (isConfigured)
            {
                return;
            }
            isConfigured = true;


            tranform2D_1D = new Dictionary<int, Dictionary<int, int>>();
            tranform1D_2D = new Dictionary<int, int[]>();
            for (int i = 0; i < 9; i++)
            {
                tranform2D_1D.Add(i, new Dictionary<int, int>());
            }

            rows = new Dictionary<int, int>();
            cols = new Dictionary<int, int>();
            sqrs = new Dictionary<int, int>();


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

            rowValues = new Dictionary<int, List<int>>();
            colValues = new Dictionary<int, List<int>>();
            sqrValues = new Dictionary<int, List<int>>();

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    sqrs[ss[i][j]] = i;
                    int index = ss[i][j];
                    int r = rows[i] = GetRow(index);
                    int c = cols[j] = GetCol(index);
                    int s = i;

                    if (!rowValues.ContainsKey(r)){rowValues[r] = new List<int>();}
                    rowValues[r].Add(index);
                    if (!colValues.ContainsKey(c)) { colValues[c] = new List<int>(); }
                    colValues[c].Add(index);
                    if (!sqrValues.ContainsKey(s)) { sqrValues[s] = new List<int>(); }
                    sqrValues[s].Add(index);

                }
            }


            string colsStr = "cols \n";
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    colsStr += sqrValues[i][j]+" , ";
                }
                colsStr += "\n";
            }
            Debug.Log(colsStr);



        }


        public static int GetSqr(int index)
        {
            return sqrs[index];
        }

        public static int GetRow(int index)
        {
            return Index1DTo2D(index)[0];
        }
        public static int GetCol(int index)
        {
            return Index1DTo2D(index)[1];
        }

        public static List<int> GetRowIndexes(int index)
        {
            return rowValues[GetRow(index)];
        }

        public static List<int> GetColIndexes(int index)
        {
            return colValues[GetCol(index)];
        }

        public static List<int> GetSqrIndexes(int index)
        {
            return sqrValues[GetSqr(index)];
        }


        public static bool IsValidGame(string board)
        {
            Checker rowChecker = new Checker();
            Checker [] colCheckers = new Checker [9];
            Checker [] sqrCheckers = new Checker [9];
            int index = 0;
            for (int i = 0; i < 9; i++)
            {
                rowChecker.Clear();
                for (int j = 0; j < 9; j++)
                {
                    if (!rowChecker.Add(board[index] - 48))
                    {
                        Debug.Log(i + " " + j+ " "+index);
                        return false;
                    }
                    if (colCheckers[j] == null)
                    {
                        colCheckers[j] = new Checker ();
                    }
                    if (!colCheckers[j].Add(board[index] - 48))
                    {
                        Debug.Log(i + " " + j);
                        return false;
                    }

                    if(sqrCheckers[GetSqr(index)] == null)
                    {
                        sqrCheckers[GetSqr(index)] = new Checker ();
                    }
                    if(!sqrCheckers[GetSqr(index)].Add(board[index] - 48))
                    {
                        Debug.Log(i + " " + j);
                        return false;
                    }
                    index++;
                }
            }

            return true;
        }


        private static Dictionary<int, Dictionary<int, int>> tranform2D_1D;
        private static Dictionary<int, int[]> tranform1D_2D;
        public static int Index2DTo1D(int row, int col)
        {
            if (tranform2D_1D[row].ContainsKey(col))
            {
                return tranform2D_1D[row][col];
            }
            else
            {
                int value = row * 9 + col;
                tranform2D_1D[row][col] = value;
                tranform1D_2D[value] = new int[2];
                tranform1D_2D[value][0] = row;
                tranform1D_2D[value][1] = col;
                return value;
            }
        }

        public static int[] Index1DTo2D(int value)
        {

            if (tranform1D_2D.ContainsKey(value))
            {
                return tranform1D_2D[value];
            }
            else
            {
                int row = value / 9;
                int col = value % 9;
                tranform2D_1D[row][col] = value;
                tranform1D_2D[value] = new int[2];
                tranform1D_2D[value][0] = row;
                tranform1D_2D[value][1] = col;
                return tranform1D_2D[value];
            }
        }


        private class Checker
        {

            private bool[] values;

            public Checker()
            {
                values = new bool[10];
            }

            public bool Add(int val)
            {
                if (val == 0)
                    return true;
                if (values[val])
                    return false;
                values[val] = true;
                return true;
            }

            public void Clear()
            {
                values = new bool[10];
            }

        }

    }

}

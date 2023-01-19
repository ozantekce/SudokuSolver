using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject [] slots;


    private Slot[] _slots = new Slot[81];

    private static List<int>[] squares;
    private static List<int>[] rows;
    private static List<int>[] columns;

    public static List<int>[] Squares { get => squares; set => squares = value; }
    public static List<int>[] Rows { get => rows; set => rows = value; }
    public static List<int>[] Columns { get => columns; set => columns = value; }
    public Slot[] Slots { get => _slots; set => _slots = value; }

    public static void CreateSets()
    {

        squares = new List<int>[9];
        squares[0] = new List<int>() { 0, 1, 2, 9, 10, 11, 18, 19, 20 };
        squares[1] = new List<int>() { 3, 4, 5, 12, 13, 14, 21, 22, 23 };
        squares[2] = new List<int>() { 6, 7, 8, 15, 16, 17, 24, 25, 26 };
        squares[3] = new List<int>() { 27, 28, 29, 36, 37, 38, 45, 46, 47 };
        squares[4] = new List<int>() { 30, 31, 32, 39, 40, 41, 48, 49, 50 };
        squares[5] = new List<int>() { 33, 34, 35, 42, 43, 44, 51, 52, 53 };
        squares[6] = new List<int>() { 54, 55, 56, 63, 64, 65, 72, 73, 74 };
        squares[7] = new List<int>() { 57, 58, 59, 66, 67, 68, 75, 76, 77 };
        squares[8] = new List<int>() { 60, 61, 62, 69, 70, 71, 78, 79, 80 };


        rows = new List<int>[9];
        int index = 0;
        for (int i = 0; i < 9; i++)
        {
            rows[i] = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                rows[i].Add(index);
                index++;
            }

        }

        columns = new List<int>[9];
        for (int i = 0; i < 9; i++)
        {
            columns[i] = new List<int>();
            for (int j = 0; j < 9; j++)
            {
                columns[i].Add(j * 9 + i);
            }
        }
    }

    private void Start()
    {
        CreateSets();
        for (int i = 0; i < slots.Length; i++)
        {

            int x = i;
            slots[i].GetComponent<Button>().onClick.AddListener(delegate { OnClickSlot(x);});

            _slots[i] = slots[i].AddComponent<Slot>();
            _slots[i].Create();

        }

    }


    private bool _isWorked;

    private void Update()
    {
        if (_isWorked)
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


    private Slot _selectedSlot = null;

    public void OnClickSlot(int slot)
    {

        //Debug.Log(slot);
        _selectedSlot = _slots[slot];

    }


    public void OnClickResetButton()
    {

        for (int i = 0; i < slots.Length; i++)
        {

            _slots[i].Create();

        }

        _isWorked = false;
    }


    

    public void OnClickSolveButton()
    {


        _isWorked = true;

        int fullSlots = 0;

        for (int i = 0;i < _slots.Length; i++)
        {
            if(_slots[i].Value != 0)
            {
                fullSlots++;
            }
        }

        if(fullSlots < 17)
        {
            _isWorked=false;
            // it is not valid game show a notice
            return;
        }

        for (int i = 0; i < _slots.Length; i++)
        {

            if (_slots[i].Value != 0)
            {
                SetValueForSolve(new Vector2Int(i, _slots[i].Value));
            }

        }


        string str = Solve();

        for (int i = 0; i < str.Length; i++)
        {
            _slots[i].Value = (str[i]-48);
        }


    }



    private void SetValueForSolve(Vector2Int seqValPair)
    {
        _slots[seqValPair.x].Potentials.Clear();

        // find row
        List<int> row = null;
        foreach (List<int> set in rows)
        {
            if (set.Contains(seqValPair.x))
            {
                row = set;
                break;
            }
        }

        foreach (int s in row)
        {
            _slots[s].Potentials.Remove(seqValPair.y);
        }

        // find column
        List<int> column = null;
        foreach (List<int> set in columns)
        {
            if (set.Contains(seqValPair.x))
            {
                column = set;
                break;
            }
        }
        foreach (int s in column)
        {
            _slots[s].Potentials.Remove(seqValPair.y);
        }

        // find square
        List<int> square = null;
        foreach (List<int> set in squares)
        {
            if (set.Contains(seqValPair.x))
            {
                square = set;
                break;
            }
        }
        foreach (int s in square)
        {
            _slots[s].Potentials.Remove(seqValPair.y);
        }

        _slots[seqValPair.x].Potentials.Add(seqValPair.y);


    }



    private string Solve()
    {

        Solver solver = new Solver(this);
        solver.Solve();
        List<string> strings = solver.Strings;

        for (int i = 0; i < strings.Count; i++)
        {
            if (IsValid(strings[i]))
            {
                return strings[i];
            }
        }

        return string.Empty;
    }


    private HashSet<char> _checker = new HashSet<char>();
    public bool IsValid(string str)
    {

        // Check Squares
        for (int i = 0; i < squares.Length; i++)
        {
            _checker.Clear();
            foreach (int s in squares[i])
            {
                if (!_checker.Add((str[s])))
                {
                    return false;
                }
            }

        }
        return true;

    }


    private class Solver
    {

        public GameManager gameManager;

        private RowSolver [] _rowSolvers;
        public Solver(GameManager gameManager)
        {
            this.gameManager = gameManager;
            _rowSolvers = new RowSolver[9];

            for (int i = 0; i < _rowSolvers.Length; i++)
            {
                _rowSolvers[i] = new RowSolver(this.gameManager,GameManager.Rows[i]);
                _rowSolvers[i].FindRows();
            }


        }

        private List<string> _strings;
        public void Solve()
        {
            _strings = new List<string>();
            _strings.Add(string.Empty);

            List<string> created = new List<string>();

            foreach (RowSolver rowSolver in _rowSolvers)
            {
                for (int i = 0; i < _strings.Count; i++)
                {

                    foreach (string s in rowSolver.Strings)
                    {
                        string current = _strings[i];
                        current += s;
                        if (isValid(current))
                        {
                            created.Add(current);
                        }
                    }
                }
                _strings.Clear();
                _strings.AddRange(created);
                created.Clear();
            }



        }





        private HashSet<char> _checker = new HashSet<char>();

        public List<string> Strings { get => _strings; set => _strings = value; }

        private bool isValid(string str)
        {
            int size = str.Length / 9;

            for (int i = 0;i < 9; i++)
            {
                _checker.Clear();
                for(int j = 0;j < size; j++)
                {
                    if (!_checker.Add(str[j*9+i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


    }


    private class RowSolver
    {
        public GameManager gameManager;
        private List<int> _responsible;

        public RowSolver(GameManager gameManager, List<int> row)
        {
            this.gameManager = gameManager;
            _responsible = new List<int>();
            _responsible.AddRange(row);
        }

        private List<string> _strings;


        public void FindRows()
        {
            _strings = new List<string>();
            _strings.Add(string.Empty);

            List<string> created = new List<string>();

            foreach (int index in _responsible)
            {
                for (int i = 0; i < _strings.Count; i++)
                {
                    
                    foreach (int p in gameManager.Slots[index].Potentials)
                    {
                        string current = _strings[i];
                        current += p;
                        if (isValid(current))
                        {
                            created.Add(current);
                        }
                    }
                }
                _strings.Clear();
                _strings.AddRange(created);
                created.Clear();
            }

            //Debug.Log(_responsible + " : " + _strings.Count);

        }


        private HashSet<char> _checker = new HashSet<char>();

        public List<string> Strings { get => _strings; set => _strings = value; }

        private bool isValid(string str)
        {
            _checker.Clear();
            for (int i = 0; i < str.Length; i++)
            {

                if (!_checker.Add(str[i]))
                {
                    return false;
                }

            }

            return true;
        }

    }





}

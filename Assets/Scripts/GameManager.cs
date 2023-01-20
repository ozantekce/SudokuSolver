using System.Collections;
using System.Collections.Generic;
using System.Text;
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



    private static Dictionary<int, List<int>> indexSquarePairs = new Dictionary<int, List<int>>();

    private static Dictionary<int, List<int>> indexRowPairs = new Dictionary<int, List<int>>();

    private static Dictionary<int, List<int>> indexColPairs = new Dictionary<int, List<int>>();


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

        for (int i = 0; i < 81; i++)
        {
            List<int> temp = null;
            foreach (List<int> sqr in squares)
            {
                if (sqr.Contains(i))
                {
                    temp = sqr;
                    break;
                }
            }
            indexSquarePairs[i] = temp;
        }


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


        for (int i = 0; i < 81; i++)
        {
            List<int> temp = null;
            foreach (List<int> sqr in rows)
            {
                if (sqr.Contains(i))
                {
                    temp = sqr;
                    break;
                }
            }
            indexRowPairs[i] = temp;
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

        for (int i = 0; i < 81; i++)
        {
            List<int> temp = null;
            foreach (List<int> sqr in columns)
            {
                if (sqr.Contains(i))
                {
                    temp = sqr;
                    break;
                }
            }
            indexColPairs[i] = temp;
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

        int fullSlots = 0;

        for (int i = 0;i < _slots.Length; i++)
        {
            if(_slots[i].Value != 0)
            {
                fullSlots++;
            }
        }


        StringBuilder initialString = new StringBuilder();

        for (int i = 0; i < _slots.Length; i++)
        {

            initialString.Append(_slots[i].Value);

        }


        Debug.Log(initialString);


        finalStr = "";



        
        Recursive(initialString, 0);
        Debug.Log(finalStr);
        


        /*
        Queue<string> q = Solve(initialString.ToString());
        */

        for (int i = 0; i < 81; i++)
        {
            _slots[i].Value = (finalStr[i] - 48);
        }




    }




    private Queue<string> Solve(string intial)
    {

        Queue<string> queue = new Queue<string>();

        queue.Enqueue(intial);

        for (int i = 0; i < 81; i++)
        {
            Queue<string> createdQueue = new Queue<string>();

            while(queue.Count > 0)
            {
                string current;
                current = queue.Dequeue();

                if (current[i] == '0')
                {
                    for (int j = 1; j <= 9; j++)
                    {
                        string created = current;
                        StringBuilder sb = new StringBuilder(created);
                        if (IsValid(sb,i,j))
                        {
                            sb[i] = ((char)(j + 48));
                            created = sb.ToString();
                            createdQueue.Enqueue(created);
                        }
                    }
                }
                else
                {
                    createdQueue.Enqueue(current);
                }

            }
            queue = new Queue<string>(createdQueue);

            //Debug.Log("size : "+queue.Count);

        }

        return queue;

    }


    private bool IsValid(StringBuilder str, int position, int val)
    {

        char newC = ((char)(val + 48));


        // Check rows
        foreach (int j in indexRowPairs[position])
        {
            if (str[j] == newC)
            {
                return false;
            }
        }

        // Check squares
        foreach (int j in indexSquarePairs[position])
        {
            if (str[j] == newC)
            {
                return false;
            }
        }

        // Check cols
        foreach (int j in indexColPairs[position])
        {
            if (str[j] == newC)
            {
                return false;
            }
        }


        return true;


    }



    private static string finalStr;
    private bool Recursive(StringBuilder str,int position)
    {
        //Debug.Log(str +" "+size);

        if(position >= 81)
        {
            finalStr = str.ToString();
            return true;
        }


        if(str[position] != '0')
        {
            return Recursive(str, position+1);
        }

        for (int i = 1; i < 10; i++)
        {
            if (IsValid(str,position,i))
            {
                str[position] = (char)(i + 48);
                if (Recursive(str, position + 1))
                {
                    return true;
                }
            }
        }
        str[position] = '0';

        return false;

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





}

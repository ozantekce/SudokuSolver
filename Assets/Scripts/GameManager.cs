using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static Slot SelectedSlot { get; set; }
    public Slot[] Slots { get; private set; }
    private Helper Helper { get; set; }


    private int[] _initialBoard;
    private int[] _finalBoard;
    private bool _isFound = false;

    [SerializeField] private GameObject[] _slots;
    [SerializeField] private GameObject _invalidText;

    private int[] _randomIntValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    private void Start()
    {
        InitializeGame();
    }

    private void Update()
    {
        HandleInput();
    }

    private void InitializeGame()
    {
        Helper = new Helper();
        Helper.Configurations();

        Slots = new Slot[_slots.Length];
        for (int i = 0; i < _slots.Length; i++)
        {
            int index = i;
            _slots[i].GetComponent<Button>().onClick.AddListener(() => OnClickSlot(index));
            Slots[i] = _slots[i].AddComponent<Slot>();
            Slots[i].Create();
        }
    }

    private void HandleInput()
    {
        if (SelectedSlot == null) return;

        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()) || Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), "Keypad" + (i + 1))))
            {
                SelectedSlot.Value = i + 1;
                break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Delete))
        {
            SelectedSlot.Value = 0;
        }
    }

    public void OnClickSlot(int slotIndex)
    {
        Debug.Log(slotIndex);
        SelectedSlot = Slots[slotIndex];
    }

    public void OnClickResetButton()
    {
        foreach (var slot in Slots)
        {
            slot.Create();
        }
    }

    public void OnClickCreateButton(InputField inputField)
    {
        if (string.IsNullOrEmpty(inputField.text)) return;

        int fullSlots = int.Parse(inputField.text);
        CreateRandomBoard(fullSlots);
    }

    private void CreateRandomBoard(int fullSlots)
    {
        int emptySlots = 81 - fullSlots;
        int[] randomSlots = Enumerable.Range(0, 81).ToArray();
        _initialBoard = new int[81];

        System.Random rnd = new System.Random();
        randomSlots = randomSlots.OrderBy(x => rnd.Next()).ToArray();

        Solve();
        ApplyBoardToSlots(randomSlots, emptySlots);
    }

    private void ApplyBoardToSlots(int[] randomSlots, int emptySlots)
    {
        int[] board = _finalBoard.ToArray();
        for (int i = 0; i < emptySlots; i++)
        {
            board[randomSlots[i]] = 0;
        }

        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].Value = board[i];
        }

        _isFound = false;
    }

    public void OnClickSolveButton()
    {
        CreateInitialBoard();
        if (!Helper.IsValidGame(_initialBoard))
        {
            Debug.Log("Not valid");
            StartCoroutine(ShowTextRoutine());
            OnClickResetButton();
            return;
        }

        SolveRoutine();
    }

    private void CreateInitialBoard()
    {
        _initialBoard = Slots.Select(slot => slot.Value).ToArray();
    }

    private void SolveRoutine()
    {
        Solve();
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].Value = _finalBoard[i];
        }
        _isFound = false;
    }

    private void Solve()
    {
        System.Random rnd = new System.Random();
        _randomIntValues = _randomIntValues.OrderBy(x => rnd.Next()).ToArray();
        int[] board = new int[_initialBoard.Length];
        Array.Copy(_initialBoard, board, _initialBoard.Length);

        RecursiveSolve(board, 0);
    }


    private void SolveBack()
    {
        System.Random rnd = new System.Random();
        _randomIntValues = _randomIntValues.OrderBy(x => rnd.Next()).ToArray();
        int[] board = new int[_initialBoard.Length];
        Array.Copy(_initialBoard, board, _initialBoard.Length);

        RecursiveSolveBack(board, 80);
    }

    private void SetFinalBoard(int[] board)
    {
        if (_isFound) return;

        _finalBoard = new int[board.Length];
        Array.Copy(board, _finalBoard, board.Length);
        _isFound = true;
    }

    private bool RecursiveSolve(int[] board, int position)
    {
        if (_isFound) 
            return true;

        if (position >= 81)
        {
            SetFinalBoard(board);
            return true;
        }

        if (board[position] != 0)
            return RecursiveSolve(board, position + 1);

        foreach (int value in _randomIntValues)
        {
            if (Helper.IsValid(board, position, value))
            {
                board[position] = value;
                if (RecursiveSolve(board, position + 1))
                    return true;
            }
        }

        board[position] = 0;
        return false;
    }

    private bool RecursiveSolveBack(int[] board, int position)
    {
        if (_isFound)
            return true;


        if (position < 0)
        {
            SetFinalBoard(board);
            return true;
        }

        if (board[position] != 0)
        {
            return RecursiveSolveBack(board, position - 1);
        }

        foreach (int value in _randomIntValues)
        {
            if (Helper.IsValid(board, position, value))
            {
                board[position] = value;
                if (RecursiveSolveBack(board, position - 1))
                {
                    return true;
                }
            }
        }

        board[position] = 0;
        return false;
    }




    private IEnumerator ShowTextRoutine()
    {
        _invalidText.SetActive(true);
        yield return new WaitForSeconds(1f);
        _invalidText.SetActive(false);
    }

}

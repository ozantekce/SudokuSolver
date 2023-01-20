using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler
{


    private int _value;

    private ISet<int> _potentials;

    public Text valueText;

    public ISet<int> Potentials { get => _potentials; set => _potentials = value; }
    public int Value { 

        get => _value;
        set { 
            _value = value;
            if (_value == 0)
            {
                valueText.text = "";
            }
            else
            {
                valueText.text = value.ToString();
            }
        }

    }

    public void Create()
    {

        _potentials = new HashSet<int>();
        for (int i = 1; i < 10; i++)
        {
            _potentials.Add(i);
        }

        if(valueText == null)
            valueText = GetComponentInChildren<Text>();

        Value = 0;

    }

    public void Create(int value)
    {
        _value = value;
        _potentials = new HashSet<int>();
        _potentials.Add(value);
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.SelectedSlot = this;
    }
}

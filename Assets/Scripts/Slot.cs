using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, ISelectHandler
{


    private int _value;


    public Text valueText;

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

        if(valueText == null)
            valueText = GetComponentInChildren<Text>();

        Value = 0;

    }

    public void Create(int value)
    {
        _value = value;
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameManager.SelectedSlot = this;
    }
}

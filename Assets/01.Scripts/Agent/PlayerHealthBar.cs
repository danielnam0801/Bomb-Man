using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHealthBar
{
    private VisualElement _barRect;
    private VisualElement _bar;
    private Label _hpLabel;

    private int _currentHP;
    public int HP
    {
        set
        {
            _currentHP = value;
            UpdateHPText();
        }
    }
    private int _maxHP;
    public int MaxHP
    {
        set
        {
            _currentHP = _maxHP = value;
            UpdateHPText();
        }
    }

    private void UpdateHPText()
    {
        Debug.Log("Bar: " + _bar);
        Debug.Log("CurrentHP : "+ _currentHP);
        _bar.transform.scale = new Vector3((float)_currentHP / _maxHP, 1, 0);
        _hpLabel.text = $"{_currentHP} / {_maxHP}";
    }

    public PlayerHealthBar(VisualElement bar)
    {
        _barRect = bar;
        _bar = bar.Q<VisualElement>("Bar");
        _hpLabel = bar.Q<Label>("HPLabel");
    }

    public void ShowBar(bool value)
    {
        if (value)
            _barRect.AddToClassList("on");
        else
            _barRect.RemoveFromClassList("on");
    }
}

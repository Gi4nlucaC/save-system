using System;
using UnityEngine;

[Serializable]
public class UniqueGUID
{
    [SerializeField] private string _value;
    public string Value => _value;
    public bool IsValid => !string.IsNullOrEmpty(_value);
}
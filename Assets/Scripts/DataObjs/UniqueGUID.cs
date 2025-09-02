using System;
using UnityEngine;

[Serializable]
public class UniqueGUID
{
    [SerializeField] private string _value;
    public string Value => _value;
    public bool IsValid => !string.IsNullOrEmpty(_value);
    public void Generate() => _value = Guid.NewGuid().ToString("N");
    public override string ToString() => _value;
}
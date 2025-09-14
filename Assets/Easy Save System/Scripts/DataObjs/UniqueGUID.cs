using System;
using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    [Serializable]
    public class UniqueGUID
    {
        [SerializeField] private string _value;
        public string Value => _value;
        public bool IsValid => !string.IsNullOrEmpty(_value);
        public void Set(string value)
        {
            _value = value;
        }
    }
}
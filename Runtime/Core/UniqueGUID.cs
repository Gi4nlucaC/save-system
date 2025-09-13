using System;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Serializable unique identifier for Unity objects
    /// </summary>
    [Serializable]
    public class UniqueGUID
    {
        [SerializeField] private string _value;
        
        /// <summary>
        /// The GUID value
        /// </summary>
        public string Value => _value;
        
        /// <summary>
        /// Whether this GUID has a valid value
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(_value);
        
        /// <summary>
        /// Set a new GUID value
        /// </summary>
        /// <param name="value">New GUID value</param>
        public void Set(string value)
        {
            _value = value;
        }
    }
}
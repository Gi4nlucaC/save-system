using UnityEngine;

namespace PizzaCompany.SaveSystem
{
    [CreateAssetMenu(fileName = "CloudSaveCredentials", menuName = "Save System/Cloud Save Credentials", order = 0)]
    public class CloudSaveCredentials : ScriptableObject
    {
        [SerializeField] string _username;
        [SerializeField] string _password;
        [SerializeField] string _clusterName;

        public string Username => _username;
        public string Password => _password;
        public string ClusterName => _clusterName;

        public bool HasValidCredentials =>
            !string.IsNullOrWhiteSpace(_username) &&
            !string.IsNullOrWhiteSpace(_password) &&
            !string.IsNullOrWhiteSpace(_clusterName);

#if UNITY_EDITOR
        [ContextMenu("Clear Credentials")]
        void ClearCredentials()
        {
            _username = string.Empty;
            _password = string.Empty;
            _clusterName = string.Empty;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}

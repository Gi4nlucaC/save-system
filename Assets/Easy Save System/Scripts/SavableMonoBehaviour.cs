using UnityEngine;
using PizzaCompany.SaveSystem;

namespace PizzaCompany.SaveSystem
{
    public class SavableMonoBehaviour : MonoBehaviour, ISavable
    {
        [SerializeField] protected UniqueGUID _persistentId;
        public string PersistentId => _persistentId != null && _persistentId.IsValid ? _persistentId.Value : null;

#if UNITY_EDITOR
        [ExecuteInEditMode]
        private void OnValidate() { Generate(); }
        [ExecuteInEditMode]
        private void Generate()
        {
            if (_persistentId == null)
                _persistentId = new UniqueGUID();
            if (!_persistentId.IsValid)
                _persistentId.Set(System.Guid.NewGuid().ToString("N"));
        }
#endif
        protected void RegisterForSave()
        {
            if (!string.IsNullOrEmpty(PersistentId))
                SaveSystemManager.RegisterSavable(this);
            else
                Debug.LogWarning($"{name}: PersistentId not generated. Click 'Generate' on the UniqueGUID field or use the context menu.");
        }
        public virtual PureRawData SaveData() { throw new System.NotImplementedException(); }
        public virtual void LoadData() { throw new System.NotImplementedException(); }
        public virtual void DeleteData() { throw new System.NotImplementedException(); }
        public virtual void SnapshotData() { throw new System.NotImplementedException(); }
    }
}

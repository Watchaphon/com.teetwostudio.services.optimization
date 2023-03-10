using UnityEngine;

namespace Services.Optimization.PoolingSystem
{
    [CreateAssetMenu(fileName = "PoolingProfile", menuName = "Optimization/PoolingProfile")]
    public class PoolProfile : ScriptableObject
    {
        [SerializeField] private string _customID;
        [Space]
        [SerializeField] private PoolingObject _prefab;
        [SerializeField] private int _amount = 1;

        public float lifeTime = 1.5f;

        public PoolingObject Prefab
        {
            get => _prefab;
        }

        public string ID
        {
            get => _customID == string.Empty ? name : _customID;
        }

        public int Amount
        {
            get => _amount < 1 ? 1 : _amount;
        }
    }
}
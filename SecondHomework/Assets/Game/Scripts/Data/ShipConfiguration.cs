using Game.Data.VFX;
using Game.Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "Game/ShipData", order = 0)]
    public class ShipConfiguration : ScriptableObject
    {
        [Header("Core")]
        
        [SerializeField] private TeamType team = TeamType.None;
        public TeamType Team => team;
        
        [field: SerializeField]
        public int Health { get; private set; } = 5;

        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 5;

        [field: SerializeField]
        public float FireCooldown { get; private set; } = 0.25f;
        
        [FormerlySerializedAs("vfxData")] [SerializeField] private VFXConfiguration vfxConfiguration;
        public VFXConfiguration VFXConfiguration => vfxConfiguration;
    }
}
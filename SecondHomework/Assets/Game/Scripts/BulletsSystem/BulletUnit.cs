using System;
using Game.Enums;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletUnit : MonoBehaviour
    {
        public event Action<BulletUnit, Collider2D> OnTriggerEntered;
        public Vector2 Direction { get; private set; }
        public int Damage { get; private set; }
        public float Speed { get; private set; }

        private TeamType team = TeamType.None;
        [SerializeField] private GameObject blueVFX;
        [SerializeField] private GameObject redVFX;

        public TeamType GetTeam() => team;

        public void SetData(BulletConfiguration config)
        {
            Direction = config.Direction;
            Speed = config.Speed;
            Damage = config.Damage;
            team = config.Team;
            transform.position = config.Position;
            transform.rotation = Quaternion.LookRotation(config.Direction, Vector3.forward);
            gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask);

            blueVFX?.SetActive(team == TeamType.Player ? true : false);
            redVFX?.SetActive(team == TeamType.Player ? false : true);
        }

        private void OnTriggerEnter2D(Collider2D other) => OnTriggerEntered?.Invoke(this, other);
    }
}
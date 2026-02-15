using System;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletData : MonoBehaviour
    {
        public event Action<BulletData, Collider2D> OnTriggerEntered;
        public Vector2 direction { get; private set; }
        public int damage { get; private set; }
        public float speed { get; private set; }
        
        private TeamType team = TeamType.None;
        [SerializeField] private GameObject blueVFX;
        [SerializeField] private GameObject redVFX;
        
        public TeamType GetTeam() => team;

        public void SetData(BulletConfiguration config)
        {
            direction = config.Direction;
            speed = config.Speed;
            damage = config.Damage;
            team = config.Team;
            transform.position = config.Position;
            transform.rotation = Quaternion.LookRotation(config.Direction, Vector3.forward);
            gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask);

            blueVFX?.SetActive(team == TeamType.Player ? true : false);
            redVFX?.SetActive(team == TeamType.Player ? false : true);
        }

        private void OnTriggerEnter2D(Collider2D other) => this.OnTriggerEntered?.Invoke(this, other);
    }
}
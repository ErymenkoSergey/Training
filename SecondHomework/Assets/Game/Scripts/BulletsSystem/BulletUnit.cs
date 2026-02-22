using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class BulletUnit : MonoBehaviour
    {
        private Vector2 Direction;
        private int Damage;
        private float Speed;
        private TeamType team = TeamType.None;
        private TransformBounds levelBounds;
        private IPoolable poolable;

        [SerializeField] private GameObject blueVFX;
        [SerializeField] private GameObject redVFX;

        public void SetData(BulletConfiguration config)
        {
            Direction = config.Direction;
            Speed = config.Speed;
            Damage = config.Damage;
            team = config.Team;
            transform.position = config.Position;
            transform.rotation = Quaternion.LookRotation(config.Direction, Vector3.forward);
            gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask);
            levelBounds = config.Bounds;
            poolable = config.Pool;
            ShowVFX(team);
        }
        
        public TeamType GetTeam() => team;

        private void ShowVFX(TeamType team)
        {
            bool isPlayer = team == TeamType.Player ? true : false;

            if (isPlayer)
            {
                blueVFX.SetActive(true);
                redVFX.SetActive(false);
            }
            else
            {
                blueVFX.SetActive(false);
                redVFX.SetActive(true);
            }
        }

        private void FixedUpdate()
        {
            Vector3 moveStep = Direction * Speed * Time.fixedDeltaTime;
            transform.position += moveStep;

            if (!levelBounds.InBounds(transform.position))
            {
                gameObject.SetActive(false);
                poolable.ReturnToPool(this);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHealth ship))
                return;

            poolable.ReturnToPool(this, TakeDamage(Damage, ship));
        }

        private bool TakeDamage(int damage, IHealth ship)
        {
            if (damage > 0)
            {
                ship.CurrentHealth =
                    Mathf.Clamp(ship.CurrentHealth - damage, ship.DeadValueHealth, ship.CurrentMaxHealth);
                ship.SetDamage(ship.CurrentHealth);

                if (ship.CurrentHealth <= ship.DeadValueHealth)
                    ship.NotifyAboutDead();
                
                return true;
            }
            return false;
        }
    }
}
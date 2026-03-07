using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class Bullet : MonoBehaviour
    {
        private Vector2 direction;
        private int damage;
        private float speed;
        private TeamType team = TeamType.None;
        private TransformBounds levelBounds; // нужна фабрика для прокидывания зависимости!!!
        private IPool pool; // Переделать на подписку) 
        
        public void SetData(BulletConfiguration config)
        {
            direction = config.Direction;
            speed = config.Speed;
            damage = config.Damage;
            team = config.Team;
            transform.position = config.Position;
            transform.rotation = Quaternion.LookRotation(config.Direction, Vector3.forward);
            gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask); // Вынести в конфиг!! 
            levelBounds = config.Bounds;
            pool = config.Pool;
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
            Vector3 moveStep = direction * speed * Time.fixedDeltaTime;
            transform.position += moveStep;

            if (!levelBounds.InBounds(transform.position))
            {
                gameObject.SetActive(false);
                pool.Return(this);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHealth ship))
                return;

            bool wasDamage = TakeDamage(ship);
            pool.Return(this, wasDamage);
        }

        private bool TakeDamage(IHealth ship)
        {
            if (damage > 0)
            {
               ship.SetDamage(damage);
                return true;
            }
            return false;
        }
    }
}
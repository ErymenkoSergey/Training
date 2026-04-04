using Game.Data.VFX;
using Game.Enums;
using Game.Interfaces;
using Modules.Utils;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private Vector2 direction;
        [SerializeField]private int damage;
        [SerializeField]private float speed;
        [SerializeField] private TeamType team = TeamType.None;
        [SerializeField] private TransformBounds levelBounds;
        [SerializeField] private IPool<Bullet> pool;
        [SerializeField] private BulletConfiguration config;

        private IFlickerVFX flickerVFX;

        public void Initialize(BulletConfiguration config, IFlickerVFX flickerVFX)
        {
            Debug.Log("Bullet initialized");
            this.flickerVFX = flickerVFX;
            this.config = config;
        }

        public void SetBounds(TransformBounds levelBounds)
        {Debug.Log("Bullet SetBounds");
            this.levelBounds = levelBounds;
        }

        // индивидуальные настройки для пуль для игрока или для енеми
        private void OnEnable()
        {
            if (config != null)
            {Debug.Log("Bullet OnEnable");
                speed = config.Speed;
                damage = config.Damage;
                team = config.Team;
                gameObject.layer = LayerMask.NameToLayer(config.BulletNameMask); // Вынести в конфиг!! 
            }
        }

        /// <summary>
        /// конфигурируется конкретным кораблём прям перед выстрелом.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="position"></param>
        /// <param name="pool"></param>
        public void SetData(BulletNavigation navigation, IPool<Bullet> pool)
        {Debug.Log("Bullet SetData");
            direction = navigation.Direction;
            transform.position = navigation.Position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.forward); // это тоже передавать отдельно.
            this.pool = pool;
            // Instantiate(flickerVFX.GetFlickerVFX(team), transform.position, transform.rotation).transform
            //     .SetParent(transform);
        }

        // public TeamType GetTeam() => team;

        private void FixedUpdate()
        {
            Vector3 moveStep = direction * speed * Time.fixedDeltaTime;
            transform.position += moveStep;

            if (!levelBounds.InBounds(transform.position))
            {
                //gameObject.SetActive(false);
                pool.Return(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IHealth ship))
                return;
            TakeDamage(ship);
            pool.Return(this);
        }

        private void TakeDamage(IHealth ship)
        {
            if (damage > 0)
                ship.SetDamage(damage);
        }
    }
}
using Game.Mechanics.BulletsSystem.Data;
using Game.Mechanics.Ship;
using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class PlayerBulletInstantiator : MonoBehaviour
    {
        [SerializeField]
        private string playerMask = "PlayerBullet";

        [SerializeField]
        private BaseShip _player;

        private void OnEnable()
        {
            _player.OnFire += this.OnFire; //go to Input system??
        }

        private void OnDisable()
        {
            _player.OnFire -= this.OnFire;
        }

        private void OnFire(BaseShip _)
        {
            _player.Gunner.Shoot(GetBulletConfiguration());
        }

        private BulletConfiguration GetBulletConfiguration()
        {
            BulletConfiguration bulletConfiguration = new BulletConfiguration();
            bulletConfiguration.Position = _player.firePoint.position;
            bulletConfiguration.Direction = _player.firePoint.up;
            bulletConfiguration.Speed = _player.bulletSpeed;
            bulletConfiguration.Damage = _player.bulletDamage;
            bulletConfiguration.Team = TeamType.Player;
            bulletConfiguration.BulletNameMask = playerMask;
            return bulletConfiguration;
        }
    }
}
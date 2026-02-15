using UnityEngine;

namespace Game.Mechanics.BulletsSystem
{
    public sealed class PlayerBulletInstantiator : MonoBehaviour
    {
        [SerializeField]
        private BulletHandler _bulletWorld;

        [SerializeField]
        private string playerMask = "PlayerBullet";

        [SerializeField]
        private PlayerShip _player;

        private void OnEnable()
        {
            _player.OnFire += this.OnFire; //go to Input system??
        }

        private void OnDisable()
        {
            _player.OnFire -= this.OnFire;
        }

        private void OnFire(ShipController _)
        {
            _bulletWorld.Spawn(GetBulletConfiguration());
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
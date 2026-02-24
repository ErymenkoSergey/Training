using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Mechanics.Ship
{
    [Serializable]
    public sealed class ShootingOnCooldown
    {
        [SerializeField] private float _fireCooldown = 1.25f; // fire config 
        private Transform targetTransform;
        private Transform firePoint;
        private float fireTime;

        private IShot iShot;

        public void SetData(Transform firePoint, Transform targget, float fireTime, IShot iShot)
        {
            this.firePoint = firePoint;
            this.targetTransform = targget;
            this.fireTime = fireTime;
            this.iShot = iShot;
        }
        
        public void ShootingCooldown()
        {
            float time = Time.time;
            if (time - fireTime >= _fireCooldown)
            {
                iShot.Fire(GetTarget());
                fireTime = time;
            }
        }
        
        private Vector3 GetTarget()  => (targetTransform.position - firePoint.position).normalized;// точка получения цели - куда стреляем
    }
}
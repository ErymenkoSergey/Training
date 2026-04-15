using System;
using DG.Tweening;
using Game.Data;
using UnityEngine;

namespace Game.Mechanics.Configuration
{
    [Serializable]
    public sealed class VisualConfiguration
    {
        [SerializeField] private VisualConfig visualConfig;
        [SerializeField] private Renderer _renderer;
        [SerializeField] private Transform _viewTransform;
        [SerializeField] private ParticleSystem _fireVFX;
        private Material _material;
        private Tweener _damageAnimation;

        public void VisualStart()
        {
            _material = new Material(visualConfig.MaterialPrefab);
            _renderer.material = _material;
        }

        public void ShowFireVFX() => _fireVFX?.Play();

        public void AnimateMovement(float deltaTime, Vector3 moveDirection)
        {
            Vector3 shipAngles = _viewTransform.localEulerAngles;
            shipAngles.x = visualConfig.MoveRotationAngle * moveDirection.y;
            shipAngles.y = visualConfig.MoveRotationAngle / 2 * moveDirection.x * -1f;

            Quaternion shipRotation = Quaternion.Euler(shipAngles);
            float t = visualConfig.MoveSpeed * deltaTime;
            _viewTransform.localRotation = Quaternion.Lerp(_viewTransform.localRotation, shipRotation, t);
        }

        public void AnimateDamage()
        {
            if (_damageAnimation.IsActive())
                _damageAnimation.Kill();

            _damageAnimation = DOVirtual.Float(
                0f,
                1f,
                visualConfig.HitDuration,
                progress => _material?.SetFloat(visualConfig.HitPropertyName,
                    visualConfig.HitAnimationCurve.Evaluate(progress))
            ).SetLink(_renderer.gameObject);
        }
    }
}
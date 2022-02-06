using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows.Combat.Projectiles
{
    public class ProjectileRaycast : MonoBehaviour, IProjectile
    {
        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private LayerMask projectileColliderMask;

        private Vector3 targetPosition;
        private Transform target;
        private float damage;

        public void Fire(Vector3 position, Vector3 direction, float damage)
        {
            this.damage = damage;
            //transform.position = position;
            //transform.forward = direction;

            if (Physics.Raycast(position, direction, out RaycastHit raycastHit, 999f, projectileColliderMask))
            {
                targetPosition = raycastHit.point;
                target = raycastHit.transform;
            }
        }

        private void Update()
        {
            float distanceBefore = Vector3.Distance(transform.position, targetPosition);

            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);

            float distanceAfter = Vector3.Distance(transform.position, targetPosition);

            if (distanceAfter > distanceBefore)
            {
                if (target.TryGetComponent(out IHurtable hurtable))
                {
                    hurtable.Hurt(damage);
                }
                Destroy(gameObject);
            }
        }
    }
}
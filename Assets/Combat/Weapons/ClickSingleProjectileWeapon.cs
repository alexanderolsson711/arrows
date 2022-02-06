using Arrows.Combat.Projectiles;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows.Combat.Weapons
{
    public class ClickSingleProjectileWeapon : MonoBehaviour, IWeapon
    {
        [Header("Stats")]
        [SerializeField] private float damage = 1f;
        [SerializeField] private float shootCooldown = 0.5f;
        [SerializeField] private float shootDelay = 0.25f;

        [Header("Projectile")]
        [SerializeField] private Transform spawnProjectilePosition;
        [SerializeField] private GameObject projectilePrefab;

        private StarterAssetsInputs starterAssetsInputs;
        private Animator animator;
        private int animIDShoot;

        private float lastShot;

        public void Attack(Vector3 attackPoint)
        {
            if (Time.time - lastShot > shootCooldown)
            {
                lastShot = Time.time + shootDelay;
                animator.SetTrigger(animIDShoot);
                StartCoroutine(FireAfterDelay(attackPoint));
            }
            starterAssetsInputs.shoot = false;
        }

        private void Awake()
        {
            starterAssetsInputs = GetComponent<StarterAssetsInputs>();
            animator = GetComponent<Animator>();
            animIDShoot = Animator.StringToHash("Shoot");
        }

        private IEnumerator FireAfterDelay(Vector3 attackPoing)
        {
            Vector3 direction = (attackPoing - spawnProjectilePosition.position).normalized;
            yield return new WaitForSeconds(shootDelay);
            GameObject projectile = Instantiate(projectilePrefab,
                                                spawnProjectilePosition.position,
                                                Quaternion.LookRotation(direction));
            projectile.GetComponent<IProjectile>().Fire(spawnProjectilePosition.position, direction, damage);
        }
    }
}
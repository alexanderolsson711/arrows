using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using Arrows.Utils;
using Arrows.Combat.Weapons;

public class ThirdPersonShooterController : MonoBehaviour
{
    private const int AIM_LAYER = 1;
    private const float AIM_SPEED = 10f;
    private const float TURN_SPEED = 20f;

    [Header("Parameters")]
    [SerializeField] private float normalSensitivity = 1.0f;
    [SerializeField] private float aimSensitivity = 0.5f;

    [Header("Components")]
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;

    private IWeapon weapon;
    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;
    private MouseHandler mouseHandler;

    private bool isAiming;

    private void Awake()
    {
        weapon = GetComponent<IWeapon>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        mouseHandler = MouseHandler.Instance;
    }

    private void Update()
    {
        if (isAiming)
        {
            if (!starterAssetsInputs.aim)
            {
                StopAiming();
            }
            else
            {
                TurnTowardsAim();
                if (starterAssetsInputs.shoot)
                {
                    weapon.Attack((mouseHandler.MouseWorldPosition - transform.position).normalized);
                }
            }        
        }
        else
        {
            if (starterAssetsInputs.aim)
            {
                // Remove saved shoot value
                starterAssetsInputs.shoot = false;
                StartAiming();
            }
        }

        SetAimLayerWeight(isAiming ? 1 : 0);
    }

    private void StartAiming()
    {
        isAiming = true;
        aimVirtualCamera.gameObject.SetActive(true);
        thirdPersonController.SetSensitivity(aimSensitivity);
        thirdPersonController.SetRotateOnMove(false);
    }

    private void StopAiming()
    {
        isAiming = false;
        aimVirtualCamera.gameObject.SetActive(false);
        thirdPersonController.SetSensitivity(normalSensitivity);
        thirdPersonController.SetRotateOnMove(true);
    }

    private void TurnTowardsAim()
    {
        Vector3 worldAimTarget = mouseHandler.MouseWorldPosition;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * TURN_SPEED);
    }

    private void SetAimLayerWeight(float goalWeight)
    {
        float aimLayerWeight = animator.GetLayerWeight(AIM_LAYER);
        aimLayerWeight = Mathf.Lerp(aimLayerWeight, goalWeight, Time.deltaTime * AIM_SPEED);
        animator.SetLayerWeight(AIM_LAYER, aimLayerWeight);
    }
}

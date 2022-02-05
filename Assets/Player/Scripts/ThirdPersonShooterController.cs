using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

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

    [Header("Raycasts")]
    [SerializeField] private LayerMask aimColliderMask;
    [SerializeField] private Transform debugTransform;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Animator animator;

    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCeneterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCeneterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (starterAssetsInputs.aim)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            animator.SetLayerWeight(AIM_LAYER, Mathf.Lerp(animator.GetLayerWeight(AIM_LAYER), 1, Time.deltaTime * AIM_SPEED));

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * TURN_SPEED);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            animator.SetLayerWeight(AIM_LAYER, Mathf.Lerp(animator.GetLayerWeight(AIM_LAYER), 0, Time.deltaTime * AIM_SPEED));
        }

        if (starterAssetsInputs.shoot)
        {
            if (hitTransform != null)
            {
                Debug.Log("PACNG");
            }
            starterAssetsInputs.shoot = false;
        }
    }
}

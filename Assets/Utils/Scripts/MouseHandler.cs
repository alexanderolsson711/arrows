using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arrows.Utils
{
    public class MouseHandler : MonoBehaviour
    {
        public static MouseHandler Instance { get; private set; }

        public Vector3 MouseWorldPosition { get; private set; }
        public Transform MouseHitTransform { get; private set; }

        [SerializeField] private LayerMask mouseColliderMask;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            Vector2 screenCeneterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Ray ray = Camera.main.ScreenPointToRay(screenCeneterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderMask))
            {
                MouseWorldPosition = raycastHit.point;
                MouseHitTransform = raycastHit.transform;
            }
        }
    }
}
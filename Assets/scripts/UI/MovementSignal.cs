using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Box.UI
{

    public class MovementSignal : MonoBehaviour
    {
        [SerializeField] BodyPart.types type;
        [SerializeField] Transform target;

        void Start()
        {
            Events.SetMovementSignal += SetMovementSignal;
        }

        void OnDestroy()
        {
            Events.SetMovementSignal -= SetMovementSignal;
        }

        private void SetMovementSignal(Vector2 pos, BodyPart bodyPart)
        {
            if (type != bodyPart.type) return;
            target = bodyPart.transform;
        }

        private void Update()
        {
            if (target == null) return;
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }
    }
}

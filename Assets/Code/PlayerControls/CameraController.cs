using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Assets
{
    public class CameraController : MonoBehaviour
    {
        public Camera Camera;
        public Vector3 LookTarget = Vector3.zero;

        public float FollowSpeed = .05f;
        public float Speed          = 5.0f;
        public float InputVertical;
        public float InputHorizontal;
        public float InputScroll;

        public float ScrollSpeed;

        private void Awake()
        {
            LookTarget.z = gameObject.transform.position.z;
        }

        private void Update()
        {
            InputHorizontal = Input.GetAxis("Horizontal");
            InputVertical = Input.GetAxis("Vertical");

            var direction = new Vector2(InputHorizontal, InputVertical);
            LookTarget += (Vector3)direction * Speed * Time.deltaTime;

            var distance = Vector3.Distance(gameObject.transform.position, LookTarget);
            if(distance > .05f)
            {
                gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, LookTarget, FollowSpeed);
            }

            InputScroll = Input.GetAxis("Mouse ScrollWheel");

            Camera.orthographicSize -= InputScroll * Time.deltaTime * ScrollSpeed;
        }
    }
}
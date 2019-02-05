using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class PawnComponent : MonoBehaviour
    {
        public float StepSpeed          = 1.0f;
        public float TimeBetweenSteps   = 1.0f;

        public Coroutine Attack(Vector2 position)
        {
            var attackPath = new List<Vector2>()
            {
                position,
                gameObject.transform.position
            };

            return StartCoroutine(Move_Coroutine(attackPath));
        }

        public Coroutine Move(List<Vector2> path)
        {
            return StartCoroutine(Move_Coroutine(path));
        }

        public IEnumerator Move_Coroutine(List<Vector2> path)
        {
            while (path.Count > 0)
            {
                var target = path[0];

                var startTime = Time.time;
                var endTime = startTime + StepSpeed;

                while(Time.time < endTime)
                {
                    var percent = (Time.time - startTime) / StepSpeed;
                    gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target, percent);
                    yield return null;
                }

                path.RemoveAt(0);

                yield return new WaitForSeconds(TimeBetweenSteps);
            }

            gameObject.transform.position = new Vector3(
                Mathf.Round(gameObject.transform.position.x),
                Mathf.Round(gameObject.transform.position.y),
                gameObject.transform.position.z
            );

            yield return null;
        }
    }
}
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Pathfinding;

namespace Game.Assets
{
    public class MouseController : MonoBehaviour
    {
        public GameObject HighlightPrefab;

        public GameObject HighlightObject;

        public Action<NodeData> OnTerrainChange;

        protected Point _previousPoint = new Point(-1, -1);

        private void Awake()
        {
            HighlightObject = GameObject.Instantiate(HighlightPrefab, Vector3.zero, Quaternion.identity);
        }

        private void Start()
        {
            OnTerrainChange += UIController.Instance.SetTerrainData;
        }

        private void Update()
        {
            var hoverPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var point = new Point((int)Mathf.Round(hoverPosition.x), (int)Mathf.Round(hoverPosition.y));

            var map = GameManager.Instance.LevelData.Map;
            var isOnMap = map.IsOnMap(point);

            if (point.X != _previousPoint.X || point.Y != _previousPoint.Y)
            {
                _previousPoint = point;
                NodeData terrainData = null;

                if (isOnMap)
                {
                    HighlightObject.transform.position = new Vector3(point.X, point.Y, -1.0f);
                    terrainData = map.GetData(point);
                }

                HighlightObject.SetActive(isOnMap);
                OnTerrainChange?.Invoke(terrainData);
            }

            if (isOnMap)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var pawn = GameManager.Instance.Board.Pawn;
                    var pawnComponent = GameManager.Instance.Board.ComponentForPawn(pawn);
                    var start = new Point((int)pawnComponent.transform.position.x, (int)pawnComponent.transform.position.y);
                    var path = map.GetPath(start, point, pawn);

                    path.Reverse();
                    pawnComponent.Move(path.Select(node => new Vector2(node.X, node.Y)).ToList());
                }
            }
        }
    }
}
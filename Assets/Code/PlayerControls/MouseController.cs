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

        public GameObject CursorHighlight;
        public GameObject UnitHighlight;

        public Action<NodeData>     OnTerrainChange;
        public Action<Pawn>         OnPawnChange;
        public Action<Pawn>         OnSelectedChange;

        protected Pawn _selectedUnit;
        protected Pawn _highlightedPawn;
        protected NodeData _highlightedTerrain;
        protected Action<Point, Pawn, NodeData> _state;
        protected Player _owner;

        protected Point _previousPoint = new Point(-1, -1);

        private void Awake()
        {
            CursorHighlight = GameObject.Instantiate(HighlightPrefab, Vector3.zero, Quaternion.identity);
            UnitHighlight = GameObject.Instantiate(HighlightPrefab, Vector3.zero, Quaternion.identity);
            _state = State_NoUnitSelected;
        }

        private void Start()
        {
            OnTerrainChange += UIController.Instance.SetTerrainData;
            OnPawnChange += UIController.Instance.SetPawnData;
            OnSelectedChange += UIController.Instance.SetSelectedPawn;
            _owner = GameManager.Instance.LevelData.Players.Where(player => player.IsHuman).First();
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(1))
            {
                _owner.EndTurn();
            }

            var hoverPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var point = new Point((int)Mathf.Round(hoverPosition.x), (int)Mathf.Round(hoverPosition.y));

            var map = GameManager.Instance.LevelData.Map;
            var isOnMap = map.IsOnMap(point);

            if (point.X != _previousPoint.X || point.Y != _previousPoint.Y)
            {
                _previousPoint = point;
                if (isOnMap)
                {
                    CursorHighlight.transform.position = new Vector3(point.X, point.Y, -1.0f);
                    _highlightedTerrain = map.GetData(point);

                    var pawns = map.GetAgent(point);
                    _highlightedPawn = pawns.Count > 0 ? (Pawn)pawns[0] : null;

                    OnTerrainChange?.Invoke(_highlightedTerrain);
                }
                else
                {
                    OnTerrainChange?.Invoke(null);
                }

                CursorHighlight.SetActive(isOnMap);
                OnPawnChange?.Invoke(_highlightedPawn);
            }

            if (isOnMap)
            {
                _state(point, _highlightedPawn, _highlightedTerrain);
            }
        }

        protected void State_NoUnitSelected(Point point, Pawn pawn, NodeData nodeData)
        {
            var map = GameManager.Instance.LevelData.Map;

            if (Input.GetMouseButtonDown(0))
            {
                if (pawn != null && pawn.Owner == _owner)
                {
                    _selectedUnit = pawn;
                    _state = State_UnitSelected;
                    OnSelectedChange?.Invoke(_selectedUnit);
                }
            }
        }

        protected void State_UnitSelected(Point point, Pawn pawn, NodeData nodeData)
        {
            var map = GameManager.Instance.LevelData.Map;

            if (Input.GetMouseButtonDown(0))
            {
                var pawnComponent = _selectedUnit.PawnComponent;
                var start = new Point((int)pawnComponent.transform.position.x, (int)pawnComponent.transform.position.y);
                var path = map.GetPath(start, point, _selectedUnit);
                _selectedUnit.X = point.X;
                _selectedUnit.Y = point.Y;

                pawnComponent.Move(path.Select(node => new Vector2(node.X, node.Y)).ToList());

                _selectedUnit = null;
                _state = State_NoUnitSelected;
                OnSelectedChange?.Invoke(_selectedUnit);
            }
        }
    }
}
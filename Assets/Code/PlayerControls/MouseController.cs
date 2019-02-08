using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Game.Pathfinding;

namespace Game.Assets
{
    public class MouseController : MonoBehaviour
    {
        public GameObject SelectorPrefab;
        public GameObject HighlightMovePrefab;
        public GameObject HighlightAttackPrefab;

        public Button EndTurnButton;

        public GameObject Selector;

        public Action<NodeData>     OnTerrainChange;
        public Action<Pawn>         OnPawnChange;
        public Action<Pawn>         OnSelectedChange;

        protected Pawn _selectedUnit;
        protected Pawn _highlightedPawn;
        protected NodeData _highlightedTerrain;
        protected Action<Point, Pawn, NodeData> _state;
        protected Player _owner;

        protected List<GameObject> _actionGraphics = new List<GameObject>();
        protected List<ActionInfo> _actionsForSelectedPawn = new List<ActionInfo>();

        protected Point _previousPoint = new Point(-1, -1);
        protected Map<NodeData> _map;

        protected bool _allowInput;

        private void Awake()
        {
            Selector = GameObject.Instantiate(SelectorPrefab, Vector3.zero, Quaternion.identity);
            _state = State_NoUnitSelected;
        }

        private void Start()
        {
            OnTerrainChange += UIController.Instance.SetTerrainData;
            OnPawnChange += UIController.Instance.SetPawnData;
            OnSelectedChange += UIController.Instance.SetSelectedPawn;
            _owner = GameManager.Instance.LevelData.Players.Where(player => player.IsHuman).First();
            _owner.OnStartTurn += OnStartTurn;
            _map = GameManager.Instance.LevelData.Map;

        }

        public void OnStartTurn()
        {
            _allowInput = true;
            EndTurnButton.interactable = true;
        }

        public void Button_EndTurn()
        {
            EndTurnButton.interactable = false;
            _allowInput = false;
            _selectedUnit = null;
            HideActions();
            _owner.EndTurn();
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
                if (isOnMap)
                {
                    Selector.transform.position = new Vector3(point.X, point.Y, -1.0f);
                    _highlightedTerrain = map.GetData(point);

                    var pawns = map.GetAgent(point);
                    _highlightedPawn = pawns.Count > 0 ? (Pawn)pawns[0] : null;

                    OnTerrainChange?.Invoke(_highlightedTerrain);
                }
                else
                {
                    OnTerrainChange?.Invoke(null);
                }

                Selector.SetActive(isOnMap);
               
                if(_highlightedPawn == null)
                {
                    UIController.Instance.HideVsPawn();
                }

                if(_selectedUnit == null)
                {
                    OnPawnChange?.Invoke(_highlightedPawn);
                }
                else if(_highlightedPawn != null)
                {
                    if (_highlightedPawn.Owner != _owner)
                    {
                        UIController.Instance.SetVsPawnData(_selectedUnit, _highlightedPawn);
                    }
                }
            }

            if (isOnMap && _allowInput)
            {
                _state(point, _highlightedPawn, _highlightedTerrain);
            }
        }

        protected IEnumerator ExecuteAction(ActionInfo action, Pawn pawn)
        {
            if(action.Prerequisite != null)
            {
                yield return StartCoroutine(ExecuteAction(action.Prerequisite, pawn));
            }

            var board = GameManager.Instance.Board;
            pawn.CanMove = false;

            switch (action.ActionType)
            {
                case ActionType.MOVE:
                    {
                        pawn.PawnComponent.HideCanMove();
                        yield return board.StartCoroutine(board.Move(pawn, action));
                        break;
                    }
                case ActionType.ATTACK:
                    {
                        var attacker = pawn;
                        var defender = action.Target;

                        yield return board.StartCoroutine(board.Attack(attacker, defender));
                        board.DeathCheck(defender);

                        // If the target died, do nothing
                        if (defender.IsDead())
                        {
                            break;
                        }

                        yield return board.StartCoroutine(board.Attack(defender, attacker));
                        board.DeathCheck(attacker);
                        break;
                    }
            }
        }

        protected void ShowActions(List<ActionInfo> actions)
        {
            actions.ForEach(action =>
            {
                var position = new Vector2(action.Location.X, action.Location.Y);
                var prefab = action.ActionType == ActionType.MOVE ? HighlightMovePrefab : HighlightAttackPrefab;
                var actionGameObject = GameObject.Instantiate(prefab, position, Quaternion.identity);
                _actionGraphics.Add(actionGameObject);
            });
        }

        protected void HideActions()
        {
            _actionGraphics.ForEach(action => { GameObject.Destroy(action); });
        }

        protected void State_NoUnitSelected(Point point, Pawn pawn, NodeData nodeData)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (pawn != null && pawn.Owner == _owner && pawn.CanMove)
                {
                    _selectedUnit = pawn;
                    _state = State_UnitSelected;
                    OnSelectedChange?.Invoke(_selectedUnit);

                    var actions = GameManager.Instance.Board.GetActionsFor(_selectedUnit);
                    ShowActions(actions);
                    _actionsForSelectedPawn = actions;
                }
            }
        }

        protected void State_UnitSelected(Point point, Pawn pawn, NodeData nodeData)
        {
            if (Input.GetMouseButtonDown(0))
            {
                var action = _actionsForSelectedPawn.Find(actionInfo => {
                    return actionInfo.Location.X == point.X && actionInfo.Location.Y == point.Y;
                });

                if(action != null)
                {
                    StartCoroutine(ExecuteAction(action, _selectedUnit));
                }

                _selectedUnit = null;
                _state = State_NoUnitSelected;
                OnSelectedChange?.Invoke(_selectedUnit);

                UIController.Instance.HideVsPawn();
                HideActions();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Pathfinding;

namespace Game.Assets
{
    public class GameManager : MonoBehaviour
    {
        public class Scenes
        {
            public const int LEVEL  = 0;
            public const int MENU   = 1;
        }

        public static GameManager Instance { get; set; }

        public LevelData LevelData { get; set; }
        public Board Board { get; set; }

        public TileAtlas TileAtlas;
        public PawnAtlas PawnAtlas;

        private void Awake()
        {
            if(Instance != null)
            {
                GameObject.Destroy(gameObject);
                return;
            }

            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            var map = new Map<NodeData>(10, 10, TileAtlas.Grass);

            for (int i = 0; i < 9; i++)
            {
                var position1 = new Point(i, 3);
                map.SetData(position1, TileAtlas.Mountain);

                var position2 = new Point(1 + i, 6);
                map.SetData(position2, TileAtlas.Mountain);
            }

            LevelData = new LevelData(map);

            LevelData.Pawn = PawnAtlas.Morty;

            LoadLevel();
        }

        public void LoadLevel()
        {
            SceneManager.LoadScene(Scenes.LEVEL);
        }
    }
}
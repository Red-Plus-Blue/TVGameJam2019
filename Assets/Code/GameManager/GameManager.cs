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
        public DialogAtlas DialogAtlas;

        public User User = new User();

        private void Awake()
        {
            if(Instance != null)
            {
                GameObject.Destroy(gameObject);
                return;
            }

            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);

            User.MetaData["user_dimension_id"] = "F-351";
            User.MetaData["commander_dimension_id"] = "R-227";
            User.MetaData["predecessor_id"] = "D-78";
            User.MetaData["mission_planet_name"] = "Alpha Centauria Beta-Prime-Delta";
        }

        public void StartLevel()
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

            var human = new Player("Human");
            human.IsHuman = true;
            human.OnStartTurn = human.WaitForInput;

            var ai = new Player("AI");

            LevelData.Players.Add(human);
            LevelData.Players.Add(ai);

            {
                (int x, int y)[] positions = { (1, 1), (2, 1), (3, 1), (4, 1) };

                for (int i = 0; i < 4; i++)
                {
                    var morty = (Pawn)PawnAtlas.Morty.Clone();
                    var position = positions[i];
                    morty.X = position.x;
                    morty.Y = position.y;
                    morty.Owner = human;
                    map.AddAgent(morty);
                }
            }

            {
                (int x, int y)[] positions = { (1, 7), (2, 7), (3, 7), (4, 7) };

                for (int i = 0; i < 4; i++)
                {
                    var enemy = (Pawn)PawnAtlas.Gromphlamite.Clone();
                    var position = positions[i];
                    enemy.X = position.x;
                    enemy.Y = position.y;
                    enemy.Owner = ai;
                    map.AddAgent(enemy);
                }
            }

            SceneManager.LoadScene(Scenes.LEVEL);
        }
    }
}
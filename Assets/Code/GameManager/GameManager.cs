using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Pathfinding;
using Game.MultiverseData;

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
        public FXAtlas FXAtlas;

        public User User = new User();
        public Multiverse Multiverse = Multiverse.Generate();

        private void Awake()
        {
            if(Instance != null)
            {
                GameObject.Destroy(gameObject);
                return;
            }

            Instance = this;
            GameObject.DontDestroyOnLoad(gameObject);

            var playerDimension = Multiverse.Dimensions[0];
            var commanderDimension = Multiverse.Dimensions[1];
            var predecessorDimension = Multiverse.Dimensions[2];
            predecessorDimension.Characters.Find(character => character.Name == "Rich").Status = CharacterStatus.KIA;

            User.MetaData["user_dimension_id"] = playerDimension.ID;
            User.MetaData["commander_dimension_id"] = predecessorDimension.ID;
            User.MetaData["predecessor_id"] = commanderDimension.ID;
            User.MetaData["mission_planet_name"] = "Alpha Centauri Beta-Prime-Delta";
        }

        public void StartLevel()
        {
            var grass = TileAtlas.Tiles.First(tile => tile.ID == "grass");
            var building = TileAtlas.Tiles.First(tile => tile.ID == "building_1");

            var map = new Map<NodeData>(10, 10, grass);

            map.SetData(new Point(3, 3), building);
            map.SetData(new Point(5, 6), building);
            map.SetData(new Point(7, 3), building);
            map.SetData(new Point(2, 8), building);

            LevelData = new LevelData(map);

            var human = new Player("Human");
            human.IsHuman = true;
            human.OnStartTurn = human.WaitForInput;

            var ai = new Player("AI");

            LevelData.Players.Add(human);
            LevelData.Players.Add(ai);
            LevelData.Mission = new KillHostilesMission(human);

            {
                (int x, int y)[] positions = { (1, 1), (2, 1), (3, 1), (4, 1) };
                var dimensions = Multiverse.Dimensions.Take(4).ToList();

                for (int i = 0; i < 4; i++)
                {
                    var dorky = (Pawn)PawnAtlas.Morty.Clone();
                    var position = positions[i];
                    dorky.X = position.x;
                    dorky.Y = position.y;
                    dorky.Owner = human;
                    dorky.Status = "Scared";
                    dorky.Dimension = dimensions[i].ID;
                    map.AddAgent(dorky);
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
                    enemy.Dimension = "F-351";
                    enemy.Status = "Vigilant";
                    enemy.Attack += (i * 10);
                    map.AddAgent(enemy);
                }
            }

            SceneManager.LoadScene(Scenes.LEVEL);
        }
    }
}
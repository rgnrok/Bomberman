using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Bomberman
{

    public class BoardManager : MonoBehaviour
    {

        public int columns = 8;
        public int rows = 8;

        public int minWallsCount = 5;
        public int maxWallsCount = 5;

        public int minBonusesCount = 1;
        public int maxBonusesCount = 3;

        public GameObject exit;
        public GameObject player;

        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] bonusTiles;
        public GameObject[] enemyTiles;
        public GameObject[] outerWallTiles;

        private Transform boardHolder;
        private List<Vector3> gridPositions = new List<Vector3>();


        void InitialiseList() {
            gridPositions.Clear();
            for (int x = 0; x < columns; x++) {
                for (int y = 0; y < rows; y++) {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }


        //Init floor and outer walls
        void BoardSetup() {
            boardHolder = new GameObject("Board").transform;
            GameObject toInstantiate;
            for (int x = -1; x < columns + 1; x++) {
                for (int y = -1; y < rows + 1; y++) {
                    toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];

                    if (x == -1 || x == columns || y == -1 || y == rows) {
                        toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    }
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity, boardHolder);
                }
            }
        }

        //Init indestructible walls
        void InitialMetalBlocks() {
            int index = 0;
            for (int x = 0; x < columns; x++) {
                for (int y = 0; y < rows; y++, index++) {
                    if ((x % 2 == 1) && (y % 2 == 1)) {
                        gridPositions.RemoveAt(index);
                        index--;
                        Instantiate(outerWallTiles[Random.Range(0, outerWallTiles.Length)], new Vector3(x, y, 0f), Quaternion.identity, boardHolder);
                    }
                }
            }
        }


        Vector3 RandomPosition() {
            int randomIndex = Random.Range(0, gridPositions.Count);
            Vector3 randomPosition = gridPositions[randomIndex];
            //Protection against reuse
            gridPositions.RemoveAt(randomIndex);

            return randomPosition;
        }


        void SpawnObjects(GameObject[] objects, int minimum, int maximum) {
            int objectCount = Random.Range(minimum, maximum + 1);
            for (int i = 0; i < objectCount; i++) {
                GameObject selectedObject = objects[Random.Range(0, objects.Length)];
                Instantiate(selectedObject, RandomPosition(), Quaternion.identity);
            }
        }


        public void SetupScene(int level) {
            BoardSetup();
            InitialiseList();
            InitialMetalBlocks();

            SpawnPlayer();
            int enemyCount = (int)Mathf.Log(level, 2f) + 1;

            SpawnObjects(enemyTiles, enemyCount, enemyCount);
            SpawnObjects(wallTiles, minWallsCount, maxWallsCount);
            SpawnObjects(bonusTiles, minBonusesCount, maxBonusesCount);
        }

        void SpawnPlayer() {
            Instantiate(player, new Vector3(0, 0, 0f), Quaternion.identity);
            //Remove near cells
            gridPositions.RemoveAt(rows*2);
            gridPositions.RemoveAt(rows);
            gridPositions.RemoveAt(2);
            gridPositions.RemoveAt(1);
            gridPositions.RemoveAt(0);

        }

        public void SpawnExit() {
            Instantiate(exit, RandomPosition(), Quaternion.identity);
        }
    }
}

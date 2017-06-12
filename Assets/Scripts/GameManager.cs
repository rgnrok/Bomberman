using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace Bomberman
{
    using System.Collections.Generic;
    using UnityEngine.UI;

    public class GameManager : MonoBehaviour
    {
        public GameObject infoPanel;

        public static GameManager instance = null;
        public float levelLoadDelay = 1f;

        public float initTimer = 60;

        private int level = 1;
        private Text levelText;
        private BoardManager boardManager;

        private bool doingSetup;
        private InfoPanel infoPanelInstance;

        private List<Enemy> enemies = new List<Enemy>();
        private Player player;

        private Coroutine timerCoroutine;

        private float _timer;
        public float Timer {
            get {
                return _timer;
            }
            private set {
                _timer = value;
                if (infoPanelInstance != null) {
                    infoPanelInstance.timer.text = Mathf.RoundToInt(_timer).ToString();
                }

                if (_timer <= 0) {
                    GameOver();
                }
            }
        }


        void Awake() {
            if (instance == null) {
                instance = this;
            }
            else if (instance != this) {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            boardManager = GetComponent<BoardManager>();
            InitGame();
        }


        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) {
            instance.level++;
            instance.InitGame();
        }

        //Reset board, enemies and timer
        void InitGame() {
            doingSetup = true;

            Timer = initTimer;

            GameObject canvas = GameObject.FindGameObjectWithTag("Canvas");
            infoPanelInstance = Instantiate(infoPanel, canvas.transform).GetComponent<InfoPanel>();

            infoPanelInstance.levelImage.Show(level);
            Invoke("HideLevelImage", levelLoadDelay);
            enemies.Clear();

            boardManager.SetupScene(level);
        }

        void HideLevelImage() {
            infoPanelInstance.levelImage.Hide();
            doingSetup = false;
            timerCoroutine = StartCoroutine(GameTimer());

        }

        private IEnumerator GameTimer() {
            while (Timer > 0) {
                Timer--;
                yield return new WaitForSecondsRealtime(1);
            }
        }

        public void GameOver() {
            infoPanelInstance.levelImage.GameOver();
            if (timerCoroutine != null) {
                StopCoroutine(timerCoroutine);
            }
            enabled = false;
        }

        public void SetPlayer(Player pl) {
            player = pl;
            player.OnBombChanged += delegate (object sender, BombChangedArgs args)
            {
                infoPanelInstance.bombs.text = args.bombsCount.ToString();
            };
        }

        public void AddEnemy(Enemy enemy) {
            enemies.Add(enemy);
            enemy.OnDie += EnemyDie;
        }

        protected void EnemyDie(object sender, EventArgs args) {
            enemies.Remove((Enemy)sender);
            ((Enemy)sender).OnDie -= EnemyDie;

            if (enemies.Count == 0) {
                boardManager.SpawnExit();
            }
        }

        public void Win() {
            StopCoroutine(timerCoroutine);
            Invoke("LoadNextLevel", levelLoadDelay);
        }

        protected void LoadNextLevel() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}


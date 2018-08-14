using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rebirth.Prototype
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager singleton = null;
        
        private int loadedLevelIndex = 0;

        void Awake()
        {

            if (singleton == null)
            {
                singleton = this;
            }

            else if (singleton != this)
            {
                Destroy(gameObject);
            }

            // Dont destroy on reloading the scene
            DontDestroyOnLoad(gameObject);
        }

        public void Init(Scene currentScene)
        {
            WorldCamera = Camera.main;

            InitSpawnPoints();
            InstantiatePlayer(currentScene);
        }

        public void InstantiatePlayer(Scene currentScene)
        {
            GameObject planet = GameObject.FindGameObjectWithTag("Planet");
            

            GameObject player = Instantiate(PrefabPlayer, SpawnPoints[0].transform.position, Quaternion.identity);
            LocalPlayer = player.GetComponent<RebirthPlayerController>();

            if(LocalPlayer != null)
            {
                LocalPlayer.Init(planet);
            }
            
        }

        public void InitSpawnPoints()
        {
            SpawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        }

        public IEnumerator LoadLevel (int levelIndex)
        {
            if (loadedLevelIndex > 0) yield return SceneManager.UnloadSceneAsync(loadedLevelIndex);

            Scene loadedScene = SceneManager.GetSceneByBuildIndex(levelIndex);
            if (loadedScene.isLoaded)
            {
                SceneManager.SetActiveScene(loadedScene);
            }
            else
            {
                yield return SceneManager.LoadSceneAsync(levelIndex, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelIndex));                
            }

            loadedLevelIndex = levelIndex;

            Init(SceneManager.GetActiveScene());
        }

        public Dictionary<string, GameObject> SpawnablePrefabs;
        public GameObject PrefabPlayer;
        public GameObject[] SpawnPoints;

        public InputManager IM = new InputManager();
        public RebirthPlayerController LocalPlayer;
        public Camera WorldCamera;
        public HUD Hud;

        public class InputManager
        {
            public InputManager()
            {
                keyJump = KeyCode.Space;
                keyForward = KeyCode.W;
                keyBackward = KeyCode.S;
                keyLeft = KeyCode.A;
                keyRight = KeyCode.D;
            }

            public bool Jump()
            {
                return Input.GetKey(keyJump);
            }

            public bool Forward()
            {
                return Input.GetKey(keyForward);
            }

            public bool Backward()
            {
                return Input.GetKey(keyBackward);
            }

            public bool Left()
            {
                return Input.GetKey(keyLeft);
            }

            public bool Right()
            {
                return Input.GetKey(keyRight);
            }

            public KeyCode keyForward { get; set; }
            public KeyCode keyBackward { get; set; }
            public KeyCode keyLeft { get; set; }
            public KeyCode keyRight { get; set; }
            public KeyCode keyJump { get; set; }
            
        }
    }
}
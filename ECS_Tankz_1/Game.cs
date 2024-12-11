using Aiv.Fast2D;

namespace ECS_Tankz_1
{
    public static class Game
    {
        // Global Values
        public static float WorkingHeight;
        public static float UnitSize { get; private set; }
        public static float Gravity {  get; set; }  

        // Window
        public static Window Win;
        public static bool IsRunning;
        public static float DeltaTime
        {
            get
            {
                if (firstFrame)
                {
                    return 0;
                }
                else
                {
                    return Win.DeltaTime;
                }
            }
        }

        // Scenes
        private static Scene currentScene;
        public static Scene CurrentScene { get { return currentScene; } }
        private static bool changeScene;
        private static Scene nextScene;
        private static bool firstFrame;

        public static void Init(string windowName, int windowWidth, int windowHeight, Scene startScene, float workingHeight, float ortographicSize)
        { 
            //Add comment

            Win = new Window(windowWidth, windowHeight, windowName);
            WorkingHeight = workingHeight;
            Win.SetDefaultViewportOrthographicSize(ortographicSize);
            UnitSize = WorkingHeight / Win.OrthoHeight;
            Win.SetVSync(false);
            ChangeScene(startScene);
        }

        public static void Play()
        {
            IsRunning = true;
            //currentScene.Start();

            while (Win.IsOpened && IsRunning)
            {
                //Game loop
                PhysicsManager.FixedUpdate();
                PhysicsManager.CheckCollisions();

                currentScene.Update();
                currentScene.LateUpdate();

                CameraManager.MoveCameras();

                DrawManager.Draw();

                firstFrame = false;
                if (changeScene)
                {
                    changeScene = false;
                    ChangeScene(nextScene);
                }

                //Fine Game loop

                Input.PerformLastKey();

                Win.Update();
            }
        }

        public static float PixelsToUnit(float pixelSize)
        {
            return pixelSize / UnitSize;
        }

        public static int UnitToPixels(float unitSize)
        {
            return (int)(unitSize * UnitSize);
        }

        private static void ChangeScene(Scene nextScene)
        {
            if (currentScene != null)
            {
                currentScene.DestroyScene();
            }
            if (nextScene == null)
            {
                IsRunning = false;
                return;
            }
            currentScene = nextScene;
            currentScene.InitializeScene();
            currentScene.Awake();
            currentScene.Start();
            currentScene.IsInizialized = true;
            firstFrame = true;
        }

        public static void TriggerChangeScene(Scene nextScene)
        {
            changeScene = true;
            Game.nextScene = nextScene;
        }
    }
}

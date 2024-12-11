using Aiv.Fast2D;
using OpenTK;

namespace ECS_Tankz_1
{
    public class PlayScene : Scene
    {
        public override void InitializeScene()
        {
            base.InitializeScene();

            CameraManager.Init(new Vector2(Game.Win.OrthoWidth * 0.5f,
                                           Game.Win.OrthoHeight * 0.5f),
                               new Vector2(Game.Win.OrthoWidth * 0.5f,
                                           Game.Win.OrthoHeight * 0.5f));
            CameraManager.Speed = 10.0f;
            CameraManager.SetCameraLimits(Game.Win.OrthoWidth * 0.5f,
                                          Game.Win.OrthoWidth * 1.5f,
                                          Game.Win.OrthoHeight * 0.5f - Game.PixelsToUnit(118),
                                          Game.Win.OrthoHeight * 0.5f);
            CameraManager.AddCamera("GUI", new Camera(), 0);
            CreateBackground();
            CreateBulletMgr();
            CreatePlayer(1);
            CreatePlayer(2);
            CreateGameLogic();
            CreateUI();
            CreateBox();
        }

        private void CreateBackground()
        {
            Vector2 startPos = new Vector2(Game.Win.OrthoWidth * 0.5f, Game.Win.OrthoHeight * 0.5f - Game.PixelsToUnit(118));
            GameObject background = new GameObject("Background", startPos);
            background.AddComponent(SpriteRenderer.Factory(background, "Background", new Vector2(0.5f, 0.5f), DrawLayer.Background));
            background = GameObject.Clone(background);
            background.Transform.Position = new Vector2(startPos.X + Game.Win.OrthoWidth * 0.5f, startPos.Y - (Game.Win.OrthoHeight * 0.5f + Game.PixelsToUnit(118)));
        }

        private void CreateBulletMgr()
        {
            GameObject bulletMgr = new GameObject("BulletMgr", Vector2.Zero);
            bulletMgr.AddComponent<BulletManager>(30);
        }

        private void CreatePlayer(int id)
        {
            Vector2 startPos = new Vector2(Game.PixelsToUnit(Game.Win.Width * (id == 1 ? 0.2f : 0.7f)), Game.PixelsToUnit(Game.Win.Height * 0.5f));

            // Logic obj
            GameObject player = new GameObject("Player_" + id, startPos);
            player.Tag = "Player";
            player.AddComponent<DontFallOverScreen>(Game.PixelsToUnit(170));
            player.AddComponent<PlayerController>(id, 10);
            RigidBody rb = player.AddComponent<RigidBody>();
            rb.IsGravityAffected = true;
            rb.Type = (uint)RigidBodyType.player;
            rb.AddCollisionType((uint)RigidBodyType.bullet);
            rb.AddCollisionType((uint)RigidBodyType.tile);
            rb.AddCollisionType((uint)RigidBodyType.player);
            player.AddComponent(new BoxCollider(player, Game.PixelsToUnit(83), Game.PixelsToUnit(60), new Vector2(0, -Game.PixelsToUnit(30))));

            // Visual objs
            GameObject track = new GameObject("Tank_Track" + id, startPos);
            track.AddComponent(SpriteRenderer.Factory(track, "Tank_Track", new Vector2(0.5f, 1.0f), DrawLayer.Playground));
            // Track Follow Player
            track.AddComponent<FollowTransform>(player.Transform, Vector2.Zero);

            GameObject cannon = new GameObject("Tank_Cannon" + id, startPos);
            cannon.AddComponent(SpriteRenderer.Factory(cannon, "Tank_Cannon", new Vector2(0.0f, 0.5f), DrawLayer.Playground));
            cannon.AddComponent<ClampRotation>(-180, 0);

            GameObject body = new GameObject("Tank_Body" + id, startPos);
            body.AddComponent(SpriteRenderer.Factory(body, "Tank_Body", new Vector2(0.5f, 1.0f), DrawLayer.Playground));
            // Body Follow Player
            body.AddComponent<FollowTransform>(player.Transform, new Vector2(0, -track.GetComponent<SpriteRenderer>().Height * 0.5f));
            
            // Cannon Follow Player
            cannon.AddComponent<FollowTransform>(player.Transform, new Vector2(
                -cannon.GetComponent<SpriteRenderer>().Width * 0.15f,
                -track.GetComponent<SpriteRenderer>().Height * 0.5f - body.GetComponent<SpriteRenderer>().Height + cannon.GetComponent<SpriteRenderer>().Height * 0.25f
                ));

            //Barre e FSM
            GameObject shootingBarBackground = new GameObject("BackgroundBar_" + id, Vector2.Zero);
            shootingBarBackground.AddComponent<FollowTransform>(player.Transform, new Vector2(-0.5f, -1.5f));
            shootingBarBackground.AddComponent(SpriteRenderer.Factory(shootingBarBackground, "BackgroundBar", new Vector2(0, 0.5f), DrawLayer.GUI));
            shootingBarBackground.IsActive = false;
            shootingBarBackground.Transform.Scale *= 0.5f;
            GameObject shootingBar = new GameObject("Bar_" + id, Vector2.Zero);
            shootingBar.AddComponent<FollowTransform>(player.Transform, new Vector2(-0.45f, -1.5f));
            shootingBar.AddComponent(SpriteRenderer.Factory(shootingBar, "Bar", new Vector2(0, 0.5f), DrawLayer.GUI));
            shootingBar.IsActive = false;
            shootingBar.Transform.Scale *= 0.5f;

            FSM playerFSM = player.AddComponent<FSM>();
            MoveState moveState = new MoveState(3, (int)FSMState.Shooting, cannon.Transform, 5, 45);
            ShootingState shootingState = new ShootingState(15, 5, shootingBarBackground, shootingBar, cannon, (int)FSMState.Idle, 45);
            WaitingState waitingState = new WaitingState();
            playerFSM.RegisterState((int)FSMState.MoveState, moveState);
            playerFSM.RegisterState((int)FSMState.Shooting, shootingState);
            playerFSM.RegisterState((int)FSMState.Idle, waitingState);
            playerFSM.SetStartState((int)FSMState.Idle);
        }

        private void CreateGameLogic()
        {
            GameObject gameLogic = new GameObject("GameLogic", Vector2.Zero);
            gameLogic.AddComponent<GameLogic>();
        }

        private void CreateUI()
        {
            Font stdFont = FontManager.GetFont("stdFont");

            // Player 1 UI
            GameObject player1Txt = new GameObject("Player1Txt", new Vector2(0.3f, 0.3f));
            player1Txt.AddComponent<TextBox>(stdFont, 8, Vector2.One).SetText("Player 1");
            player1Txt.GetComponent<TextBox>().Camera = CameraManager.GetCamera("GUI");
            GameObject player1BarFrame = new GameObject("Player1BarFrame", new Vector2(0.3f, 0.7f));
            player1BarFrame.AddComponent<SpriteRenderer>("BackgroundBar", Vector2.Zero, DrawLayer.GUI).Camera = CameraManager.GetCamera("GUI");
            GameObject player1Bar = new GameObject("Player1Bar", new Vector2(0.35f, 0.75f));
            player1Bar.AddComponent<SpriteRenderer>("Bar", Vector2.Zero, DrawLayer.GUI).Camera = CameraManager.GetCamera("GUI");

            // Player 2 UI
            GameObject player2Txt = new GameObject("Player2Txt", new Vector2(Game.Win.OrthoWidth - 2.5f, 0.3f));
            player2Txt.AddComponent<TextBox>(stdFont, 8, Vector2.One).SetText("Player 2");
            player2Txt.GetComponent<TextBox>().Camera = CameraManager.GetCamera("GUI");
            GameObject player2BarFrame = new GameObject("Player2BarFrame", new Vector2(Game.Win.OrthoWidth - 3.0f, 0.7f));
            player2BarFrame.AddComponent<SpriteRenderer>("BackgroundBar", Vector2.Zero, DrawLayer.GUI).Camera = CameraManager.GetCamera("GUI");
            GameObject player2Bar = new GameObject("Player2Bar", new Vector2(Game.Win.OrthoWidth - 2.95f, 0.75f));
            player2Bar.AddComponent<SpriteRenderer>("Bar", Vector2.Zero, DrawLayer.GUI).Camera = CameraManager.GetCamera("GUI");

            // UI CONTROLLER
            GameObject turnFeedback = new GameObject("TurnFeedback", new Vector2(Game.Win.OrthoWidth * 0.5f - 9.0f * Game.PixelsToUnit(stdFont.CharacterWidth), 2.0f));
            turnFeedback.AddComponent<TextBox>(stdFont, 14, Vector2.One * 1.3f).SetText("Player 1 Turn");
            turnFeedback.GetComponent<TextBox>().Camera = CameraManager.GetCamera("GUI");

            // Turn Timer
            GameObject turnTimer = new GameObject("TurnTimer", new Vector2(Game.Win.OrthoWidth * 0.5f - 2 * Game.PixelsToUnit(stdFont.CharacterWidth), 3.0f));
            turnTimer.AddComponent<TextBox>(stdFont, 2, Vector2.One * 2.0f).SetText("15");
            turnTimer.GetComponent<TextBox>().Camera = CameraManager.GetCamera("GUI");

            // UI Logic
            GameObject UIController = new GameObject("UIController", Vector2.Zero);
            UIController.AddComponent<UIController>();
        }

        private void CreateBox()
        {
            float boxWidth = Game.PixelsToUnit(GfxManager.GetTexture("Box").Width);
            float boxHeight = Game.PixelsToUnit(GfxManager.GetTexture("Box").Height);

            GameObject tmp;
            Vector2 startPos = new Vector2(boxWidth * 0.5f, 0.0f);

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    tmp = new GameObject("Box_" + i + " - " + j, startPos + new Vector2(boxWidth * i, boxHeight * j));
                    tmp.Tag = "Box";
                    tmp.AddComponent<SpriteRenderer>("Box", new Vector2(0.5f, 1.0f), DrawLayer.Playground);

                    RigidBody rb = tmp.AddComponent<RigidBody>();
                    rb.IsGravityAffected = true;
                    rb.Type = (uint)RigidBodyType.tile;
                    rb.AddCollisionType((uint)RigidBodyType.tile);
                    Collider c = ColliderFactory.CreateBoxFor(tmp);
                    tmp.AddComponent(c);
                    tmp.AddComponent<DontFallOverScreen>(Game.PixelsToUnit(170));
                    tmp.AddComponent<StopOnBox>();
                }
            }
        }

        protected override void LoadAssets()
        {
            GfxManager.AddTexture("Background", "Assets/bg_cartoon.jpg");
            GfxManager.AddTexture("Tank_Track", "Assets/tanks_tankTracks1.png");
            GfxManager.AddTexture("Tank_Body", "Assets/tanks_tankGreen_body1.png");
            GfxManager.AddTexture("Tank_Cannon", "Assets/tanks_turret2.png");
            GfxManager.AddTexture("Bullet_1", "Assets/tank_bullet1.png");
            GfxManager.AddTexture("BackgroundBar", "Assets/loadingBar_frame.png");
            GfxManager.AddTexture("Bar", "Assets/loadingBar_bar.png");
            GfxManager.AddTexture("Box", "Assets/crate.png");

            //Font
            Font stdFont = FontManager.AddFont("stdFont", "Assets/textSheet.png", 15, 32, 20, 20);
        }
    }
}

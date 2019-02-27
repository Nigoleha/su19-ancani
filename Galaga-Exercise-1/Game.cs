using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Timers; 
using DIKUArcade.Math; 
using DIKUArcade.Graphics;
using DIKUArcade.Physics;

namespace Galaga_Exercise_1 {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private DIKUArcade.Timers.GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;
        private List<Enemy> enemies; 
        private List<Image> enemyStrides;
        public List<PlayerShot> playerShots{get; private set; }
		
		//Explosions
		private List<Image> explosionStrides; 
		private AnimationContainer explosions; 
		private int explosionLength = 500; 
       

        public Game() {
            // TODO: Choose some reasonable values for the window and timer constructor.
            // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            win = new Window("Arcade Game", 500, 500);
            gameTimer = new GameTimer(60, 60);

            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
            player.Move();
            eventBus = new GameEventBus<object>();
                eventBus.InitializeEventBus(new List<GameEventType>() {
                    GameEventType.InputEvent, // key press / key release
                    GameEventType.WindowEvent, // messages to the window
                });
            win.RegisterEventBus(eventBus);
            eventBus.Subscribe(GameEventType.InputEvent, this);
            eventBus.Subscribe(GameEventType.WindowEvent, this);
       
            enemyStrides = ImageStride.CreateStrides(4,
                Path.Combine("Assets", "Images", "BlueMonster.png"));
            enemies = new List<Enemy>();
            AddEnemies();
            playerShots = new List<PlayerShot>();
                
            //explosions 
			explosionStrides = ImageStride.CreateStrides(8, 
				Path.Combine("Assets", "Images", "Explosion.png")); 
			explosions = new AnimationContainer(8); 
             
        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents(); // Update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    player.RenderEntity();
                    foreach (var ene in enemies) {
                        ene.RenderEntity();
                    }
                    IterateShots();
                    win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                }
                
                eventBus.ProcessEvents();

                
            }
            
            
            
            
        }

        public void KeyPress(string key) {
            switch (key) {
            case "KEY_ESCAPE":
                eventBus.RegisterEvent(
                    GameEventFactory<object>.CreateGameEventForAllProcessors(
                        GameEventType.WindowEvent, this, "CLOSE_WINDOW", "", ""));
                break;
            }
        
    }

        public void KeyRelease(string key) {
            throw new NotImplementedException();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
                if (eventType == GameEventType.WindowEvent) {
                    switch (gameEvent.Message) {
                    case "CLOSE_WINDOW":
                        win.CloseWindow();
                        break;
                    default:
                        break;
                    }
                } else if (eventType == GameEventType.InputEvent) {
                    switch (gameEvent.Parameter1) {
                    case "KEY_PRESS":
                        KeyPress(gameEvent.Message);
                        break;
                    case "KEY_RELEASE":
                        KeyRelease(gameEvent.Message);
                        break;
                    }
                }
           
        }

        public void AddEnemies() {
            Enemy enemy = new Enemy(this,
                new DynamicShape(new Vec2F(0.45f, 0.8f), new Vec2F(0.1f, 0.1f)), 
                new ImageStride(80, Path.Combine("Assets", "Images", "BlueMonster.png")));
            
            enemies.Add(enemy);
        }
        
		public void AddExplosion(float posX, float posY, float extentX, float extentY){
			explosions.AddAnimation(
				new StationaryShape(posX, posY, extentX, extentY), explosionLength, 
				new ImageStride(explosionLength/8, explosionStrides)); 
		}			
			
        public void IterateShots() {
            foreach (var shot in playerShots) {
                shot.Shape.Move();
                if (shot.Shape.Position.Y > 1.0f) {
                    shot.DeleteEntity();
                }

                foreach (var enemy in enemies) {
                    var collide = CollisionDetection.Aabb(enemy.Shape.AsDynamicShape(), shot.Shape);
                    if (collide.Collision) {
                        AddExplosion (enemy.Shape.Position.X, enemy.Shape.Position.Y, 10.0f, 10.0f); 
                        enemy.DeleteEntity();
                        shot.DeleteEntity();
                    }
                }
                
                List<Enemy> newEnemies = new List<Enemy>();
                foreach (Enemy enemy in enemies) {
                    if (!enemy.IsDeleted()) {
                        newEnemies.Add(enemy);
                    }
                }
                enemies = newEnemies;
                
                List<PlayerShot> newShots = new List<PlayerShot>();
                foreach (PlayerShot shots in playerShots) {
                    if (!shots.IsDeleted()) {
                        newShots.Add(shots);
                    }
                }

                playerShots = newShots;
            }
        }
    }

}
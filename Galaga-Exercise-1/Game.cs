using System;
using System.Collections.Generic;
using System.IO;
using DIKUArcade;
using DIKUArcade.EventBus;
using DIKUArcade.Entities;
using DIKUArcade.Timers; 
using DIKUArcade.Math; 
using DIKUArcade.Graphics;

namespace Galaga_Exercise_1 {
    public class Game : IGameEventProcessor<object> {
        private Window win;
        private DIKUArcade.Timers.GameTimer gameTimer;
        private Player player;
        private GameEventBus<object> eventBus;

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
                List<Image> enemyStrides;
                List<Enemy> enemies;
                enemyStrides = ImageStride.CreateStrides(4,
                    Path.Combine("Assets", "Images", "BlueMonster.png"));
                enemies = new List<Enemy>();
                
                
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
            enemy = new Enemy(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)), 
                new Image(Path.Combine("Assets", "Images", "Player.png")));
        }
        
        //public List<PlayerShot> playerShots { get; private set; }
    }

}
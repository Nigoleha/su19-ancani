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

        public void Game1() {
            // TODO: Choose some reasonable values for the window and timer constructor.
            // For the window, we recommend a 500x500 resolution (a 1:1 aspect ratio).
            win = new Window("Arcade Game", 500, 500);
            gameTimer = new GameTimer(60, 60);

            player = new Player(this,
                new DynamicShape(new Vec2F(0.45f, 0.1f), new Vec2F(0.1f, 0.1f)),
                new Image(Path.Combine("Assets", "Images", "Player.png")));


        }

        public void GameLoop() {
            while (win.IsRunning()) {
                gameTimer.MeasureTime();
                while (gameTimer.ShouldUpdate()) {
                    win.PollEvents(); // Update game logic here
                }

                if (gameTimer.ShouldRender()) {
                    win.Clear();
                    // Render gameplay entities here win.SwapBuffers();
                }

                if (gameTimer.ShouldReset()) {
                    // 1 second has passed - display last captured ups and fps
                    win.Title = "Galaga | UPS: " + gameTimer.CapturedUpdates + ", FPS: " +
                                gameTimer.CapturedFrames;
                }
            }

            player.RenderEntity();
        }

        public void KeyPress(string key) {
            throw new NotImplementedException();
        }

        public void KeyRelease(string key) {
            throw new NotImplementedException();
        }

        public void ProcessEvent(GameEventType eventType, GameEvent<object> gameEvent) {
            throw new NotImplementedException();
        }
        public List<PlayerShot> playerShots { get; private set; }
    }

}
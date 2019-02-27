using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
        }

        public void Direction(Vec2F vec) {
            var fig = Shape.AsDynamicShape();
            fig.ChangeDirection(vec);

        }
        public void Move() {
            if (!(Shape.Position.X <= 0 || Shape.Position.X >= 1)) {
                Shape.Move();
            }
            
        }
        public void CreateShots() {
            // while (space is pressed by player) {
            PlayerShot playershot = new PlayerShot(game, 
                new DynamicShape(new Vec2F(Shape.Position.X, Shape.Position.Y-2), new Vec2F(0.008f, 0.027f)), 
                new Image(Path.Combine("Assets", "Images", "BulletRed2.png")));
            game.playerShots.Add(playershot);
        }
    }
}
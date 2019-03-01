using System.IO;
using DIKUArcade.Entities;
using DIKUArcade.Graphics;
using DIKUArcade.Math;

namespace Galaga_Exercise_1 {
    public class Player : Entity {
        private Game game;
        private Image imageShot = new Image(Path.Combine("Assets", "Images", "BulletRed2.png")); 
        public Player(Game game, DynamicShape shape, IBaseImage image)
            : base(shape, image) {
            this.game = game;
        }

        public void Direction(Vec2F vec) {
            var fig = Shape.AsDynamicShape();
            fig.ChangeDirection(vec);

        }
        public void Move() {
            Vec2F fixLeft = new Vec2F( 0.02f,  0.0f);
            Vec2F fixRight = new Vec2F( -0.02f,  0.0f);
            if (Shape.Position.X <= 0) {
                Direction(fixLeft);
                Shape.Move();
            }
            else if (Shape.Position.X >= 0.9) {
                Direction(fixRight);
                Shape.Move();
            } else {
                Shape.Move(); 
            }
        }
        public void CreateShots() {
            PlayerShot playershot = new PlayerShot(game, 
                new DynamicShape(new Vec2F(Shape.Position.X +0.05f, Shape.Position.Y+0.05f), new Vec2F(0.008f, 0.027f)), 
                imageShot);
            game.playerShots.Add(playershot);
        }
    }
}
using Infart.Drawing;
using Infart.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Infart.Astronaut
{
    public abstract class Actor : AnimatedGameObject
    {
        protected Vector2 Velocity = Vector2.Zero;
        protected float XMoveSpeed = 180.0f;
        public bool Dead = false;

        protected List<GameObject> CollidingObjsReference;

        protected Actor(
            float depth,
            Vector2 startingPos,
            List<GameObject> collidingObjs)
            : base()
        {
            Velocity.Y = FallSpeed;

            CollidingObjsReference = collidingObjs;
        }

        public List<GameObject> CollidingObjectsReference
        {
            set { CollidingObjsReference = value; }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public float HorizontalMoveSpeed
        {
            get { return XMoveSpeed; }
            set { XMoveSpeed = value; }
        }

        public float FallSpeed { get; set; } = 20.0f;

        public bool OnGround { get; private set; } = false;

        private Vector2 CollisionTest(Vector2 moveAmount)
        {
            Rectangle afterMoveRect = _collisionRectangle;
            Vector2 corner1, corner2;

            if (moveAmount.X != 0)
            {
                afterMoveRect.Offset((int)moveAmount.X, 0);

                corner1 = new Vector2(
                        afterMoveRect.X + afterMoveRect.Width,
                        afterMoveRect.Y + 0.0f);
                corner2 = new Vector2(
                        afterMoveRect.X + afterMoveRect.Width,
                        afterMoveRect.Y + afterMoveRect.Height - 80.0f);

                if (CollisionSolver.CheckCollisions(
                    new Rectangle(
                        (int)corner1.X, (int)corner1.Y,
                        1, Math.Abs((int)(corner1.Y - corner2.Y))),
                        CollidingObjsReference))
                {
                    moveAmount.X = 0;
                    Velocity.X = 0;
                }
            }

            if (moveAmount.Y == 0)
                return moveAmount;
            else
            {
                afterMoveRect = _collisionRectangle;
                afterMoveRect.Offset((int)moveAmount.X, (int)moveAmount.Y);

                if (Velocity.Y > 0)
                {
                    corner1 = new Vector2(
                        afterMoveRect.X + 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);
                    corner2 = new Vector2(
                        afterMoveRect.X + afterMoveRect.Width - 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);

                    int collIndex = CollisionSolver.CheckCollisionsReturnCollidedObject(
                       new Rectangle(
                            (int)corner1.X,
                            (int)corner1.Y + 10,
                            Math.Abs((int)(corner1.X - corner2.X)),
                            1),
                            CollidingObjsReference);

                    if (collIndex != -1)
                    {
                        OnGround = true;

                        moveAmount.Y = 0;
                        Velocity.Y = 0;

                        ((GameObject) this).Position = new Vector2(
                            ((GameObject) this).Position.X,
                            MathHelper.Lerp(
                                ((GameObject) this).Position.Y,
                                CollidingObjsReference[collIndex].CollisionRectangle.Y - Height + 2,
                                0.1f));
                    }
                }
            }

            return moveAmount;
        }

        public override void Update(double gameTime)
        {
            Velocity.Y += FallSpeed;

            if (!Dead)
            {
                float elapsed = (float)gameTime / 1000.0f;

                Vector2 moveAmount = Velocity * elapsed;
                moveAmount = CollisionTest(moveAmount);

                if (Velocity.Y < 0)
                {
                    OnGround = false;
                }

                Position += moveAmount;

                base.Update(gameTime);
            }
        }
    }
}
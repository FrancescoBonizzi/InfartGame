











using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;



namespace fge
{
    public abstract class Actor : AnimatedGameObject
    {
        
        protected Vector2 velocity_ = Vector2.Zero;
        protected float x_move_speed_ = 180.0f;
        private float y_fall_speed_ = 20.0f;
        private bool on_ground_ = false;
        protected bool dead_ = false;

        protected List<GameObject> colliding_objs_reference_;

        

        
        protected Actor(
            float depth,
            Vector2 starting_pos,
            List<GameObject> CollidingObjs) 
            : base()
        {
            velocity_.Y = y_fall_speed_;

            colliding_objs_reference_ = CollidingObjs;
        }

        public List<GameObject> CollidingObjectsReference
        {
            set { colliding_objs_reference_ = value; }
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        

        
        public float HorizontalMoveSpeed
        {
            get { return x_move_speed_; }
            set { x_move_speed_ = value; }
        }

        public Vector2 Velocity
        {
            get { return velocity_; }
            set { velocity_ = value; }
        }

        public float FallSpeed
        {
            get { return y_fall_speed_; }
            set { y_fall_speed_ = value; }
        }

        public bool Dead
        {
            get { return dead_; }
            set { dead_ = value; }
        }

        public bool OnGround
        {
            get { return on_ground_; }
        }

        

        
        private Vector2 collisionTest(Vector2 move_amount)
        {
            
            Rectangle afterMoveRect = collision_rectangle_;
            Vector2 corner1, corner2;

            
            if (move_amount.X != 0)
            {
                afterMoveRect.Offset((int)move_amount.X, 0);

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
                        colliding_objs_reference_))
                {
                    move_amount.X = 0;
                    velocity_.X = 0;
                }
            }

            

            
            if (move_amount.Y == 0)
                return move_amount;
            else
            {
                afterMoveRect = collision_rectangle_;
                afterMoveRect.Offset((int)move_amount.X, (int)move_amount.Y);

                if (velocity_.Y > 0)
                {
                    
                    corner1 = new Vector2(
                        afterMoveRect.X + 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);
                    corner2 = new Vector2(
                        afterMoveRect.X + afterMoveRect.Width - 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);

                    
                    int coll_index = CollisionSolver.CheckCollisionsReturnCollidedObject(
                       new Rectangle(
                            (int)corner1.X,
                            (int)corner1.Y + 10,
                            Math.Abs((int)(corner1.X - corner2.X)),
                            1),
                            colliding_objs_reference_);

                    if (coll_index != -1)
                    {
                        on_ground_ = true;

                        move_amount.Y = 0;
                        velocity_.Y = 0;

                        
                        position_ = new Vector2(
                            position_.X,
                            MathHelper.Lerp(
                                position_.Y,
                                colliding_objs_reference_[coll_index].CollisionRectangle.Y - Height + 2,
                                0.1f));
                    }

                }
            }

            

            return move_amount;
        }


        

        
        public override void Update(double gameTime)
        {
            velocity_.Y += y_fall_speed_;

            if (!dead_)
            {
                float elapsed = (float)gameTime / 1000.0f;

                Vector2 moveAmount = velocity_ * elapsed;
                moveAmount = collisionTest(moveAmount);

                if (velocity_.Y < 0)
                {
                    on_ground_ = false;
                }

                Position += moveAmount;

                base.Update(gameTime);
            }
        }

        
    }
}

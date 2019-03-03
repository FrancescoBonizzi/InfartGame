#region Descrizione
//-----------------------------------------------------------------------------
// Actor.cs
//
// Classe astratta che implementa comuni azioni come morire, saltare, e altre
// E' valida per qualunque personaggio 
//
// fbonizzi_Game_Engine
// Copyright (C) Francesco Bonizzi. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace fge
{
    public abstract class Actor : AnimatedGameObject
    {
        #region Dichiarazioni

        protected Vector2 velocity_ = Vector2.Zero;
        protected float x_move_speed_ = 180.0f;
        private float y_fall_speed_ = 20.0f;
        private bool on_ground_ = false;
        protected bool dead_ = false;

        protected List<GameObject> colliding_objs_reference_;

        #endregion

        #region Costruttore / Distruttore

        protected Actor(
            float depth,
            Vector2 starting_pos,
            List<GameObject> CollidingObjs) // deve diventare templatico o specializzato
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

        #endregion

        #region Proprietà

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

        #endregion

        #region Metodi

        private Vector2 collisionTest(Vector2 move_amount)
        {
            // Attenzione! Si basa sul fatto che si muoverà solo a destra!
            Rectangle afterMoveRect = collision_rectangle_;
            Vector2 corner1, corner2;

            #region Horizontal

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

            #endregion

            #region Vertical

            if (move_amount.Y == 0)
                return move_amount;
            else
            {
                afterMoveRect = collision_rectangle_;
                afterMoveRect.Offset((int)move_amount.X, (int)move_amount.Y);

                if (velocity_.Y > 0)
                {
                    // 2 Sono i pixel di offset perché l'animazione non è precisa
                    corner1 = new Vector2(
                        afterMoveRect.X + 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);
                    corner2 = new Vector2(
                        afterMoveRect.X + afterMoveRect.Width - 20.0f,
                        afterMoveRect.Y + afterMoveRect.Height - 2.0f);

                    // Qui mi serve l'oggetto con cui ho colliso per porre l'oggetto al di sopra di esso
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

                        // 2 Sono i pixel di offset perché l'animazione non è precisa
                        position_ = new Vector2(
                            position_.X,
                            MathHelper.Lerp(
                                position_.Y,
                                colliding_objs_reference_[coll_index].CollisionRectangle.Y - Height + 2,
                                0.1f));
                    }

                }
            }

            #endregion

            return move_amount;
        }


        #endregion

        #region Update

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

        #endregion
    }
}

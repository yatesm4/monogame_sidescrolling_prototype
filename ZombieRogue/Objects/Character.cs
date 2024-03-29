﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace ZombieRogue.Objects
{
    public abstract class Character
    {
        public ContentManager Content;
        public GraphicsDevice GraphDevice;

        public Animation Spr_Walk;
        public Animation Spr_Run;
        public Animation Spr_Punch;
        public Animation Spr_Kick;
        public Animation Spr_Uppercut;

        public AnimationPlayer Sprite;

        public static Texture2D Debug_Rect;
        public bool IsDebugging = false;

        public Vector2 Position { get; set; }
        public int Depth
        {
            get { return (int)Math.Round(Position.Y); }
        }
        public Vector2 MoveDirection { get; set; }
        public bool IsFlipped = false;
        public Vector2 Scale { get; set; } = new Vector2(1, 1);

        public Vector2 Movement;
        public Vector2 Velocity;

        public const float MoveAcceleration = 4000.0f;
        public const float MaxMoveSpeed = 500.0f;
        public const float GroundDragFactor = 0.48f;
        public const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        public const float MaxJumpTime = 0.35f;
        public const float JumpLaunchVelocity = -3500.0f;
        public const float GravityAcceleration = 3400.0f;
        public const float MaxFallSpeed = 550.0f;
        public const float JumpControlPower = 0.14f;

        // Input configuration
        public const float MoveStickScale = 1.0f;
        public const float AccelerometerScale = 1.5f;

        public Rectangle localBounds;

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X - (localBounds.Width / 2), (int)Position.Y - localBounds.Height, localBounds.Width, localBounds.Height);
            }
        }

        public Character(ContentManager content, Vector2 position, int[] skin_args)
        {
            Content = content;
            LoadContent(content, skin_args);
            Reset(position);
        }

        public void LoadContent(ContentManager content, int[] skin_args)
        {
            Spr_Run = new Animation(content.Load<Texture2D>($"Sprites/Characters/Run/0{skin_args[0]}"), 0.25f, true);
            Spr_Walk = new Animation(content.Load<Texture2D>($"Sprites/Characters/Walk/0{skin_args[1]}"), 0.2f, true);
            Spr_Punch = new Animation(content.Load<Texture2D>($"Sprites/Characters/Punch/0{skin_args[2]}"), 0.15f, true);
            Spr_Kick = new Animation(content.Load<Texture2D>($"Sprites/Characters/Kick/0{skin_args[3]}"), 0.15f, true);
            Spr_Uppercut = new Animation(content.Load<Texture2D>($"Sprites/Characters/Uppercut/0{skin_args[5]}"), 0.4f, true);

            int width = (int)(Spr_Walk.FrameWidth * 0.6);
            int left = (Spr_Walk.FrameWidth - width);
            int height = (int)(Spr_Walk.FrameWidth);
            int top = Spr_Walk.FrameHeight - height;
            localBounds = new Rectangle(left, top, width, height);
        }

        public void Reset(Vector2 reset_position)
        {
            Position = reset_position;
            Velocity = Vector2.Zero;
            Sprite.PlayAnimation(Spr_Walk);
        }

        public abstract void Update(GameTime gameTime, KeyboardState keyboardState, Map map);

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Draw(gameTime, spriteBatch, Position, IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

            /**
             * DEBUG - DRAW HITBOX
             */

            if (IsDebugging.Equals(true))
            {
                // draw characters hitbox
                //DrawDebugRect(Hitbox, Color.Red, spriteBatch);
            }
        }

        public void DrawDebugRect(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            //Console.WriteLine("Drawing debug rect");

            Debug_Rect = new Texture2D(GraphDevice, 1, 1);
            Debug_Rect.SetData(new[] { Color.Red });

            spriteBatch.Draw(Debug_Rect, coords, new Color(color, 0.25f));
        }
    }
}

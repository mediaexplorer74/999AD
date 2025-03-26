
// Type: GameManager.Player
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace GameManager
{
  internal static class Player
  {
    private static Texture2D spritesheet;
    private static List<Animation> animations;
    private static Player.AnimationTypes currentAnimation;
    public static Vector2 position;
    public static readonly int height = 22;
    public static readonly int width = 12;
    private static Vector2 jumpSpeed;
    private static bool isFacingRight;
    public static readonly float maxHorizontalMovementSpeed = 300f;//100f;
    public static float horizontalMovementSpeed;
    public static readonly Vector2 initialJumpSpeed = new Vector2(190f * 3, -400f * 3);//(190f, -400f);
    private static bool isTouchingTheGround;
    private static bool isOnTheWall;
    private static bool isOnMovingPlatform;
    private static bool canDoubleJump = true; // hack
    private static bool isWallJumping = true; //hack
    public static bool doubleJumpUnlocked = true; //hack
    public static bool wallJumpUnlocked = true; // hack

    public static readonly Vector2 projectileInitialVelocity = new Vector2(200f, -200f);
    public static readonly float timeBetweenShots = 0.4f;
    private static float elapsedShotTime;
    public static readonly int maxHealthPoints = 5;//3;
    public static int healthPoints;
    private static readonly float invulnerabilityTime = 3f;
    private static float elapsedInvulnerabilityTime;
    private static bool invulnerable;
    private static float alphaValue;
    public static bool haltInput = false;

    public static void Inizialize(Texture2D _spritesheet, Vector2 _position)
    {
      Player.Reset(_position);
      Player.spritesheet = _spritesheet;
      Player.doubleJumpUnlocked = false;
      Player.wallJumpUnlocked = false;
      Player.animations = new List<Animation>();
      Player.animations.Add(new Animation(new Rectangle(0, 0, 128, 24), 16, 24, 8, 0.3f, true));
      Player.animations.Add(new Animation(new Rectangle(0, 24, 160, 24), 16, 24, 10, 0.06f, true));
      Player.animations.Add(new Animation(new Rectangle(0, 48, 80, 24), 16, 24, 5, 0.06f, false, true));
      Player.animations.Add(new Animation(new Rectangle(0, 72, 112, 24), 16, 24, 7, 0.06f, false));
      Player.animations.Add(new Animation(new Rectangle(0, 96, 64, 24), 16, 24, 4, 0.1f, true));
      Player.animations.Add(new Animation(new Rectangle(0, 120, 160, 24), 16, 24, 10, 0.1f, false, true));
      Player.animations.Add(new Animation(new Rectangle(0, 144, 16, 24), 16, 24, 1, 0.0f, false, true));
    }

    public static void Reset(Vector2 _position)
    {
      Player.position = _position;
      Player.jumpSpeed = Vector2.Zero;
      Player.isFacingRight = true;
      Player.horizontalMovementSpeed = 0.0f;
      Player.isTouchingTheGround = false;
      Player.isOnTheWall = false;
      Player.isOnMovingPlatform = false;
      Player.canDoubleJump = true;
      Player.isWallJumping = true;//false; // hack
      Player.elapsedShotTime = 0.0f;
      Player.healthPoints = Player.maxHealthPoints;
      Player.elapsedInvulnerabilityTime = 0.0f;
      Player.invulnerable = false;
      Player.alphaValue = 1f;
      Player.haltInput = false;
      Player.currentAnimation = Player.AnimationTypes.idle;
    }

    public static Rectangle CollisionRectangle
    {
      get
      {
        return new Rectangle((int) Player.position.X, (int) Player.position.Y + 2, Player.width, Player.height - 4);
      }
    }

    public static Vector2 Center
    {
      get
      {
        return new Vector2(Player.position.X + (float) Player.width / 2f, Player.position.Y + (float) Player.height / 2f);
      }
    }

    public static Vector2 JumpSpeed => Player.jumpSpeed;

    private static Vector2 ProjectileInitialVelocity
    {
      get
      {
        return new Vector2((Player.isFacingRight ? Player.projectileInitialVelocity.X : -Player.projectileInitialVelocity.X) 
            + Player.jumpSpeed.X, Player.projectileInitialVelocity.Y + 0.5f * Player.jumpSpeed.Y);
      }
    }

    public static bool IsOnMovingPlatform
    {
      get => Player.isOnMovingPlatform;
      set => Player.isOnMovingPlatform = value;
    }

    public static void Update(float elapsedTime)
    {
      if (Player.healthPoints > 0 && !Player.haltInput)
      {
        Player.CheckMovementInput();
        Player.CheckAttackInput(elapsedTime);
      }
      Player.Move(elapsedTime);
      Player.AnimationStateMachine(elapsedTime);
      if (!Player.invulnerable)
        return;
      Player.elapsedInvulnerabilityTime += elapsedTime;
      Player.alphaValue = Player.alphaValue + 0.1f - (float) (int) ((double) Player.alphaValue + 0.10000000149011612);
      if ((double) Player.elapsedInvulnerabilityTime <= (double) Player.invulnerabilityTime)
        return;
      Player.invulnerable = false;
      Player.alphaValue = 1f;
    }

    private static void AnimationStateMachine(float elapsedTime)
    {
      switch (Player.currentAnimation)
      {
        case Player.AnimationTypes.idle:
          Player.animations[2].Reset();
          break;
        case Player.AnimationTypes.walk:
          Player.animations[2].Reset();
          break;
        case Player.AnimationTypes.attack:
          if (!Player.animations[(int) Player.currentAnimation].Active)
          {
            Player.animations[3].Reset();
            Player.animations[2].Reset();
            Player.currentAnimation = Player.AnimationTypes.idle;
            break;
          }
          break;
        case Player.AnimationTypes.fall:
          Player.animations[2].Reset();
          break;
        case Player.AnimationTypes.die:
          if (!Player.animations[(int) Player.currentAnimation].Active)
          {
            Player.animations[3].Reset();
            Player.animations[2].Reset();
            Game1.currentGameState = Game1.GameStates.dead;
            break;
          }
          break;
        case Player.AnimationTypes.wall:
          if (!Player.isOnTheWall)
          {
            Player.currentAnimation = Player.AnimationTypes.jump;
            break;
          }
          break;
      }
      Player.animations[(int) Player.currentAnimation].Update(elapsedTime);
    }

    private static void CheckAttackInput(float elapsedTime)
    {
      if ((double) Player.elapsedShotTime > (double) Player.timeBetweenShots)
      {
            bool flag0 = false;
            int PrevVal = 0;

            try
            {
                if (Game1.currentTouchState.Count > 0)
                    PrevVal = Game1.previousTouchState.Count;
            }
            catch
            {
                Game1.previousTouchState = Game1.currentTouchState;
            }

                
            if (Game1.currentTouchState.Count > 0)
                flag0 = (Game1.currentTouchState.Count == 1 /*&& PrevVal != 1*/);                

                if 
                (
                    ( !Game1.currentKeyboardState.IsKeyDown(Keys.Space)/* || Game1.previousKeyboardState.IsKeyDown(Keys.Space) */) 
                    && ( Game1.currentGamePadState.Buttons.X != ButtonState.Pressed  /*|| Game1.previousGamePadState.Buttons.X != ButtonState.Released*/ )
                    && ( !flag0 )

                    || Player.isOnTheWall
                )
                    return;

        ProjectilesManager.ShootPlayerProjectile(Player.isFacingRight 
            ? Player.position + new Vector2((float) Player.width, 0.0f) 
            : Player.position, Player.ProjectileInitialVelocity);

        Player.elapsedShotTime = 0.0f;
        SoundEffects.PlayerAttack.Play();
        if (Player.currentAnimation == Player.AnimationTypes.die)
          return;
        Player.currentAnimation = Player.AnimationTypes.attack;
      }
      else
        Player.elapsedShotTime += elapsedTime;
    }

    private static void CheckMovementInput()
    {

        bool flag1 = false;
        float PrevPos = 0;

        try
        {
            if ((Game1.currentTouchState.Count > 0) &&
                (Game1.previousTouchState.Count > 0))
                PrevPos = Game1.previousTouchState[0].Position.Y;
        }
        catch
        {
            Game1.previousTouchState = Game1.currentTouchState;
        }

           
        if (Game1.currentTouchState.Count > 0)
            flag1 = (PrevPos - Game1.currentTouchState[0].Position.Y > 10);
           
        if (
             (!Game1.currentKeyboardState.IsKeyDown(Keys.Up)/*|| Game1.previousKeyboardState.IsKeyDown(Keys.Up)*/)
             && (!Game1.currentKeyboardState.IsKeyDown(Keys.W)/* || Game1.previousKeyboardState.IsKeyDown(Keys.W)*/)
             && (/*Game1.currentTouchState[0].Position.Y < Game1.previousTouchState[0].Position.Y*/!flag1)
             )
      {
        GamePadButtons buttons = Game1.currentGamePadState.Buttons;
        if (buttons.A == ButtonState.Pressed)
        {
          buttons = Game1.previousGamePadState.Buttons;
          if (buttons.A != ButtonState.Released)
            goto label_23;
        }
        else
          goto label_23;
      }

      if (Player.isTouchingTheGround)
      {
        if (Player.currentAnimation != Player.AnimationTypes.die 
             && Player.currentAnimation != Player.AnimationTypes.attack)
          Player.currentAnimation = Player.AnimationTypes.jump;

        Player.jumpSpeed.Y = Player.initialJumpSpeed.Y;
        SoundEffects.PlayerJump.Play();


            bool flag2 = false;
            float PrevPos2 = 0;

            try
            {
                    if (Game1.currentTouchState.Count > 0)
                        PrevPos2 = Game1.previousTouchState[0].Position.X;
            }
            catch
            {
                    Game1.previousTouchState = Game1.currentTouchState;
            }

            
            if (Game1.currentTouchState.Count > 0)
                flag2 = (Game1.currentTouchState[0].Position.X - PrevPos2 > 1);  



            if (
            ( !Game1.currentKeyboardState.IsKeyDown(Keys.Right) 
            && !Game1.currentKeyboardState.IsKeyDown(Keys.D))
            && (/*Game1.currentTouchState[0].Position.X > Game1.previousTouchState[0].Position.X*/!flag2)
           )
        {
          GamePadDPad dpad = Game1.currentGamePadState.DPad;
          if (dpad.Right != ButtonState.Pressed)
          {
            GamePadThumbSticks thumbSticks = Game1.currentGamePadState.ThumbSticks;

            bool flag3 = false;
            float PrevPos3 = 0;

            try
            {
                if (Game1.currentTouchState.Count > 0)
                    PrevPos3 = Game1.previousTouchState[0].Position.X;
            }
            catch
            {
                Game1.previousTouchState = Game1.currentTouchState;
            }

            if (Game1.currentTouchState.Count > 0)
               flag3 = (PrevPos3 - Game1.currentTouchState[0].Position.X > 1);
           

            if ((double) thumbSticks.Left.X <= 0.800000011920929)
            {
              if (
                     (!Game1.currentKeyboardState.IsKeyDown(Keys.Left) 
                     && !Game1.currentKeyboardState.IsKeyDown(Keys.A))
                     && (/*Game1.currentTouchState[0].Position.X < Game1.previousTouchState[0].Position.X*/!flag3)
                 )
              {
                dpad = Game1.currentGamePadState.DPad;
                if (dpad.Left != ButtonState.Pressed)
                {
                  thumbSticks = Game1.currentGamePadState.ThumbSticks;
                  if ((double) thumbSticks.Left.X >= -0.800000011920929)
                    goto label_14;
                }
              }
              Player.jumpSpeed.X = -Player.initialJumpSpeed.X;
              goto label_14;
            }
          }
        }
        Player.jumpSpeed.X = Player.initialJumpSpeed.X;
label_14:
        Player.isTouchingTheGround = false;
        Player.isOnMovingPlatform = false;
        return;
      }
      if (Player.isOnTheWall && Player.wallJumpUnlocked)
      {
        if (Player.currentAnimation != Player.AnimationTypes.die 
                    && Player.currentAnimation != Player.AnimationTypes.attack)
          Player.currentAnimation = Player.AnimationTypes.jump;

        Player.jumpSpeed.Y = Player.initialJumpSpeed.Y;
        SoundEffects.PlayerJump.Play();
        Player.canDoubleJump = true;//false;
        Player.isOnTheWall = false;
        Player.isWallJumping = true;
        Player.jumpSpeed.X = Player.isFacingRight ? -Player.initialJumpSpeed.X : Player.initialJumpSpeed.X;
        Player.isFacingRight = !Player.isFacingRight;
      }
      else if (Player.canDoubleJump && Player.doubleJumpUnlocked && !Player.isOnTheWall)
      {
        if (Player.currentAnimation != Player.AnimationTypes.die 
                    && Player.currentAnimation != Player.AnimationTypes.attack)
          Player.currentAnimation = Player.AnimationTypes.jump;

        Player.jumpSpeed.Y = Player.initialJumpSpeed.Y;
        SoundEffects.PlayerJump.Play();
        Player.canDoubleJump = false;
        return;
      }
label_23:
      GamePadDPad dpad1;
      GamePadThumbSticks thumbSticks1;

        bool flag4 = false;
        float PrevPos4 = 0;

        try
        { 
          if (Game1.previousTouchState.Count > 0)
                PrevPos4 = Game1.previousTouchState[0].Position.X;
        }
        catch
        {
                Game1.previousTouchState = Game1.currentTouchState;
        }

        if (Game1.currentTouchState.Count > 0)
          flag4 = (Game1.currentTouchState[0].Position.X - PrevPos4 > 1);
           

      if 
      (
          (!Game1.currentKeyboardState.IsKeyDown(Keys.Right) 
          && !Game1.currentKeyboardState.IsKeyDown(Keys.D))
          && (/*Game1.currentTouchState[0].Position.X > Game1.previousTouchState[0].Position.X*/!flag4)
      )
      {
        dpad1 = Game1.currentGamePadState.DPad;
        if (dpad1.Right != ButtonState.Pressed)
        {
          thumbSticks1 = Game1.currentGamePadState.ThumbSticks;
          if ((double) thumbSticks1.Left.X <= 0.800000011920929)
            goto label_28;
        }
      }
      Player.horizontalMovementSpeed += Player.maxHorizontalMovementSpeed;

      if ( Player.currentAnimation != Player.AnimationTypes.die 
                && Player.currentAnimation != Player.AnimationTypes.attack && Player.isTouchingTheGround )
        Player.currentAnimation = Player.AnimationTypes.walk;
label_28:


        bool flag5 = false;
        float PrevPos5 = 0;

        try
        {
                if (Game1.previousTouchState.Count > 0)
                    PrevPos5 = Game1.previousTouchState[0].Position.X;
        }
        catch
        {
                Game1.previousTouchState = Game1.currentTouchState;
        }

        if (Game1.currentTouchState.Count > 0)
          flag5 = (PrevPos5 - Game1.currentTouchState[0].Position.X > 1);
       

      if 
      ( (!Game1.currentKeyboardState.IsKeyDown(Keys.Left) 
        && !Game1.currentKeyboardState.IsKeyDown(Keys.A))
        &&  (/*Game1.currentTouchState[0].Position.X < Game1.previousTouchState[0].Position.X*/!flag5)
      )
      {
        dpad1 = Game1.currentGamePadState.DPad;
        if (dpad1.Left != ButtonState.Pressed)
        {
          thumbSticks1 = Game1.currentGamePadState.ThumbSticks;
          if ((double) thumbSticks1.Left.X >= -0.800000011920929)
            goto label_33;
        }
      }
      Player.horizontalMovementSpeed -= Player.maxHorizontalMovementSpeed;
      if (Player.currentAnimation != Player.AnimationTypes.die 
                && Player.currentAnimation != Player.AnimationTypes.attack && Player.isTouchingTheGround)
        Player.currentAnimation = Player.AnimationTypes.walk;

label_33:
      if (Player.isTouchingTheGround && (double) Player.horizontalMovementSpeed == 0.0 
                && Player.currentAnimation != Player.AnimationTypes.die 
                && Player.currentAnimation != Player.AnimationTypes.attack)
      {
        Player.currentAnimation = Player.AnimationTypes.idle;
      }
      else
      {
        if (Player.isTouchingTheGround || !Player.isOnTheWall
             || Player.currentAnimation == Player.AnimationTypes.die)
          return;
        Player.currentAnimation = Player.AnimationTypes.wall;
      }
    }

    private static void Move(float elapsedTime)
    {
      Vector2 vector2 = new Vector2();
      int num1 = (int) MathHelper.Clamp(Player.position.Y / (float) Tile.tileSize, 0.0f, 
          (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));

      int num2 = (int) MathHelper.Clamp((float) ((double) Player.position.Y +
          (double) Player.height - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, 
          (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int index1 = (int) MathHelper.Clamp((Player.position.X - 1f) / (float) Tile.tileSize,
          0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));

      int index2 = (int) MathHelper.Clamp((float) ((double) Player.position.X + 1.0 
          + (double) Player.width - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, 
          (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      if (Player.isOnTheWall)
      {
        Player.isOnTheWall = false;
        if (Player.isFacingRight)
        {
          for (int index3 = num1; index3 <= num2; ++index3)
          {
            if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index3, index2].isSolid())
              Player.isOnTheWall = true;
          }
        }
        else
        {
          for (int index4 = num1; index4 <= num2; ++index4)
          {
            if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index4, index1].isSolid())
              Player.isOnTheWall = true;
          }
        }
      }

      if (Player.isWallJumping)
      {
        if ((double) Math.Abs(Player.jumpSpeed.X) > (double) Player.maxHorizontalMovementSpeed)
          vector2.X = Player.jumpSpeed.X;
        else if (Math.Sign(Player.jumpSpeed.X) == -Math.Sign(Player.horizontalMovementSpeed))
        {
          if ((double) Math.Abs(Player.jumpSpeed.X) >= (double) Player.maxHorizontalMovementSpeed / 2.0)
          {
            vector2.X = Player.jumpSpeed.X;
          }
          else
          {
            Player.isWallJumping = false;
            Player.jumpSpeed.X = 0.0f;
            vector2.X = Player.horizontalMovementSpeed;
          }
        }
        else
        {
          vector2.X = Player.jumpSpeed.X + Player.horizontalMovementSpeed;
          vector2.X = MathHelper.Clamp(vector2.X, -Player.maxHorizontalMovementSpeed, Player.maxHorizontalMovementSpeed);
        }
        Player.jumpSpeed.X -= 2f * Player.jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;
      }
      else if (!Player.isTouchingTheGround)
      {
        if (Math.Sign(Player.jumpSpeed.X) == -Math.Sign(Player.horizontalMovementSpeed))
          vector2.X = Player.horizontalMovementSpeed;
        else if ((double) Math.Abs(Player.jumpSpeed.X) > (double) Player.maxHorizontalMovementSpeed)
        {
          vector2.X = Player.jumpSpeed.X;
          Player.jumpSpeed.X -= 2f * Player.jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;
        }
        else
        {
          vector2.X = Player.jumpSpeed.X + Player.horizontalMovementSpeed;
          vector2.X = MathHelper.Clamp(vector2.X, -Player.maxHorizontalMovementSpeed, Player.maxHorizontalMovementSpeed);
          Player.jumpSpeed.X -= 6f * Player.jumpSpeed.X * Gravity.airFrictionCoeff * elapsedTime;
        }
      }
      else
        vector2.X = Player.horizontalMovementSpeed;

      Player.horizontalMovementSpeed = 0.0f;
      if ((double) vector2.X > 0.0)
        Player.isFacingRight = true;
      else if ((double) vector2.X < 0.0)
        Player.isFacingRight = false;
      if (!Player.isOnMovingPlatform)
      {
        if (Player.isOnTheWall)
        {
          if ((double) Gravity.gravityAcceleration - (double) Player.jumpSpeed.Y * (double) Gravity.wallFrictionCoeff > 0.0)
            Player.jumpSpeed.Y += (Gravity.gravityAcceleration - Player.jumpSpeed.Y * Gravity.wallFrictionCoeff) * elapsedTime;
        }
        else if ((double) Gravity.gravityAcceleration - (double) Player.jumpSpeed.Y * (double) Gravity.airFrictionCoeff > 0.0)
          Player.jumpSpeed.Y += (Gravity.gravityAcceleration - Player.jumpSpeed.Y * Gravity.airFrictionCoeff) * elapsedTime;
        if ((double) Player.jumpSpeed.Y > (double) Gravity.gravityAcceleration * (double) elapsedTime * 15.0)
        {
          Player.isTouchingTheGround = false;
          Player.isOnMovingPlatform = false;
          if (Player.currentAnimation != Player.AnimationTypes.die 
                        && Player.currentAnimation != Player.AnimationTypes.attack && !Player.isOnTheWall)
            Player.currentAnimation = Player.AnimationTypes.fall;
        }
        vector2.Y = Player.jumpSpeed.Y;
      }
      else
        vector2 += PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex].Shift / elapsedTime;
      Player.position.X += vector2.X * elapsedTime;
      int num3 = (int) MathHelper.Clamp(Player.position.Y / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int num4 = (int) MathHelper.Clamp((float) ((double) Player.position.Y + (double) Player.height - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int index5 = (int) MathHelper.Clamp((Player.position.X - 1f) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      int index6 = (int) MathHelper.Clamp((float) ((double) Player.position.X + 1.0 + (double) Player.width - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      if ((double) vector2.X >= 0.0)
      {
        for (int index7 = num3; index7 <= num4; ++index7)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index7, index6].isSolid())
          {
            Player.position.X = (float) (index6 * Tile.tileSize - Player.width);
            if (!Player.isTouchingTheGround)
            {
              Player.isWallJumping = false;
              if ((double) Player.jumpSpeed.Y > 0.0 && !Player.isOnTheWall)
                Player.jumpSpeed.Y = 0.0f;
              if ((double) Player.jumpSpeed.Y > (double) Player.initialJumpSpeed.Y * 0.20000000298023224)
                Player.isOnTheWall = true;
              Player.jumpSpeed.X = 0.0f;
              break;
            }
            break;
          }
        }
      }
      if ((double) vector2.X <= 0.0)
      {
        for (int index8 = num3; index8 <= num4; ++index8)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index8, index5].isSolid())
          {
            Player.position.X = (float) ((index5 + 1) * Tile.tileSize);
            if (!Player.isTouchingTheGround)
            {
              Player.isWallJumping = false;
              if ((double) Player.jumpSpeed.Y > 0.0 && !Player.isOnTheWall)
                Player.jumpSpeed.Y = 0.0f;
              if ((double) Player.jumpSpeed.Y > (double) Player.initialJumpSpeed.Y * 0.20000000298023224)
                Player.isOnTheWall = true;
              Player.jumpSpeed.X = 0.0f;
              break;
            }
            break;
          }
        }
      }
      Player.position.Y += vector2.Y * elapsedTime;
      int index9 = (int) MathHelper.Clamp(Player.position.Y / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int num5 = (int) MathHelper.Clamp((float) ((double) Player.position.Y + (double) Player.height - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int num6 = (int) MathHelper.Clamp(Player.position.X / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      int num7 = (int) MathHelper.Clamp((float) ((double) Player.position.X + (double) Player.width - 1.0 / 1000.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      if ((double) vector2.Y > 0.0)
      {
        for (int index10 = (int) ((double) vector2.Y * (double) elapsedTime + (double) Tile.tileSize) / Tile.tileSize - 1; index10 >= 0; --index10)
        {
          for (int index11 = num6; index11 <= num7 && num5 - index10 >= 0; ++index11)
          {
            if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[num5 - index10, index11].isSolid())
            {
              Player.position.Y = (float) ((num5 - index10) * Tile.tileSize - Player.height);
              Player.jumpSpeed.Y = 0.0f;
              Player.jumpSpeed.X = 0.0f;
              Player.isTouchingTheGround = true;
              Player.isOnMovingPlatform = false;
              Player.canDoubleJump = true;
              Player.isOnTheWall = false;
              Player.isWallJumping = false;
              index10 = 0;
              break;
            }
          }
        }
      }
      else if ((double) vector2.Y < 0.0)
      {
        for (int index12 = num6; index12 <= num7; ++index12)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index9, index12].isSolid())
          {
            Player.position.Y = (float) ((index9 + 1) * Tile.tileSize);
            Player.jumpSpeed.Y = 0.0f;
            Player.isOnMovingPlatform = false;
            break;
          }
        }
      }
      if (!Player.isOnMovingPlatform)
      {
        for (int index13 = 0; index13 < PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length; ++index13)
        {
          MovingPlatform movingPlatform = PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[index13];
          if (!movingPlatform.Transparent && (double) Player.position.X + (double) Player.width > (double) movingPlatform.Position.X && (double) Player.position.X < (double) movingPlatform.Position.X + (double) movingPlatform.width && (double) Player.position.Y + (double) Player.height - (double) movingPlatform.Position.Y >= 0.0 && (double) Player.position.Y + (double) Player.height - (double) movingPlatform.Position.Y <= (double) vector2.Y * (double) elapsedTime - (double) movingPlatform.Shift.Y)
          {
            Player.isOnMovingPlatform = true;
            Player.position.Y = movingPlatform.Position.Y - (float) Player.height;
            Player.jumpSpeed.Y = 0.0f;
            Player.jumpSpeed.X = 0.0f;
            PlatformsManager.platformIndex = index13;
            Player.isTouchingTheGround = true;
            Player.canDoubleJump = true;
            Player.isOnTheWall = false;
            Player.isWallJumping = false;
            break;
          }
        }
      }
      else
      {
        MovingPlatform movingPlatform = PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[PlatformsManager.platformIndex];
        if ((double) Player.position.X - (double) movingPlatform.Position.X > (double) -Player.width && (double) Player.position.X - (double) movingPlatform.Position.X < (double) movingPlatform.width && !movingPlatform.Transparent)
          return;
        Player.isOnMovingPlatform = false;
      }
    }

    public static void Rebound(float ratioReboundToNormalJump)
    {
      Player.jumpSpeed.Y = Player.initialJumpSpeed.Y * ratioReboundToNormalJump;
      SoundEffects.PlayerJump.Play();
      Player.canDoubleJump = true;
      Player.isTouchingTheGround = false;
      Player.isOnMovingPlatform = false;
      Player.isWallJumping = false;
    }

    public static void Rebound(
      float ratioReboundToNormalJump_Y,
      float ratioReboundToNormalJump_X,
      bool toTheRight)
    {
      Player.jumpSpeed.X = (float) ((double) Player.initialJumpSpeed.X * (double) ratioReboundToNormalJump_X * (toTheRight ? 1.0 : -1.0));
      Player.jumpSpeed.Y = Player.initialJumpSpeed.Y * ratioReboundToNormalJump_Y;
      SoundEffects.PlayerJump.Play();
      Player.canDoubleJump = true;
      Player.isTouchingTheGround = false;
      Player.isOnMovingPlatform = false;
      Player.isWallJumping = false;
    }

    public static void takeDamage(int damage = 1, bool damageEvenIfInvulnerable = false)
    {
      if (Player.invulnerable && !damageEvenIfInvulnerable || Player.healthPoints <= 0)
        return;

      // Hack
      //Player.healthPoints -= damage; // decrease lifes life count

      SoundEffects.PlayerHurt.Play();
      ++GameStats.hitsCount;
      if (Player.healthPoints <= 0)
      {
        Player.currentAnimation = Player.AnimationTypes.die;
        Player.healthPoints = 0;
        ++GameStats.deathsCount;
      }
      else
      {
        Player.invulnerable = true;
        Player.elapsedInvulnerabilityTime = 0.0f;
      }
    }

    public static void IncreaseHealth()
    {
      if (Player.healthPoints >= Player.maxHealthPoints)
        return;
      ++Player.healthPoints;
    }

    public static void ReplenishHealth()
    {
      Player.healthPoints = Player.maxHealthPoints;
      Player.animations[5].Reset();
      Player.currentAnimation = Player.AnimationTypes.idle;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(Player.spritesheet, Camera.RelativeRectangle(new Vector2(Player.Center.X - (float) (Player.animations[(int) Player.currentAnimation].Frame.Width / 2), Player.position.Y - (float) Player.animations[(int) Player.currentAnimation].Frame.Height + (float) Player.height), Player.animations[(int) Player.currentAnimation].Frame.Width, Player.animations[(int) Player.currentAnimation].Frame.Height), new Rectangle?(Player.animations[(int) Player.currentAnimation].Frame), Color.White * (Player.invulnerable ? Player.alphaValue : 1f), 0.0f, Vector2.Zero, Player.isFacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
    }

    public static void DrawGUI(SpriteBatch spriteBatch)
    {
      for (int index = 0; index < Player.healthPoints; ++index)
        spriteBatch.Draw(Collectable.Sprites,
            new Vector2((float) (5 + index * 16), 5f),
            new Rectangle?(new Rectangle(0, 110, 16, 19)), Color.White);
    }

    private enum AnimationTypes
    {
      idle,
      walk,
      jump,
      attack,
      fall,
      die,
      wall,
      total,
    }
  }
}

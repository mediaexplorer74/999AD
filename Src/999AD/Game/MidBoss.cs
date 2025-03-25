
// Type: GameManager.MidBoss
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameManager
{
  internal static class MidBoss
  {
    private static Texture2D bossSheet;
    private static Animation[] bossAnimations;
    public static readonly float timeUntilShot = 2f;
    private static float elapsedShotTime;
    private static readonly float timeUntilChange = 0.5f;
    private static float elapsedChangeTime;
    public static readonly int bossWidth = 48;
    public static readonly int bossHeight = 48;
    private static Vector2 point1;
    private static Vector2 point2;
    private static Vector2 bossPoint;
    private static bool isFacingLeft = true;
    private static float movementSpeed;
    private static bool moveToP1;
    private static bool moveToP2;
    public static readonly Vector2 projectileInitialVelocity = new Vector2(500f, -75f);
    public static MidBoss.BossState bossState = MidBoss.BossState.idle;
    private static readonly int maxHP = 10;
    private static int bossHP;
    private static Random random = new Random();
    private static bool dead;
    private static Color bossColor = Color.White;
    private static readonly float timeRedColor = 0.2f;
    private static float elapsedRedColorTime;

    public static void Initialise(Texture2D BossSheet)
    {
      MidBoss.dead = false;
      MidBoss.bossSheet = BossSheet;
      MidBoss.bossAnimations = new Animation[4]
      {
        new Animation(new Rectangle(0, 0, 240, 48), 48, 48, 5, 0.2f, true),
        new Animation(new Rectangle(0, 48, 480, 48), 48, 48, 10, 0.2f, true),
        new Animation(new Rectangle(0, 96, 624, 48), 48, 48, 13, 0.2f, true),
        new Animation(new Rectangle(0, 144, 336, 48), 48, 48, 7, 0.2f, false, true)
      };
      MidBoss.Reset();
    }

    public static void Reset()
    {
      MidBoss.point1 = new Vector2(350f, 193f);
      MidBoss.point2 = new Vector2(150f, 193f);
      MidBoss.bossColor = Color.White;
      MidBoss.bossHP = MidBoss.maxHP;
      MidBoss.bossPoint = MidBoss.point1;
      MidBoss.movementSpeed = 0.0f;
      MidBoss.moveToP2 = false;
      MidBoss.moveToP1 = false;
    }

    public static Rectangle BossCollisionRect
    {
      get
      {
        return new Rectangle((int) ((double) MidBoss.bossPoint.X - (double) (MidBoss.bossWidth / 2)), (int) ((double) MidBoss.bossPoint.Y - (double) (MidBoss.bossHeight / 2)), MidBoss.bossWidth, MidBoss.bossHeight);
      }
    }

    public static Rectangle BossDrawRect
    {
      get
      {
        return new Rectangle((int) ((double) MidBoss.bossPoint.X - (double) (MidBoss.bossAnimations[(int) MidBoss.bossState].Frame.Width / 2)), (int) ((double) MidBoss.bossPoint.Y - (double) (MidBoss.bossAnimations[(int) MidBoss.bossState].Frame.Height / 2)), MidBoss.bossAnimations[(int) MidBoss.bossState].Frame.Width, MidBoss.bossAnimations[(int) MidBoss.bossState].Frame.Height);
      }
    }

    public static bool Dead
    {
      get => MidBoss.dead;
      set => MidBoss.dead = value;
    }

    public static void Update(float elapsedTime)
    {
      if (MidBoss.dead)
        return;
      if ((double) MidBoss.elapsedRedColorTime < (double) MidBoss.timeRedColor)
      {
        MidBoss.elapsedRedColorTime += elapsedTime;
        if ((double) MidBoss.elapsedRedColorTime >= (double) MidBoss.timeRedColor)
          MidBoss.bossColor = Color.White;
      }
      MidBoss.bossAnimations[(int) MidBoss.bossState].Update(elapsedTime);
      if (MidBoss.bossHP <= 0)
        MidBoss.bossState = MidBoss.BossState.death;
      MidBoss.isFacingLeft = MidBoss.BossCollisionRect.X + 5 < Player.CollisionRectangle.X;
      switch (MidBoss.bossState)
      {
        case MidBoss.BossState.idle:
          MidBoss.ChangeFromIdle(elapsedTime);
          break;
        case MidBoss.BossState.move:
          MidBoss.Move(elapsedTime);
          break;
        case MidBoss.BossState.attack:
          MidBoss.Attack(elapsedTime);
          break;
        case MidBoss.BossState.death:
          MidBoss.Death();
          break;
      }
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      if (MidBoss.dead)
        return;
      if (MidBoss.moveToP2)
        spriteBatch.Draw(MidBoss.bossSheet, Camera.RelativeRectangle(MidBoss.BossDrawRect), new Rectangle?(MidBoss.bossAnimations[(int) MidBoss.bossState].Frame), MidBoss.bossColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
      else if (MidBoss.moveToP1)
        spriteBatch.Draw(MidBoss.bossSheet, Camera.RelativeRectangle(MidBoss.BossDrawRect), new Rectangle?(MidBoss.bossAnimations[(int) MidBoss.bossState].Frame), MidBoss.bossColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      else
        spriteBatch.Draw(MidBoss.bossSheet, Camera.RelativeRectangle(MidBoss.BossDrawRect), new Rectangle?(MidBoss.bossAnimations[(int) MidBoss.bossState].Frame), MidBoss.bossColor, 0.0f, Vector2.Zero, MidBoss.isFacingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
    }

    public static void Attack(float elapsedTime)
    {
      if ((double) MidBoss.elapsedShotTime > (double) MidBoss.timeUntilShot)
      {
        ProjectilesManager.ShootBossProjectile(MidBoss.isFacingLeft ? MidBoss.bossPoint : MidBoss.bossPoint - new Vector2((float) (MidBoss.bossWidth - 5), 0.0f), MidBoss.projectileInitialVelocity * (MidBoss.isFacingLeft ? new Vector2(1f, 1f) : new Vector2(-1f, 1f)));
        MidBoss.elapsedShotTime = 0.0f;
        SoundEffects.MidbossAttack.Play();
        MidBoss.bossState = MidBoss.BossState.idle;
      }
      else
        MidBoss.elapsedShotTime += elapsedTime;
    }

    public static void Move(float elapsedTime)
    {
      MidBoss.movementSpeed = 100f;
      if (MidBoss.bossPoint == MidBoss.point1)
        MidBoss.moveToP2 = true;
      else if (MidBoss.bossPoint == MidBoss.point2)
        MidBoss.moveToP1 = true;
      if (MidBoss.moveToP2)
      {
        MidBoss.bossPoint.X -= MidBoss.movementSpeed * elapsedTime;
        if ((double) MidBoss.bossPoint.X <= (double) MidBoss.point2.X)
        {
          MidBoss.bossPoint = MidBoss.point2;
          MidBoss.moveToP2 = false;
          MidBoss.movementSpeed = 0.0f;
          MidBoss.bossState = MidBoss.BossState.idle;
        }
      }
      if (!MidBoss.moveToP1)
        return;
      MidBoss.bossPoint.X += MidBoss.movementSpeed * elapsedTime;
      if ((double) MidBoss.bossPoint.X < (double) MidBoss.point1.X)
        return;
      MidBoss.bossPoint = MidBoss.point1;
      MidBoss.moveToP1 = false;
      MidBoss.movementSpeed = 0.0f;
      MidBoss.bossState = MidBoss.BossState.idle;
    }

    public static bool BossHitByRect(Rectangle collisionRect)
    {
      if (!MidBoss.BossCollisionRect.Intersects(collisionRect))
        return false;
      --MidBoss.bossHP;
      SoundEffects.MidbossHurt.Play();
      MidBoss.bossColor = Color.Red * 0.8f;
      MidBoss.elapsedRedColorTime = 0.0f;
      return true;
    }

    public static bool PlayerHitByRect(Rectangle collisionRect)
    {
      if (!Player.CollisionRectangle.Intersects(collisionRect))
        return false;
      Player.takeDamage();
      return true;
    }

    public static void ChangeFromIdle(float elapsedTime)
    {
      if ((double) MidBoss.elapsedChangeTime > (double) MidBoss.timeUntilChange)
      {
        MidBoss.ChangeState();
        MidBoss.elapsedChangeTime = 0.0f;
      }
      else
        MidBoss.elapsedChangeTime += elapsedTime;
    }

    public static void ChangeState()
    {
      if ((double) Math.Abs(MidBoss.bossPoint.X - (float) Player.CollisionRectangle.X) >= 75.0)
        return;
      switch (MidBoss.random.Next(0, 2))
      {
        case 0:
          MidBoss.bossState = MidBoss.BossState.move;
          SoundEffects.MidbossMove.Play();
          break;
        case 1:
          MidBoss.bossState = MidBoss.BossState.attack;
          break;
      }
    }

    public static void Death()
    {
      if (MidBoss.bossAnimations[(int) MidBoss.bossState] != MidBoss.bossAnimations[3])
      {
        SoundEffects.MidbossHurt.Play();
        MidBoss.bossAnimations[(int) MidBoss.bossState] = MidBoss.bossAnimations[3];
      }
      else
      {
        if (MidBoss.bossAnimations[(int) MidBoss.bossState].Active)
          return;
        MidBoss.dead = true;
        LoadSaveManager.SaveGameProgress();
        MidBoss.bossPoint = new Vector2((float) -MidBoss.bossWidth, (float) -MidBoss.bossHeight);
      }
    }

    public enum BossState
    {
      idle,
      move,
      attack,
      death,
      total,
    }
  }
}

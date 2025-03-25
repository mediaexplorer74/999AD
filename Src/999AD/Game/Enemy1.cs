
// Type: GameManager.Enemy1
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameManager
{
  internal class Enemy1
  {
    private static Texture2D enemySheet;
    private Animation[] enemyAnimations;
    public readonly int width = 42;
    public readonly int height = 48;
    private Vector2 currentPoint;
    private Vector2 enemyPoint;
    private Vector2 enemyPoint2;
    private bool isFacingLeft = true;
    private float movementSpeed;
    private readonly int collisionDistance = 50;
    private bool hit;
    public Enemy1.EnemyState enemyState;
    private readonly int maxHP = 2;
    private int enemyHP;
    private bool dead;
    private Color enemyColor = Color.White;
    private readonly float timeRedColor = 0.2f;
    private float elapsedRedColorTime;

    public Enemy1(Vector2 EnemyPoint, Vector2 EnemyPoint2)
    {
      this.elapsedRedColorTime = this.timeRedColor;
      this.enemyPoint = EnemyPoint;
      this.enemyPoint2 = EnemyPoint2;
      if ((double) this.enemyPoint.X > (double) this.enemyPoint2.X)
      {
        Vector2 enemyPoint2 = this.enemyPoint2;
        this.enemyPoint2 = this.enemyPoint;
        this.enemyPoint = this.enemyPoint2;
      }
      this.enemyColor = Color.White;
      this.enemyHP = this.maxHP;
      this.currentPoint = this.enemyPoint;
      this.movementSpeed = 0.0f;
      this.enemyAnimations = new Animation[3]
      {
        new Animation(new Rectangle(0, 0, 420, 48), 42, 48, 10, 0.2f, true),
        new Animation(new Rectangle(0, 48, 420, 48), 42, 48, 10, 0.2f, true),
        new Animation(new Rectangle(0, 96, 252, 48), 42, 48, 5, 0.2f, false, true)
      };
    }

    public static void Inizialize(Texture2D spritesheet) => Enemy1.enemySheet = spritesheet;

    public Rectangle Enemy1CollisionRect
    {
      get
      {
        return new Rectangle((int) ((double) this.currentPoint.X - (double) (this.width / 2)), (int) ((double) this.currentPoint.Y - (double) (this.height / 2)), this.width, this.height);
      }
    }

    public Rectangle Enemy1DrawRect
    {
      get
      {
        return new Rectangle((int) ((double) this.currentPoint.X - (double) (this.enemyAnimations[(int) this.enemyState].Frame.Width / 2)), (int) ((double) this.currentPoint.Y - (double) (this.enemyAnimations[(int) this.enemyState].Frame.Height / 2)), this.enemyAnimations[(int) this.enemyState].Frame.Width, this.enemyAnimations[(int) this.enemyState].Frame.Height);
      }
    }

    public bool Dead => this.dead;

    public void Update(float elapsedTime)
    {
      if (this.dead)
        return;
      if ((double) this.elapsedRedColorTime < (double) this.timeRedColor)
      {
        this.elapsedRedColorTime += elapsedTime;
        if ((double) this.elapsedRedColorTime >= (double) this.timeRedColor)
          this.enemyColor = Color.White;
      }
      this.enemyAnimations[(int) this.enemyState].Update(elapsedTime);
      if (this.enemyHP <= 0)
        this.enemyState = Enemy1.EnemyState.death;
      this.isFacingLeft = this.Enemy1CollisionRect.X + 5 < Player.CollisionRectangle.X;
      switch (this.enemyState)
      {
        case Enemy1.EnemyState.idle:
          this.ChangeFromIdle();
          break;
        case Enemy1.EnemyState.attack:
          this.Attack(elapsedTime);
          break;
        case Enemy1.EnemyState.death:
          this.Death();
          break;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.dead)
        return;
      spriteBatch.Draw(Enemy1.enemySheet, Camera.RelativeRectangle(this.Enemy1DrawRect), new Rectangle?(this.enemyAnimations[(int) this.enemyState].Frame), this.enemyColor, 0.0f, Vector2.Zero, this.isFacingLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
    }

    public void Attack(float elapsedTime)
    {
      this.CheckCollisions();
      if (!this.hit)
      {
        this.movementSpeed = 80f;
        if (this.isFacingLeft)
        {
          this.currentPoint.X += this.movementSpeed * elapsedTime;
          if ((double) this.currentPoint.X > (double) this.enemyPoint2.X)
          {
            this.currentPoint.X = this.enemyPoint2.X;
            this.enemyState = Enemy1.EnemyState.idle;
          }
        }
        else
        {
          this.currentPoint.X -= this.movementSpeed * elapsedTime;
          if ((double) this.currentPoint.X < (double) this.enemyPoint.X)
          {
            this.currentPoint.X = this.enemyPoint.X;
            this.enemyState = Enemy1.EnemyState.idle;
          }
        }
        this.PlayerHitByEnemy1();
      }
      else
      {
        Player.Rebound(0.2f, 1.4f, (double) Player.Center.X > (double) this.currentPoint.X);
        this.hit = false;
      }
    }

    public bool Enemy1HitByRect(Rectangle collisionRect)
    {
      if (!this.Enemy1CollisionRect.Intersects(collisionRect))
        return false;
      --this.enemyHP;
      SoundEffects.EnemyHurt.Play();
      this.enemyColor = Color.Red * 0.6f;
      this.elapsedRedColorTime = 0.0f;
      return true;
    }

    public void PlayerHitByEnemy1()
    {
      if (!this.Enemy1CollisionRect.Intersects(Player.CollisionRectangle))
        return;
      Player.takeDamage();
      this.hit = true;
      this.movementSpeed = 0.0f;
    }

    public void ChangeFromIdle()
    {
      this.CheckCollisions();
      if ((double) Math.Abs(this.currentPoint.Y - (float) Player.CollisionRectangle.Y) < 15.0 && ((double) Player.CollisionRectangle.X > (double) this.enemyPoint2.X + 15.0 || (double) Player.CollisionRectangle.Right < (double) this.enemyPoint.X - 15.0))
      {
        this.enemyState = Enemy1.EnemyState.idle;
      }
      else
      {
        if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) > (double) this.collisionDistance || (double) Math.Abs(this.currentPoint.Y - (float) Player.CollisionRectangle.Y) >= 15.0)
          return;
        this.enemyState = Enemy1.EnemyState.attack;
        SoundEffects.Enemy1Attack.Play();
      }
    }

    public void Death()
    {
      this.enemyColor = Color.White;
      if (this.enemyAnimations[(int) this.enemyState] != this.enemyAnimations[2])
      {
        SoundEffects.EnemyHurt.Play();
        this.enemyAnimations[(int) this.enemyState] = this.enemyAnimations[2];
      }
      else
      {
        if (this.enemyAnimations[(int) this.enemyState].Active)
          return;
        this.dead = true;
      }
    }

    public void CheckCollisions()
    {
      if (this.dead)
        return;
      Rectangle rectangle = Player.CollisionRectangle;
      if (!rectangle.Intersects(this.Enemy1CollisionRect))
        return;
      rectangle = Player.CollisionRectangle;
      int bottom = rectangle.Bottom;
      rectangle = this.Enemy1CollisionRect;
      int top = rectangle.Top;
      if (Math.Abs(bottom - top) > 5)
        return;
      --this.enemyHP;
      SoundEffects.EnemyHurt.Play();
      Player.Rebound(0.75f);
      this.enemyColor = Color.Red * 0.6f;
      this.elapsedRedColorTime = 0.0f;
    }

    public enum EnemyState
    {
      idle,
      attack,
      death,
      total,
    }
  }
}

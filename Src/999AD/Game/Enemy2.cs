
// Type: GameManager.Enemy2
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameManager
{
  internal class Enemy2
  {
    private static Texture2D enemySheet;
    private Animation[] enemyAnimations;
    public readonly int width = 42;
    public readonly int height = 48;
    private Vector2 currentPoint;
    private Vector2 enemyPoint;
    private Vector2 enemyPoint2;
    private bool isFacingLeft = true;
    private bool moveToP1;
    private bool moveToP2;
    private bool moving;
    private float movementSpeed;
    private readonly int meleeDistance = 30;
    private readonly int shootDistance = 60;
    public Enemy2.EnemyState enemyState;
    private readonly int maxHP = 3;
    private int enemyHP;
    private bool dead;
    private Color enemyColor = Color.White;
    private readonly float timeRedColor = 0.2f;
    private float elapsedRedColorTime;
    public readonly float timeUntilShot = 3.5f;
    private float elapsedShotTime;
    public readonly float timeUntilMelee = 3f;
    private float elapsedMeleeTime;
    public readonly Vector2 projectileInitialVelocity = new Vector2(500f, -60f);

    public Enemy2(Vector2 EnemyPoint, Vector2 EnemyPoint2)
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
      this.enemyAnimations = new Animation[4]
      {
        new Animation(new Rectangle(0, 0, 440, 48), 44, 48, 10, 0.4f, true),
        new Animation(new Rectangle(0, 48, 616, 48), 56, 48, 11, 0.4f, true),
        new Animation(new Rectangle(0, 96, 704, 48), 44, 48, 16, 0.4f, true),
        new Animation(new Rectangle(0, 144, 264, 48), 44, 48, 5, 0.4f, false, true)
      };
    }

    public static void Inizialize(Texture2D spritesheet) => Enemy2.enemySheet = spritesheet;

    public Rectangle Enemy2CollisionRect
    {
      get
      {
        return new Rectangle((int) ((double) this.currentPoint.X - (double) (this.width / 2)), (int) ((double) this.currentPoint.Y - (double) (this.height / 2)), this.width, this.height);
      }
    }

    public Rectangle Enemy2DrawRect
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
        this.enemyState = Enemy2.EnemyState.death;
      this.isFacingLeft = this.Enemy2CollisionRect.X + 10 < Player.CollisionRectangle.X;
      switch (this.enemyState)
      {
        case Enemy2.EnemyState.idle:
          this.Idle(elapsedTime);
          break;
        case Enemy2.EnemyState.melee:
          this.Melee(elapsedTime);
          break;
        case Enemy2.EnemyState.attack:
          this.Attack(elapsedTime);
          break;
        case Enemy2.EnemyState.death:
          this.Death();
          break;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this.dead)
        return;
      if (this.moveToP2 && this.moving)
        spriteBatch.Draw(Enemy2.enemySheet, Camera.RelativeRectangle(this.Enemy2DrawRect), new Rectangle?(this.enemyAnimations[(int) this.enemyState].Frame), this.enemyColor, 0.0f, Vector2.Zero, SpriteEffects.None, 0.0f);
      else if (this.moveToP1 && this.moving)
        spriteBatch.Draw(Enemy2.enemySheet, Camera.RelativeRectangle(this.Enemy2DrawRect), new Rectangle?(this.enemyAnimations[(int) this.enemyState].Frame), this.enemyColor, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0.0f);
      else
        spriteBatch.Draw(Enemy2.enemySheet, Camera.RelativeRectangle(this.Enemy2DrawRect), new Rectangle?(this.enemyAnimations[(int) this.enemyState].Frame), this.enemyColor, 0.0f, Vector2.Zero, this.isFacingLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
    }

    public void Attack(float elapsedTime)
    {
      this.CheckCollisions();
      this.moving = false;
      this.isFacingLeft = this.Enemy2CollisionRect.X + 5 >= Player.CollisionRectangle.X;
      if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) < (double) this.meleeDistance && (double) Math.Abs(this.currentPoint.Y - (float) Player.CollisionRectangle.Y) < 5.0)
        this.enemyState = Enemy2.EnemyState.melee;
      else if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) >= (double) this.shootDistance)
        this.enemyState = Enemy2.EnemyState.idle;
      else if ((double) this.elapsedShotTime > (double) this.timeUntilShot)
      {
        SoundEffects.Enemy2Attack.Play();
        ProjectilesManager.ShootEnemyProjectile(this.currentPoint, this.projectileInitialVelocity * (this.isFacingLeft ? new Vector2(-1f, 1f) : new Vector2(1f, 1f)));
        this.elapsedShotTime = 0.0f;
      }
      else
        this.elapsedShotTime += elapsedTime;
    }

    public void Melee(float elapsedTime)
    {
      this.CheckCollisions();
      this.moving = false;
      this.isFacingLeft = this.Enemy2CollisionRect.X + 5 >= Player.CollisionRectangle.X;
      if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) >= (double) this.meleeDistance && (double) Math.Abs(this.currentPoint.Y - (float) Player.CollisionRectangle.Y) < 5.0)
        this.enemyState = Enemy2.EnemyState.attack;
      else if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) >= (double) this.shootDistance)
        this.enemyState = Enemy2.EnemyState.idle;
      else if ((double) this.elapsedMeleeTime > (double) this.timeUntilMelee)
      {
        SoundEffects.Enemy2Melee.Play();
        Player.takeDamage();
        this.elapsedMeleeTime = 0.0f;
        Player.Rebound(0.2f, 1.4f, (double) Player.Center.X > (double) this.currentPoint.X);
      }
      else
        this.elapsedMeleeTime += elapsedTime;
    }

    public bool Enemy2HitByRect(Rectangle collisionRect)
    {
      if (!this.Enemy2CollisionRect.Intersects(collisionRect))
        return false;
      --this.enemyHP;
      SoundEffects.EnemyHurt.Play();
      this.enemyColor = Color.Red * 0.6f;
      this.elapsedRedColorTime = 0.0f;
      return true;
    }

    public void Death()
    {
      this.enemyColor = Color.White;
      if (this.enemyAnimations[(int) this.enemyState] != this.enemyAnimations[3])
      {
        SoundEffects.EnemyHurt.Play();
        this.enemyAnimations[(int) this.enemyState] = this.enemyAnimations[3];
      }
      else
      {
        if (this.enemyAnimations[(int) this.enemyState].Active)
          return;
        this.dead = true;
      }
    }

    public void Idle(float elapsedTime)
    {
      this.CheckCollisions();
      this.moving = true;
      if ((double) Math.Abs(this.currentPoint.X - (float) Player.CollisionRectangle.X) < (double) this.shootDistance && (double) Math.Abs(this.currentPoint.Y - (float) Player.CollisionRectangle.Y) < 5.0)
      {
        this.movementSpeed = 0.0f;
        if (this.Enemy2CollisionRect.X + 5 < Player.CollisionRectangle.X)
          this.isFacingLeft = false;
        this.enemyState = Enemy2.EnemyState.attack;
      }
      else
      {
        this.movementSpeed = 25f;
        if (this.currentPoint == this.enemyPoint)
          this.moveToP2 = true;
        else if (this.currentPoint == this.enemyPoint2)
          this.moveToP1 = true;
        if (this.moveToP2)
        {
          this.currentPoint.X += this.movementSpeed * elapsedTime;
          if ((double) this.currentPoint.X >= (double) this.enemyPoint2.X)
          {
            this.moveToP2 = false;
            this.moveToP1 = true;
          }
        }
        if (!this.moveToP1)
          return;
        this.currentPoint.X -= this.movementSpeed * elapsedTime;
        if ((double) this.currentPoint.X > (double) this.enemyPoint.X)
          return;
        this.moveToP1 = false;
        this.moveToP2 = true;
      }
    }

    public bool PlayerHitByRect(Rectangle collisionRect)
    {
      if (!Player.CollisionRectangle.Intersects(collisionRect))
        return false;
      Player.takeDamage();
      return true;
    }

    public void CheckCollisions()
    {
      if (this.dead)
        return;
      Rectangle rectangle = Player.CollisionRectangle;
      if (!rectangle.Intersects(this.Enemy2CollisionRect))
        return;
      rectangle = Player.CollisionRectangle;
      int bottom = rectangle.Bottom;
      rectangle = this.Enemy2CollisionRect;
      int top = rectangle.Top;
      if (Math.Abs(bottom - top) <= 5)
      {
        Player.Rebound(0.75f);
        SoundEffects.EnemyHurt.Play();
        --this.enemyHP;
        this.enemyColor = Color.Red * 0.6f;
        this.elapsedRedColorTime = 0.0f;
      }
      else
      {
        Player.takeDamage();
        if ((double) Player.Center.X > (double) this.currentPoint.X)
          Player.position.X = this.currentPoint.X + (float) (this.width / 2);
        else
          Player.position.X = this.currentPoint.X - (float) (this.width / 2) - (float) Player.width;
      }
    }

    public enum EnemyState
    {
      idle,
      melee,
      attack,
      death,
      total,
    }
  }
}

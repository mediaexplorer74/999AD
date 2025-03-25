
// Type: GameManager.FireBall
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameManager
{
  internal class FireBall
  {
    private static Texture2D spritesheet;
    public static readonly int size = 10;
    public static readonly float radialShootingVelocity = 400f;
    public static readonly float shotLifeTime = 3f;
    private Vector2 rotationCenter;
    private Animation animation;
    private float radialDistance;
    private float angleRadiants;
    public float[] radialVelocities;
    public float[] angularVelocities;
    private float[] lifeTimes;
    private int phaseIndex;
    private float elapsedLifeTime;
    private bool targetPlayer;
    private bool active;

    public FireBall(
      float _angleDegrees,
      float[] _radialVelocities,
      float[] _angularVelocities,
      float[] _lifeTimes,
      bool _targetPlayer = false,
      float centerOffsetX = 0.0f,
      float centerOffsetY = 0.0f,
      float _radialDistance = 0.0f)
    {
      this.phaseIndex = 0;
      this.elapsedLifeTime = 0.0f;
      this.active = true;
      this.rotationCenter = FireBallsManager.fireballsCenter + new Vector2(centerOffsetX, centerOffsetY);
      this.radialDistance = _radialDistance;
      this.angleRadiants = (float) ((double) _angleDegrees / 180.0 * 3.1415927410125732);
      if ((double) this.angleRadiants >= 6.2831854820251465)
        this.angleRadiants -= 6.28318548f;
      else if ((double) this.angleRadiants < 0.0)
        this.angleRadiants += 6.28318548f;
      this.radialVelocities = _radialVelocities;
      this.angularVelocities = _angularVelocities;
      this.lifeTimes = _lifeTimes;
      this.targetPlayer = _targetPlayer;
      this.animation = new Animation(new Rectangle(336, 48, FireBall.size * 4, FireBall.size), FireBall.size, FireBall.size, 4, 0.1f, true);
    }

    public static void Inizialize(Texture2D _spritesheet) => FireBall.spritesheet = _spritesheet;

    public bool Active => this.active;

    public Rectangle CollisionRectangle
    {
      get
      {
        return new Rectangle((int) ((double) this.Center.X - (double) FireBall.size * 0.5), (int) ((double) this.Center.Y - (double) FireBall.size * 0.5), FireBall.size, FireBall.size);
      }
    }

    private Vector2 Center
    {
      get
      {
        return this.rotationCenter + this.radialDistance * new Vector2((float) Math.Sin((double) this.angleRadiants), -(float) Math.Cos((double) this.angleRadiants));
      }
    }

    public void Update(float elapsedTime)
    {
      this.animation.Update(elapsedTime);
      if ((double) this.elapsedLifeTime > (double) this.lifeTimes[this.phaseIndex])
      {
        this.elapsedLifeTime = 0.0f;
        ++this.phaseIndex;
        if (this.phaseIndex != this.lifeTimes.Length)
          return;
        this.active = false;
      }
      else
      {
        this.elapsedLifeTime += elapsedTime;
        this.radialDistance += this.radialVelocities[this.phaseIndex] * elapsedTime;
        this.angleRadiants += this.angularVelocities[this.phaseIndex] * elapsedTime;
        if ((double) this.angleRadiants >= 6.2831854820251465)
          this.angleRadiants -= 6.28318548f;
        else if ((double) this.angleRadiants < 0.0)
          this.angleRadiants += 6.28318548f;
        if (!this.targetPlayer || this.phaseIndex != this.lifeTimes.Length - 2 || !this.checkPlayerAngle())
          return;
        this.targetPlayer = false;
        this.phaseIndex = 0;
        this.elapsedLifeTime = 0.0f;
        this.angularVelocities = new float[1];
        this.radialVelocities = new float[1]
        {
          FireBall.radialShootingVelocity
        };
        this.lifeTimes = new float[1]
        {
          FireBall.shotLifeTime
        };
      }
    }

    private bool checkPlayerAngle()
    {
      Vector2 vector2 = Player.Center - this.rotationCenter;
      vector2.Normalize();
      float num = (float) Math.Acos((double) Vector2.Dot(vector2, new Vector2(0.0f, -1f)));
      return (double) Math.Abs(((double) vector2.X > 0.0 ? num : 6.28318548f - num) - this.angleRadiants) < 0.087266463249902282;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(FireBall.spritesheet, Camera.RelativeRectangle(new Vector2(this.Center.X - (float) FireBall.size * 0.5f, this.Center.Y - (float) FireBall.size * 0.5f), FireBall.size, FireBall.size), new Rectangle?(this.animation.Frame), Color.White);
    }
  }
}

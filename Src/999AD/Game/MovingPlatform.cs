
// Type: GameManager.MovingPlatform
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace GameManager
{
  internal class MovingPlatform
  {
    public static Texture2D spritesheet;
    public static Rectangle[] sourceRectangles;
    private readonly MovingPlatform.TextureType textureType;
    private Vector2 rotationCenter;
    public readonly int radius;
    private readonly Vector2 centerStartingPoint;
    private readonly Vector2 centerEndingPoint;
    public readonly int width;
    public readonly int height;
    private Vector2 platformMidpointPosition;
    private Vector2 platformMidpointPreviousPosition;
    private float angleRadiants;
    private readonly float angularSpeed;
    private double normalizedLinearProgression;
    private float linearSpeed;
    private readonly float centerRestingTime;
    private float elapsedRestingTime;
    public bool active;
    private bool moveOnce;
    private bool disappearing;
    private bool transparent;
    private float maxTransparentTime;
    private float maxSolidTime;
    private float elapsedTransparencyTime;
    private static readonly float minAlpha = 0.15f;
    private static readonly float alphaChangeSpeed = 3f;
    private float alphaValue;
    private float linearSpeed_pixelsPerSecond;
    private float delay;

    public MovingPlatform(
      MovingPlatform.TextureType _textureType,
      int _radius,
      Vector2 _centerStartingPoint,
      Vector2 _centerEndingPoint,
      float _angularSpeed,
      float _linearSpeed_pixelsPerSecond,
      bool _active = true,
      float _startingAngleDegrees = 0.0f,
      float _normalizedLinearProression = 0.0f,
      float _centerRestingTime = 0.0f,
      bool _moveOnce = false,
      bool _disappearing = false,
      float _maxTransparentTime = 0.0f,
      float _maxSolidTime = 0.0f,
      float _delay = 0.0f)
    {
      this.textureType = _textureType;
      this.radius = _radius;
      this.centerStartingPoint = _centerStartingPoint;
      this.centerEndingPoint = _centerEndingPoint;
      this.width = MovingPlatform.sourceRectangles[(int) this.textureType].Width;
      this.height = MovingPlatform.sourceRectangles[(int) this.textureType].Height;
      this.angleRadiants = (float) ((double) _startingAngleDegrees / 180.0 * 3.1415927410125732);
      this.angularSpeed = _angularSpeed;
      this.linearSpeed_pixelsPerSecond = _linearSpeed_pixelsPerSecond;
      this.linearSpeed = this.linearSpeed_pixelsPerSecond / Vector2.Distance(this.centerEndingPoint, this.centerStartingPoint);
      this.centerRestingTime = _centerRestingTime;
      this.elapsedRestingTime = this.centerRestingTime;
      this.normalizedLinearProgression = (double) MathHelper.Clamp(_normalizedLinearProression, 0.0f, 1f);
      this.active = _active;
      this.rotationCenter = Vector2.Lerp(this.centerStartingPoint, this.centerEndingPoint, (float) this.normalizedLinearProgression);
      this.platformMidpointPosition = new Vector2(this.rotationCenter.X + (float) this.radius * (float) Math.Sin((double) this.angleRadiants), this.rotationCenter.Y - (float) this.radius * (float) Math.Cos((double) this.angleRadiants));
      this.platformMidpointPreviousPosition = this.platformMidpointPosition;
      this.moveOnce = _moveOnce;
      this.disappearing = _disappearing;
      this.maxTransparentTime = _maxTransparentTime;
      this.maxSolidTime = _maxSolidTime;
      this.delay = _delay;
      this.elapsedTransparencyTime = -this.delay;
      this.alphaValue = 1f;
    }

    public static void loadTextures(Texture2D _spritesheet)
    {
      MovingPlatform.spritesheet = _spritesheet;
      MovingPlatform.sourceRectangles = new Rectangle[11]
      {
        new Rectangle(0, 0, 40, 8),
        new Rectangle(40, 0, 40, 8),
        new Rectangle(80, 0, 40, 8),
        new Rectangle(120, 0, 24, 8),
        new Rectangle(144, 0, 24, 8),
        new Rectangle(168, 0, 24, 8),
        new Rectangle(192, 0, 24, 8),
        new Rectangle(0, 8, 272, 40),
        new Rectangle(0, 48, 296, 40),
        new Rectangle(0, 88, 112, 224),
        new Rectangle(112, 88, 184, 112)
      };
    }

    public float AngleRadiants => this.angleRadiants;

    public float AngularSpeed => this.angularSpeed;

    public Vector2 Position
    {
      get
      {
        return new Vector2(this.platformMidpointPosition.X - 0.5f * (float) this.width, this.platformMidpointPosition.Y - 0.5f * (float) this.height);
      }
    }

    public Vector2 Shift
    {
      get
      {
        return !this.active ? Vector2.Zero : this.platformMidpointPosition - this.platformMidpointPreviousPosition;
      }
    }

    public Rectangle Rectangle
    {
      get
      {
        return new Rectangle((int) ((double) this.platformMidpointPosition.X - 0.5 * (double) this.width), (int) ((double) this.platformMidpointPosition.Y - 0.5 * (double) this.height), this.width, this.height);
      }
    }

    public bool Transparent => this.transparent;

    public void Update(float elapsedTime)
    {
      if (this.active)
      {
        this.platformMidpointPreviousPosition = this.platformMidpointPosition;
        if ((double) this.elapsedRestingTime < (double) this.centerRestingTime)
        {
          this.elapsedRestingTime += elapsedTime;
          return;
        }
        this.normalizedLinearProgression += (double) this.linearSpeed * (double) elapsedTime;
        if (this.normalizedLinearProgression > 1.0)
        {
          this.normalizedLinearProgression = 1.0;
          this.linearSpeed *= -1f;
          this.elapsedRestingTime = 0.0f;
          if (this.moveOnce)
            this.active = false;
        }
        else if (this.normalizedLinearProgression < 0.0)
        {
          this.normalizedLinearProgression = 0.0;
          this.linearSpeed *= -1f;
          this.elapsedRestingTime = 0.0f;
          if (this.moveOnce)
            this.active = false;
        }
        this.rotationCenter = Vector2.Lerp(this.centerStartingPoint, this.centerEndingPoint, (float) this.normalizedLinearProgression);
        this.angleRadiants += this.angularSpeed * elapsedTime;
        if ((double) this.angleRadiants >= 6.2831854820251465)
          this.angleRadiants -= 6.28318548f;
        else if ((double) this.angleRadiants < 0.0)
          this.angleRadiants += 6.28318548f;
        this.platformMidpointPosition.X = this.rotationCenter.X + (float) this.radius * (float) Math.Sin((double) this.angleRadiants);
        this.platformMidpointPosition.Y = this.rotationCenter.Y - (float) this.radius * (float) Math.Cos((double) this.angleRadiants);
      }
      if (!this.disappearing)
        return;
      if (this.transparent)
      {
        this.elapsedTransparencyTime += elapsedTime;
        if ((double) this.alphaValue > (double) MovingPlatform.minAlpha)
        {
          this.alphaValue -= MovingPlatform.alphaChangeSpeed * elapsedTime;
          if ((double) this.alphaValue < (double) MovingPlatform.minAlpha)
            this.alphaValue = MovingPlatform.minAlpha;
        }
        if ((double) this.elapsedTransparencyTime < (double) this.maxTransparentTime)
          return;
        this.elapsedTransparencyTime = 0.0f;
        this.transparent = false;
      }
      else
      {
        this.elapsedTransparencyTime += elapsedTime;
        if ((double) this.alphaValue < 1.0)
        {
          this.alphaValue += MovingPlatform.alphaChangeSpeed * elapsedTime;
          if ((double) this.alphaValue > 1.0)
            this.alphaValue = 1f;
        }
        if ((double) this.elapsedTransparencyTime < (double) this.maxSolidTime)
          return;
        this.elapsedTransparencyTime = 0.0f;
        this.transparent = true;
      }
    }

    public void Reset(bool _active)
    {
      this.angleRadiants = 0.0f;
      this.elapsedRestingTime = this.centerRestingTime;
      this.normalizedLinearProgression = 0.0;
      this.active = _active;
      this.rotationCenter = Vector2.Lerp(this.centerStartingPoint, this.centerEndingPoint, (float) this.normalizedLinearProgression);
      this.platformMidpointPosition = new Vector2(this.rotationCenter.X + (float) this.radius * (float) Math.Sin((double) this.angleRadiants), this.rotationCenter.Y - (float) this.radius * (float) Math.Cos((double) this.angleRadiants));
      this.elapsedTransparencyTime = -this.delay;
      this.alphaValue = 1f;
    }

    public void RemovePlatform()
    {
      this.platformMidpointPosition.X = (float) -this.width;
      this.platformMidpointPosition.Y = (float) -this.height;
      this.active = false;
    }

    public MovingPlatform DeepCopy()
    {
      return new MovingPlatform(this.textureType, this.radius, this.centerStartingPoint, this.centerEndingPoint, this.angularSpeed, this.linearSpeed_pixelsPerSecond, this.active, (float) ((double) this.angleRadiants * 180.0 / 3.1415927410125732), (float) this.normalizedLinearProgression, this.centerRestingTime, this.moveOnce, this.disappearing, this.maxTransparentTime, this.maxSolidTime, this.delay);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(MovingPlatform.spritesheet, Camera.RelativeRectangle(this.Position, this.width, this.height), new Rectangle?(MovingPlatform.sourceRectangles[(int) this.textureType]), Color.White * this.alphaValue);
    }

    public enum TextureType
    {
      grass40_8,
      dirt40_8,
      marble40_8,
      dirt24_8,
      marble24_8,
      crackedDirt24_8,
      crackedMarble24_8,
      fallingFloor272_40,
      fallingFloor296_40,
      fallingFloor112_216,
      fallingFloor184_216,
      total,
    }
  }
}

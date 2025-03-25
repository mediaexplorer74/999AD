
// Type: GameManager.Animation
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace GameManager
{
  internal class Animation
  {
    private int totalFrames;
    private Rectangle[] sourceRectangles;
    private int currentFrame;
    private int previousFrame;
    private float timePerFrame;
    private float elapsedFrameTime;
    private bool loop;
    private bool keepLastFrameWhenInactive;
    private bool active;

    public Animation(
      Rectangle _frameLocationOnSpritesheet,
      int _frameWidth,
      int _frameHeight,
      int _totalFrames,
      float _timePerFrame,
      bool _loop,
      bool _keepLastFrameWhenInactive = false)
    {
      this.currentFrame = 0;
      this.previousFrame = 0;
      this.elapsedFrameTime = 0.0f;
      this.active = true;
      this.totalFrames = _totalFrames;
      this.timePerFrame = _timePerFrame;
      this.loop = _loop;
      this.keepLastFrameWhenInactive = _keepLastFrameWhenInactive;
      this.sourceRectangles = new Rectangle[this.totalFrames];
      for (int index = 0; index < this.totalFrames; ++index)
        this.sourceRectangles[index] = new Rectangle(_frameLocationOnSpritesheet.X + _frameWidth * (index % (_frameLocationOnSpritesheet.Width / _frameWidth)), _frameLocationOnSpritesheet.Y + _frameHeight * (index / (_frameLocationOnSpritesheet.Width / _frameWidth)), _frameWidth, _frameHeight);
    }

    public Animation(
      Rectangle[] _sourceRectangles,
      float _timePerFrame,
      bool _loop,
      bool _keepLastFrameWhenInactive = false)
    {
      this.currentFrame = 0;
      this.elapsedFrameTime = 0.0f;
      this.active = true;
      this.sourceRectangles = _sourceRectangles;
      this.totalFrames = this.sourceRectangles.Length;
      this.timePerFrame = _timePerFrame;
      this.loop = _loop;
      this.keepLastFrameWhenInactive = _keepLastFrameWhenInactive;
    }

    public Rectangle Frame => this.sourceRectangles[this.currentFrame];

    public bool Active => this.active;

    public float TotalTime => this.timePerFrame * (float) this.sourceRectangles.Length;

    public int FrameIndex => this.currentFrame;

    public int FreviousFrameIndex => this.previousFrame;

    public void Update(float elapsedTime)
    {
      if (!this.active)
        return;
      this.previousFrame = this.currentFrame;
      this.elapsedFrameTime += elapsedTime;
      if ((double) this.elapsedFrameTime < (double) this.timePerFrame)
        return;
      this.elapsedFrameTime = 0.0f;
      this.currentFrame = (this.currentFrame + 1) % this.totalFrames;
      if (this.loop || this.currentFrame != 0)
        return;
      this.active = false;
      if (!this.keepLastFrameWhenInactive)
        return;
      this.currentFrame = this.totalFrames - 1;
    }

    public bool IsLastFrame() => this.currentFrame == this.totalFrames - 1;

    public void Reset()
    {
      this.elapsedFrameTime = 0.0f;
      this.currentFrame = 0;
      this.active = true;
    }

    public Animation DeepCopy()
    {
      return new Animation(this.sourceRectangles, this.timePerFrame, this.loop, this.keepLastFrameWhenInactive);
    }
  }
}

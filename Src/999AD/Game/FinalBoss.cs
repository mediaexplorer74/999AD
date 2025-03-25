
// Type: GameManager.FinalBoss
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GameManager
{
  internal static class FinalBoss
  {
    private static Texture2D bossSpritesheet;
    private static Texture2D[] wingSpritesheets;
    private static Animation[] bossAnimations;
    private static Animation[] rightWingAnimations;
    private static Animation[] leftWingAnimations;
    public static readonly float recoveryTime = 15f;
    private static float elapsedRecoveryTime;
    public static readonly float attackAnimationTime = 3f;
    private static float elapsedAttackAnimationTime;
    public static readonly int bossWidth = 80;
    public static readonly int bossHeight = 128;
    public static readonly int wingWidth = 136;
    public static readonly int wingHeight = 8;
    public static readonly Vector2 fireballsCenter = new Vector2(384f, 338f);
    private static Vector2 bossInitialMidPoint;
    public static Vector2 bossMidPoint;
    public static readonly int wingsRelativeYPosition = -2;
    private static float YSpeed;
    private static FinalBoss.BossAnimations bossAnimation;
    private static FinalBoss.WingAnimations rightWingAnimation;
    private static FinalBoss.WingAnimations leftWingAnimation;
    private static FinalBoss.WingTextures rightWingTexture;
    private static FinalBoss.WingTextures leftWingTexture;
    private static FinalBoss.Phases currentPhase;
    private static readonly int maxBossHp = 25;
    private static readonly int maxWingHP = 2;
    private static int bossHP;
    private static int rightWingHP;
    private static int leftWingHP;
    private static readonly Random rand = new Random();
    private static bool dead;
    private static Color bossColor;
    private static readonly int framesOfDifferentColor = 5;
    private static int frameCount;

    public static void Inizialize(Texture2D _bossSpritesheet, Texture2D[] _wingSpritesheets)
    {
      FinalBoss.dead = false;
      FinalBoss.Reset();
      FinalBoss.bossSpritesheet = _bossSpritesheet;
      FinalBoss.wingSpritesheets = _wingSpritesheets;
      FinalBoss.bossAnimations = new Animation[8]
      {
        new Animation(new Rectangle(0, 0, 288, 256), 288, 256, 1, 0.0f, false, true),
        new Animation(new Rectangle(0, 0, 2016, 1024), 288, 256, 22, 0.2f, false, true),
        new Animation(new Rectangle(288, 768, 1728, 256), 288, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 1024, 1728, 256), 288, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 1280, 2016, 256), 288, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle(0, 1536, 1728, 256), 288, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 1792, 2016, 256), 288, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle(0, 1536, 1728, 256), 288, 256, 6, 0.2f, true)
      };
      FinalBoss.rightWingAnimations = new Animation[7]
      {
        new Animation(new Rectangle(0, 0, 144, 256), 144, 256, 1, 0.0f, false, true),
        new Animation(new Rectangle(0, 0, 1584, 512), 144, 256, 22, 0.2f, false, true),
        new Animation(new Rectangle(0, 0, 864, 256), 144, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 768, 1008, 256), 144, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle(0, 512, 864, 256), 144, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 768, 1008, 256), 144, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle[7]
        {
          new Rectangle(864, 768, 144, 256),
          new Rectangle(720, 768, 144, 256),
          new Rectangle(576, 768, 144, 256),
          new Rectangle(432, 768, 144, 256),
          new Rectangle(288, 768, 144, 256),
          new Rectangle(144, 768, 144, 256),
          new Rectangle(0, 768, 144, 256)
        }, 0.2f, false, true)
      };
      FinalBoss.leftWingAnimations = new Animation[7]
      {
        new Animation(new Rectangle(0, 0, 144, 256), 144, 256, 1, 0.0f, false, true),
        new Animation(new Rectangle(0, 0, 1584, 512), 144, 256, 22, 0.2f, false, true),
        new Animation(new Rectangle(0, 0, 864, 256), 144, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 768, 1008, 256), 144, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle(0, 512, 864, 256), 144, 256, 6, 0.2f, true),
        new Animation(new Rectangle(0, 768, 1008, 256), 144, 256, 7, 0.2f, false, true),
        new Animation(new Rectangle[7]
        {
          new Rectangle(864, 768, 144, 256),
          new Rectangle(720, 768, 144, 256),
          new Rectangle(576, 768, 144, 256),
          new Rectangle(432, 768, 144, 256),
          new Rectangle(288, 768, 144, 256),
          new Rectangle(144, 768, 144, 256),
          new Rectangle(0, 768, 144, 256)
        }, 0.2f, false, true)
      };
    }

    public static void Reset()
    {
      FinalBoss.elapsedRecoveryTime = 0.0f;
      FinalBoss.elapsedAttackAnimationTime = 0.0f;
      FinalBoss.YSpeed = 0.0f;
      FinalBoss.bossAnimation = FinalBoss.BossAnimations.stone;
      FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.stoneIdle;
      FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.stoneIdle;
      FinalBoss.rightWingTexture = FinalBoss.WingTextures.stone;
      FinalBoss.leftWingTexture = FinalBoss.WingTextures.stone;
      FinalBoss.currentPhase = FinalBoss.Phases.one;
      FinalBoss.bossColor = Color.White;
      FinalBoss.frameCount = 0;
      FinalBoss.bossInitialMidPoint = new Vector2(FinalBoss.fireballsCenter.X, FinalBoss.fireballsCenter.Y - 13f);
      FinalBoss.bossHP = FinalBoss.maxBossHp;
      FinalBoss.rightWingHP = FinalBoss.maxWingHP;
      FinalBoss.leftWingHP = FinalBoss.maxWingHP;
      FinalBoss.bossMidPoint = FinalBoss.bossInitialMidPoint + new Vector2(0.0f, 5f);
    }

    public static Rectangle BossCollisionRectangle
    {
      get
      {
        return new Rectangle((int) ((double) FinalBoss.bossMidPoint.X - (double) (FinalBoss.bossWidth / 2)), (int) ((double) FinalBoss.bossMidPoint.Y - (double) (FinalBoss.bossHeight / 2)), FinalBoss.bossWidth, FinalBoss.bossHeight);
      }
    }

    public static Rectangle BossDrawRectangle
    {
      get
      {
        return (double) Player.position.X < (double) FinalBoss.bossMidPoint.X || FinalBoss.bossAnimation == FinalBoss.BossAnimations.stone ? new Rectangle((int) ((double) FinalBoss.bossMidPoint.X - 126.0), (int) ((double) FinalBoss.bossMidPoint.Y - 134.0), FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Frame.Width, FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Frame.Height) : new Rectangle((int) ((double) FinalBoss.bossMidPoint.X - 162.0), (int) ((double) FinalBoss.bossMidPoint.Y - 134.0), FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Frame.Width, FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Frame.Height);
      }
    }

    public static Rectangle RightWingCollisionRectangle
    {
      get
      {
        if (FinalBoss.bossAnimation != FinalBoss.BossAnimations.recovering)
          return new Rectangle(0, 0, 0, 0);
        return (double) Player.position.X < (double) FinalBoss.bossMidPoint.X ? new Rectangle((int) FinalBoss.bossMidPoint.X + 18, (int) ((double) FinalBoss.bossMidPoint.Y - (double) FinalBoss.wingsRelativeYPosition), FinalBoss.wingWidth, FinalBoss.wingHeight) : new Rectangle((int) FinalBoss.bossMidPoint.X - 18, (int) ((double) FinalBoss.bossMidPoint.Y - (double) FinalBoss.wingsRelativeYPosition), FinalBoss.wingWidth, FinalBoss.wingHeight);
      }
    }

    public static Rectangle LeftWingCollisionRectangle
    {
      get
      {
        if (FinalBoss.bossAnimation != FinalBoss.BossAnimations.recovering)
          return new Rectangle(0, 0, 0, 0);
        return (double) Player.position.X < (double) FinalBoss.bossMidPoint.X ? new Rectangle((int) FinalBoss.bossMidPoint.X + 18 - FinalBoss.wingWidth, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition), FinalBoss.wingWidth, FinalBoss.wingHeight) : new Rectangle((int) FinalBoss.bossMidPoint.X - 18 - FinalBoss.wingWidth, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition), FinalBoss.wingWidth, FinalBoss.wingHeight);
      }
    }

    public static Rectangle RightWingDrawRectangle
    {
      get
      {
        return (double) Player.position.X < (double) FinalBoss.bossMidPoint.X || FinalBoss.bossAnimation == FinalBoss.BossAnimations.stone ? new Rectangle((int) FinalBoss.bossMidPoint.X + 38, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition - 118.0), FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Frame.Width, FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Frame.Height) : new Rectangle((int) FinalBoss.bossMidPoint.X - 2, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition - 118.0), FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Frame.Width, FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Frame.Height);
      }
    }

    public static Rectangle LeftWingDrawRectangle
    {
      get
      {
        return (double) Player.position.X < (double) FinalBoss.bossMidPoint.X || FinalBoss.bossAnimation == FinalBoss.BossAnimations.stone ? new Rectangle((int) FinalBoss.bossMidPoint.X - FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Width - 2, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition - 118.0), FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Width, FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Height) : new Rectangle((int) FinalBoss.bossMidPoint.X - FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Width - 38, (int) ((double) FinalBoss.bossMidPoint.Y + (double) FinalBoss.wingsRelativeYPosition - 118.0), FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Width, FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame.Height);
      }
    }

    public static bool Dead
    {
      get => FinalBoss.dead;
      set => FinalBoss.dead = value;
    }

    public static void WakeUp()
    {
      FinalBoss.bossAnimation = FinalBoss.BossAnimations.stoneToIdle;
      FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Reset();
      FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.stoneToIdle;
      FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.stoneToIdle;
      FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
      FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
      MediaPlayer.Play(SoundEffects.FinaBossSoundTrack);
      MediaPlayer.IsRepeating = true;
    }

    public static void Update(float elapsedTime)
    {
      if (FinalBoss.dead || FinalBoss.bossAnimation == FinalBoss.BossAnimations.stone)
        return;
      if (FinalBoss.bossAnimation != FinalBoss.BossAnimations.stone && FinalBoss.bossAnimation != FinalBoss.BossAnimations.stoneToIdle && FinalBoss.bossAnimation != FinalBoss.BossAnimations.falling)
      {
        FinalBoss.bossMidPoint.Y += FinalBoss.YSpeed;
        FinalBoss.YSpeed += (float) (((double) FinalBoss.bossInitialMidPoint.Y - (double) FinalBoss.bossMidPoint.Y) * 0.20000000298023224) * elapsedTime;
      }
      FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Update(elapsedTime);
      FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Update(elapsedTime);
      FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Update(elapsedTime);
      if (!FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Active)
      {
        if (FinalBoss.bossAnimation == FinalBoss.BossAnimations.startRecovering)
        {
          FinalBoss.bossAnimation = FinalBoss.BossAnimations.recovering;
          FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.flap;
          FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.flap;
          FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Reset();
          FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
          FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
        }
        else
        {
          FinalBoss.bossAnimation = FinalBoss.BossAnimations.idle;
          FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.idle;
          FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.idle;
          if (FinalBoss.rightWingTexture == FinalBoss.WingTextures.stone || FinalBoss.leftWingTexture == FinalBoss.WingTextures.stone)
          {
            FinalBoss.rightWingTexture = FinalBoss.WingTextures.healthy;
            FinalBoss.leftWingTexture = FinalBoss.WingTextures.healthy;
          }
          FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Reset();
          FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
          FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
        }
      }
      switch (FinalBoss.bossAnimation)
      {
        case FinalBoss.BossAnimations.idle:
          if (!FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].IsLastFrame())
            break;
          switch (FinalBoss.currentPhase)
          {
            case FinalBoss.Phases.one:
              if (FinalBoss.bossHP <= 0)
              {
                FinalBoss.bossAnimation = FinalBoss.BossAnimations.startRecovering;
                FinalBoss.bossAnimations[4].Reset();
                LavaGeyserManager.Reset();
                FireBallsManager.Reset();
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                return;
              }
              if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                return;
              switch (FinalBoss.rand.Next(4))
              {
                case 0:
                  FireBallsManager.ThrowAtPlayer((int) MathHelper.Lerp(3f, 6f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp)), 2f + (float) FinalBoss.rand.NextDouble(), 1f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 1:
                  FireBallsManager.ThrowInAllDirections((int) MathHelper.Lerp(3f, 7f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp)), FireBall.radialShootingVelocity, (float) (0.5 + FinalBoss.rand.NextDouble()));
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 2:
                  int length1 = (int) MathHelper.Lerp(2f, 4f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp));
                  int[] numArray1 = new int[length1];
                  for (int index = 0; index < length1; ++index)
                  {
                    int num = FinalBoss.rand.Next(PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    while (((IEnumerable<int>) numArray1).Contains<int>(num))
                      num = (num + 1) % (PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    numArray1[index] = num;
                  }
                  FireBallsManager.TargetPlatform(numArray1, 6, 2f);
                  FireBallsManager.AddGhostFireball(7f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                default:
                  FireBallsManager.Sweep((float) (0.4 + 0.2 * FinalBoss.rand.NextDouble()), (float) FinalBoss.rand.Next(3, 5));
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
              }
              SoundEffects.FinalBossAttack.Play();
              return;
            case FinalBoss.Phases.two:
              if (FinalBoss.bossHP <= 0)
              {
                FinalBoss.bossAnimation = FinalBoss.BossAnimations.startRecovering;
                FinalBoss.bossAnimations[4].Reset();
                LavaGeyserManager.Reset();
                FireBallsManager.Reset();
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                return;
              }
              if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                return;
              switch (FinalBoss.rand.Next(3))
              {
                case 0:
                  if (FinalBoss.rand.Next(2) == 0)
                    FireBallsManager.ThrowAtPlayer((int) MathHelper.Lerp(4f, 9f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp)), (float) (3.0 + FinalBoss.rand.NextDouble()), 1f);
                  else
                    FireBallsManager.ThrowInAllDirections(6, FireBall.radialShootingVelocity, (float) (1.0 + FinalBoss.rand.NextDouble()));
                  int length2 = (int) MathHelper.Lerp(2f, 4f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp));
                  int[] numArray2 = new int[length2];
                  for (int index = 0; index < length2; ++index)
                  {
                    int num = FinalBoss.rand.Next(PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    while (((IEnumerable<int>) numArray2).Contains<int>(num))
                      num = (num + 1) % (PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    numArray2[index] = num;
                  }
                  FireBallsManager.TargetPlatform(numArray2, 6, 2f);
                  FireBallsManager.AddGhostFireball(7f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 1:
                  float amplitudeDegrees = MathHelper.Lerp(90f, 180f, (float) (1.0 - (double) FinalBoss.bossHP / (double) FinalBoss.maxBossHp));
                  FireBallsManager.TrowWithinCircularSector((int) ((double) amplitudeDegrees / 6.0), FireBall.radialShootingVelocity * 0.8f, (float) (0.25 + 0.5 * FinalBoss.rand.NextDouble()), amplitudeDegrees);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                default:
                  FireBallsManager.RandomSweep((float) (1.0 + 0.2 * FinalBoss.rand.NextDouble()), 2f, 4);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
              }
              SoundEffects.FinalBossAttack.Play();
              return;
            case FinalBoss.Phases.three:
              if (FinalBoss.bossHP <= 0)
              {
                FinalBoss.bossAnimation = FinalBoss.BossAnimations.startRecovering;
                FinalBoss.bossAnimations[4].Reset();
                LavaGeyserManager.Reset();
                FireBallsManager.Reset();
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                return;
              }
              if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                return;
              switch (FinalBoss.rand.Next(4))
              {
                case 0:
                  if (FinalBoss.rand.Next(2) == 0)
                  {
                    int length3 = 2;
                    int[] numArray3 = new int[length3];
                    for (int index = 0; index < length3; ++index)
                    {
                      int num = FinalBoss.rand.Next(PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                      while (((IEnumerable<int>) numArray3).Contains<int>(num))
                        num = (num + 1) % (PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                      numArray3[index] = num;
                    }
                    FireBallsManager.TargetPlatform(numArray3, 6, 2f);
                    FireBallsManager.AddGhostFireball(7f);
                  }
                  else
                    FireBallsManager.ThrowAtPlayer(4, (float) (3.0 + FinalBoss.rand.NextDouble()), 1f);
                  LavaGeyserManager.ShootGeyser(new float[2]
                  {
                    (float) FinalBoss.rand.Next((int) FinalBoss.fireballsCenter.X - 200, (int) FinalBoss.fireballsCenter.X + 200),
                    (float) FinalBoss.rand.Next((int) FinalBoss.fireballsCenter.X - 200, (int) FinalBoss.fireballsCenter.X + 200)
                  }, 3f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 1:
                  int length4 = 3;
                  int[] numArray4 = new int[length4];
                  for (int index = 0; index < length4; ++index)
                  {
                    int num = FinalBoss.rand.Next(PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    while (((IEnumerable<int>) numArray4).Contains<int>(num))
                      num = (num + 1) % (PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length - 1);
                    numArray4[index] = num;
                  }
                  FireBallsManager.TargetPlatform(numArray4, 6, 5f);
                  FireBallsManager.TrowWithinCircularSector(20, FireBall.radialShootingVelocity * 0.8f, (float) (0.5 + 0.1 * FinalBoss.rand.NextDouble()), 120f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 2:
                  FireBallsManager.Spiral(30, 20f, MathHelper.Lerp(0.3f, 0.5f, (float) FinalBoss.bossHP / (float) FinalBoss.maxBossHp), FireBall.radialShootingVelocity * 0.4f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                default:
                  LavaGeyserManager.EquallySpaced((float) (LavaGeyser.size * 7), 2f, 0.0f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
              }
              SoundEffects.FinalBossAttack.Play();
              return;
            case FinalBoss.Phases.four:
              if (FinalBoss.bossHP <= 0)
              {
                FinalBoss.bossAnimation = FinalBoss.BossAnimations.startRecovering;
                FinalBoss.bossAnimations[4].Reset();
                LavaGeyserManager.Reset();
                FireBallsManager.Reset();
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.spread;
                FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                return;
              }
              if (FireBallsManager.NumberOfActiveFireballs > 0 || LavaGeyserManager.ActiveLavaGeysers > 0)
                return;
              switch (FinalBoss.rand.Next(3))
              {
                case 0:
                  FireBallsManager.Sweep((float) (0.4 + 0.2 * FinalBoss.rand.NextDouble()), 10f);
                  LavaGeyserManager.SweepAcross(1f, 0.5f, (int) MathHelper.Lerp(4f, 10f, (float) FinalBoss.bossHP / (float) FinalBoss.maxBossHp), 244f, 524f, FinalBoss.rand.Next(2) == 0);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                case 1:
                  FireBallsManager.TrowWithinCircularSector(20, FireBall.radialShootingVelocity * 0.8f, (float) (0.25 + 0.5 * FinalBoss.rand.NextDouble()), 120f);
                  LavaGeyserManager.EquallySpaced((float) (LavaGeyser.size * 6), 2f, (float) (LavaGeyser.size * 6) * (float) FinalBoss.rand.NextDouble());
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
                default:
                  LavaGeyserManager.ShootGeyser(new float[2]
                  {
                    (float) FinalBoss.rand.Next((int) FinalBoss.fireballsCenter.X - 200, (int) FinalBoss.fireballsCenter.X + 200),
                    (float) FinalBoss.rand.Next((int) FinalBoss.fireballsCenter.X - 200, (int) FinalBoss.fireballsCenter.X + 200)
                  }, 3f);
                  FireBallsManager.Spiral(30, 20f, 0.3f, FireBall.radialShootingVelocity * 0.4f);
                  FinalBoss.bossAnimation = FinalBoss.BossAnimations.attack;
                  FinalBoss.bossAnimations[3].Reset();
                  FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
                  FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
                  break;
              }
              SoundEffects.FinalBossAttack.Play();
              return;
            default:
              return;
          }
        case FinalBoss.BossAnimations.attack:
          FinalBoss.elapsedAttackAnimationTime += elapsedTime;
          if ((double) FinalBoss.elapsedAttackAnimationTime < (double) FinalBoss.attackAnimationTime)
            break;
          FinalBoss.elapsedAttackAnimationTime = 0.0f;
          FinalBoss.bossAnimation = FinalBoss.BossAnimations.idle;
          FinalBoss.bossAnimations[2].Reset();
          FinalBoss.bossAnimations[6].Reset();
          FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
          FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
          break;
        case FinalBoss.BossAnimations.recovering:
          if (FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].FrameIndex == 0 && FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].FreviousFrameIndex != 0)
            SoundEffects.FinalBossRecover.Play();
          else if (FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].FrameIndex == 2 && FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].FreviousFrameIndex != 2)
            SoundEffects.FinalBossAwaken.Play();
          FinalBoss.elapsedRecoveryTime += elapsedTime;
          if ((double) FinalBoss.elapsedRecoveryTime >= (double) FinalBoss.recoveryTime)
          {
            FinalBoss.bossAnimation = FinalBoss.BossAnimations.endRecovering;
            FinalBoss.rightWingAnimation = FinalBoss.rightWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
            FinalBoss.leftWingAnimation = FinalBoss.leftWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
            FinalBoss.bossAnimations[6].Reset();
            FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
            FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
            FinalBoss.elapsedRecoveryTime = 0.0f;
            FinalBoss.bossHP = FinalBoss.maxBossHp / 2;
            break;
          }
          switch (FinalBoss.currentPhase)
          {
            case FinalBoss.Phases.one:
              if (FinalBoss.rightWingHP + FinalBoss.leftWingHP != 3)
                return;
              FinalBoss.bossHP = FinalBoss.maxBossHp;
              FinalBoss.elapsedRecoveryTime = 0.0f;
              FinalBoss.currentPhase = FinalBoss.Phases.two;
              FinalBoss.bossAnimation = FinalBoss.BossAnimations.endRecovering;
              FinalBoss.rightWingAnimation = FinalBoss.rightWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.leftWingAnimation = FinalBoss.leftWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.bossAnimations[3].Reset();
              FinalBoss.bossAnimations[6].Reset();
              FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
              FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
              SoundEffects.FinalBossHurt.Play();
              FireBallsManager.AddGhostFireball(3f);
              return;
            case FinalBoss.Phases.two:
              if (FinalBoss.rightWingHP + FinalBoss.leftWingHP != 2)
                return;
              FinalBoss.bossHP = FinalBoss.maxBossHp;
              FinalBoss.elapsedRecoveryTime = 0.0f;
              FinalBoss.currentPhase = FinalBoss.Phases.three;
              FinalBoss.bossAnimation = FinalBoss.BossAnimations.endRecovering;
              FinalBoss.rightWingAnimation = FinalBoss.rightWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.leftWingAnimation = FinalBoss.leftWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.bossAnimations[3].Reset();
              FinalBoss.bossAnimations[6].Reset();
              FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
              FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
              SoundEffects.FinalBossHurt.Play();
              FireBallsManager.ThrowAtPlayer(20, 2f, 0.1f);
              CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(374, 475), Collectable.ItemType.heart));
              return;
            case FinalBoss.Phases.three:
              if (FinalBoss.rightWingHP + FinalBoss.leftWingHP != 1)
                return;
              FinalBoss.bossHP = FinalBoss.maxBossHp;
              FinalBoss.elapsedRecoveryTime = 0.0f;
              FinalBoss.currentPhase = FinalBoss.Phases.four;
              FinalBoss.bossAnimation = FinalBoss.BossAnimations.endRecovering;
              FinalBoss.rightWingAnimation = FinalBoss.rightWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.leftWingAnimation = FinalBoss.leftWingHP <= 0 ? FinalBoss.WingAnimations.withdrawDead : FinalBoss.WingAnimations.withdraw;
              FinalBoss.bossAnimations[3].Reset();
              FinalBoss.bossAnimations[6].Reset();
              FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
              FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
              SoundEffects.FinalBossHurt.Play();
              LavaGeyserManager.SweepAcross(1f, 0.0f, 100, 159f, 609f, true);
              CollectablesManager.collectablesRoomManagers[(int) RoomsManager.CurrentRoom].AddCollectableToMap(new Collectable(new Point(378, 475), Collectable.ItemType.heart));
              return;
            case FinalBoss.Phases.four:
              if (FinalBoss.rightWingHP + FinalBoss.leftWingHP != 0)
                return;
              if ((double) Player.CollisionRectangle.Right > (double) FinalBoss.bossMidPoint.X)
              {
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.withdraw;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.withdrawDead;
              }
              else
              {
                FinalBoss.rightWingAnimation = FinalBoss.WingAnimations.withdrawDead;
                FinalBoss.leftWingAnimation = FinalBoss.WingAnimations.withdraw;
              }
              FinalBoss.bossAnimation = FinalBoss.BossAnimations.falling;
              FinalBoss.bossAnimations[3].Reset();
              FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Reset();
              FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Reset();
              FinalBoss.elapsedRecoveryTime = 0.0f;
              SoundEffects.FinalBossHurt.Play();
              FireBallsManager.Reset();
              LavaGeyserManager.Reset();
              return;
            default:
              return;
          }
        case FinalBoss.BossAnimations.falling:
          FinalBoss.bossMidPoint.Y += FinalBoss.YSpeed * elapsedTime;
          FinalBoss.YSpeed += Gravity.gravityAcceleration * elapsedTime;
          if (FinalBoss.BossDrawRectangle.Y <= MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx)
            break;
          FinalBoss.dead = true;
          LoadSaveManager.SaveGameProgress();
          MediaPlayer.Stop();
          break;
      }
    }

    public static bool DamageWing(bool rightWing)
    {
      if (rightWing)
      {
        if (FinalBoss.rightWingHP > 0)
        {
          --FinalBoss.rightWingHP;
          switch (FinalBoss.rightWingHP)
          {
            case 0:
              FinalBoss.rightWingTexture = FinalBoss.WingTextures.dead;
              break;
            case 1:
              FinalBoss.rightWingTexture = FinalBoss.WingTextures.damaged;
              break;
          }
          return true;
        }
      }
      else if (FinalBoss.leftWingHP > 0)
      {
        --FinalBoss.leftWingHP;
        switch (FinalBoss.leftWingHP)
        {
          case 0:
            FinalBoss.leftWingTexture = FinalBoss.WingTextures.dead;
            break;
          case 1:
            FinalBoss.leftWingTexture = FinalBoss.WingTextures.damaged;
            break;
        }
        return true;
      }
      return false;
    }

    public static bool WingHitByReactangle(
      Rectangle collisionRect,
      float yVelocity,
      float elapsedTime)
    {
      if ((double) yVelocity <= 0.0)
        return false;
      Rectangle rectangle = new Rectangle(collisionRect.X, (int) ((double) collisionRect.Bottom - (double) yVelocity * (double) elapsedTime), collisionRect.Width, (int) ((double) yVelocity * (double) elapsedTime));
      Rectangle collisionRectangle;
      if (FinalBoss.rightWingAnimation == FinalBoss.WingAnimations.flap && FinalBoss.rightWingHP > 0)
      {
        collisionRectangle = FinalBoss.RightWingCollisionRectangle;
        if (collisionRectangle.Intersects(rectangle))
          return FinalBoss.DamageWing(true);
      }
      if (FinalBoss.leftWingAnimation == FinalBoss.WingAnimations.flap && FinalBoss.leftWingHP > 0)
      {
        collisionRectangle = FinalBoss.LeftWingCollisionRectangle;
        if (collisionRectangle.Intersects(rectangle))
          return FinalBoss.DamageWing(false);
      }
      return false;
    }

    public static bool BossHitByRectangle(Rectangle collisionRect)
    {
      if (FinalBoss.bossAnimation != FinalBoss.BossAnimations.idle && FinalBoss.bossAnimation != FinalBoss.BossAnimations.attack || !FinalBoss.BossCollisionRectangle.Intersects(collisionRect))
        return false;
      --FinalBoss.bossHP;
      FinalBoss.bossColor = Color.Red * 0.8f;
      return true;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      if (FinalBoss.dead)
        return;
      spriteBatch.Draw(FinalBoss.wingSpritesheets[(int) FinalBoss.rightWingTexture], Camera.RelativeRectangle(FinalBoss.RightWingDrawRectangle), new Rectangle?(FinalBoss.rightWingAnimations[(int) FinalBoss.rightWingAnimation].Frame), Color.White);
      spriteBatch.Draw(FinalBoss.wingSpritesheets[(int) FinalBoss.leftWingTexture], Camera.RelativeRectangle(FinalBoss.LeftWingDrawRectangle), new Rectangle?(FinalBoss.leftWingAnimations[(int) FinalBoss.leftWingAnimation].Frame), Color.White, 0.0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 1f);
      spriteBatch.Draw(FinalBoss.bossSpritesheet, Camera.RelativeRectangle(FinalBoss.BossDrawRectangle), new Rectangle?(FinalBoss.bossAnimations[(int) FinalBoss.bossAnimation].Frame), FinalBoss.bossColor, 0.0f, Vector2.Zero, (double) Player.position.X < (double) FinalBoss.bossMidPoint.X || FinalBoss.bossAnimation == FinalBoss.BossAnimations.stone ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0.0f);
      if (FinalBoss.frameCount >= FinalBoss.framesOfDifferentColor)
      {
        FinalBoss.frameCount = 0;
        FinalBoss.bossColor = Color.White;
      }
      else
        ++FinalBoss.frameCount;
    }

    public enum BossAnimations
    {
      stone,
      stoneToIdle,
      idle,
      attack,
      startRecovering,
      recovering,
      endRecovering,
      falling,
      total,
    }

    public enum WingAnimations
    {
      stoneIdle,
      stoneToIdle,
      idle,
      spread,
      flap,
      withdraw,
      withdrawDead,
      total,
    }

    public enum WingTextures
    {
      stone,
      healthy,
      damaged,
      dead,
      total,
    }

    public enum Phases
    {
      one,
      two,
      three,
      four,
      total,
    }
  }
}

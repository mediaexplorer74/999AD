
// Type: GameManager.FireBallsManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GameManager
{
  public static class FireBallsManager
  {
    public static Vector2 fireballsCenter;
    private static Texture2D spritesheet;
    private static Rectangle laserSourceRect;
    private static List<int> targetedPlatforms;
    private static float relativeLaserProgression = 0.0f;
    private static readonly float laserVelocity = 2f;
    private static readonly float laserPersistanceTime = 4f;
    private static float elapsedLaserPersistanceTime;
    private static int fireballsPerPlatform;
    private static float targetPlatformLifetime;
    private static List<FireBall> fireballs;
    private static readonly Random rand = new Random();

    public static void Inizialize(Texture2D _spritesheet)
    {
      FireBallsManager.spritesheet = _spritesheet;
      FireBallsManager.laserSourceRect = new Rectangle(409, 41, 30, 180);
      FireBall.Inizialize(_spritesheet);
      FireBallsManager.fireballsCenter = FinalBoss.fireballsCenter;
      FireBallsManager.targetedPlatforms = new List<int>();
      FireBallsManager.fireballs = new List<FireBall>();
      FireBallsManager.Reset();
    }

    public static void Reset()
    {
      FireBallsManager.targetedPlatforms.Clear();
      FireBallsManager.fireballs.Clear();
      FireBallsManager.elapsedLaserPersistanceTime = 0.0f;
      FireBallsManager.fireballsPerPlatform = 0;
      FireBallsManager.targetPlatformLifetime = 0.0f;
    }

    public static int NumberOfActiveFireballs => FireBallsManager.fireballs.Count;

    public static void ThrowAtPlayer(int amount, float angularVelocity, float timeBtweenShots)
    {
      for (int index = 0; index < amount; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) (360.0 / (double) amount * (double) index), new float[4]
        {
          20f,
          0.0f,
          0.0f,
          FireBall.radialShootingVelocity
        }, new float[4]
        {
          angularVelocity,
          angularVelocity,
          angularVelocity,
          0.0f
        }, new float[4]
        {
          2f,
          (float) (2.0 + (double) timeBtweenShots * (double) index),
          5f,
          4f
        }, true));
    }

    public static void ThrowInAllDirections(int amount, float shotVelocity, float angularVelocity)
    {
      float num = (float) (2.0 + 2.0 * FireBallsManager.rand.NextDouble());
      for (int index = 0; index < amount; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) (360.0 / (double) amount * (double) index), new float[3]
        {
          20f,
          0.0f,
          shotVelocity
        }, new float[3]
        {
          angularVelocity,
          angularVelocity,
          0.0f
        }, new float[3]{ 2f, num, 5f }));
    }

    public static void TrowWithinCircularSector(
      int amount,
      float shotVelocity,
      float angularVelocity,
      float amplitudeDegrees)
    {
      float num1 = (float) (3.0 + 2.0 * FireBallsManager.rand.NextDouble());
      float num2 = (float) FireBallsManager.rand.Next(360);
      for (int index = 0; index < amount; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) ((double) num2 + (double) amplitudeDegrees / (double) amount * (double) index), new float[3]
        {
          20f,
          0.0f,
          shotVelocity
        }, new float[3]
        {
          angularVelocity,
          angularVelocity,
          0.0f
        }, new float[3]{ 2f, num1, 5f }));
    }

    public static void Sweep(float angularVelocity, float lifetime)
    {
      int _angleDegrees = FireBallsManager.rand.Next(0, 360);
      int num = 1 - 2 * FireBallsManager.rand.Next(0, 2);
      for (int index = 0; index < 15; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) _angleDegrees, new float[3]
        {
          0.0f,
          100f,
          0.0f
        }, new float[3]
        {
          0.0f,
          0.0f,
          (float) num * angularVelocity
        }, new float[3]
        {
          0.2f * (float) index,
          (float) (3.0 - 0.20000000298023224 * (double) index),
          lifetime
        }));
    }

    public static void RandomSweep(
      float angularVelocity,
      float maxTimeWithoutInverion,
      int numberOfInversions)
    {
      int _angleDegrees = FireBallsManager.rand.Next(0, 360);
      int num = 1 - 2 * FireBallsManager.rand.Next(0, 2);
      float[] second1 = new float[numberOfInversions + 1];
      float[] second2 = new float[numberOfInversions + 1];
      float[] second3 = new float[numberOfInversions + 1];
      for (int index = 0; index <= numberOfInversions; ++index)
      {
        second1[index] = (float) (1.0 + ((double) maxTimeWithoutInverion - 1.0) * FireBallsManager.rand.NextDouble());
        second2[index] = (float) num * angularVelocity;
        num *= -1;
        second3[index] = 0.0f;
      }
      for (int index = 0; index < 15; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) _angleDegrees, ((IEnumerable<float>) new float[2]
        {
          0.0f,
          100f
        }).Concat<float>((IEnumerable<float>) second3).ToArray<float>(), ((IEnumerable<float>) new float[2]).Concat<float>((IEnumerable<float>) second2).ToArray<float>(), ((IEnumerable<float>) new float[2]
        {
          0.2f * (float) index,
          (float) (3.0 - 0.20000000298023224 * (double) index)
        }).Concat<float>((IEnumerable<float>) second1).ToArray<float>()));
    }

    public static void Spiral(
      int amount,
      float angleOffset,
      float timeBetweenShots,
      float shotVelocity)
    {
      for (int index = 0; index < amount; ++index)
        FireBallsManager.fireballs.Add(new FireBall((float) ((double) angleOffset * (double) index), new float[2]
        {
          0.0f,
          shotVelocity
        }, new float[2], new float[2]
        {
          timeBetweenShots * (float) index,
          4f
        }));
    }

    public static void TargetPlatform(
      int[] platformIndexes,
      int _fireballsPerPlatform,
      float lifetime)
    {
      if (FireBallsManager.targetedPlatforms.Count != 0)
        return;
      FireBallsManager.elapsedLaserPersistanceTime = 0.0f;
      FireBallsManager.targetedPlatforms = new List<int>((IEnumerable<int>) platformIndexes);
      FireBallsManager.relativeLaserProgression = 0.0f;
      FireBallsManager.fireballsPerPlatform = _fireballsPerPlatform;
      FireBallsManager.targetPlatformLifetime = lifetime;
    }

    private static void TargetPlatformGo(int amount, float lifetime)
    {
      foreach (int targetedPlatform in FireBallsManager.targetedPlatforms)
      {
        float angleRadiants = PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].AngleRadiants;
        float angularSpeed = PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].AngularSpeed;
        float width = (float) PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].width;
        float height = (float) PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].height;
        float radius = (float) PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].radius;
        for (int index = 0; index < amount; ++index)
          FireBallsManager.fireballs.Add(new FireBall((float) ((double) angleRadiants * 180.0 / 3.1415927410125732), new float[1], new float[1]
          {
            angularSpeed
          }, new float[1]{ lifetime }, centerOffsetX: MathHelper.Lerp((float) (-(double) width / 2.0), width / 2f, ((float) index + 0.5f) / (float) amount), centerOffsetY: (float) (-(double) height / 2.0), _radialDistance: radius));
      }
    }

    public static void AddGhostFireball(float lifetime)
    {
      FireBallsManager.fireballs.Add(new FireBall(0.0f, new float[1], new float[1], new float[1]
      {
        lifetime
      }, _radialDistance: (float) MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomHeightPx));
    }

    public static void Update(float elapsedTime)
    {
      for (int index = FireBallsManager.fireballs.Count - 1; index >= 0; --index)
      {
        if (FireBallsManager.fireballs[index].Active)
          FireBallsManager.fireballs[index].Update(elapsedTime);
        else
          FireBallsManager.fireballs.RemoveAt(index);
      }
      if (FireBallsManager.targetedPlatforms.Count == 0)
        return;
      if ((double) FireBallsManager.relativeLaserProgression < 1.0)
      {
        FireBallsManager.relativeLaserProgression += FireBallsManager.laserVelocity * elapsedTime;
        if ((double) FireBallsManager.relativeLaserProgression <= 1.0)
          return;
        FireBallsManager.relativeLaserProgression = 1f;
      }
      else
      {
        FireBallsManager.elapsedLaserPersistanceTime += elapsedTime;
        if ((double) FireBallsManager.elapsedLaserPersistanceTime <= (double) FireBallsManager.laserPersistanceTime)
          return;
        FireBallsManager.TargetPlatformGo(FireBallsManager.fireballsPerPlatform, FireBallsManager.targetPlatformLifetime);
        FireBallsManager.targetedPlatforms.Clear();
      }
    }

    public static bool FireballIntersectsRectangle(Rectangle collisionRect)
    {
      for (int index = FireBallsManager.fireballs.Count - 1; index >= 0; --index)
      {
        if (FireBallsManager.fireballs[index].CollisionRectangle.Intersects(collisionRect))
        {
          FireBallsManager.fireballs.RemoveAt(index);
          return true;
        }
      }
      return false;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (int targetedPlatform in FireBallsManager.targetedPlatforms)
        spriteBatch.Draw(FireBallsManager.spritesheet, Camera.RelativeRectangle(new Rectangle((int) FireBallsManager.fireballsCenter.X, (int) ((double) FireBallsManager.fireballsCenter.Y - (double) (FireBallsManager.laserSourceRect.Width / 2)), FireBallsManager.laserSourceRect.Width, (int) ((double) (PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].radius + 20) * (double) FireBallsManager.relativeLaserProgression))), new Rectangle?(FireBallsManager.laserSourceRect), Color.White, PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[targetedPlatform].AngleRadiants + 3.14159274f, new Vector2((float) (FireBallsManager.laserSourceRect.Width / 2), 0.0f), SpriteEffects.None, 1f);
      foreach (FireBall fireball in FireBallsManager.fireballs)
        fireball.Draw(spriteBatch);
    }
  }
}


// Type: GameManager.LavaGeyserManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;


namespace GameManager
{
  internal static class LavaGeyserManager
  {
    private static List<LavaGeyser> lavaGeysers;
    private static readonly Random rand = new Random();

    public static void Inizialize(Texture2D spritesheet)
    {
      LavaGeyser.Inizialize(spritesheet);
      LavaGeyserManager.lavaGeysers = new List<LavaGeyser>();
    }

    public static void Reset() => LavaGeyserManager.lavaGeysers.Clear();

    public static int ActiveLavaGeysers => LavaGeyserManager.lavaGeysers.Count;

    public static void ShootGeyser(
      float[] xPositions,
      float timeBeforeErupting,
      int initialYVelocity = -1200)
    {
      foreach (float xPosition in xPositions)
        LavaGeyserManager.lavaGeysers.Add(new LavaGeyser(xPosition, timeBeforeErupting, initialYVelocity));
    }

    public static void SweepAcross(
      float delay,
      float timeBetweenEruptions,
      int safeAreas,
      float safeAreaMinX,
      float safeAreaMaxX,
      bool leftToRight)
    {
      int num1 = (MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomWidthtPx - 1 + LavaGeyser.size)
                / LavaGeyser.size;

      int minValue = (int) ((double) safeAreaMinX / (double) LavaGeyser.size);
      int num2 = (int) ((double) safeAreaMaxX / (double) LavaGeyser.size);
      int num3;
      if (num2 - minValue + 1 >= safeAreas)
      {
        num3 = LavaGeyserManager.rand.Next(minValue, num2 + 1 - safeAreas);
      }
      else
      {
        num3 = minValue;
        safeAreas = num2 - minValue + 1;
      }
      if (leftToRight)
      {
        int num4 = 0;
        int num5 = 0;
        for (; num4 < num1; ++num4)
        {
          if (num4 == num3)
            num4 += safeAreas;
          LavaGeyserManager.lavaGeysers.Add(new LavaGeyser(((float) num4 + 0.5f) 
              * (float) LavaGeyser.size, delay + (float) num5 * timeBetweenEruptions));
          ++num5;
        }
      }
      else
      {
        int num6 = num1 - 1;
        int num7 = 0;
        for (; num6 >= 0; --num6)
        {
          if (num6 == num3 - 1 + safeAreas)
            num6 -= safeAreas;
          LavaGeyserManager.lavaGeysers.Add(new LavaGeyser(((float) num6 + 0.5f) * (float) LavaGeyser.size, delay + (float) num7 * timeBetweenEruptions));
          ++num7;
        }
      }
    }

    public static void EquallySpaced(float distance, float timeBeforeErupting, float offset)
    {
      int num = (int) (((double) (MapsManager.maps[(int) RoomsManager.CurrentRoom].RoomWidthtPx - 1)
                + 2.0 * (double) distance) / (double) distance);

      for (int index = 0; index < num; ++index)
        LavaGeyserManager.lavaGeysers.Add(new LavaGeyser(((float) index + 0.5f) * distance + offset, 
            timeBeforeErupting));
    }

    public static void Update(float elapsedTime)
    {
      for (int index = LavaGeyserManager.lavaGeysers.Count - 1; index >= 0; --index)
      {
        if (LavaGeyserManager.lavaGeysers[index].isActive())
          LavaGeyserManager.lavaGeysers[index].Update(elapsedTime);
        else
          LavaGeyserManager.lavaGeysers.RemoveAt(index);
      }
    }

    public static bool LavaGeyserIntersectsRectangle(Rectangle collisionRect)
    {
      foreach (LavaGeyser lavaGeyser in LavaGeyserManager.lavaGeysers)
      {
        if (lavaGeyser.CollisionRectangle.Intersects(collisionRect))
          return true;
      }
      return false;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (LavaGeyser lavaGeyser in LavaGeyserManager.lavaGeysers)
        lavaGeyser.Draw(spriteBatch);
    }
  }
}

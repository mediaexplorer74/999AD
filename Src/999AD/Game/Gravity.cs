
// Type: GameManager.Gravity
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace GameManager
{
  internal static class Gravity
  {
    public static float gravityAcceleration = 1000f;
    public static float airFrictionCoeff = 1.2f;
    public static float wallFrictionCoeff = 3f;

    public static void MoveDestructableObject(
      ref Vector2 velocity,
      ref Vector2 position,
      int width,
      int height,
      ref bool active,
      float elapsedTime)
    {
      position.X += velocity.X * elapsedTime;
      int num1 = (int) MathHelper.Clamp(position.Y / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int num2 = (int) MathHelper.Clamp((float) ((double) position.Y + (double) height - 1.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int index1 = (int) MathHelper.Clamp(position.X / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      int index2 = (int) MathHelper.Clamp((float) ((double) position.X + (double) width - 1.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      if ((double) velocity.X > 0.0)
      {
        for (int index3 = num1; index3 <= num2; ++index3)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index3, index2].isSolid())
          {
            position.X = (float) (index2 * Tile.tileSize - width);
            active = false;
            return;
          }
        }
      }
      else if ((double) velocity.X < 0.0)
      {
        for (int index4 = num1; index4 <= num2; ++index4)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index4, index1].isSolid())
          {
            position.X = (float) ((index1 + 1) * Tile.tileSize);
            active = false;
            return;
          }
        }
      }
      velocity.Y += (Gravity.gravityAcceleration - velocity.Y * Gravity.airFrictionCoeff) * elapsedTime;
      position.Y += velocity.Y * elapsedTime;
      int index5 = (int) MathHelper.Clamp(position.Y / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int index6 = (int) MathHelper.Clamp((float) ((double) position.Y + (double) height - 1.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomHeightTiles - 1));
      int num3 = (int) MathHelper.Clamp(position.X / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      int num4 = (int) MathHelper.Clamp((float) ((double) position.X + (double) width - 1.0) / (float) Tile.tileSize, 0.0f, (float) (MapsManager.maps[(int) RoomsManager.CurrentRoom].roomWidthTiles - 1));
      if ((double) velocity.Y > 0.0)
      {
        for (int index7 = num3; index7 <= num4; ++index7)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index6, index7].isSolid())
          {
            position.Y = (float) (index6 * Tile.tileSize - height);
            active = false;
            return;
          }
        }
      }
      else if ((double) velocity.Y < 0.0)
      {
        for (int index8 = num3; index8 <= num4; ++index8)
        {
          if (MapsManager.maps[(int) RoomsManager.CurrentRoom].array[index5, index8].isSolid())
          {
            position.Y = (float) ((index5 + 1) * Tile.tileSize);
            active = false;
            return;
          }
        }
      }
      for (int index9 = 0; index9 < PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms.Length; ++index9)
      {
        MovingPlatform movingPlatform = PlatformsManager.platformsRoomManagers[(int) RoomsManager.CurrentRoom].movingPlatforms[index9];
        if (!movingPlatform.Transparent && (double) position.X + (double) width > (double) movingPlatform.Position.X && (double) position.X < (double) movingPlatform.Position.X + (double) movingPlatform.width && (double) position.Y + (double) height - (double) movingPlatform.Position.Y >= 0.0 && (double) position.Y + (double) height - (double) movingPlatform.Position.Y <= (double) velocity.Y * (double) elapsedTime - (double) movingPlatform.Shift.Y)
        {
          position.Y = movingPlatform.Position.Y - (float) height;
          active = false;
          break;
        }
      }
    }
  }
}

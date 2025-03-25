
// Type: GameManager.ProjectilesManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace GameManager
{
  internal static class ProjectilesManager
  {
    public static List<Projectile> enemyProjectiles;
    public static List<Projectile> playerProjectiles;
    public static List<Projectile> midbossProjectiles;

    public static void Inizialize(Texture2D spritesheet)
    {
      Projectile.Inizialize(spritesheet);
      ProjectilesManager.playerProjectiles = new List<Projectile>();
      ProjectilesManager.midbossProjectiles = new List<Projectile>();
      ProjectilesManager.enemyProjectiles = new List<Projectile>();
    }

    public static void ShootPlayerProjectile(Vector2 position, Vector2 initialVelocity)
    {
      ProjectilesManager.playerProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.holyWater));
    }

    public static void ShootBossProjectile(Vector2 position, Vector2 initialVelocity)
    {
      ProjectilesManager.midbossProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.lava));
    }

    public static void ShootEnemyProjectile(Vector2 position, Vector2 initialVelocity)
    {
      ProjectilesManager.enemyProjectiles.Add(new Projectile(position, initialVelocity, Projectile.SpriteType.lava));
    }

    public static void Update(float elapsedTime)
    {
      for (int index = ProjectilesManager.playerProjectiles.Count - 1; index >= 0; --index)
      {
        if (ProjectilesManager.playerProjectiles[index].active)
          ProjectilesManager.playerProjectiles[index].Update(elapsedTime);
        else
          ProjectilesManager.playerProjectiles.RemoveAt(index);
      }
      for (int index = ProjectilesManager.midbossProjectiles.Count - 1; index >= 0; --index)
      {
        if (ProjectilesManager.midbossProjectiles[index].active)
          ProjectilesManager.midbossProjectiles[index].Update(elapsedTime);
        else
          ProjectilesManager.midbossProjectiles.RemoveAt(index);
      }
      for (int index = ProjectilesManager.enemyProjectiles.Count - 1; index >= 0; --index)
      {
        if (ProjectilesManager.enemyProjectiles[index].active)
          ProjectilesManager.enemyProjectiles[index].Update(elapsedTime);
        else
          ProjectilesManager.enemyProjectiles.RemoveAt(index);
      }
    }

    public static void Reset()
    {
      ProjectilesManager.playerProjectiles.Clear();
      ProjectilesManager.midbossProjectiles.Clear();
      ProjectilesManager.enemyProjectiles.Clear();
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      foreach (Projectile playerProjectile in ProjectilesManager.playerProjectiles)
        playerProjectile.Draw(spriteBatch);
      foreach (Projectile midbossProjectile in ProjectilesManager.midbossProjectiles)
        midbossProjectile.Draw(spriteBatch);
      foreach (Projectile enemyProjectile in ProjectilesManager.enemyProjectiles)
        enemyProjectile.Draw(spriteBatch);
    }
  }
}

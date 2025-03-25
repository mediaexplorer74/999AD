
// Type: GameManager.Collisions
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;


namespace GameManager
{
  internal static class Collisions
  {
    public static void Update(float elapsedTiem)
    {
      Collisions.PlayerCollisions(elapsedTiem);
      Collisions.PlayerProjectilesCollisions();
      Collisions.BossProjectilesCollisions();
      Collisions.EnemyProjectilesCollisions();
    }

    private static void PlayerCollisions(float elapsedTime)
    {
      if (MapsManager.maps[(int) RoomsManager.CurrentRoom].HarmfulTileIntersectsRectangle(Player.CollisionRectangle))
        Player.takeDamage();
      else if (FireBallsManager.FireballIntersectsRectangle(Player.CollisionRectangle))
        Player.takeDamage();
      else if (LavaGeyserManager.LavaGeyserIntersectsRectangle(Player.CollisionRectangle))
      {
        Player.takeDamage();
      }
      else
      {
        if (RoomsManager.CurrentRoom == RoomsManager.Rooms.finalBoss && FinalBoss.WingHitByReactangle(Player.CollisionRectangle, Player.JumpSpeed.Y, elapsedTime))
          Player.Rebound(2f);
        if (RoomsManager.CurrentRoom != RoomsManager.Rooms.midBoss || MidBoss.bossState != MidBoss.BossState.move || !Player.CollisionRectangle.Intersects(MidBoss.BossCollisionRect))
          return;
        Player.takeDamage();
      }
    }

    private static void PlayerProjectilesCollisions()
    {
      foreach (Projectile playerProjectile in ProjectilesManager.playerProjectiles)
      {
        switch (RoomsManager.CurrentRoom)
        {
          case RoomsManager.Rooms.midBoss:
            if (!MidBoss.Dead && MidBoss.BossHitByRect(playerProjectile.Rectangle))
            {
              playerProjectile.active = false;
              break;
            }
            break;
          case RoomsManager.Rooms.churchAltarRoom:
            using (List<Torch>.Enumerator enumerator = TorchManager.torches.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                if (enumerator.Current.TorchHitByRect(playerProjectile.Rectangle))
                  playerProjectile.active = false;
              }
              break;
            }
          case RoomsManager.Rooms.finalBoss:
            if (FinalBoss.BossHitByRectangle(playerProjectile.Rectangle))
            {
              playerProjectile.active = false;
              break;
            }
            break;
        }
        foreach (Enemy1 enemy1 in EnemyManager.enemyRoomManagers[(int) RoomsManager.CurrentRoom].enemiesType1)
        {
          if (!enemy1.Dead && enemy1.Enemy1HitByRect(playerProjectile.Rectangle))
            playerProjectile.active = false;
        }
        foreach (Enemy2 enemy2 in EnemyManager.enemyRoomManagers[(int) RoomsManager.CurrentRoom].enemiesType2)
        {
          if (!enemy2.Dead && enemy2.Enemy2HitByRect(playerProjectile.Rectangle))
            playerProjectile.active = false;
        }
      }
    }

    private static void BossProjectilesCollisions()
    {
      foreach (Projectile midbossProjectile in ProjectilesManager.midbossProjectiles)
      {
        if (RoomsManager.CurrentRoom == RoomsManager.Rooms.midBoss && MidBoss.PlayerHitByRect(midbossProjectile.Rectangle))
          midbossProjectile.active = false;
      }
    }

    private static void EnemyProjectilesCollisions()
    {
      foreach (Projectile enemyProjectile in ProjectilesManager.enemyProjectiles)
      {
        foreach (Enemy2 enemy2 in EnemyManager.enemyRoomManagers[(int) RoomsManager.CurrentRoom].enemiesType2)
        {
          if (enemy2.PlayerHitByRect(enemyProjectile.Rectangle))
            enemyProjectile.active = false;
        }
      }
    }
  }
}

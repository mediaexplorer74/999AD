
// Type: GameManager.EnemyRoomManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal class EnemyRoomManager
  {
    public Enemy1[] enemiesType1;
    public Enemy2[] enemiesType2;

    public EnemyRoomManager(Enemy1[] EnemiesType1, Enemy2[] EnemiesType2)
    {
      this.enemiesType1 = EnemiesType1;
      this.enemiesType2 = EnemiesType2;
    }

    public void Update(float elapsedTime)
    {
      foreach (Enemy1 enemy1 in this.enemiesType1)
        enemy1.Update(elapsedTime);
      foreach (Enemy2 enemy2 in this.enemiesType2)
        enemy2.Update(elapsedTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (Enemy1 enemy1 in this.enemiesType1)
      {
        if (enemy1.Enemy1DrawRect.Intersects(Camera.Rectangle))
          enemy1.Draw(spriteBatch);
      }
      foreach (Enemy2 enemy2 in this.enemiesType2)
      {
        if (enemy2.Enemy2DrawRect.Intersects(Camera.Rectangle))
          enemy2.Draw(spriteBatch);
      }
    }
  }
}

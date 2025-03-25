
// Type: GameManager.EnemyManager
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class EnemyManager
  {
    public static EnemyRoomManager[] enemyRoomManagers;

    public static void Initialise(Texture2D e1Spritesheet, Texture2D e2Spritesheet)
    {
      Enemy1.Inizialize(e1Spritesheet);
      Enemy2.Inizialize(e2Spritesheet);
      EnemyManager.Reset();
    }

    public static void Reset()
    {
      EnemyManager.enemyRoomManagers = new EnemyRoomManager[18]
      {
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[2]
        {
          new Enemy1(new Vector2(200f, 192f), new Vector2(500f, 192f)),
          new Enemy1(new Vector2(250f, 87f), new Vector2(450f, 87f))
        }, new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[2]
        {
          new Enemy1(new Vector2(130f, 120f), new Vector2(230f, 120f)),
          new Enemy1(new Vector2(110f, 616f), new Vector2(220f, 616f))
        }, new Enemy2[1]
        {
          new Enemy2(new Vector2(100f, 937f), new Vector2(125f, 937f))
        }),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[1]
        {
          new Enemy2(new Vector2(220f, 1177f), new Vector2(400f, 1177f))
        }),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[3]
        {
          new Enemy1(new Vector2(250f, 400f), new Vector2(375f, 400f)),
          new Enemy1(new Vector2(875f, 375f), new Vector2(925f, 375f)),
          new Enemy1(new Vector2(80f, 304f), new Vector2(180f, 304f))
        }, new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[1]
        {
          new Enemy1(new Vector2(860f, 200f), new Vector2(985f, 200f))
        }, new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[1]
        {
          new Enemy1(new Vector2(1125f, 344f), new Vector2(1275f, 344f))
        }, new Enemy2[1]
        {
          new Enemy2(new Vector2(1150f, 441f), new Vector2(1300f, 441f))
        }),
        new EnemyRoomManager(new Enemy1[2]
        {
          new Enemy1(new Vector2(155f, 120f), new Vector2(320f, 120f)),
          new Enemy1(new Vector2(170f, 232f), new Vector2(170f, 232f))
        }, new Enemy2[1]
        {
          new Enemy2(new Vector2(220f, 232f), new Vector2(320f, 232f))
        }),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0]),
        new EnemyRoomManager(new Enemy1[0], new Enemy2[0])
      };
    }
  }
}

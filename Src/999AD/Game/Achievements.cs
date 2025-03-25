
// Type: GameManager.Achievements
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  internal static class Achievements
  {
    private static SpriteFont spriteFont8;
    private static SpriteFont spriteFont12;
    public static bool gameCompleted;
    public static bool noDeath;
    public static bool noHits;
    public static float bestTime;

    public static void Initialize(SpriteFont _spriteFont8, SpriteFont _spriteFont12)
    {
      Achievements.spriteFont8 = _spriteFont8;
      Achievements.spriteFont12 = _spriteFont12;
      LoadSaveManager.LoadHighScores();
    }

    public static void UpdateAchievements(
      bool _gameCompleted,
      bool _noDeaths,
      bool _noHits,
      float _bestTime)
    {
      Achievements.gameCompleted = _gameCompleted;
      Achievements.noDeath = _noDeaths;
      Achievements.noHits = _noHits;
      Achievements.bestTime = _bestTime;
    }

    public static void Draw(SpriteBatch spriteBatch)
    {
      int num1 = (int) Achievements.bestTime / 3600;
      int num2 = (int) Achievements.bestTime / 60 % 60;
      int num3 = (int) Achievements.bestTime % 60;
      int num4 = (int) ((double) Achievements.bestTime * 1000.0) % 1000;
      string str;
      if ((double) Achievements.bestTime != 0.0)
        str = num1.ToString() + ":" + num2.ToString() + ":" + num3.ToString() + "." + num4.ToString();
      else
        str = "-:-:-";
      string text = str;
      Vector2 vector2 = Achievements.spriteFont12.MeasureString(text);
      spriteBatch.DrawString(Achievements.spriteFont8, Achievements.gameCompleted ? "COMPLETED" : "NOT COMPLETED", new Vector2(200f, 59f), Color.White);
      spriteBatch.DrawString(Achievements.spriteFont8, Achievements.noDeath ? "COMPLETED" : "NOT COMPLETED", new Vector2(200f, 81f), Color.White);
      spriteBatch.DrawString(Achievements.spriteFont8, Achievements.noHits ? "COMPLETED" : "NOT COMPLETED", new Vector2(200f, 103f), Color.White);
      spriteBatch.DrawString(Achievements.spriteFont12, text, new Vector2((float) (Game1.gameWidth / 2) - vector2.X / 2f, 160f), Color.White);
    }
  }
}

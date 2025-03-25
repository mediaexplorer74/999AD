
// Type: GameManager.GameStats
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer


namespace GameManager
{
  internal static class GameStats
  {
    public static int deathsCount;
    public static int hitsCount;

    public static float gameTime { get; private set; }

    public static void Inizialize(float _gameTime = 0.0f, int _deathsCount = 0, int _hitsCount = 0)
    {
      GameStats.gameTime = _gameTime;
      GameStats.deathsCount = _deathsCount;
      GameStats.hitsCount = _hitsCount;
    }

    public static void Update(float elapsedTime) => GameStats.gameTime += elapsedTime;

    public static string GetTimeString()
    {
      int num1 = (int) GameStats.gameTime / 3600;
      int num2 = (int) GameStats.gameTime / 60 % 60;
      int num3 = (int) GameStats.gameTime % 60;
      int num4 = (int) ((double) GameStats.gameTime * 1000.0) % 1000;
      return num1.ToString() + ":" + num2.ToString() + ":" + num3.ToString() + "." + num4.ToString();
    }
  }
}

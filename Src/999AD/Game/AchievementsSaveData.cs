
// Type: GameManager.AchievementsSaveData
// Assembly: 999AD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 88FA0CA8-8FB6-4FB7-AA8C-8F78C65FFFD3
// Modded by [M]edia[E]xplorer

using System;


namespace GameManager
{
  //[Serializable]
  internal class AchievementsSaveData
  {
    public bool gameCompleted;
    public bool noDeath;
    public bool noHits;
    public float bestTime;

    public AchievementsSaveData()
    {
      this.gameCompleted = false;
      this.noDeath = false;
      this.noHits = false;
      this.bestTime = 0.0f;
    }

    public AchievementsSaveData(
      bool _gameCompleted,
      bool _noDeaths,
      bool _noHits,
      float _bestTime)
    {
      this.gameCompleted = _gameCompleted;
      this.noDeath = _noDeaths;
      this.noHits = _noHits;
      this.bestTime = _bestTime;
    }

    public void ApplySaveData()
    {
      Achievements.gameCompleted = this.gameCompleted;
      Achievements.noDeath = this.noDeath;
      Achievements.noHits = this.noHits;
      Achievements.bestTime = this.bestTime;
    }
  }
}

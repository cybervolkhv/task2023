using System;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            switch (levelIndex)
            {
                case 1:
                    TextAsset textAssetLevel1 = (TextAsset)Resources.Load("WordSearch/Levels/1");
                    LevelInfo level1 = JsonUtility.FromJson<LevelInfo>(textAssetLevel1.text);
                    return level1;
                case 2:
                    TextAsset textAssetLevel2 = (TextAsset)Resources.Load("WordSearch/Levels/2");
                    LevelInfo level2 = JsonUtility.FromJson<LevelInfo>(textAssetLevel2.text);
                    return level2;                   
                case 3:
                    TextAsset textAssetLevel3 = (TextAsset)Resources.Load("WordSearch/Levels/3");
                    LevelInfo level3 = JsonUtility.FromJson<LevelInfo>(textAssetLevel3.text);
                    return level3;
                default:
                    TextAsset textAssetLevel = (TextAsset)Resources.Load("WordSearch/Levels/1");
                    LevelInfo level = JsonUtility.FromJson<LevelInfo>(textAssetLevel.text);
                    return level;
            }
        }
    }
}
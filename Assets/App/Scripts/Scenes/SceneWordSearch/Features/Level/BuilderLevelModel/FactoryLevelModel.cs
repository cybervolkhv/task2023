using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using App.Scripts.Libs.Factory;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;
using UnityEngine.UIElements;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel
{
    public class FactoryLevelModel : IFactory<LevelModel, LevelInfo, int>
    {
        public LevelModel Create(LevelInfo value, int levelNumber)
        {
            var model = new LevelModel();

            model.LevelNumber = levelNumber;

            model.Words = value.words;
            model.InputChars = BuildListChars(value.words);

            return model;
        }

        private List<char> BuildListChars(List<string> words)
        {
            List<char> fullCharsList = words[0].ToList();

            List<char> charsList0 = words[0].ToList();

            List<char> charsList1 = words[1].ToList();

            List<char> charsList2 = words[2].ToList();   

            for (int i = 0; i < charsList1.Count; i++) 
            {
                if (charsList0.Contains(charsList1[i]))
                {
                    charsList0.Remove(charsList1[i]);
                    charsList1.Remove(charsList1[i]);
                    --i;
                }
                else
                {
                    fullCharsList.Add(charsList1[i]);
                }
            }

            char[] tempCharArrey = fullCharsList.ToArray();
            string tempString = new string(tempCharArrey);

            List<char> tempCharsList = tempString.ToList();

            for (int i = 0; i < charsList2.Count; i++)
            {
                if (tempCharsList.Contains(charsList2[i]))
                {
                    tempCharsList.Remove(charsList2[i]);
                    charsList2.Remove(charsList2[i]);
                    --i;
                }
                else
                {
                    fullCharsList.Add(charsList2[i]);
                }
            }
            return fullCharsList;
        }
    }
}
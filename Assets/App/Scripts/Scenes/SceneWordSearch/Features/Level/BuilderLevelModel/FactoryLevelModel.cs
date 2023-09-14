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
            List<char> charsList = new List<char>();
            List<char> charsList1 = new List<char>();
            List<char> charsList2 = new List<char>();
            List<char> charsListTemp = new List<char>();
            List<char> charsList3 = new List<char>();
            charsList = words[0].ToList();
            charsList1 = words[0].ToList();
            charsList2 = words[1].ToList();
            charsList3 = words[2].ToList();

            for (int i = 0; i < charsList2.Count; i++) //Find All extra lit
            {
                if (charsList1.Contains(charsList2[i]))
                {
                    charsList1.Remove(charsList2[i]);
                    charsList2.Remove(charsList2[i]);
                    --i;
                }
                else
                {
                    charsList.Add(charsList2[i]);
                }
            }
            char[] temp = charsList.ToArray();
            string txt = new string(temp);
            Debug.Log(txt);
            charsListTemp = txt.ToList();

            for (int i = 0; i < charsList3.Count; i++)
            {
                if (charsListTemp.Contains(charsList3[i]))
                {
                    charsListTemp.Remove(charsList3[i]);
                    charsList3.Remove(charsList3[i]);
                    --i;
                }
                else
                {
                    charsList.Add(charsList3[i]);
                }
            }
            char[] temp4 = charsList.ToArray();
            string txt4 = new string(temp4);
            Debug.Log(txt4);
            return charsList;







            //for (int i = 1; i < words.Count; i++)
            //{
            //    for (int j = 0; j < words[i].Length; j++)
            //    {
            //        if (!charsList.Contains(words[i][j]))
            //        {
            //            charsList.Add(words[i][j]);
            //        }

            //    }
            //}

            //напиши реализацию не меняя сигнатуру функции
            //throw new NotImplementedException();
        }
    }
}
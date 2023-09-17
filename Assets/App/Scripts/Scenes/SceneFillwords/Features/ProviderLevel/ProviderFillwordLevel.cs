using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.Linq;
using Unity.VisualScripting;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            Debug.Log(index + " -index");
            TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            string[] packs = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            Debug.Log(packs[index] + " без обрезания");
            packs[index] = packs[index].Replace(";", "");
            packs[index] = packs[index].Remove(0, 2);
            Debug.Log(packs[index] + " после обрезания");
            char[] paksCharArrey = packs[index].TrimEnd().ToArray();

            TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");
            string[] wordsList = textAsset2.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            string tempString = wordsList[index].TrimEnd();
            char[] lettersOfIndexWord = tempString.ToArray();
            Debug.Log(lettersOfIndexWord.Length + "lettersOfIndexWord.Length");



            int[] levelPack = new int[paksCharArrey.Length];

            for (int i = 0; i < paksCharArrey.Length; i++)
            {
                levelPack[i] = (int)Char.GetNumericValue(paksCharArrey[i]);
            }
            Debug.Log(levelPack.Length + "levelPack.Length");
            if (levelPack.Length == 4)
            {
                Vector2Int size = new Vector2Int(2, 2);
                GridFillWords gridFillWords = new GridFillWords(size);

                for (int i = 0, k = 0; i < size.y; i++)
                {
                    for (int j = 0; j < size.x; j++)
                    {
                        CharGridModel charGridModel = new CharGridModel(lettersOfIndexWord[levelPack[k]]);
                        k++;
                        gridFillWords.Set(i, j, charGridModel);
                    }
                }
                return gridFillWords;

            }
            else
            {
                return LoadModel(index + 1);

            }



            //напиши реализацию не меняя сигнатуру функции
            throw new NotImplementedException();
            throw new Exception();
        }
    }
}
using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Text;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            string[] packs = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");
            string[] wordsList = textAsset2.text.Split(new string[] { "\n" }, StringSplitOptions.None);


            int[] levelPack = LoadLevel(index, packs);
            char[] lettersOfIndexWord = LoadWord(index, wordsList);            

            if (levelPack.Length < 4)
            {
                levelPack = LoadLevel(index + 1, packs);
            }

            if (lettersOfIndexWord.Length < 4)
            {
                lettersOfIndexWord = LoadWord(index + 1, wordsList);
            }

            if (levelPack.Length == 4 && lettersOfIndexWord.Length == 4)
            {
                Vector2Int size = new Vector2Int(2, 2);

                FillSquare(levelPack, lettersOfIndexWord, size);

                GridFillWords gridFillWords = FillSquare(levelPack, lettersOfIndexWord, size);
                return gridFillWords;
            }

            if(levelPack.Length == 9 && lettersOfIndexWord.Length == 9)
            {
                Vector2Int size = new Vector2Int(3, 3);
                FillSquare(levelPack, lettersOfIndexWord, size);

                GridFillWords gridFillWords = FillSquare(levelPack, lettersOfIndexWord, size);
                return gridFillWords;
            }

            return null;
      
        }

        public GridFillWords FillSquare(int[] levelPack, char[] lettersOfIndexWord, Vector2Int size)
        {           
            GridFillWords gridFillWords = new GridFillWords(size);

            for (int i = 0, k = 0; i < size.y; i++)
            {
                for (int j = 0; j < size.x; j++)
                {
                    int levelInsex = Array.IndexOf(levelPack, k);
                    CharGridModel charGridModel = new CharGridModel(lettersOfIndexWord[levelInsex]);
                    k++;
                    gridFillWords.Set(i, j, charGridModel);
                }
            }
            return gridFillWords;
        }

        public int[] LoadLevel(int index, string[] packs)
        {
            Debug.Log("start metod  " + packs[index].Length);
            //if (index > 9)
            //{
            //    throw new Exception();
            //}

            if (packs[index].Length <= 10)
            {
                Debug.Log("tyt");
                packs[index] = packs[index].Replace(";", "");
                packs[index] = packs[index].Remove(0, 2);
                char[] paksCharArrey = packs[index].TrimEnd().ToArray();
                int[] levelPack = CharsInInts(paksCharArrey);
                return levelPack;
            }
            else if (packs[index].Length > 10 && packs[index].Length <= 27)
            {
                Debug.Log("hi");
                StringBuilder stringBuilder = new StringBuilder();
                packs[index] = packs[index].Replace(";", "");
                string[] splitStrigs = packs[index].Split(new string[] { " " }, StringSplitOptions.None);
                for (int i = 0; i < splitStrigs.Length; i++)
                {
                    if(splitStrigs[i].Length > 4)
                    {
                        stringBuilder.Append(splitStrigs[i]);
                    }
                }
                Debug.Log(stringBuilder.ToString());
                char[] paksCharArrey = stringBuilder.ToString().TrimEnd().ToArray();
                int[] levelPack = CharsInInts(paksCharArrey);
                return levelPack;
            }
            else
            {
                Debug.Log("bad");
                return null;
            }
        }

        public int[] CharsInInts(char[] paksCharArrey)
        {
            int[] levelPack = new int[paksCharArrey.Length];
            for (int i = 0; i < paksCharArrey.Length; i++)
            {
                levelPack[i] = (int)Char.GetNumericValue(paksCharArrey[i]);
            }
            return levelPack;
        }

        public char[] LoadWord(int index, string[] wordsList)
        {
            string tempString = wordsList[index].TrimEnd();
            char[] lettersOfIndexWord = tempString.ToArray();
            return lettersOfIndexWord;
        }


    }
}
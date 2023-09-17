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
            TextAsset textAssetPacks = (TextAsset)Resources.Load("Fillwords/pack_0");
            string[] packs = textAssetPacks.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            TextAsset textAssetwordsList = (TextAsset)Resources.Load("Fillwords/words_list");
            string[] wordsList = textAssetwordsList.text.Split(new string[] { "\n" }, StringSplitOptions.None);


            int[] levelPack = LoadLevel(index, packs);
            char[] lettersOfIndexWord = LoadWord(index, wordsList);

            if(levelPack is null)
            {
                levelPack = LoadLevel(index + 1, packs);
            }

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

                GridFillWords gridFillWords = FillSquare(levelPack, lettersOfIndexWord, size);
                return gridFillWords;
            }

            if(levelPack.Length == 9 && lettersOfIndexWord.Length != 9)
            {
                int numberOfSimpleLevels = 7;
                int indexLongWord = index - numberOfSimpleLevels;
                lettersOfIndexWord = LoadLongWord(indexLongWord, wordsList);
            }

            if(levelPack.Length == 9 && lettersOfIndexWord.Length == 9)
            {
                Vector2Int size = new Vector2Int(3, 3);

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
                    int levelIndex = Array.IndexOf(levelPack, k);
                    CharGridModel charGridModel = new CharGridModel(lettersOfIndexWord[levelIndex]);
                    k++;
                    gridFillWords.Set(i, j, charGridModel);
                }
            }
            return gridFillWords;
        }

        public int[] LoadLevel(int index, string[] packs)
        {
            if (index > 9)
            {
                throw new Exception();
            }

            if (packs[index].Length <= 10)
            {
                packs[index] = packs[index].Replace(";", "").Remove(0, 2);
                char[] packCharArrey = packs[index].TrimEnd().ToArray();
                int[] levelPack = CharsToInts(packCharArrey);
                return levelPack;
            }
            else if (packs[index].Length > 10 && packs[index].Length <= 27)
            {
                StringBuilder stringBuilder = new StringBuilder();
                packs[index] = packs[index].Replace(";", "");
                string[] splitStrigs = packs[index].Split(new string[] { " " }, StringSplitOptions.None);
                for (int i = 0; i < splitStrigs.Length; i++)
                {
                    if (splitStrigs[i].Length > 2)
                    {
                        if(i == splitStrigs.Length - 1)
                        {
                            _ = splitStrigs[i].TrimEnd();
                        }
                        stringBuilder.Append(splitStrigs[i]);
                    }
                }
                char[] packCharArrey = stringBuilder.ToString().TrimEnd().ToArray();
                int[] levelPack = CharsToInts(packCharArrey);
                return levelPack;
            }
            else
            {
                int[] levelPack = null;
                return levelPack;
            }
        }

        public int[] CharsToInts(char[] packCharArrey)
        {
            int[] levelPack = new int[packCharArrey.Length];
            for (int i = 0; i < packCharArrey.Length; i++)
            {
                levelPack[i] = (int)Char.GetNumericValue(packCharArrey[i]);
            }
            return levelPack;
        }

        public char[] LoadWord(int index, string[] wordsList)
        {
            string tempString = wordsList[index].TrimEnd();
            char[] lettersOfIndexWord = tempString.ToArray();
            return lettersOfIndexWord;
        }

        public char[] LoadLongWord(int indexLongWord, string[] wordsList)
        {
            string[] longWordsList = new string[wordsList.Length];
            for (int i = 0, j = 0; i < wordsList.Length; i++)
            {
                if (wordsList[i].Length == 10)
                {
                    longWordsList[j] = wordsList[i];
                    j++;
                }
            }
            string tempString = longWordsList[indexLongWord].TrimEnd();

            char[] lettersOfIndexWord = tempString.ToArray();
            return lettersOfIndexWord;
        }
    }
}
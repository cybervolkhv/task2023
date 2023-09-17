using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.Linq;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {

        //private string[] _strings;
        //private string[] _strings2;
        //private char[] _chars;
        //private char[] _chars2;

        public GridFillWords LoadModel(int index)
        {
            //TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            //_strings = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            //_strings[0] = _strings[0].Replace(";", "");
            //_strings[0] = _strings[0].Remove(0, 2);
            //Debug.Log(_strings[0]);
            //_chars = _strings[0].Replace(";", "").ToCharArray();

            //int intVal = (int)Char.GetNumericValue(_strings[0][0]);

            //if (intVal == 0)
            //{
            //    Debug.Log("works");
            //}



            //TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");
            //_strings2 = textAsset2.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            //_chars2 = _strings2[0].Replace(" ","").ToCharArray();
            //for (int i = 0; i < _chars2.Length; i++)
            //{
            //    Debug.Log(_chars2[i]);
            //}

            //Debug.Log(_chars2.Length);
            //if (_chars2.Length == 4)
            //{
            //    Debug.Log("works2");
            //}

            Debug.Log(index + " -index");
            TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            string[] packs = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            packs[index] = packs[index].Replace(";", "");
            packs[index] = packs[index].Remove(0, 2);
            char[] paksCharArrey = packs[index].ToArray();

            TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");
            string[] wordsList = textAsset2.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            string tempString = wordsList[index].Replace(" ", "");
            char[] lettersOfIndexWord = tempString.ToArray();
            Debug.Log(lettersOfIndexWord.Length + "lettersOfIndexWord.Length");



            int[] levelPack = new int[paksCharArrey.Length];
           
            for (int i = 0; i < paksCharArrey.Length; i++)
            {
                levelPack[i] = (int)Char.GetNumericValue(paksCharArrey[i]);
            }
            Debug.Log(levelPack.Length + "levelPack.Length");
            if (levelPack.Length == 5)
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
            return null;





            //GridFillWords gridFillWords = new GridFillWords(size);
            //CharGridModel charGridModel = new CharGridModel(_chars2[0]);
            //gridFillWords.Set(0, 0, charGridModel);
            //charGridModel = new CharGridModel(_chars2[1]);
            //gridFillWords.Set(0, 1, charGridModel);
            //charGridModel = new CharGridModel(_chars2[3]);
            //gridFillWords.Set(1, 0, charGridModel);
            //charGridModel = new CharGridModel(_chars2[2]);
            //gridFillWords.Set(1, 1, charGridModel);
            //return gridFillWords;



            //напиши реализацию не меняя сигнатуру функции
            throw new NotImplementedException();
            throw new Exception();
        }
    }
}
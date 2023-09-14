using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {

        private string[] _strings;
        private string[] _strings2;
        private char[] _chars;
        private char[] _chars2;

        public GridFillWords LoadModel(int index)
        {


            TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            _strings = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            _strings[0] = _strings[0].Replace(";", "");
            _strings[0] = _strings[0].Remove(0, 2);
            Debug.Log(_strings[0]);
            _chars = _strings[0].Replace(";", "").ToCharArray();

            int intVal = (int)Char.GetNumericValue(_strings[0][0]);

            if (intVal == 0)
            {
                Debug.Log("works");
            }



            TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");
            _strings2 = textAsset2.text.Split(new string[] { "\n" }, StringSplitOptions.None);
            _chars2 = _strings2[0].Replace(" ","").ToCharArray();
            for (int i = 0; i < _chars2.Length; i++)
            {
                Debug.Log(_chars2[i]);
            }

            Debug.Log(_chars2.Length);
            if (_chars2.Length == 4)
            {
                Debug.Log("works2");
            }
            Vector2Int size = new Vector2Int(2, 2);
            GridFillWords gridFillWords = new GridFillWords(size);
            CharGridModel charGridModel = new CharGridModel(_chars2[0]);
            gridFillWords.Set(0, 0, charGridModel);
            charGridModel = new CharGridModel(_chars2[1]);
            gridFillWords.Set(0, 1, charGridModel);
            charGridModel = new CharGridModel(_chars2[3]);
            gridFillWords.Set(1, 0, charGridModel);
            charGridModel = new CharGridModel(_chars2[2]);
            gridFillWords.Set(1, 1, charGridModel);
            return gridFillWords;



            //напиши реализацию не меняя сигнатуру функции
            throw new NotImplementedException();
            throw new Exception();
        }
    }
}
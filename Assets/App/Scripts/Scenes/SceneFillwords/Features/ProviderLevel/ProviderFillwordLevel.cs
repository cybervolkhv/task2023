using System;
using UnityEngine;
using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {

        private string[] _strings;

        public GridFillWords LoadModel(int index)
        {
            TextAsset textAsset = (TextAsset)Resources.Load("Fillwords/pack_0");
            _strings = textAsset.text.Split(new string[] { "\n" }, StringSplitOptions.None);

            TextAsset textAsset2 = (TextAsset)Resources.Load("Fillwords/words_list");


            if (_strings[0][0] == (char)index)
            {
                Debug.Log(_strings[0][0]);
            }


            //_chars = _strings[index].Split(new char[]);
            Vector2Int size = new Vector2Int(2, 2);
            GridFillWords gridFillWords = new GridFillWords(size);
            CharGridModel charGridModel = new CharGridModel('a');
            gridFillWords.Set(0, 0, charGridModel);
            gridFillWords.Set(0, 1, charGridModel);
            gridFillWords.Set(1, 0, charGridModel);
            gridFillWords.Set(1, 1, charGridModel);

            return gridFillWords;
            //напиши реализацию не меняя сигнатуру функции
            throw new NotImplementedException();
            throw new Exception();
        }
    }
}
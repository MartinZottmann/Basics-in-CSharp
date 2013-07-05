using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.IO
{
    [Serializable]
    public class SaveList : List<SaveTuple>
    {
        public void TryAdd(ISaveable s)
        {
            var v = s.SaveValue();
            if (v == null)
                return;
            base.Add(new SaveTuple(new SaveKey(s), v));
        }
    }
}

using System;

namespace MartinZottmann.Game.IO
{
    [Serializable]
    public class SaveTuple : Tuple<SaveKey, SaveValue>
    {
        public SaveKey Key { get { return Item1; } }

        public SaveValue Value { get { return Item2; } }

        public SaveTuple(SaveKey k, SaveValue v) : base(k, v) { }
    }
}

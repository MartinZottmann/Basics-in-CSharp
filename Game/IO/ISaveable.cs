using System.Collections.Generic;

namespace MartinZottmann.Game.IO
{
    public interface ISaveable
    {
        SaveObject Save();

        void Load(SaveObject status);
    }
}

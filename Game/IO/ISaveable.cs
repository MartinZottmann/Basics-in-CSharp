namespace MartinZottmann.Game.IO
{
    public interface ISaveable
    {
        SaveValue SaveValue();

        void Load(SaveValue status);
    }
}

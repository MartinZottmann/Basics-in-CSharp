using OpenTK.Input;

namespace MartinZottmann.Game.Input
{
    public class InputKeyboardEventArgs
    {
        public readonly Key Key;

        public readonly uint ScanCode;

        public bool Handled;

        public InputKeyboardEventArgs(KeyboardKeyEventArgs e)
        {
            Key = e.Key;
            ScanCode = e.ScanCode;
        }
    }
}

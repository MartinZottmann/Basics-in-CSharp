using OpenTK.Input;
using System.Drawing;

namespace MartinZottmann.Game.Input
{
    public class InputMouseEventArgs
    {
        public readonly MouseButton Button;

        public readonly bool IsPressed;

        public readonly Point Position;

        public readonly int X;

        public readonly int Y;

        public readonly int XDelta;

        public readonly int YDelta;

        public readonly int Delta;

        public readonly float DeltaPrecise;

        public readonly int Value;

        public readonly float ValuePrecise;

        public bool Handled;

        public InputMouseEventArgs(MouseButtonEventArgs e)
        {
            Button = e.Button;
            IsPressed = e.IsPressed;
            Position = e.Position;
            X = e.X;
            Y = e.Y;
        }

        public InputMouseEventArgs(MouseMoveEventArgs e)
        {
            Position = e.Position;
            X = e.X;
            XDelta = e.XDelta;
            Y = e.Y;
            YDelta = e.YDelta;
        }

        public InputMouseEventArgs(MouseWheelEventArgs e)
        {
            Delta = e.Delta;
            DeltaPrecise = e.DeltaPrecise;
            Position = e.Position;
            Value = e.Value;
            ValuePrecise = e.ValuePrecise;
            X = e.X;
            Y = e.Y;
        }
    }
}

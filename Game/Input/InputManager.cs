using OpenTK;
using OpenTK.Input;
using System;
using System.Diagnostics;

namespace MartinZottmann.Game.Input
{
    public class InputManager
    {
        public readonly GameWindow Window;

        public event EventHandler<InputKeyboardEventArgs> KeyDown;

        public event EventHandler<InputKeyboardEventArgs> KeyUp;

        public event EventHandler<InputMouseEventArgs> ButtonDown;

        public event EventHandler<InputMouseEventArgs> ButtonUp;

        public event EventHandler<InputMouseEventArgs> MouseMove;

        public event EventHandler<InputMouseEventArgs> MouseWheelChanged;

        public bool this[Key key] { get { return Window.Keyboard[key]; } }

        public bool this[uint scancode] { get { return Window.Keyboard[scancode]; } }

        public bool this[MouseButton button] { get { return Window.Mouse[button]; } }

        public InputManager(GameWindow window)
        {
            Window = window;

            Window.Keyboard.KeyDown += OnKeyboardKeyDown;
            Window.Keyboard.KeyUp += OnKeyboardKeyUp;
            Window.Mouse.ButtonDown += OnMouseButtonDown;
            Window.Mouse.ButtonUp += OnMouseButtonUp;
            Window.Mouse.Move += OnMouseMove;
            Window.Mouse.WheelChanged += OnMouseWheelChanged;
        }

        protected void OnKeyboardKeyDown(object sender, KeyboardKeyEventArgs e)
        {
            Debug.WriteLine("OnKeyboardKeyDown: Key: {0} ScanCode: {1}", e.Key, e.ScanCode);
            if (null != KeyDown)
                KeyDown(this, new InputKeyboardEventArgs(e));
        }

        protected void OnKeyboardKeyUp(object sender, KeyboardKeyEventArgs e)
        {
            Debug.WriteLine("OnKeyboardKeyUp: Key: {0} ScanCode: {1}", e.Key, e.ScanCode);
            if (null != KeyUp)
                KeyUp(this, new InputKeyboardEventArgs(e));
        }

        protected void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnMouseButtonDown: Button: {0}, IsPressed: {1}, Position: {2}, X: {3}, Y: {4}", e.Button, e.IsPressed, e.Position, e.X, e.Y);
            if (null != ButtonDown)
                ButtonDown(this, new InputMouseEventArgs(e));
        }

        protected void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("OnMouseButtonUp: Button: {0}, IsPressed: {1}, Position: {2}, X: {3}, Y: {4}", e.Button, e.IsPressed, e.Position, e.X, e.Y);
            if (null != ButtonUp)
                ButtonUp(this, new InputMouseEventArgs(e));
        }

        protected void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            //Debug.WriteLine("OnMouseButtonUp: Position: {0}, X: {1}, XDelta: {2}, Y: {3}, YDelta: {4}", e.Position, e.X, e.XDelta, e.Y, e.YDelta);
            if (null != MouseMove)
                MouseMove(this, new InputMouseEventArgs(e));
        }

        protected void OnMouseWheelChanged(object sender, MouseWheelEventArgs e)
        {
            Debug.WriteLine("OnMouseButtonUp: Delta: {0}, DeltaPrecise: {1}, Position: {2}, Value: {3}, ValuePrecise: {4}, X: {5}, Y: {6}", e.Delta, e.DeltaPrecise, e.Position, e.Value, e.ValuePrecise, e.X, e.Y);
            if (null != MouseWheelChanged)
                MouseWheelChanged(this, new InputMouseEventArgs(e));
        }
    }
}

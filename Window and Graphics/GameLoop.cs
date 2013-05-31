using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MartinZottmann
{
    public class GameLoop
    {
        public delegate void LoopCallback(double elapsedTime);

        private readonly LoopCallback callback;

        protected DateTime last_frame;

        public bool running = true;

        public GameLoop(LoopCallback callback)
        {
            this.callback = callback;
            Application.Idle += OnApplicationEnterIdle;
            last_frame = DateTime.Now;
        }

        private void OnApplicationEnterIdle(object sender, EventArgs e)
        {
            while (IsAppStillIdle())
            {
                DateTime frame = DateTime.Now;
                double delta_time = frame.Subtract(last_frame).TotalMilliseconds;
                last_frame = frame;

                Console.Write(1000.0 / delta_time);
                Console.Write(", ");
                Console.WriteLine(delta_time);

                if (running)
                {
                    callback(delta_time);
                }

                //System.Threading.Thread.Sleep(1);
            }
        }

        private bool IsAppStillIdle()
        {
            Message msg;
            return !PeekMessage(out msg, IntPtr.Zero, 0, 0, 0);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Message
        {
            public IntPtr hWnd;
            public Int32 msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern bool PeekMessage(out Message msg, IntPtr hWnd, uint messageFilterMin, uint messageFilterMax, uint flags);
    }
}

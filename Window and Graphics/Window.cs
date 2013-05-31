using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;

namespace MartinZottmann
{
    public partial class Window : Form
    {
        protected Bitmap back_buffer;

        protected Game game;

        protected GameLoop game_loop;

        public Window()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            //FormBorderStyle = FormBorderStyle.None;
            ClientSize = new Size(640, 480);
            BackBuffer();

            game = new Game(ClientSize);
            ClientSizeChanged += new EventHandler(Window_ClientSizeChanged);
            Paint += new PaintEventHandler(Window_Paint);

            //game_loop = new GameLoop(Update);
            //LostFocus += new EventHandler(Window_LostFocus);
            //GotFocus += new EventHandler(Window_GotFocus);

            Show();

            Thread t = new Thread(Loop);
            t.Start();
        }

        public void Loop()
        {
            DateTime previous_frame = DateTime.Now;
            DateTime current_frame;
            double delta_time;
            while (Created)
            {
                current_frame = DateTime.Now;
                delta_time = current_frame.Subtract(previous_frame).TotalMilliseconds;
                previous_frame = current_frame;
                
                Console.WriteLine("{0:F}, {1:F}", 1000.0 / delta_time, delta_time);

                Update(delta_time);

                Thread.Sleep(1);
            }
        }

        public void Update(double delta_time)
        {
            game.Update(delta_time);
            Window_Draw(delta_time);
        }

        protected void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            BackBuffer();
            game.ClientSize = ClientSize;
        }

        protected void BackBuffer()
        {
            if (back_buffer != null)
            {
                back_buffer.Dispose();
            }

            back_buffer = new Bitmap(ClientSize.Width, ClientSize.Height);
        }

        protected void Window_Paint(object sender, PaintEventArgs e)
        {
            lock (back_buffer)
            {
                e.Graphics.DrawImageUnscaled(back_buffer, Point.Empty);
            }
        }

        protected void Window_GotFocus(object sender, EventArgs e)
        {
            game_loop.running = true;
        }

        protected void Window_LostFocus(object sender, EventArgs e)
        {
            game_loop.running = false;
        }

        protected void Window_Draw(double delta_time)
        {
            lock (back_buffer)
            {
                using (var g = Graphics.FromImage(back_buffer))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    game.Draw(delta_time, g);
                }
            }

            Invalidate();
        }
    }
}
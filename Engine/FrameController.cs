using System;
using System.Diagnostics;

namespace MartinZottmann.Engine
{
    public class FrameController
    {
        protected double DeltaTime;

        protected readonly double MaxDeltaTime;

        protected readonly double MinDeltaTime;

        protected readonly bool CatchUp;

        protected readonly Action<double> Action;

        protected readonly Stopwatch Watch;

        public FrameController(int min_fps, int max_fps, bool catch_up, Action<double> action)
            : this(catch_up, action)
        {
            MaxDeltaTime = 1.0 / min_fps;
            MinDeltaTime = 1.0 / max_fps;
        }

        public FrameController(double min_delta_time, double max_delta_time, bool catch_up, Action<double> action)
            : this(catch_up, action)
        {
            MaxDeltaTime = min_delta_time;
            MinDeltaTime = max_delta_time;
        }

        protected FrameController(bool catch_up, Action<double> action)
        {
            CatchUp = catch_up;
            Action = action;
            Watch = new Stopwatch();
            Watch.Start();
        }

        public void TryFrame()
        {
            var delta_time = Watch.Elapsed.TotalSeconds;
            Watch.Restart();
            DeltaTime += delta_time;

            //Debug.Write("{0} {1:F5} {2:F5}\t", Action.Method.Name, delta_time, DeltaTime);
            if (DeltaTime <= MinDeltaTime)
            {
                //Debug.WriteLine("Skip");
            }
            else if (DeltaTime >= MaxDeltaTime)
            {
                //Debug.WriteLine("Slow");
                DoFrame(CatchUp ? MaxDeltaTime : DeltaTime);
            }
            else
            {
                //Debug.WriteLine("Good");
                DoFrame(DeltaTime);
            }
        }

        protected void DoFrame(double delta_time)
        {
            Action(delta_time);
            DeltaTime -= delta_time;
        }
    }
}

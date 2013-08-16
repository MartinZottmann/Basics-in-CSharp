using System;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Components
{
    public class Children : Abstract, IDisposable
    {
        public List<GameObject> List = new List<GameObject>();

        public Children(GameObject game_object) : base(game_object) { }

        public override void Dispose()
        {
            foreach (var child in List)
                child.Dispose();
        }

        public void Add(GameObject child)
        {
            List.Add(child);
        }

        public void Remove(GameObject child)
        {
            List.Remove(child);
        }

        public override void Update(double delta_time, Graphics.RenderContext render_context)
        {
            if (List.Count == 0)
                return;

            render_context = render_context.Push();
            List.ForEach(s => s.Update(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public override void Render(double delta_time, Graphics.RenderContext render_context)
        {
            if (List.Count == 0)
                return;

            render_context = render_context.Push();
            List.ForEach(s => s.Render(delta_time, render_context));
            render_context = render_context.Pop();
        }
    }
}

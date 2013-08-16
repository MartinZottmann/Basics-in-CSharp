using MartinZottmann.Game.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;

namespace MartinZottmann.Game.Entities.Components
{
    public class Children : Abstract, IDisposable, IEnumerable<GameObject>
    {
        protected List<GameObject> list = new List<GameObject>();

        public int Count { get { return list.Count; } }

        public Children(GameObject game_object) : base(game_object) { }

        public override void Dispose()
        {
            foreach (var child in list)
                child.Dispose();
        }

        public void Add(GameObject child)
        {
            list.Add(child);
        }

        public void Remove(GameObject child)
        {
            list.Remove(child);
        }

        public override void Update(double delta_time, RenderContext render_context)
        {
            if (list.Count == 0)
                return;

            render_context = render_context.Push();
            list.ForEach(s => s.Update(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public override void Render(double delta_time, RenderContext render_context)
        {
            if (list.Count == 0)
                return;

            render_context = render_context.Push();
            list.ForEach(s => s.Render(delta_time, render_context));
            render_context = render_context.Pop();
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}

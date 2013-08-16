using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    public interface IComponent : IDisposable, ISaveable
    {
        void Update(double delta_time, RenderContext render_context);

        void Render(double delta_time, RenderContext render_context);
    }
}

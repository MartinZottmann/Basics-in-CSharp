using MartinZottmann.Game.Graphics;
using MartinZottmann.Game.IO;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    public interface IComponent : MartinZottmann.Engine.Entities.IComponent, IDisposable, ISaveable { }
}

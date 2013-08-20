using MartinZottmann.Engine.Entities;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class ParticleEmitterComponent : IComponent
    {
        public uint NumParticles = 10000;

        public double MaxAge = Double.MaxValue;
    }
}

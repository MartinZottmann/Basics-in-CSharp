using MartinZottmann.Engine.Entities;
using System;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class GameStateComponent : IComponent
    {
        public bool Debug = false;

        public string CameraEntityName = "Camera";

        [NonSerialized]
        protected internal Entity camera_entity;

        public string InputEntityName = "Camera";

        [NonSerialized]
        protected internal Entity input_entity;
    }
}

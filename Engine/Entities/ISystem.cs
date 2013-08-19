﻿namespace MartinZottmann.Engine.Entities
{
    public interface ISystem
    {
        void Bind(EntityManager entitiy_manager);

        void Update(double delta_time);

        void Render(double delta_time);
    }
}

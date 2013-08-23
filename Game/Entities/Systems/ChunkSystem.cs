using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace MartinZottmann.Game.Entities.Systems
{
    public struct Point3i
    {
        public int X;

        public int Y;

        public int Z;

        public Point3i(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public static bool operator ==(Point3i left, Point3i right)
        {
            return left.X == right.X && left.Y == right.Y && left.Z == right.Z;
        }

        public static bool operator !=(Point3i left, Point3i right)
        {
            return left.X != right.X || left.Y != right.Y || left.Z != right.Z;
        }
    }

    public class ChunkSystem : ISystem
    {
        public const uint CHUNK_SIZE = 100;

        public const uint LOADED_RADIUS = 200;

        protected EntityManager entitiy_manager;

        protected NodeList<InputNode> input_nodes;

        public void Bind(EntityManager entitiy_manager)
        {
            this.entitiy_manager = entitiy_manager;
            input_nodes = this.entitiy_manager.Get<InputNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var input_node in input_nodes)
            {
                var input_position = VectorToPoint(input_node.Base.Position);
                foreach (var entitiy in entitiy_manager.Entities)
                {
                    var entity_position = VectorToPoint(entitiy.Get<BaseComponent>().Position);
                    foreach (var chunk in Chunks(input_position))
                        if (entity_position == chunk)
                            goto CHUNK_FOUND;
                CHUNK_NOT_FOUND:
                    Save(entity_position, entitiy);
                    entitiy_manager.Remove(entitiy);
                CHUNK_FOUND:
                    ;
                }

                LoadAround(input_position);
            }
        }

        public Point3i VectorToPoint(Vector3d vector)
        {
            return new Point3i(
                (int)Math.Round(vector.X / CHUNK_SIZE, MidpointRounding.AwayFromZero),
                (int)Math.Round(vector.Y / CHUNK_SIZE, MidpointRounding.AwayFromZero),
                (int)Math.Round(vector.Z / CHUNK_SIZE, MidpointRounding.AwayFromZero)
            );
        }

        public void Render(double delta_time) { }

        public IEnumerable<Point3i> Chunks(Point3i position)
        {
            var c = (int)Math.Ceiling((double)LOADED_RADIUS / CHUNK_SIZE);
            for (var x = -c; x <= c; x++)
                for (var y = -c; y <= c; y++)
                    for (var z = -c; z <= c; z++)
                        yield return new Point3i(position.X + x, position.Y + y, position.Z + z);
        }

        protected void Save(Point3i position, object @object)
        {
            Debug.WriteLine("Save {0}: {1}", position, @object);
            var directory = DirectoryInfo(position);
            if (!directory.Exists)
                directory.Create();

            var file = new FileSystem(String.Format("{0}/{1}.save", directory, @object.GetHashCode()));
            file.Save(@object);
        }

        protected void LoadAround(Point3i position)
        {
            foreach (var chunk in Chunks(position))
                LoadAt(chunk);
        }

        protected void LoadAt(Point3i position)
        {
            var directory_info = DirectoryInfo(position);
            if (!directory_info.Exists)
                return;

            foreach(var file_info in directory_info.GetFiles("*.save"))
            {
                Debug.WriteLine("Load {0}: {1}", position, file_info.FullName);
                var file = new FileSystem(file_info.FullName);
                var entity = file.Load<Entity>();
                entitiy_manager.Add(entity);
                file_info.Delete();
            }
        }

        protected string DirectoryName(Point3i position)
        {
            return String.Format("Savegames/{0}.{1}.{2}/", position.X, position.Y, position.Z);
        }

        protected DirectoryInfo DirectoryInfo(Point3i position)
        {
            return new DirectoryInfo(DirectoryName(position));
        }
    }
}

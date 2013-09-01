using MartinZottmann.Engine.Entities;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

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

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public override bool Equals(object @object)
        {
            if (!(@object is Point3i))
                return false;
            return Equals((Point3i)@object);
        }

        public bool Equals(Point3i other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }

    public class ChunkSystem : ISystem, IDisposable
    {
        public const uint CHUNK_SIZE = 100;

        public const uint LOADED_RADIUS = 200;

        protected bool running = true;

        protected Thread loader;

        protected object loader_lock = new Object();

        protected List<Entity> loader_entities = new List<Entity>();

        protected EntityManager entity_manager;

        protected NodeList<InputNode> input_nodes;

        public ChunkSystem()
        {
            loader = new Thread(
                () =>
                {
                    while (running)
                    {
                        Debug.WriteLine("Chunk loader: Loading");
                        foreach (var input_node in input_nodes)
                        {
                            var input_position = VectorToPoint(input_node.Base.Position);

                            LoadAround(input_position);
                        }
                        Debug.WriteLine("Chunk loader: Loaded");
                        Thread.Sleep(1000);
                    }
                }
            );
            loader.Start();
        }

        public void Dispose()
        {
            Debug.WriteLine("ChunkSystem.Disposing");
            running = false;
            loader.Join();
            Debug.WriteLine("ChunkSystem.Disposed");
        }

        public void Bind(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;
            input_nodes = this.entity_manager.Get<InputNode>();
        }

        public void Update(double delta_time)
        {
            foreach (var input_node in input_nodes)
            {
                var input_position = VectorToPoint(input_node.Base.Position);
                foreach (var entity in entity_manager.Entities)
                {
                    if (entity.Has<CursorComponent>())
                        continue;

                    if (!entity.Has<BaseComponent>())
                        continue;

                    var entity_position = VectorToPoint(entity.Get<BaseComponent>().Position);
                    foreach (var chunk in Chunks(input_position))
                        if (entity_position == chunk)
                            goto CHUNK_FOUND;
                    entity_manager.Remove(entity);
                    new Thread(s => Save(entity_position, entity)).Start();
                CHUNK_FOUND:
                    ;
                }
            }

            lock (loader_lock)
            {
                foreach (var loader_entity in loader_entities)
                {
                    entity_manager.Add(loader_entity);
                }
                loader_entities.Clear();
            }
        }

        public void Render(double delta_time) { }

        public Point3i VectorToPoint(Vector3d vector)
        {
            return new Point3i(
                (int)Math.Round(vector.X / CHUNK_SIZE, MidpointRounding.AwayFromZero),
                (int)Math.Round(vector.Y / CHUNK_SIZE, MidpointRounding.AwayFromZero),
                (int)Math.Round(vector.Z / CHUNK_SIZE, MidpointRounding.AwayFromZero)
            );
        }

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

            var file = new FileSystem(String.Format("{0}/{1}.xml", directory, @object.GetHashCode()));
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

            lock (loader_lock)
            {
                foreach (var file_info in directory_info.GetFiles("*.xml"))
                {
                    Debug.WriteLine("Load {0}: {1}", position, file_info.FullName);
                    var file = new FileSystem(file_info.FullName);
                    var entity = file.Load<Entity>();
                    file_info.Delete();
                    loader_entities.Add(entity);
                }
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

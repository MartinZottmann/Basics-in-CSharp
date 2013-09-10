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

    public class ChunkSystem : ISystem
    {
        public const uint CHUNK_SIZE = 100;

        public const uint LOADED_RADIUS = 200;

        protected bool running;

        protected Thread loader;

        protected object loader_lock = new Object();

        protected List<Entity> loader_entities = new List<Entity>();

        protected EntityManager entity_manager;

        protected NodeList<ChunkLoaderNode> chunk_loader_nodes;

        public ChunkSystem()        {        }

        public void Start(EntityManager entity_manager)
        {
            this.entity_manager = entity_manager;
            chunk_loader_nodes = this.entity_manager.GetNodeList<ChunkLoaderNode>();

            running = true;

            loader = new Thread(
                () =>
                {
                    while (running)
                    {
                        if (null == chunk_loader_nodes)
                            continue;

                        //Debug.WriteLine("Chunk loader: Loading");
                        foreach (var chunk_loader_node in chunk_loader_nodes)
                            LoadAround(VectorToPoint(chunk_loader_node.Base.Position));
                        //Debug.WriteLine("Chunk loader: Loaded");
                        Thread.Sleep(1000);
                    }
                }
            );
            loader.Start();
        }

        public void Update(double delta_time)
        {
            var entities = entity_manager.Entities;
            var n = entities.Length;

            foreach (var chunk_loader_node in chunk_loader_nodes)
            {
                var chunk_loader_position = VectorToPoint(chunk_loader_node.Base.Position);

                for (var i = 0; i < n; i++)
                {
                    var entity = entities[i];

                    if (null == entity)
                        continue;

                    if (!entity.Has<BaseComponent>())
                    {
                        entities[i] = null;
                        continue;
                    }

                    var entity_position = VectorToPoint(entity.Get<BaseComponent>().Position);
                    foreach (var chunk in Chunks(chunk_loader_position))
                        if (chunk == entity_position)
                            entities[i] = null;
                }
            }

            for (var i = 0; i < n; i++)
            {
                var entity = entities[i];

                if (null == entity)
                    continue;

                entity_manager.RemoveEntity(entity);
                new Thread(() => { Save(VectorToPoint(entity.Get<BaseComponent>().Position), entity); }).Start();
            }

            lock (loader_lock)
            {
                foreach (var loader_entity in loader_entities)
                {
                    entity_manager.AddEntity(loader_entity);
                }
                loader_entities.Clear();
            }
        }

        public void Render(double delta_time) { }

        public void Stop()
        {
            running = false;

            loader.Join();

            chunk_loader_nodes = null;
        }

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

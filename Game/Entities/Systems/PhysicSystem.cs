using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Nodes;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace MartinZottmann.Game.Entities.Systems
{
    public class PhysicSystem : ISystem
    {
        public Camera Camera;

        public ResourceManager ResourceManager;

        protected NodeList<PhysicNode> physic_nodes;

        public PhysicSystem(Camera camera, ResourceManager resource_manager)
        {
            Camera = camera;
            ResourceManager = resource_manager;
        }

        public void Bind(EntityManager entitiy_manager)
        {
            physic_nodes = entitiy_manager.Get<PhysicNode>();
            physic_nodes.NodeAdded += OnNodeAdded;
        }

        public void Update(double delta_time)
        {
            foreach (var physic_node in physic_nodes)
            {
                physic_node.UpdateVelocity(delta_time);
                physic_node.UpdatePosition(delta_time);
            }
        }

        public void Render(double delta_time)
        {
#if DEBUG
            foreach (var physic_node in physic_nodes)
            {
                physic_node.DebugModel.Program.UniformLocations["in_ModelViewProjection"].Set(Matrix4d.CreateTranslation(physic_node.Base.Position) * Camera.ViewMatrix * Camera.ProjectionMatrix);
                physic_node.DebugModel.Draw();
            }
#endif
        }

        protected void OnNodeAdded(object sender, NodeEventArgs<PhysicNode> e)
        {
#if DEBUG
            InitDebugModel(e.Node);
#endif
        }

#if DEBUG
        protected void InitDebugModel(PhysicNode physic_node)
        {
            var min = physic_node.Physic.BoundingBox.Min;
            var max = physic_node.Physic.BoundingBox.Max;
            var x0 = (float)min.X;
            var y0 = (float)min.Y;
            var z0 = (float)min.Z;
            var x1 = (float)max.X;
            var y1 = (float)max.Y;
            var z1 = (float)max.Z;
            var r = 1.0f;
            var g = 0.0f;
            var b = 1.0f;
            var a = 1.0f;
            var verticies = new VertexP3C4[] {
                new VertexP3C4(x0, y0, z0, r, g, b, a),
                new VertexP3C4(x0, y1, z0, r, g, b, a),
                new VertexP3C4(x0, y0, z1, r, g, b, a),
                new VertexP3C4(x0, y1, z1, r, g, b, a),
                new VertexP3C4(x1, y0, z0, r, g, b, a),
                new VertexP3C4(x1, y1, z0, r, g, b, a),
                new VertexP3C4(x1, y0, z1, r, g, b, a),
                new VertexP3C4(x1, y1, z1, r, g, b, a),
                new VertexP3C4(x0, y0, z0, r, g, b, a),
                new VertexP3C4(x1, y0, z0, r, g, b, a),
                new VertexP3C4(x0, y1, z0, r, g, b, a),
                new VertexP3C4(x1, y1, z0, r, g, b, a),
                new VertexP3C4(x0, y0, z1, r, g, b, a),
                new VertexP3C4(x1, y0, z1, r, g, b, a),
                new VertexP3C4(x0, y1, z1, r, g, b, a),
                new VertexP3C4(x1, y1, z1, r, g, b, a),
                new VertexP3C4(x0, y0, z0, r, g, b, a),
                new VertexP3C4(x0, y0, z1, r, g, b, a),
                new VertexP3C4(x1, y0, z0, r, g, b, a),
                new VertexP3C4(x1, y0, z1, r, g, b, a),
                new VertexP3C4(x0, y1, z0, r, g, b, a),
                new VertexP3C4(x0, y1, z1, r, g, b, a),
                new VertexP3C4(x1, y1, z0, r, g, b, a),
                new VertexP3C4(x1, y1, z1, r, g, b, a)
            };
            physic_node.DebugModel = new Model();
            physic_node.DebugModel.Mesh(new Mesh<VertexP3C4, uint>(verticies));
            physic_node.DebugModel.Mode = BeginMode.Lines;
            physic_node.DebugModel.Program = ResourceManager.Programs["normal"];
        }
#endif
    }
}

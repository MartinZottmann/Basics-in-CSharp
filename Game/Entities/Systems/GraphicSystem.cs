using MartinZottmann.Engine;
using MartinZottmann.Engine.Entities;
using MartinZottmann.Engine.Graphics;
using MartinZottmann.Engine.Graphics.Mesh;
using MartinZottmann.Engine.Graphics.OpenGL;
using MartinZottmann.Engine.Resources;
using MartinZottmann.Game.Entities.Components;
using MartinZottmann.Game.Entities.Nodes;
using MartinZottmann.Game.Graphics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using Color4 = MartinZottmann.Engine.Graphics.Color4;

namespace MartinZottmann.Game.Entities.Systems
{
    public class GraphicSystem : ISystem, IDisposable
    {
        public Camera Camera;

        public ResourceManager ResourceManager;

        public MartinZottmann.Game.Graphics.RenderContext RenderContext = new MartinZottmann.Game.Graphics.RenderContext();

        protected NodeList<GraphicNode> graphic_nodes;

        protected NodeList<GameStateNode> game_state_nodes;

        protected FrameBuffer frame_buffer;

        public GraphicSystem(Camera camera, ResourceManager resource_manager)
        {
            Camera = camera;
            ResourceManager = resource_manager;

            // @todo Remove
            RenderContext.Window = Camera.Window;
            RenderContext.Projection = Camera.ProjectionMatrix;
            RenderContext.View = Camera.ViewMatrix;

            var texture_target = TextureTarget.Texture2D;
            RenderContext.DepthTexture = new Texture(texture_target);
            using (new Bind(RenderContext.DepthTexture))
            {
                GL.TexImage2D(texture_target, 0, PixelInternalFormat.DepthComponent, 1024, 1024, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
                GL.TexParameter(texture_target, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                GL.TexParameter(texture_target, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(texture_target, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(texture_target, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(texture_target, TextureParameterName.TextureCompareMode, (int)TextureCompareMode.CompareRToTexture);
                GL.TexParameter(texture_target, TextureParameterName.TextureCompareFunc, (int)DepthFunction.Lequal);
                //GL.TexParameter(texture_target, TextureParameterName.DepthTextureMode, (int)All.Intensity);
                //GL.TexParameter(texture_target, TextureParameterName.DepthTextureMode, (int)All.Luminance);
            }

            frame_buffer = new FrameBuffer(FramebufferTarget.Framebuffer);
            frame_buffer.Texture(FramebufferAttachment.DepthAttachment, RenderContext.DepthTexture, 0);
            frame_buffer.DrawBuffer(DrawBufferMode.None);
            frame_buffer.CheckStatus();
        }

        ~GraphicSystem()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                frame_buffer.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Start(EntityManager entity_manager)
        {
            graphic_nodes = entity_manager.GetNodeList<GraphicNode>();
            graphic_nodes.NodeAdded += OnNodeAdded;
            game_state_nodes = entity_manager.GetNodeList<GameStateNode>();
        }

        public void Update(double delta_time)
        {
            RenderContext.Projection = Camera.ProjectionMatrix;
            RenderContext.View = Camera.ViewMatrix;
        }

        public void Render(double delta_time)
        {
            #region Depth
            var light = new Vector3d(10, 10, 0);
            var light_target = new Vector3d(0, 0, 0);
            var depth_render_context = new MartinZottmann.Game.Graphics.RenderContext()
            {
                Window = RenderContext.Window,
                Projection = Matrix4d.CreateOrthographicOffCenter(-10, 10, -10, 10, 0, 100),
                //Projection = Matrix4d.Perspective(MathHelper.PiOver2, Camera.Aspect, 0.1, 30),
                View = Matrix4d.LookAt(light, light_target, Vector3d.UnitY),
                Model = Matrix4d.Identity,
                Program = ResourceManager.Programs["depth"]
            };
            var depth_bias = Matrix4d.Scale(0.5, 0.5, 0.5) * Matrix4d.CreateTranslation(0.5, 0.5, 0.5); // Map [-1, 1] to [0, 1]
            var depth_bias_MVP = RenderContext.InvertedView * depth_render_context.ProjectionViewModel * depth_bias;
            ResourceManager.Programs["standard"].UniformLocations["in_DepthBiasMVP"].Set(depth_bias_MVP);
            ResourceManager.Programs["standard"].UniformLocations["in_Texture"].Set(0);
            ResourceManager.Programs["standard"].UniformLocations["in_ShadowTexture"].Set(1);
            ResourceManager.Programs["standard"].UniformLocations["in_LightPosition"].Set(light);
            //GL.Enable(EnableCap.PolygonOffsetFill);
            //GL.PolygonOffset(2.0f, 4.0f);
            //GL.CullFace(CullFaceMode.Front);
            using (new Bind(frame_buffer))
            {
                GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
                GL.Viewport(0, 0, 1024, 1024);
                Draw(delta_time, depth_render_context);
            }
            //GL.CullFace(CullFaceMode.Back);
            //GL.Disable(EnableCap.PolygonOffsetFill);
            #endregion

            #region Render
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, RenderContext.Window.Width, RenderContext.Window.Height);
            Draw(delta_time, RenderContext);
            #endregion

            //screen.Render(delta_time, screen_render_context);

            #region Debug
            if (game_state_nodes.First.GameState.Debug)
            {
                GL.Viewport(0, 0, RenderContext.Window.Width, RenderContext.Window.Height);
                var debug_render_context = new MartinZottmann.Game.Graphics.RenderContext()
                {
                    Window = RenderContext.Window,
                    Projection = Matrix4d.CreateOrthographicOffCenter(-1, 1, -1, 1, -1, 1),
                    View = Matrix4d.Identity,
                    Model = Matrix4d.Scale(0.25) * Matrix4d.CreateTranslation(-0.75, -0.75, 0)
                };
                var debug_screen = new Model();
                var shape = new MartinZottmann.Engine.Graphics.Shapes.Quad();
                shape.Vertices[0].Texcoord.Y = 0;
                shape.Vertices[1].Texcoord.Y = 0;
                shape.Vertices[2].Texcoord.Y = 1;
                shape.Vertices[3].Texcoord.Y = 1;
                debug_screen.Mesh(shape);
                debug_screen.Program = ResourceManager.Programs["plain_texture"];
                debug_screen.Program.UniformLocations["in_ModelViewProjection"].Set(debug_render_context.ProjectionViewModel);
                debug_screen.Program.UniformLocations["in_Texture"].Set(0);
                debug_screen.Texture = RenderContext.DepthTexture;
                debug_screen.Draw();
            }
            #endregion
        }

        public void Stop()
        {
            graphic_nodes.NodeAdded -= OnNodeAdded;
            graphic_nodes = null;
            game_state_nodes = null;
        }

        protected void OnNodeAdded(object sender, NodeEventArgs<GraphicNode> e)
        {
            LoadModel(e.Node);
#if DEBUG
            if (e.Node.Entity.Has<BaseComponent>())
                InitModelDebugOrientation(e.Node);
            if (e.Node.Entity.Has<PhysicComponent>())
                InitModelDebugPhysic(e.Node);
#endif
        }

        protected void LoadModel(GraphicNode graphic_node)
        {
            if (null == graphic_node.Graphic.ModelName)
                return;

            if (null != graphic_node.Graphic.Model)
                return;

            try
            {
                graphic_node.Graphic.Model = (Model)Activator.CreateInstance(Type.GetType(graphic_node.Graphic.ModelName));
            }
            catch
            {
                graphic_node.Graphic.Model = ResourceManager.Models.Load(graphic_node.Graphic.ModelName);
            }
            if (null != graphic_node.Graphic.ProgramName)
                graphic_node.Graphic.Model.Program = ResourceManager.Programs[graphic_node.Graphic.ProgramName];
            if (null != graphic_node.Graphic.TextureName)
                graphic_node.Graphic.Model.Texture = ResourceManager.Textures[graphic_node.Graphic.TextureName];
        }

#if DEBUG
        protected void InitModelDebugOrientation(GraphicNode graphic_node)
        {
            var b = graphic_node.Base;
            var forward = new Color4(1.0f, 0.0f, 0.0f, 0.5f);
            var up = new Color4(0.0f, 1.0f, 0.0f, 0.5f);
            var right = new Color4(0.0f, 0.0f, 1.0f, 0.5f);
            graphic_node.ModelDebugOrientation = new Model();
            graphic_node.ModelDebugOrientation.Mesh(
                new Mesh<VertexP3C4, uint>()
                {
                    Vertices = new VertexP3C4[] {
                        new VertexP3C4(Vector3.Zero, forward),
                        new VertexP3C4((Vector3)b.Forward, forward),
                        new VertexP3C4(Vector3.Zero, up),
                        new VertexP3C4((Vector3)b.Up, up),
                        new VertexP3C4(Vector3.Zero, right),
                        new VertexP3C4((Vector3)b.Right, right),
                    },
                    Indices = new uint[] {
                        0, 1, 2, 3, 4, 5
                    }
                }
            );
            graphic_node.ModelDebugOrientation.Mode = BeginMode.Lines;
            graphic_node.ModelDebugOrientation.Program = ResourceManager.Programs["normal"];
        }

        protected void InitModelDebugPhysic(GraphicNode graphic_node)
        {
            var physic = graphic_node.Entity.Get<PhysicComponent>();
            var min = physic.BoundingBox.Min;
            var max = physic.BoundingBox.Max;
            var x0 = (float)min.X;
            var y0 = (float)min.Y;
            var z0 = (float)min.Z;
            var x1 = (float)max.X;
            var y1 = (float)max.Y;
            var z1 = (float)max.Z;
            var verticies = new VertexP3[] {
                new VertexP3(x0, y0, z0),
                new VertexP3(x0, y1, z0),
                new VertexP3(x0, y0, z1),
                new VertexP3(x0, y1, z1),
                new VertexP3(x1, y0, z0),
                new VertexP3(x1, y1, z0),
                new VertexP3(x1, y0, z1),
                new VertexP3(x1, y1, z1),
                new VertexP3(x0, y0, z0),
                new VertexP3(x1, y0, z0),
                new VertexP3(x0, y1, z0),
                new VertexP3(x1, y1, z0),
                new VertexP3(x0, y0, z1),
                new VertexP3(x1, y0, z1),
                new VertexP3(x0, y1, z1),
                new VertexP3(x1, y1, z1),
                new VertexP3(x0, y0, z0),
                new VertexP3(x0, y0, z1),
                new VertexP3(x1, y0, z0),
                new VertexP3(x1, y0, z1),
                new VertexP3(x0, y1, z0),
                new VertexP3(x0, y1, z1),
                new VertexP3(x1, y1, z0),
                new VertexP3(x1, y1, z1)
            };
            graphic_node.ModelDebugPhysic = new Model();
            graphic_node.ModelDebugPhysic.Mesh(new Mesh<VertexP3, uint>(verticies));
            graphic_node.ModelDebugPhysic.Mode = BeginMode.Lines;
            graphic_node.ModelDebugPhysic.Program = ResourceManager.Programs["primitive_colored"];
        }
#endif

        protected void Draw(double delta_time, MartinZottmann.Game.Graphics.RenderContext render_context)
        {
            foreach (var graphic_node in graphic_nodes)
            {
                if (null != graphic_node.Graphic.Model)
                {
                    render_context.Model = graphic_node.Base.Model;

                    var program = null == render_context.Program
                        ? graphic_node.Graphic.Model.Program
                        : render_context.Program;

                    foreach (var s in program.UniformLocations)
                        switch (s.Key)
                        {
                            case "alpha_cutoff": s.Value.Set(render_context.alpha_cutoff); break;
                            case "in_DepthBiasMVP": break;
                            case "in_ShadowTexture":
                                render_context.DepthTexture.Bind();
                                s.Value.Set(render_context.DepthTexture.BindId);
                                break;
                            case "in_LightPosition": s.Value.Set(new Vector3d(10, 10, 10)); break; // @todo
                            case "in_Model": s.Value.Set(render_context.Model); break;
                            case "in_View": s.Value.Set(render_context.View); break;
                            case "in_ModelView": s.Value.Set(render_context.ViewModel); break;
                            case "in_ModelViewProjection": s.Value.Set(render_context.ProjectionViewModel); break;
                            case "in_NormalView": s.Value.Set(render_context.Normal); break;
                            case "in_Texture":
                                graphic_node.Graphic.Model.Texture.Bind();
                                s.Value.Set(graphic_node.Graphic.Model.Texture.BindId);
                                break;
                            default: throw new NotImplementedException(s.Key);
                        }

                    graphic_node.Graphic.Model.Draw(program);

                    foreach (var s in program.UniformLocations)
                        switch (s.Key)
                        {
                            case "alpha_cutoff": break;
                            case "in_DepthBiasMVP": break;
                            case "in_ShadowTexture":
                                render_context.DepthTexture.UnBind();
                                break;
                            case "in_LightPosition": break;
                            case "in_Model": break;
                            case "in_View": break;
                            case "in_ModelView": break;
                            case "in_ModelViewProjection": break;
                            case "in_NormalView": break;
                            case "in_Texture":
                                graphic_node.Graphic.Model.Texture.UnBind();
                                break;
                            default: throw new NotImplementedException(s.Key);
                        }
                }
#if DEBUG
                if (game_state_nodes.First.GameState.Debug)
                {
                    if (null != graphic_node.ModelDebugOrientation)
                    {
                        GL.LineWidth(5);
                        graphic_node.ModelDebugOrientation.Program.UniformLocations["in_ModelViewProjection"].Set(Matrix4d.CreateTranslation(graphic_node.Base.WorldPosition) * Camera.ViewMatrix * Camera.ProjectionMatrix);
                        graphic_node.ModelDebugOrientation.Draw();
                        GL.LineWidth(1);
                    }
                    if (null != graphic_node.ModelDebugPhysic)
                    {
                        graphic_node.ModelDebugPhysic.Program.UniformLocations["in_Color"].Set(graphic_node.Base.Mark);
                        graphic_node.ModelDebugPhysic.Program.UniformLocations["in_ModelViewProjection"].Set(Matrix4d.CreateTranslation(graphic_node.Base.WorldPosition) * Camera.ViewMatrix * Camera.ProjectionMatrix);
                        graphic_node.ModelDebugPhysic.Draw();
                    }
                }
#endif
            }
        }
    }
}

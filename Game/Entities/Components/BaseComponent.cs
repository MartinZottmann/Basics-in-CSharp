using MartinZottmann.Engine.Entities;
using OpenTK;
using OpenTK.Graphics;
using System;
using System.Xml.Serialization;

namespace MartinZottmann.Game.Entities.Components
{
    [Serializable]
    public class BaseComponent : IComponent
    {
        public static Random Random = new Random();

        public Vector3d Scale = new Vector3d(1, 1, 1);

        public Quaterniond Orientation = Quaterniond.Identity;

        public Vector3d Position = Vector3d.Zero;

        /// <summary>
        /// Model Matrix = Scale * Rotation * Translation
        /// </summary>
        [XmlIgnore]
        public Matrix4d Model
        {
            get
            {
                return Parent == null
                    ? Matrix4d.Scale(Scale) * Matrix4d.Rotate(Orientation) * Matrix4d.CreateTranslation(Position)
                    : Matrix4d.Scale(Scale) * Matrix4d.Rotate(Orientation) * Matrix4d.CreateTranslation(Position) * Parent.Model;
            }
        }

        [XmlIgnore]
        public Matrix4d OrientationMatrix
        {
            get { return Matrix4d.CreateFromQuaternion(ref Orientation); }
            set { Orientation = value.ExtractRotation(); }
        }

        [XmlIgnore]
        public Matrix4d InverseOrientationMatrix
        {
            get { return Matrix4d.CreateFromQuaternion(ref Orientation).Inverted(); }
            set { Orientation = value.Inverted().ExtractRotation(); }
        }

        public Color4 Mark = Color4.Pink;

        public Vector3d Forward = -Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = Vector3d.UnitX;

        public Vector3d RightRelative { get { return Vector3d.Transform(Right, Orientation); } }

        protected BaseComponent Parent { get; set; }

        protected BaseComponent[] Children { get; set; }

        public BaseComponent Add(Entity child)
        {
            return Add(child.Get<BaseComponent>());
        }

        public BaseComponent Add(BaseComponent child)
        {
            var children = Children;
            var i = 0;
            var n = children == null ? 0 : children.Length;
            Children = new BaseComponent[n + 1];
            for (; i < n; i++)
                Children[i] = children[i];
            Children[i] = child;
            child.Parent = this;

            return this;
        }
    }
}

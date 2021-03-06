﻿using MartinZottmann.Engine.Entities;
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

        public Vector3d WorldPosition { get { return null == parent_base ? Position : Position + parent_base.Position; } }

        /// <summary>
        /// Model Matrix = Scale * Rotation * Translation
        /// </summary>
        public Matrix4d Model
        {
            get
            {
                return null == parent_base
                    ? Matrix4d.Scale(Scale) * Matrix4d.Rotate(Orientation) * Matrix4d.CreateTranslation(Position)
                    : Matrix4d.Scale(Scale) * Matrix4d.Rotate(Orientation) * Matrix4d.CreateTranslation(Position) * parent_base.Model;
            }
        }

        [XmlIgnore]
        public Matrix4d OrientationMatrix
        {
            get
            {
                Matrix4d result;
                Matrix4d.CreateFromQuaternion(ref Orientation, out result);
                return result;
            }
            set { Orientation = value.ExtractRotation(); }
        }

        [XmlIgnore]
        public Matrix4d InverseOrientationMatrix
        {
            get
            {
                Matrix4d result;
                Matrix4d.CreateFromQuaternion(ref Orientation, out result);
                return result.Inverted();
            }
            set { Orientation = value.Inverted().ExtractRotation(); }
        }

        public Color4 Mark = Color4.Pink;

        public Vector3d Forward = -Vector3d.UnitZ;

        public Vector3d ForwardRelative { get { return Vector3d.Transform(Forward, Orientation); } }

        public Vector3d Up = Vector3d.UnitY;

        public Vector3d UpRelative { get { return Vector3d.Transform(Up, Orientation); } }

        public Vector3d Right = Vector3d.UnitX;

        public Vector3d RightRelative { get { return Vector3d.Transform(Right, Orientation); } }

        public string ParentName;

        [NonSerialized]
        protected internal BaseComponent parent_base;
    }
}

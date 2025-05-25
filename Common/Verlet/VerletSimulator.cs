using Microsoft.Xna.Framework;
using System;
using Terraria;

namespace TRAEProject.Common.Verlet
{
    public class VerletSimulator
    {
        public Dot[] dots;
        const float deltaTime = 0.01666666666f;//60 fps
        const float deltaTimeSqr = 0.00027777777f;
        public Vector2 force;
        public int iterations;
        public VerletSimulator(int iterations, Dot[] dots)
        {
            this.iterations = iterations;
            this.dots = dots;
        }
        public Vector2[] GetPositions()
        {
            Vector2[] result = new Vector2[dots.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = dots[i].pos;
            }
            return result;
        }
        public void AddForce(Vector2 force)
        {
            this.force += force;
        }
        public void Flip(Vector2 pivot)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                Dot dot = dots[i];
                dot.pos.X = 2 * pivot.X - dot.pos.X;
                dot.pos.Y = 2 * pivot.Y - dot.pos.Y;
                dot.oldPos.X = 2 * pivot.X - dot.oldPos.X;
                dot.oldPos.Y = 2 * pivot.Y - dot.oldPos.Y;
            }
        }
        public void FlipOnY(float pivotY)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                Dot dot = dots[i];
                dot.pos.Y = 2 * pivotY - dot.pos.Y;
                dot.oldPos.Y = 2 * pivotY - dot.oldPos.Y;
            }
        }
        public void FlipOnX(float pivot)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                Dot dot = dots[i];
                dot.pos.X = 2 * pivot - dot.pos.X;
                dot.oldPos.X = 2 * pivot - dot.oldPos.X;
            }
        }

        public void Simulate()
        {
            ApplyPhysics();
            ContrainLengths();
        }
        void ContrainLengths()
        {
            int repeatCount = iterations;
            for (int k = 0; k < repeatCount; k++)
            {
                for (int i = dots.Length - 1; i >= 0; i--)
                {
                    Dot dotA = dots[i];
                    for (int j = dotA.connections.Length - 1; j >= 0; j--)
                    {
                        DotConnection connection = dotA.connections[j];
                        if(connection == null)
                        {
                            continue;
                        }
                        Dot dotB = connection.Other(dotA);
                        Vector2 aPos = dotA.pos;
                        Vector2 bPos = dotB.pos;
                        Vector2 center = (aPos + bPos) * .5f;
                        Vector2 dir = Vector2.Normalize(aPos - bPos);
                        Vector2 connectionOffset = dir * connection.length * .5f;
                        if (!dotA.locked)
                        {
                            dotA.pos = center + connectionOffset;
                        }
                        if (!dotB.locked)
                        {
                            dotB.pos = center - connectionOffset;
                        }
                    }
                }
            }
        }
        void ApplyPhysics()
        {
            Vector2 accel = force;
            Vector2 deltaPos = accel * deltaTimeSqr;
            for (int i = 0; i < dots.Length; i++)
            {
                Dot dot = dots[i];
                if (dot.locked)
                {
                    continue;
                }
                Vector2 oldPos = dot.pos;
                dot.pos += dot.pos - dot.oldPos;
                dot.pos += deltaPos;
                dot.oldPos = oldPos;
            }
            force = Vector2.Zero;
        }
    }
    public class Dot(Vector2 pos, bool locked)
    {
        public Vector2 pos = pos;
        public Vector2 oldPos = pos;
        public bool locked = locked;
        public DotConnection[] connections = new DotConnection[1];
        public void AddConnection(DotConnection newConnection)
        {
            if (connections[^1] != null)
            {
                Array.Resize(ref connections, connections.Length + 1);
            }
            connections[^1] = newConnection;
        }
        public void RemoveConnection(DotConnection connectionToRemove)
        {
            for (int i = 0; i < connections.Length; i++)
            {
                DotConnection current = connections[i];
                if(current == connectionToRemove)
                {
                    //overwrite the current connection with the last entry
                    connections[i] = connections[^1];
                    //now that the last entry is a repeat, we can resize the array to shave off the repeat.
                    Array.Resize(ref connections, connections.Length - 1);
                    break;
                }
            }
        }
        public static void Disconnect(DotConnection connection)
        {
            connection.dotA.RemoveConnection(connection);
            connection.dotB.RemoveConnection(connection);
        }
        public static DotConnection Connect(Dot dotA, Dot dotB, float length = -1)
        {
            DotConnection connection = length < 0 ? new DotConnection(dotA, dotB) : new DotConnection(dotA, dotB, length);
            dotA.AddConnection(connection);
            dotB.AddConnection(connection);
            return connection;
        }
    }
    public class DotConnection
    {
        public Dot dotA;
        public Dot dotB;
        public float length;

        public DotConnection(Dot dotA, Dot dotB)
        {
            this.dotA = dotA;
            this.dotB = dotB;
            length = (dotA.pos - dotB.pos).Length();
        }

        public DotConnection(Dot dotA, Dot dotB, float length)
        {
            this.dotA = dotA;
            this.dotB = dotB;
            this.length = length;
        }
        public Dot Other(Dot dot) => dot == dotA ? dotB : dotA;
    }
}

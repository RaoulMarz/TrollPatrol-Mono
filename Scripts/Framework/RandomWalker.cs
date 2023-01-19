using Godot;
using System;
using System.Collections.Generic;

namespace TrollSmasher.Framework
{
    public class RandomWalker : Node2D
    {
        private List<Vector2> listPathVertices = new List<Vector2>();
        private Vector2 startPoint;
        private Vector2 endPoint;
        private int numPoints;

        public RandomWalker(Vector2 startPointP, Vector2 endPointP, int numPointsP, float cycles)
        {
            startPoint = startPointP;
            endPoint = endPointP;
            numPoints = numPointsP;
        }

        public void SetNumberOfPoints(int npoints)
        {
            numPoints = npoints;
        }

        public void Generate()
        {
            listPathVertices.Clear();
            listPathVertices.Add(startPoint);
            RandomNumberGenerator rngGenner = new RandomNumberGenerator();
            for (int idxPt = 1; idxPt < numPoints; idxPt++)
            {
                Vector2 newPoint = new Vector2();
                listPathVertices.Add(newPoint);
            }
            listPathVertices.Add(endPoint);
        }

        public Path2D GetVerticesPath()
        {
            Path2D res = null;
            if ( (listPathVertices != null) && (listPathVertices.Count > 0) )
            {
                res = new Path2D();
                Curve2D curvePath = new Curve2D();
                res.Curve = curvePath;
            }
            return res;
        }

        public override void _Ready()
		{
			GD.Randomize();
			GD.Print("RandomWalker, _Ready() called");
        }
    }
}
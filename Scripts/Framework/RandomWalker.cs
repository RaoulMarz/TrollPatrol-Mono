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
            Vector2 differenceVector = endPoint - startPoint;
            float yRange = differenceVector.y;
            float yRangeStep = yRange / numPoints;
            RandomNumberGenerator rngGenner = new RandomNumberGenerator();
            for (int idxPt = 1; idxPt < numPoints; idxPt++)
            {
                float xDeviation = ( (differenceVector.x / numPoints) * idxPt) + rngGenner.RandfRange(1.0f, 50.0f);
                if (rngGenner.Randi() % 100 <= 45)
                    xDeviation = -xDeviation;
                float yDeviation = rngGenner.RandfRange(2.0f, yRangeStep * 0.175f);
                if (rngGenner.Randi() % 100 <= 38)
                    yDeviation = -yDeviation;
                Vector2 newPoint = new Vector2(startPoint.x + xDeviation, startPoint.y + (yRangeStep * idxPt) + yDeviation);
                GD.Print($"RandomWalker, Generate(), point[{idxPt}] = {newPoint}");
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
                foreach (Vector2 valuePos in listPathVertices)
                {
                    curvePath.AddPoint(valuePos);
                }
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
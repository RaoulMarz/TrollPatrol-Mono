using Godot;
using System;
using System.Threading;

namespace TrollPatrolMono.Framework
{
    public class SceneUtilities : Node
    {
        public PackedScene newScene;

        public static void ThreadSleep(int milliSeconds)
        {
            if (milliSeconds >= 10)
            {
                System.Threading.Thread.Sleep(milliSeconds);
            }
        }

        public void CleanPreviousScenes(Spatial referenceScene, String callSource = "")
        {
            int numChildren = referenceScene.GetTree().Root.GetChildCount();
            GD.Print($"CleanPreviousScenes() - scenes count = {numChildren}, called from {callSource} \n");
            Node previousScene = referenceScene.GetTree().Root.GetChild(0);
            /*
            var props = Diagnostics.GetProperties(previousScene);
            if (props.Count > 0)
            {
                PrintObjectProperties("previousScene", introTimer);
            }
            */
            referenceScene.GetTree().Root.RemoveChild(previousScene);
        }

        public static void DebugPrintScenesList(Spatial referenceScene)
        {
            int numChildren = referenceScene.GetTree().Root.GetChildCount();
            if (numChildren > 0)
            {
                GD.Print($"DebugPrintScenesList - scenes count = {numChildren}\n");
                int idx = 0;
                foreach (object sceneX in referenceScene.GetTree().Root.GetChildren())
                {
                    Diagnostics.PrintObjectProperties($"scene [{idx}] = ", sceneX);
                    idx += 1;
                }
            }
        }

        public void ChangeScene(Spatial referenceScene, string scenePath)
        {
            newScene = (PackedScene)ResourceLoader.Load(scenePath);
            if (newScene != null)
                referenceScene.GetTree().Root.AddChild(newScene.Instance());
        }

        public static void ExitApplication(Node referenceScene)
        {
            referenceScene.GetTree().Quit();
        }

        public static void SetCameraPosition(Camera aCamera, Vector3 newPosition)
        {
            if ((aCamera != null) && (newPosition != null))
            {
                aCamera.Translation = newPosition;
            }
        }

        public static Rect2 GetApplicationWindowExtent(Spatial referenceScene)
        {
            Rect2 res;
            res = referenceScene.GetTree().Root.GetVisibleRect();
            return res;
        }

        public static Rect2 GetApplicationWindowExtent(Node2D referenceScene)
        {
            Rect2 res;
            res = referenceScene.GetTree().Root.GetVisibleRect();
            return res;
        }

        public static Vector2 GetExtentOffsetsForCenter(Spatial referenceScene, Control graphicsControl)
        {
            Vector2 res = new Vector2(100.0f, 20.0f);
            if (graphicsControl != null)
            {
                Rect2 appExtent = GetApplicationWindowExtent(referenceScene);
                GD.Print($"GetExtentOffsetsForCenter(), appExtent = {appExtent}\n");
                if ( (appExtent.Size.x >= 100.0f) && (graphicsControl.RectSize.x >= 100.0f) )
                {
                    GD.Print($"GetExtentOffsetsForCenter(), appExtent = {appExtent}, graphicsControl size={graphicsControl.RectSize}\n");
                    res = new Vector2( (appExtent.Size.x / 2.0f) - (graphicsControl.RectSize.x / 2.0f), (appExtent.Size.y / 2.0f) - (graphicsControl.RectSize.y / 2.0f) );
                }
            }
            return res;
        }

        public static Vector2 GetExtentOffsetsForCenter(Node2D referenceScene, Control graphicsControl)
        {
            Vector2 res = new Vector2(100.0f, 20.0f);
            if (graphicsControl != null)
            {
                Rect2 appExtent = GetApplicationWindowExtent(referenceScene);
                GD.Print($"GetExtentOffsetsForCenter(), appExtent = {appExtent}\n");
                if ((appExtent.Size.x >= 100.0f) && (graphicsControl.RectSize.x >= 100.0f))
                {
                    GD.Print($"GetExtentOffsetsForCenter(), appExtent = {appExtent}, graphicsControl size={graphicsControl.RectSize}\n");
                    res = new Vector2((appExtent.Size.x / 2.0f) - (graphicsControl.RectSize.x / 2.0f), (appExtent.Size.y / 2.0f) - (graphicsControl.RectSize.y / 2.0f));
                }
            }
            return res;
        }

        public static void PlaceControlCentered(Spatial referenceScene, Control graphicsControl)
        {
            Vector2 placeCenterPos = GetExtentOffsetsForCenter(referenceScene, graphicsControl);
            PlaceControlTopLeft(referenceScene, graphicsControl, placeCenterPos);
        }

        public static void PlaceControlCentered(Node2D referenceScene, Control graphicsControl)
        {
            Vector2 placeCenterPos = GetExtentOffsetsForCenter(referenceScene, graphicsControl);
            PlaceControlTopLeft(referenceScene, graphicsControl, placeCenterPos);
        }

        public static void PlaceControlTopLeft(Spatial referenceScene, Control graphicsControl, Vector2 placePosition)
        {
            if ( (graphicsControl != null) && (placePosition != null) )
            {
                Rect2 appExtent = GetApplicationWindowExtent(referenceScene);
                if (appExtent.Size.x >= 100.0f)
                {
                    Vector2 controlExtent = graphicsControl.RectSize;
                    graphicsControl.RectPosition = placePosition;
                }
            }
        }

        public static void PlaceControlTopLeft(Node2D referenceScene, Control graphicsControl, Vector2 placePosition)
        {
            if ((graphicsControl != null) && (placePosition != null))
            {
                Rect2 appExtent = GetApplicationWindowExtent(referenceScene);
                if (appExtent.Size.x >= 100.0f)
                {
                    Vector2 controlExtent = graphicsControl.RectSize;
                    graphicsControl.RectPosition = placePosition;
                }
            }
        }
    }
}

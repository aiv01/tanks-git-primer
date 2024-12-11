﻿using Aiv.Fast2D;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS_Tankz_1
{
    struct CameraLimits
    {
        public float MaxX;
        public float MinX;
        public float MaxY;
        public float MinY;
    }

    public static class CameraManager
    {

        private static float speed;
        public static float Speed
        {
            get { return speed; }
            set { speed = value == 0 ? speed : value; }
        }
        public static GameObject target;
        public static Camera MainCamera { get; private set; }
        private static CameraLimits limits;
        private static Dictionary<string, Tuple<Camera, float>> cameras;

        static CameraManager()
        {
            MainCamera = new Camera();
            speed = 10f;
            cameras = new Dictionary<string, Tuple<Camera, float>>();
        }

        public static void Init(Vector2 position, Vector2 pivot)
        {
            MainCamera.position = position;
            MainCamera.pivot = pivot;
        }

        public static void SetCameraLimits(float minX, float maxX, float minY, float maxY)
        {
            limits.MinX = minX;
            limits.MaxX = maxX;
            limits.MinY = minY;
            limits.MaxY = maxY;
        }

        public static void AddCamera(string cameraName, Camera camera = null, float cameraSpeed = 0.0f)
        {
            if (camera == null)
            {
                camera = new Camera(MainCamera.position.X, MainCamera.position.Y);
                camera.pivot = MainCamera.pivot;
            }
            cameras[cameraName] = new Tuple<Camera, float>(camera, cameraSpeed);
        }

        public static Camera GetCamera(string cameraName)
        {
            if (cameras.ContainsKey(cameraName))
            {
                return cameras[cameraName].Item1;
            }
            return null;
        }

        public static bool InsideCameraLimits(Vector2 position)
        {
            return position.X >= limits.MinX - Game.Win.OrthoWidth * 0.5f
                && position.X <= limits.MaxX + Game.Win.OrthoWidth * 0.5f
                && position.Y >= limits.MinY - Game.Win.OrthoHeight * 0.5f
                && position.Y <= limits.MaxY + Game.Win.OrthoHeight * 0.5f;
        }

        public static void MoveCameras()
        {
            if (target == null) return;
            Vector2 oldCameraPos = MainCamera.position;
            MainCamera.position = Vector2.Lerp(MainCamera.position, target.Transform.Position, Game.DeltaTime * Speed);
            FixPosition();
            Vector2 cameraDelta = MainCamera.position - oldCameraPos;
            foreach (var cam in cameras)
            {
                cam.Value.Item1.position += cameraDelta * cam.Value.Item2;
            }
        }

        private static void FixPosition()
        {
            if (MainCamera.position.Y < limits.MinY)
            {
                MainCamera.position.Y = limits.MinY;
            }
            else if (MainCamera.position.Y > limits.MaxY)
            {
                MainCamera.position.Y = limits.MaxY;
            }
            if (MainCamera.position.X < limits.MinX)
            {
                MainCamera.position.X = limits.MinX;
            }
            else if (MainCamera.position.X > limits.MaxX)
            {
                MainCamera.position.X = limits.MaxX;
            }
        }

        public static void ClearAll()
        {
            cameras.Clear();
        }

    }
}

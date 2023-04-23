﻿using System;
using System.Drawing;

namespace HeightMapLib
{
    public delegate float NoiseExpresion(float f);

    public interface ILandGenerator
    {
        int Seed { get; set; }
        float[,] GenMap(int width, int height);
    }

    public class HeightMap
    {
        int _width, _height;
        NoiseExpresion _expresion;

        ILandGenerator _generator;

        public HeightMap(int width, int height, ILandGenerator lg, NoiseExpresion exp = null)
        {
            Width = width;
            Height = height;
            _generator = lg;
            _expresion = exp;

            GenMap();
        }

        public HeightMap(HeightMap h)
        {
            Width = h.Width;
            Height = h.Height;
            NoiseMap = h.NoiseMap;
            LandGenerator = h.LandGenerator;
            Seed = h.Seed;
        }

        public float[,] NoiseMap { get; private set; }

        public int Width
        {
            get => _width;
            set => _width = value > 0 ? value : throw new Exception("Width is positive value");
        }

        public int Height
        {
            get => _height;
            set => _height = value > 0 ? value : throw new Exception("Height is positive value");
        }

        public int Seed
        {
            get => LandGenerator.Seed;
            set => LandGenerator.Seed = value;
        }

        ILandGenerator LandGenerator
        {
            get => _generator;
            set
            {
                _generator = value;
                GenMap();
            }
        }

        NoiseExpresion NoiseExpression
        {
            get => _expresion;
            set
            {
                _expresion = value;
                GenMap();
            }
        }

        private void GenMap()
        {
            NoiseMap = LandGenerator.GenMap(Width, Height);

            if (_expresion != null)
                for (var i = 0; i < _width; i++)
                    for (var j = 0; j < _height; j++)
                        NoiseMap[i, j] = _expresion(NoiseMap[i, j]);
        }

        public float this[int i, int j]
        {
            get => NoiseMap[i, j];
            set => NoiseMap[i, j] = value;
        }

        public static HeightMap Add(HeightMap h1, HeightMap h2)
        {
            var h_temp = new HeightMap(h1);

            if (h1.Width != h2.Width || h1.Height != h2.Height)
                throw new Exception("Sizes isn't equal");

            for (var i = 0; i < h1.Width; i++)
                for (var j = 0; j < h1.Height; j++)
                    h_temp[i, j] += h2[i, j];

            return h_temp;
        }

        public static HeightMap Subtract(HeightMap h1, HeightMap h2)
        {
            var h_temp = new HeightMap(h1);

            if (h1.Width != h2.Width || h1.Height != h2.Height)
                throw new Exception("Sizes isn't equal");

            for (var i = 0; i < h1.Width; i++)
                for (var j = 0; j < h1.Height; j++)
                    h_temp[i, j] -= h2[i, j];

            return h_temp;
        }

        public static HeightMap MultSingle(HeightMap h, float val)
        {
            var h_temp = new HeightMap(h);

            for (var i = 0; i < h.Width; i++)
                for (var j = 0; j < h.Height; j++)
                    h_temp[i, j] *= val;

            return h_temp;
        }

        public static HeightMap operator +(HeightMap h1, HeightMap h2) => Add(h1, h2);
        public static HeightMap operator -(HeightMap h1, HeightMap h2) => Subtract(h1, h2);
        public static HeightMap operator *(HeightMap h, float val) => MultSingle(h, val);

        private (float, float) MinMax()
        {
            var max = NoiseMap[0, 0];
            var min = max;

            for (var i = 0; i < _width; i++)
                for (var j = 0; j < _height; j++)
                {
                    min = NoiseMap[i, j] < min ? NoiseMap[i, j] : min;
                    max = NoiseMap[i, j] > max ? NoiseMap[i, j] : max;
                }

            return (min, max);
        }

        public void Normalize()
        {
            var (h_min, h_max) = MinMax();
            var delta = h_max - h_min;

            for (var i = 0; i < Width; i++)
                for (var j = 0; j < Height; j++)
                    NoiseMap[i, j] = (NoiseMap[i, j] - h_min) / delta;
        }

        //public void SaveToBmp(string path, string name)
        //{
        //    var hm = new byte[_width, _height];
        //    var (h_min, h_max) = MinMax();
        //    var delta = h_max - h_min;

        //    for (int i = 0; i < Width; i++)
        //        for (int j = 0; j < Height; j++)
        //            hm[i, j] = (byte)((NoiseMap[i, j] - h_min) / delta * 255);

        //    var bmp = new System.Drawing.Bitmap(_width, _height);
        //    for (int i = 0; i < _width; i++)
        //        for (int j = 0; j < _height; j++)
        //            bmp.SetPixel(i, j, Color.FromArgb(hm[i, j], hm[i, j], hm[i, j]));

        //    bmp.Save(path + name + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
        //}
    }
}
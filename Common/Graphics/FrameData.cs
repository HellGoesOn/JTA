﻿using Microsoft.Xna.Framework;

namespace JTA.Common.Graphics
{
    public class FrameData(int x, int y, int width, int height, int timePerFrame)
    {
        public int x = x;
        public int y = y;
        public int width = width;
        public int height = height;
        public int timePerFrame = timePerFrame;

        public Rectangle AsRect() => new(x, y, width, height);
    }
}

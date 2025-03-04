using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JTA.Extensions
{
    public static class Vector2Ext
    {
        public static Vector2 Floor(this Vector2 v)
        {
            return new Vector2((int)Math.Floor(v.X), (int)Math.Floor(v.Y));
        }
    }
}

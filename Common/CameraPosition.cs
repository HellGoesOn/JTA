using Microsoft.Xna.Framework;

namespace JTA.Common
{
    public class CameraPosition
    {
        public int time;
        public int maxTime = 60;
        public Vector2 targetPosition;

        public virtual void Update()
        {
            if (time < maxTime)
                time++;
        }

        public float Progress => (float)time / (float)maxTime;
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace JTA.Common.Graphics
{
    public class SpriteAnimation
    {
        public int time;
        public int currentFrame;
        public bool looping;
        public bool finished;
        public string textureName;
        public Asset<Texture2D> texture;
        public List<FrameData> frames;

        public AnimationEvent onAnimationEnd;
        public AnimationEvent onAnimationPlay;

        public SpriteAnimation(string texture, bool looping = true)
        {
            this.looping = looping;
            frames = [];
            this.textureName = texture;
            if(Main.netMode != NetmodeID.Server) {
                this.texture = ModContent.Request<Texture2D>(textureName);
            }
        }

        public void Update()
        {
            onAnimationPlay?.Invoke(this);

            var frame = frames[currentFrame];

            if (++time >= frame.timePerFrame) {
                if(++currentFrame >= frames.Count - 1) {
                    finished = true;
                    onAnimationEnd?.Invoke(this);
                    if (looping)
                        currentFrame = 0;
                    else
                        currentFrame = frames.Count - 1;
                }

                time = 0;
            }
        }

        public void Draw(SpriteBatch sb, Texture2D texture, Vector2 position, Color color, float rotation = 0, Vector2? scale = null, Vector2? origin = null, Vector2? frameOffset = null, SpriteEffects sfx = SpriteEffects.None)
        {
            var frame = frames[currentFrame];
            var fr = frame.AsRect();
            if (frameOffset != null) {
                fr = new Rectangle(frame.x + (int)frameOffset.Value.X, frame.y + (int)frameOffset.Value.Y, frame.width, frame.height);
            }

            var finalPos = new Vector2((int)position.X, (int)position.Y);

            if (sb != null) {
                sb.Draw(texture, finalPos, fr, color, rotation, origin ?? new Vector2(frame.width, frame.height) * 0.5f, scale ?? Vector2.One, sfx, 0f);
                return;
            }

            Main.EntitySpriteDraw(texture, finalPos, fr, color, rotation, origin ?? new Vector2(frame.width, frame.height) * 0.5f, scale ?? Vector2.One, sfx);
        }

        public void Reset()
        {
            time = 0;
            currentFrame = 0;
            finished = false;
        }

        /// <summary>
        /// Adds simple frames to an animation
        /// </summary>
        /// <param name="frameCount">Number of frames added</param>
        /// <param name="width">Width of a single frame</param>
        /// <param name="height">Height of a single frame</param>
        /// <param name="frameTime">Animation speed</param>
        /// <param name="style">Allows you to specify of animation is layed out vertically or horizontally. Don't set this parameter unless you know what you are doing.</param>
        /// <returns>Same instance of SpriteAnimation, allowing you to chain commands</returns>
        public SpriteAnimation FillFrames(int frameCount, int width, int height, int frameTime, FrameFillStyle style = FrameFillStyle.Vertical)
        {
            var x = 0;
            var y = 0;

            switch(style) {
                case FrameFillStyle.Vertical:
                    y = height;
                    break;
                    case FrameFillStyle.Horizontal:
                    x = width;
                    break;
            }

            for (int i = 0; i <= frameCount; i++) {
                frames.Add(new(x * i, y * i, width, height, frameTime));
            }

            return this;
        }

        public SpriteAnimation SetLoop(bool  loop)
        {
            this.looping = loop;
            return this;
        }

        /// <summary>
        /// Allows you to set animation speed for specific frames
        /// </summary>
        /// <param name="startIndex">Index of first frame for speed adjustment</param>
        /// <param name="newSpeed">New speed</param>
        /// <param name="endIndex">End index. If not set, will adjust speed for all frames starting from startIndex</param>
        /// <returns>Same instance of SpriteAnimation, allowing you to chain commands</returns>
        public SpriteAnimation SetSpeed(int startIndex, int newSpeed, int endIndex = -1)
        {
            if(endIndex == -1) 
                endIndex = frames.Count - 1;
            for(int i = startIndex; i < endIndex; i++) {
                frames[i].timePerFrame = newSpeed;
            }

            return this;
        }
    }

    public enum FrameFillStyle
    {
        Horizontal,
        Vertical,
    }

    public delegate void AnimationEvent(SpriteAnimation animation);
}

using JTA.Common.Stands;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace JTA.Common.UI
{
    public class DraggablePanel : UIPanel
    {
        public Vector2 minPos;
        public Vector2 maxPos;
        public DragStyle style;

        bool dragging;

        Vector2 dragPosition;

        public override void LeftMouseDown(UIMouseEvent evt)
        {
            base.LeftMouseDown(evt);
            dragging = true;

            switch (style) {
                case DragStyle.Horizontal:
                    dragPosition = new Vector2(Main.mouseX - Left.Pixels, Top.Pixels);
                    break;
                case DragStyle.Vertical:
                    dragPosition = new Vector2(Left.Pixels, Main.mouseY-Top.Pixels);
                    break;
                case DragStyle.Omnidirectional:
                default:
                    dragPosition = new Vector2(Main.mouseX - Left.Pixels, Main.mouseY - Top.Pixels);
                    break;
            }
        }

        public override void LeftMouseUp(UIMouseEvent evt)
        {
            base.LeftMouseUp(evt);
            dragging = false;
            dragPosition = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ContainsPoint(Main.MouseScreen)) {
                Main.LocalPlayer.mouseInterface = true;
            }

            if (dragging) {
                var xoff = 0;
                var yoff = 0;

                switch (style) 
                    {
                    case DragStyle.Horizontal:
                        xoff = Main.mouseX;
                        break;
                        case DragStyle.Vertical:
                        yoff = Main.mouseY; break;
                    case DragStyle.Omnidirectional:
                    default:
                        xoff = Main.mouseX;
                        yoff = Main.mouseY;
                        break;
                }

                Left.Set(xoff - dragPosition.X, 0);
                Top.Set(yoff - dragPosition.Y, 0);

                Recalculate();
            }
        }

        public enum DragStyle
        {
            Horizontal,
            Vertical,
            Omnidirectional
        }
    }
}

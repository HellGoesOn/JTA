//using JTA.Common.Stands;
//using Microsoft.Xna.Framework.Graphics;
//using ReLogic.Content;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using Terraria.GameContent.UI.Elements;
//using Terraria.UI;

//namespace JTA.Common.UI
//{
//    public class PerkCollection : UIElement
//    {
//        List<UIPerk> perks = [];

//        public PerkCollection(List<StandPerk> perks)
//        {
//            float[] alignments = [0.12f, 0.5f, 0.88f];

//            foreach (var perk in perks) {
//                var perkVisual = new UIPerk(perk);

//                perkVisual.position = new Vector2(0, 0);
//                perkVisual.size = new Vector2(64);
//            }

//            foreach (var perk in this.perks) {
//                var tryFind = this.perks.FirstOrDefault(x => x.id )
//                perk.next = 
//            }
//        }

//        public override void Draw(SpriteBatch spriteBatch)
//        {
//            base.Draw(spriteBatch);

//            foreach (var perk in perks) {

//            }
//        }
//    }

//    public class UIPerk
//    {
//        public UIPerk previous;
//        public UIPerk next;
//        public Vector2 position;
//        public Vector2 size;
//        public string description;
//        public string name;
//        public int id;
//        public List<int> requiredPerks;

//        public UIPerk(StandPerk perk)
//        {
//            id = perk.id;
//            name = perk.name;
//            requiredPerks = perk.requiredPerks;
//            description = perk.description;
//            previous = null;
//            next = null;
//        }
//    }
//}

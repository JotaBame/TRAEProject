using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Audio;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using TRAEProject.NewContent.Items.Accesories.MobilityJumps;

namespace TRAEProject.Changes.Accesory
{
    public class GravitationPlayer : ModPlayer
    {
        public bool noFlipGravity = false;
        public override void ResetEffects()
        {
            noFlipGravity = false;
        }
        bool returnantiGrav = false;
        public override bool PreItemCheck()
        {
            if(noFlipGravity && Player.gravDir == -1)
            {
                Player.gravDir = 1;
                returnantiGrav = true;
            }
            return base.PreItemCheck();
        }
        public override void PostItemCheck()
        {
            if(returnantiGrav)
            {
                Player.gravDir = -1;
                returnantiGrav = false;
            }
        }
        public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
        {
            base.ModifyDrawInfo(ref drawInfo);
        }
        public override void PostUpdate()
        {
            if(noFlipGravity && Player.gravDir == -1)
            {
                Player.gravDir = 1;
                returnantiGrav = true;
            }
            base.PostUpdate();
        }
        public override void PreUpdate()
        {
            if(returnantiGrav)
            {
                Player.gravDir = -1;
                returnantiGrav = false;
            }
            base.PreUpdate();
        }
    }
    public class GravitationSystem : ModSystem
    {
    }
}
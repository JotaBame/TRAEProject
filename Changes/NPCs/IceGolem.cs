using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria;

using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.Changes.NPCs
{
	public class Icegolem : GlobalNPC
	{
		public override bool InstancePerEntity => true;
		public override void SetDefaults(NPC npc)
		{
		}
        public override void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
		{
		}
		public override void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)/* tModPorter Note:bossAdjustment -> balance (bossAdjustment is different, see the docs for details) */
		{
		}
		public override void AI(NPC npc)
		{
			if (npc.type == NPCID.IceGolem)
			{
				NPCAimedTarget targetData = npc.GetTargetData(); 
				Vector2 newMove = npc.Center - Main.player[npc.target].Center;
		
				float distanceTo = MathF.Sqrt(newMove.X * newMove.X + newMove.Y * newMove.Y);
				if (distanceTo < 500f)
				{
					npc.ai[1] += 1f;
					if (npc.ai[1] > 15)
					{
						MakeSpikesForward(npc, 1, targetData);
						npc.netUpdate = true;
						if (npc.ai[1] > 120)
						{
							npc.ai[1] = 0;
						}
					}
				}
			}

		}
	
		private void MakeSpikesForward(NPC npc, int AISLOT_PhaseCounter, NPCAimedTarget targetData)
		{
			int num = 2;
			int num2 = 5;
			if (Main.netMode == 1)
			{
				return;
			}
			int num3 = num * num2;
			if (!(npc.ai[AISLOT_PhaseCounter] < (float)num3))
			{
				Point sourceTileCoords = npc.Bottom.ToTileCoordinates();
				int num4 = 20;
				int num5 = 1;
				sourceTileCoords.X += npc.direction * 3;
				int num6 = (int)npc.ai[AISLOT_PhaseCounter] - num3;

				int num7 = 4;
				int num8 = num6 / num7 * num7;
				int num9 = num8 + num7;
				if (num6 % num7 != 0)
				{
					num9 = num8;
				}
				for (int i = num8; i < num9 && i < num4; i++)
				{
					int xOffset = i * num5;
					TryMakingSpike(npc, ref sourceTileCoords, npc.direction, num4, i, xOffset);
				}
			}
		}
		private void TryMakingSpike(NPC npc, ref Point sourceTileCoords, int dir, int howMany, int whichOne, int xOffset)
		{
			int num = 13;
			int num2 = sourceTileCoords.X + xOffset * dir;
			int num3 = TryMakingSpike_FindBestY(npc, ref sourceTileCoords, num2);
			if (WorldGen.ActiveAndWalkableTile(num2, num3))
			{
				Vector2 vector = new Vector2(num2 * 16 + 8, num3 * 16 - 8);
				Vector2 vector2 = new Vector2(0f, -1f).RotatedBy((float)(whichOne * dir) * 0.7f * (MathF.PI / 4f / (float)howMany));
				Projectile.NewProjectile(npc.GetSource_FromThis(), vector, vector2, 961, num, 0f, Main.myPlayer, 0f, 0.1f + Main.rand.NextFloat() * 0.1f + (float)xOffset * 1.1f / (float)howMany);
			}
		}
		private int TryMakingSpike_FindBestY(NPC npc, ref Point sourceTileCoords, int x)
		{
			int num = sourceTileCoords.Y;
			NPCAimedTarget targetData = npc.GetTargetData();
			if (!targetData.Invalid)
			{
				Rectangle hitbox = targetData.Hitbox;
				Vector2 vector = new Vector2(hitbox.Center.X, hitbox.Bottom);
				int num2 = (int)(vector.Y / 16f);
				int num3 = Math.Sign(num2 - num);
				int num4 = num2 + num3 * 15;
				int? num5 = null;
				float num6 = float.PositiveInfinity;
				for (int i = num; i != num4; i += num3)
				{
					if (WorldGen.ActiveAndWalkableTile(x, i))
					{
						float num7 = new Point(x, i).ToWorldCoordinates().Distance(vector);
						if (!num5.HasValue || !(num7 >= num6))
						{
							num5 = i;
							num6 = num7;
						}
					}
				}
				if (num5.HasValue)
				{
					num = num5.Value;
				}
			}
			for (int j = 0; j < 20; j++)
			{
				if (num < 10)
				{
					break;
				}
				if (!WorldGen.SolidTile(x, num))
				{
					break;
				}
				num--;
			}
			for (int k = 0; k < 20; k++)
			{
				if (num > Main.maxTilesY - 10)
				{
					break;
				}
				if (WorldGen.ActiveAndWalkableTile(x, num))
				{
					break;
				}
				num++;
			}
			return num;
		}
	}
	public class IceBeamSlower : GlobalProjectile
	{
		public override bool InstancePerEntity => true;

        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.type == ProjectileID.FrostBeam)
            {
				projectile.tileCollide = false;
            }
        }
        public override void OnHitPlayer(Projectile projectile, Player target, Player.HurtInfo info)
        {
			if(projectile.type == ProjectileID.FrostBeam)
			{
				target.ClearBuff(BuffID.Chilled);
				target.AddBuff(BuffID.Chilled, 240);
				target.AddBuff(BuffID.Frozen, 30);
			}
		}
    }
}
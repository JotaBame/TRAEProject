using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
 
using System;
using System.Collections.Generic;
using Terraria;
 
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Common.ModPlayers;
 
using static Terraria.ModLoader.ModContent;

namespace TRAEProject.NewContent.Items.Armor.EchoHunter;

public partial class EchoHunterRework : EchoHunterSet
{
public override void Load()
    {
        IL_Main.DrawDust += ModifyDrawDust;
    }
public override void Unload()
{
    IL_Main.DrawDust -= ModifyDrawDust;
}
private void ModifyDrawDust(ILContext il)
{
    Mod.Logger.Info("[EchoHunter] ModifyDrawDust WAS CALLED.");
    ILCursor c = new ILCursor(il);

    if (!c.TryGotoNext(MoveType.After, i => i.MatchCallvirt<SpriteBatch>("Begin")))
    {
        Mod.Logger.Error("Echo Hunter: Could not find SpriteBatch.Begin in Main.DrawDust.");
        return;
    }

    // Draw obstruction immediately after Begin()
    c.EmitDelegate(() =>
    {
        if (Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterModeActiveFrames > 0)
        {

            DrawEchoHunterDarkness(Main.spriteBatch, Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterModeActiveFrames);
        }
    });

    // =========================================================
    // Find Dust.active
    // =========================================================

    c.Index = 0;

    if (!c.TryGotoNext(MoveType.Before, i => i.MatchLdfld<Dust>(nameof(Dust.active))))
    {
        Mod.Logger.Error("[EchoHunter] Could not find Dust.active.");
        return;
    }

    Mod.Logger.Info($"[EchoHunter] Found Dust.active at {c.Index}.");

    c.Remove();
    c.EmitDelegate<Func<Dust, bool>>(ShouldDrawEchoHunterDust);
}
private bool ShouldDrawEchoHunterDust(Dust dust)
{
    // Preserve vanilla behavior when Echo Hunter isn't active
    if (!Main.LocalPlayer.active)
        return dust.active;

        // this is my current modPlayer, replace it with whatever modPlayer you're using for this
        EchoHunterSet modPlayer = Main.LocalPlayer.GetModPlayer<EchoHunterSet>();

    if (!modPlayer.EchoHunterMode)
        return dust.active;

    // only let echolocation particles through
    if (!dust.active)
        return false;

    return dust.type == DustID.Clentaminator_Purple || dust.type == DustID.Clentaminator_Green;
}
private void DrawScreenObstruction(On_ScreenObstruction.orig_Draw orig, SpriteBatch spriteBatch)
{
    if (Main.LocalPlayer.active && Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterMode)
    {
        return;
    }
    orig(spriteBatch);
}
public static void DrawEchoHunterDarkness(SpriteBatch spriteBatch, int progress)
{
    // this is effectively just the normal Obstructed layer code, but reimplemented so we can place it pre-drawDust
 

    Player player = Main.LocalPlayer;

    float obstruction = 0.92f * (float)((float)progress /  player.GetModPlayer<EchoHunterSet>().EchoHunterModeFramesToDarkenScreen );
      
    Color color = Color.Black * obstruction;
    int num1 = TextureAssets.Extra[49].Width();
    int num2 = 10;
    Rectangle rect = player.getRect();
    rect.Inflate((num1 - rect.Width) / 2, (num1 - rect.Height) / 2 + num2 / 2);
    rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)player.gfxOffY - num2);
    Rectangle destinationRectangle1 = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
    Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
    Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
    Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
    spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle1, new Rectangle(0, 0, 1, 1), color);
    spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, new Rectangle(0, 0, 1, 1), color);
    spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, new Rectangle(0, 0, 1, 1), color);
    spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, new Rectangle(0, 0, 1, 1), color);
    spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
}
public override void PostUpdate()
{
    if (Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterSetBonus)
    {
        if (EchoHunterMode)
        {
                Main.LocalPlayer.GetModPlayer<CritDamage>().critDamage += 0.2f;

            // lmao
            // Fine, i'll keep it -bame
            if (Main.rand.NextBool(3000000))
            {
                CombatText.NewText(Player.Hitbox, Color.Red, "GET HIS ASS, DAREDEVIL!");
            }
        }
    }
    else
    {
        EchoHunterMode = false;
    }
}
public class EchoHunterGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;
    public int EchoMarker = -1;
    public override void PostAI(NPC npc)
    {
        if (!Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterMode)
        {
            EchoMarker = -1;
            return;
        }
        if (!npc.active || !npc.chaseable && !npc.friendly)
        {
            return;
        }
        // Prevent duplicates
        if (EchoMarker >= 0 && EchoMarker < Main.maxProjectiles && Main.projectile[EchoMarker].active)
            return;

        EchoMarker = EchoHunterPing.AttachToNPC(npc);
    }
}
public class EchoSenseGlobalProjectile : GlobalProjectile
{
    public override bool InstancePerEntity => true;
    public int EchoMarker = -1;
    public override void PostAI(Projectile projectile)
    {
        if (!Main.LocalPlayer.GetModPlayer<EchoHunterSet    >().EchoHunterMode)
        {
            EchoMarker = -1;
            return;
        }

        if (!projectile.active || projectile.type == ProjectileType<EchoHunterPing>())
            return;

        if (!projectile.hostile && !projectile.friendly)
            return;

        if (projectile.friendly && !projectile.hostile)
            return;

        // Prevent duplicates
        if (EchoMarker >= 0 && EchoMarker < Main.maxProjectiles && Main.projectile[EchoMarker].active)
            return;

        EchoMarker = EchoHunterPing.AttachToProjectile(projectile);
    }
}
public class EchoHunterPing : ModProjectile
{
    private Vector2 EchoDustLastPosition;
    private float EchoDustDistance;
    private ulong EchoDustLastSpawn;
    private readonly List<int> EchoDustIndices = new();
    public override string Texture => "Terraria/Images/Projectile_0";

    // ai[0] = target index
    // ai[1] = target type
    //
    // 0 = NPC
    // 1 = Projectile
    private const int NPCType = 0;
    private const int ProjectileType = 1;

    public override void SetDefaults()
    {
        Projectile.width = 16;
        Projectile.height = 16;
        Projectile.aiStyle = -1;
        Projectile.friendly = false;
        Projectile.hostile = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 2;
        Projectile.tileCollide = false;
        Projectile.hide = true;
    }
    public override void AI()
    {
        int targetIndex = (int)Projectile.ai[0];
        int targetType = (int)Projectile.ai[1];

        if (targetType == NPCType)
        {
            FollowNPC(targetIndex);
        }
        else if (targetType == ProjectileType)
        {
            FollowProjectile(targetIndex);
        }
        else
        {
            Projectile.Kill();
        }

        if (!Main.LocalPlayer.GetModPlayer<EchoHunterSet>().EchoHunterMode)
        {
            Projectile.Kill();
        }
    }
    private void FollowNPC(int index)
    {
        if (index < 0 || index >= Main.maxNPCs)
        {
            Projectile.Kill();
            return;
        }

        NPC target = Main.npc[index];

        if (!target.active)
        {
            Projectile.Kill();
            return;
        }

        Projectile.Center = target.Center;
        Projectile.timeLeft = 2;

        float size = GetNPCSize(target);

        SpawnEchoDust(size, target.chaseable, target.friendly, target.velocity.X, target.velocity.Y);
    }
    private void FollowProjectile(int index)
    {
        if (index < 0 || index >= Main.maxProjectiles)
        {
            Projectile.Kill();
            return;
        }

        Projectile target = Main.projectile[index];

        if (!target.active || target == Projectile)
        {
            Projectile.Kill();
            return;
        }

        Projectile.Center = target.Center;
        Projectile.timeLeft = 2;

        float size = GetProjectileSize(target);
        SpawnEchoDust(size, target.hostile, target.friendly, 0, 0);
    }
    private float GetNPCSize(NPC npc)
    {
        float targetSize = Math.Max(npc.width, npc.height);
        return MathHelper.Clamp(targetSize / 60f, 0.25f, 4f);
    }
    private float GetProjectileSize(Projectile projectile)
    {
        float targetSize = Math.Max(projectile.width, projectile.height);

        return MathHelper.Clamp(targetSize / 32f, 0.25f, 3f);
    }
    private void SpawnEchoDust(float size, bool hostile, bool friendly, float SpeedX, float SpeedY)
    {
        Vector2 currentPosition = Projectile.Center;

        float distanceMoved = Vector2.Distance(
            currentPosition,
            EchoDustLastPosition
        );

        EchoDustDistance += distanceMoved;
        EchoDustLastPosition = currentPosition;

        ulong ticksSinceSpawn = Main.GameUpdateCount - EchoDustLastSpawn;

        const float distancePerBurst = 45f;
        const ulong maximumSpawnInterval = 45;

        if (EchoDustDistance < distancePerBurst &&
            ticksSinceSpawn < maximumSpawnInterval)
        {
            return;
        }

        // At this point either:
        // 1. The target moved far enough, OR
        // 2. Enough time passed

        EchoDustDistance = 0f;
        EchoDustLastSpawn = Main.GameUpdateCount;

        int num = (int)Math.Clamp(size * 8, 4.0, 20.0);

        if (EchoDustIndices.Count > num * 2)
        {
            KillPreviousEchoDust();
            EchoDustIndices.Clear();
        }

        for (int i = 0; i < num; i++)
        {
            float angle = i * MathHelper.TwoPi / num;
            Vector2 velocity = angle.ToRotationVector2() * (5f * size);

            int dustIndex = Dust.NewDust(
                Projectile.Center,
                0,
                0,
                GetEchoDustType(hostile, friendly),
                SpeedX,
                SpeedY,
                0,
                default,
                2f
            );
            EchoDustIndices.Add(dustIndex);

            Dust dust = Main.dust[dustIndex];
            dust.noGravity = true;
            dust.noLight = false;
            dust.velocity = velocity;
        }
    }
    private int GetEchoDustType(bool hostile, bool friendly)
    {
        if (hostile && !friendly)
            return DustID.Clentaminator_Purple;
        if (friendly && !hostile)
            return DustID.Clentaminator_Green;
        else
            return DustID.Clentaminator_Green;
    }
    public static int AttachToNPC(NPC npc)
    {
        if (Main.netMode == NetmodeID.Server)
            return -1;

        return Projectile.NewProjectile(
            Main.LocalPlayer.GetSource_FromThis(),
            npc.Center,
            Vector2.Zero,
            ProjectileType<EchoHunterPing>(),
            0,
            0f,
            Main.myPlayer,
            npc.whoAmI,
            0
        );
    }
    public static int AttachToProjectile(Projectile target)
    {
        if (Main.netMode == NetmodeID.Server)
            return -1;

        return Projectile.NewProjectile(
            Main.LocalPlayer.GetSource_FromThis(),
            target.Center,
            Vector2.Zero,
            ProjectileType<EchoHunterPing>(),
            0,
            0f,
            Main.myPlayer,
            target.whoAmI,
            1
        );
    }
    private void KillPreviousEchoDust()
    {
        foreach (int index in EchoDustIndices)
        {
            if (index >= 0 && index < Main.maxDustToDraw)
            {
                Main.dust[index].active = false;
            }
        }
        EchoDustIndices.Clear();
    }
    public override bool? CanCutTiles()
    {
        return false;
    }
    public override bool? CanDamage()
    {
        return false;
    }

}
}
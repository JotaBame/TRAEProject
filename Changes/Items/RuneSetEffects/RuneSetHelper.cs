using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;

namespace TRAEProject.Changes.Items.RuneSetEffects
{
    public class RuneSetHelper
    {
        public static bool ValidIndexedTarget(int targetIndex, Projectile proj, out NPC target, bool includeImmuneNPCs = true)
        {
            if (targetIndex < 0)
            {
                target = null;
                return false;
            }
            if (targetIndex >= Main.maxNPCs)
            {
                target = null;
                return false;
            }
            bool invalid = !ValidHomingTarget(Main.npc[targetIndex], proj, includeImmuneNPCs);
            target = invalid ? null : Main.npc[targetIndex];
            return !invalid;

        }
        public static bool ValidHomingTarget(NPC npc, Projectile proj, bool includeImmuneNPCs = true)
        {
            bool npcImmuneToProj = false;
            if (!includeImmuneNPCs)
            {
                if (proj.usesLocalNPCImmunity)
                {
                    npcImmuneToProj = proj.localNPCImmunity[npc.whoAmI] != 0;

                }
                else if (proj.usesIDStaticNPCImmunity)
                {
                    npcImmuneToProj = Projectile.perIDStaticNPCImmunity[proj.type][npc.type] != 0;
                }
                else
                {
                    npcImmuneToProj = npc.immune[proj.owner] != 0;
                }
            }//eol becomes invincible during dash, phasde transition and spawn animation, so ignore donttake damage if EoL to avoid some weirdness
            //      ai0 as 8 or 9 is if she's dashng, so that she doesn't get targeted during the phase transition or spawn animation
            return npc.CanBeChasedBy(null, npc.type == NPCID.HallowBoss && (npc.ai[0] == 8 || npc.ai[0] == 9)) && (includeImmuneNPCs || !npcImmuneToProj);
        }
        public static void Homing(Projectile proj, float maxVel, ref int targetIndex, ref float homingTimer, float maxHomingStrength = .1f, float range = 1500)
        {
            float targetIndexFloat = targetIndex;
            Homing(proj, maxVel, ref targetIndexFloat, ref homingTimer, maxHomingStrength, range);
            targetIndex = (int)targetIndexFloat;
        }
        public static void Homing(Projectile proj, float maxVel, ref float targetIndex, ref float homingTimer, float maxHomingStrength = .1f, float range = 1500)
        {
            float rangeSQ = range * range;
            if (!ValidIndexedTarget((int)targetIndex, proj, out _))
            {
                proj.velocity = Vector2.Lerp(proj.velocity, Vector2.Normalize(proj.velocity) * maxVel, 0.1f);
                int closestNPC = -1;
                Vector2 center = proj.Center;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC potentialTarget = Main.npc[i];
                    float distToClosestPointInPotentialTargetHitbox = center.DistanceSQ(potentialTarget.Hitbox.ClosestPointInRect(center));
                    bool notValidTarget = !ValidHomingTarget(potentialTarget, proj);
                    if (notValidTarget || distToClosestPointInPotentialTargetHitbox > rangeSQ)
                        continue;
                    if (!Main.npc.IndexInRange(closestNPC) || center.DistanceSQ(potentialTarget.Hitbox.ClosestPointInRect(center)) < center.DistanceSQ(Main.npc[closestNPC].Hitbox.ClosestPointInRect(center)))
                        closestNPC = i;
                }
                targetIndex = closestNPC;
                homingTimer = 1;
            }
            if (ValidIndexedTarget((int)targetIndex, proj, out NPC target))
            {
                homingTimer++;
                float homingStrength = RemapEased(homingTimer, 1, 20, 0, maxHomingStrength);
                proj.velocity = Vector2.Lerp(proj.velocity, Vector2.Normalize(target.Center - proj.Center) * maxVel, homingStrength);
            }
        }
        public static int FindHomingTarget(Projectile proj, float maxVel, ref int targetIndex, ref float homingTimer, float range = 1500)
        {
            float rangeSQ = range * range;
            if (!ValidIndexedTarget((int)targetIndex, proj, out _))
            {
                int closestNPC = -1;
                Vector2 center = proj.Center;
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC potentialTarget = Main.npc[i];
                    float distToClosestPointInPotentialTargetHitbox = center.DistanceSQ(potentialTarget.Hitbox.ClosestPointInRect(center));
                    bool notValidTarget = !ValidHomingTarget(potentialTarget, proj);
                    if (notValidTarget || distToClosestPointInPotentialTargetHitbox > rangeSQ)
                        continue;
                    if (!Main.npc.IndexInRange(closestNPC) || center.DistanceSQ(potentialTarget.Hitbox.ClosestPointInRect(center)) < center.DistanceSQ(Main.npc[closestNPC].Hitbox.ClosestPointInRect(center)))
                        closestNPC = i;
                }
                targetIndex = closestNPC;
                homingTimer = 1;
            }
            return targetIndex;
        }
        public static float EaseInOutSine(float progress)
        {
            return -(MathF.Cos(MathF.PI * progress) - 1) / 2;
        }
        public static float RemapEased(float fromValue, float fromMin, float fromMax, float toMin, float toMax)
        {
            return MathHelper.Lerp(toMin, toMax, EaseInOutSine(Utils.GetLerpValue(fromMin, fromMax, fromValue, true)));
        }
        public static Vector2 GetDoubledPositioningOffset(float helixTimer, float rotation, Vector2 center, float helixSpeed, float helixWidth)
        {
            Vector2 sineOff = (rotation + MathF.PI * .5f).ToRotationVector2() * (MathF.Cos(helixTimer * helixSpeed) * helixWidth);
            return sineOff;
        }
        public static void GetDoubledPosition(float helixTimer, float rotation, Vector2 center, float helixSpeed, float helixWidth, out Vector2 pos1, out Vector2 pos2)
        {
            Vector2 sineOff = rotation.ToRotationVector2() * new Vector2(MathF.Sin(helixTimer * helixSpeed) * helixWidth, 0);
            pos1 = center + sineOff;
            pos2 = center - sineOff;
        }
        public enum RuneEffects : int
        {
            Homing = 1,
            FastThenSlow = 2,
            CanBounce = 4,
            AoEExplosion = 8,
            SwirlTwin1 = 16,
            ArPenSpread = 32,
            BiggerHitbox = 64,
            SwirlTwin2 = 128

        }
        public static bool IsHoming(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 1) == 1;
        }
        public static bool IsFastThenSlow(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 2) == 2;
        }
        public static bool CanBounce(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 4) == 4;
        }
        public static bool AoEExplosion(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 8) == 8;
        }
        public static bool IsSwirlTwins(float ai2, out int twinSign)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            if ((code & 16) == 16)
            {
                twinSign = 1;
                return true;
            }
            else if ((code & 128) == 128)
            {
                twinSign = -1;
                return true;
            }
            twinSign = 0;
            return false;
        }
        /// <summary>
        /// ruby staff effect, shoots proj twice
        /// </summary>
        public static bool ArPenSpread(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 32) == 32;
        }
        public static bool BiggerHitbox(float ai2)
        {
            int code = BitConverter.SingleToInt32Bits(ai2);
            return (code & 64) == 64;
        }
        public static void SetUpAI2ExcludeBounce(Player player, ref float ai2, out bool doubleFire, out float secondAi2, out float damageMult, out float secondProjVelMult, out int extraFlatDamage, out float globalVeloityMult)
        {
            int newRuneEffect = RuneSetPlayer.IncreaseRuneEffectCounter(player);
            int[] runeEffectCodes = [1, 2, 4, 8, 16, 32, 64];
            int code = runeEffectCodes[newRuneEffect];
            if (code == (int)RuneEffects.CanBounce)
            {
                code = runeEffectCodes[RuneSetPlayer.IncreaseRuneEffectCounter(player)];
            }
            ai2 = BitConverter.Int32BitsToSingle(code);
            secondAi2 = 0;
            doubleFire = false;
            damageMult = 1f;
            extraFlatDamage = 0;
            secondProjVelMult = 1f;
            globalVeloityMult = 1f;
            if (code == (int)RuneEffects.SwirlTwin1)
            {
                doubleFire = true;
                secondAi2 = BitConverter.Int32BitsToSingle((int)RuneEffects.SwirlTwin2);
                damageMult = .5f;
                extraFlatDamage += 10;
            }
            if (code == (int)RuneEffects.FastThenSlow)
            {
                //globalVeloityMult = 2f;
            }
            if (code == (int)RuneEffects.ArPenSpread)
            {
                doubleFire = true;
                secondAi2 = ai2;
                damageMult = .5f;
                extraFlatDamage += 10;
                secondProjVelMult = .75f;
            }
        }
        public static void SetUpAI2(Player player, Item weaponItem, ref float ai2, out bool doubleFire, out float secondAi2, out float damageMult, out float secondProjVelMult, out int extraFlatDamage, out float globalVeloityMult)
        {
            int newRuneEffect = RuneSetPlayer.IncreaseRuneEffectCounter(player);
            int code = GetRuneEffectCode(player, weaponItem);
            ai2 = BitConverter.Int32BitsToSingle(code);
            secondAi2 = 0;
            doubleFire = false;
            damageMult = 1f;
            extraFlatDamage = 0;
            secondProjVelMult = 1f;
            globalVeloityMult = 1f;
            if (code == (int)RuneEffects.SwirlTwin1)
            {
                doubleFire = true;
                secondAi2 = BitConverter.Int32BitsToSingle((int)RuneEffects.SwirlTwin2);
                damageMult = .5f;
                extraFlatDamage += 10;
            }
            if (code == (int)RuneEffects.FastThenSlow)
            {
                globalVeloityMult = 2f;
            }
            if (code == (int)RuneEffects.ArPenSpread)
            {
                //doubleFire = true;
                secondAi2 = ai2;
                damageMult = .5f;
                extraFlatDamage += 10;
                secondProjVelMult = .75f;
            }
        }
        public static int FindItemInvSlot(Player player, Item item)
        {
            for (int i = 0; i < player.inventory.Length; i++)
            {
                if (player.inventory[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }
        static int GetRuneEffectCode(Player player, Item item)
        {
            int curItemSlot = FindItemInvSlot (player, item);
            FindClosestStaffToSlot(player, curItemSlot, out int closestGemStaffID);
            return closestGemStaffID switch
            {
                ItemID.AmethystStaff => (int)RuneEffects.Homing,
                ItemID.TopazStaff => (int)RuneEffects.AoEExplosion,
                ItemID.EmeraldStaff => (int)RuneEffects.FastThenSlow,
                ItemID.SapphireStaff => (int)RuneEffects.SwirlTwin1,
                ItemID.RubyStaff => (int)RuneEffects.ArPenSpread,
                ItemID.DiamondStaff => (int)RuneEffects.BiggerHitbox,
                ItemID.AmberStaff => (int)RuneEffects.CanBounce,
                //arbitrary failsafe
                _ => (int)RuneEffects.CanBounce,
            };
        }
        public static int FindClosestStaffToSlot(Player player, int targetSlot, out int closestGemStaffID)
        {
            int invWidth = 10;
            int targetX = targetSlot % invWidth;
            int targetY = targetSlot / invWidth;
            int closestX = 999999;
            int closestY = 999999;
            closestGemStaffID = 0;
            int closestGemStaffSlot = -1;
            int[] gemStaffInvSlots = FindGemStaffsInInv(player, out int[] gemStaffIDs);
            if (gemStaffIDs.Length <= 0)
            {
                return 0;
            }
            for (int i = 0; i < gemStaffInvSlots.Length; i++)
            {
                InvSlotToXY(gemStaffInvSlots[i], out int possiblyNewClosestX, out int possiblyNewClosestY);

                if (closestGemStaffSlot == -1 || IsCloser(targetX, targetY, closestX, closestY, possiblyNewClosestX, possiblyNewClosestY))
                {
                    closestX = possiblyNewClosestX;
                    closestY = possiblyNewClosestY;
                    closestGemStaffID = gemStaffIDs[i];
                    closestGemStaffSlot = gemStaffInvSlots[i];
                }
            }
            return closestGemStaffSlot;
        }

        private static bool IsCloser(int targetX, int targetY, int closestX, int closestY, int possiblyNewClosestX, int possiblyNewClosestY)
        {
            int deltaClosestX = targetX - closestX;
            int deltaClosestY = targetY - closestY;
            int deltaPossiblyNewClosestX = targetX - possiblyNewClosestX;
            int deltaPossiblyNewClosestY = targetY - possiblyNewClosestY;

            return deltaPossiblyNewClosestX * deltaPossiblyNewClosestX + deltaPossiblyNewClosestY * deltaPossiblyNewClosestY
                < deltaClosestX * deltaClosestX + deltaClosestY * deltaClosestY;
        }

        static void InvSlotToXY(int slot, out int X, out int Y)
        {
            int invWidth = 10;
            X = slot % invWidth;
            Y = slot / invWidth;
        }
        /// <returns>inventory slots of every gem staff in the player's inventory</returns>
        static int[] FindGemStaffsInInv(Player player, out int[] gemStaffItemIDs)
        {
            List<int> invSlots = new();
            List<int> itemIDs = new();
            //don't need to check coin slots or ammo slots
            int[] staffIds = [ItemID.AmethystStaff, ItemID.TopazStaff, ItemID.EmeraldStaff, ItemID.SapphireStaff, ItemID.RubyStaff, ItemID.DiamondStaff, ItemID.AmberStaff];
            for (int i = 0; i < 50; i++)
            {
                Item invItem = player.inventory[i];
                if (staffIds.Contains(invItem.type))
                {
                    invSlots.Add(i);
                    itemIDs.Add(invItem.type);
                }
            }
            gemStaffItemIDs = itemIDs.ToArray();
            return invSlots.ToArray();
        }

        public static float DisableExplosiveness(float ai2)
        {
            return 0;
        }

        public static bool DoubleShootRate(Player player, Item itemBeingUsed)
        {
            int invSlot = FindItemInvSlot(player, itemBeingUsed);
            FindClosestStaffToSlot(player, invSlot, out int closestGemStaffID);
            if (closestGemStaffID == ItemID.RubyStaff)
            {
                return true;
            }
            return false;
        }
    }
}

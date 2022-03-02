using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TRAEProject.Items.Accesories.PalladiumShield;
using static Terraria.ModLoader.ModContent;
public class ChangesWorld : ModSystem
{
    public static int[] PyramidItems;
    public static int[] SecondaryPyramidItems;
    public static int[] ShadowItems;
    public override void PostSetupContent()
    {
        PyramidItems = new int[] { ItemID.SandstorminaBottle, ItemID.FlyingCarpet, ItemID.AnkhCharm, ItemID.AncientChisel, ItemID.SandBoots, ItemID.ThunderSpear, ItemID.ThunderStaff};
        SecondaryPyramidItems = new int[] { ItemID.CatBast, ItemID.MagicConch, ItemID.MysticCoilSnake };
        ShadowItems = new int[] { ItemID.HellwingBow, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.Sunfury, ItemType<PalladiumShield>() };
    }
    public override void PostWorldGen()
    {
        for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
        {
            Chest chest = Main.chest[chestIndex];
            if (chest != null)
            {
                for (int i = 0; i < 40; i++)
                {
                    if (chest.item[i].type == ItemID.Dynamite)
                    {
                        chest.item[i].stack = Main.rand.Next(5, 8);
                        break;
                    }
                }
                if (chest.item[0].type == ItemID.TreasureMagnet)
                {
                    chest.item[0].SetDefaults(Main.rand.Next(ShadowItems), false);
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].IsAir)
                        {
                            chest.item[i].SetDefaults(ItemID.TreasureMagnet);
                            break;
                        }
                    }
                }
                if (chest.item[0].type == ItemID.CatBast)
                {
                    chest.item[0].SetDefaults(Main.rand.Next(ShadowItems), false);
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].IsAir)
                        {
                            chest.item[i].SetDefaults(ItemID.TreasureMagnet);
                            break;
                        }
                    }
                }
                if (chest.item[0].type == ItemID.DarkLance)
                {
                    chest.item[0].SetDefaults(ItemType<PalladiumShield>(), false);
                }

                if (WorldGen.genRand.NextBool(2)  && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 0 * 36)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            chest.item[inventoryIndex].SetDefaults(ItemID.FlaskofPoison);
                            break;
                        }
                    }
                }
                if (WorldGen.genRand.NextBool(2) && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].type == ItemID.None)
                        {
                            {
                                chest.item[i].SetDefaults(ItemID.FlaskofFire, false);
                            }
                            break;
                        }
                        if (chest.item[i].type == ItemID.SandstorminaBottle || chest.item[i].type == ItemID.FlyingCarpet)
                        {
                            chest.item[i].SetDefaults(ItemID.PharaohsMask, false);
                            ++i;
                            chest.item[i].SetDefaults(ItemID.PharaohsRobe, false);
                            break;
                        }
                    }
                }
                if (WorldGen.genRand.NextBool(2) && Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 1 * 36)
                {
                    for (int i = 0; i < 40; i++)
                    {
                        if (chest.item[i].type == ItemID.None)
                        {
                            {
                                chest.item[i].SetDefaults(ItemID.FlaskofFire, false);
                            }
                            break;
                        }
                    }
                }
                if (Main.tile[chest.x, chest.y].type == TileID.Containers && Main.tile[chest.x, chest.y].frameX == 11 * 36)
                {
                    if (WorldGen.genRand.NextBool(8))
                    {
                        chest.item[0].SetDefaults(ItemID.FrostStaff, false);
                    }
                }
                if (Main.tile[chest.x, chest.y].type == TileID.Containers2 && Main.tile[chest.x, chest.y].frameX == 10 * 36)
                {
                    int pyramiditem = Main.rand.Next(PyramidItems);
                    chest.item[0].SetDefaults(pyramiditem, false);
                    if (WorldGen.genRand.NextBool(3))
                    {
                        int secondaryitem = Main.rand.Next(SecondaryPyramidItems);
                        chest.item[1].SetDefaults(secondaryitem);
                       break;
                    }

                }
            }
        }
    }
}





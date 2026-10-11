using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class TurboCan : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 22; Item.height = 32;
        Item.useTime = 40; Item.useAnimation = 40; Item.useStyle = ItemUseStyleID.HoldUp;
        Item.rare = ItemRarityID.Orange; Item.UseSound = Mix.Style("boost");
    }

    public override bool? UseItem(Player player)
    {
        player.AddBuff(ModContent.BuffType<TurboBuff>(), 360);
        return true;
    }
}

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class BananaPeel : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 26;
        Item.damage = 28; Item.DamageType = DamageClass.Ranged; Item.knockBack = 3f;
        Item.useTime = 20; Item.useAnimation = 20; Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true; Item.noUseGraphic = true; Item.autoReuse = true;
        Item.UseSound = SoundID.Item1; Item.rare = ItemRarityID.Yellow;
        Item.shoot = ModContent.ProjectileType<BananaPeelProj>(); Item.shootSpeed = 7f;
    }
}

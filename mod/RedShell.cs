using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Sigf.Content;

public class RedShell : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 32; Item.height = 26;
        Item.damage = 36; Item.DamageType = DamageClass.Ranged; Item.knockBack = 5f;
        Item.useTime = 26; Item.useAnimation = 26; Item.useStyle = ItemUseStyleID.Swing;
        Item.noMelee = true; Item.noUseGraphic = true; Item.autoReuse = true;
        Item.UseSound = SoundID.Item61; Item.rare = ItemRarityID.Red;
        Item.shoot = ModContent.ProjectileType<RedShellProj>(); Item.shootSpeed = 9f;
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Sigf.Content;

/// <summary>Draws the kart over the player's legs so the driver sits inside it.</summary>
public class KartLayer : PlayerDrawLayer
{
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Shoes);

    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => !drawInfo.drawPlayer.dead && drawInfo.shadow == 0f;

    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        Player p = drawInfo.drawPlayer;
        var tex = Terraria.GameContent.TextureAssets.Projectile[ModContent.ProjectileType<PlayerKart>()].Value;
        Vector2 pos = p.Bottom + new Vector2(0f, -14f + 2f + p.gfxOffY) - Main.screenPosition;
        pos = new Vector2((int)pos.X, (int)pos.Y);
        float tilt = MathHelper.Clamp(p.velocity.Y * 0.02f, -0.15f, 0.15f) * p.direction;
        Color c = Lighting.GetColor((int)(p.Center.X / 16f), (int)(p.Center.Y / 16f));
        drawInfo.DrawDataCache.Add(new DrawData(tex, pos, null, c, tilt, tex.Size() / 2f, 1f,
            p.direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0));
    }
}

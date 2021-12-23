using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoStepBack
{
    enum TileType
    {
        None,
        Water,
        Sand,
        Ground1lvl,
        Ground2lvl,
        Ground3lvl,
        Ground4lvl,
        Rock1,
        Rock2,
        CHECK
    }

    class Tile : Transformable, Drawable
    {
        public TileType type = TileType.None;

        public const string contentDir = "..\\Content\\Textures\\";

        TileType[] M = { TileType.None, TileType.Water, TileType.Sand, TileType.Ground1lvl, TileType.Ground2lvl, TileType.Ground3lvl, TileType.Ground4lvl, TileType.Rock1, TileType.Rock2, TileType.CHECK };
        public const float offsetX = 78;
        public const float offsetY = 90;

        public Vector2f[] vecDot = new Vector2f[6];

        float rx = 90;
        float ry = 60;

        public int H = 10;

        ConvexShape hexUp;
        ConvexShape hexLeft;
        ConvexShape hexRight;

        public bool isSelectedTile = false;

        public Tile(TileType type)
        {
            this.type = type;

            H = Array.IndexOf(M, this.type) * 10;

            hexUp = new ConvexShape(6);
            double angle = Math.PI/2;
            for (int i = 0; i < 6; i++)
            {
                hexUp.SetPoint((uint) i, new SFML.System.Vector2f((float) (Math.Floor((Math.Cos(angle) * rx))), (float) (Math.Round(Math.Sin(angle) * ry))));
                angle += Math.PI / 3;
                vecDot[i] = hexUp.GetPoint((uint) i);
            }
            hexUp.OutlineThickness = 1.3f;
            hexUp.OutlineColor = new SFML.Graphics.Color(255, 255, 255);

            hexLeft = new ConvexShape(4);
            hexLeft.SetPoint(0, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y));
            hexLeft.SetPoint(1, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y + H));
            hexLeft.SetPoint(2, new SFML.System.Vector2f(hexUp.GetPoint(1).X, hexUp.GetPoint(1).Y + H));
            hexLeft.SetPoint(3, new SFML.System.Vector2f(hexUp.GetPoint(1).X, hexUp.GetPoint(1).Y));
            hexLeft.OutlineThickness = 1.3f;
            hexLeft.OutlineColor = new SFML.Graphics.Color(255, 255, 255);

            hexRight = new ConvexShape(4);
            hexRight.SetPoint(0, new SFML.System.Vector2f(hexUp.GetPoint(5).X, hexUp.GetPoint(5).Y));
            hexRight.SetPoint(1, new SFML.System.Vector2f(hexUp.GetPoint(5).X, hexUp.GetPoint(5).Y + H));
            hexRight.SetPoint(2, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y + H));
            hexRight.SetPoint(3, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y));
            hexRight.OutlineThickness = 1.3f;
            hexRight.OutlineColor = new SFML.Graphics.Color(255, 255, 255);

            switch (type)
            {
                case TileType.Water:
                    hexUp.Texture = new Texture(contentDir + "TextureWater.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureWaterGran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureWaterGran.png");
                    break;
                case TileType.Sand:
                    hexUp.Texture = new Texture(contentDir + "TextureSand.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureSandGran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureSandGran.png");
                    break;
                case TileType.Ground1lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass1.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass1Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass1Gran.png");
                    break;
                case TileType.Ground2lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass2.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass2Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass2Gran.png");
                    break;
                case TileType.Ground3lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass3.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass3Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass3Gran.png");
                    break;
                case TileType.Ground4lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass4.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass4Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass4Gran.png");
                    break;
                case TileType.Rock1:
                    hexUp.Texture = new Texture(contentDir + "TextureRock1.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureRock1Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureRock1Gran.png");
                    break;
                case TileType.Rock2:
                    hexUp.Texture = new Texture(contentDir + "TextureRock2.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureRock2Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureRock2Gran.png");
                    break;
                case TileType.CHECK:
                    hexUp.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    hexLeft.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    hexRight.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    break;
            }

        }

        public void TileCLick()
        {
            isSelectedTile = !isSelectedTile;
            if (isSelectedTile)
            {
                    hexUp.OutlineThickness = 8f;
                    hexLeft.OutlineThickness = 8f;
                    hexRight.OutlineThickness = 8f;
            }
            else
            {
                    hexUp.OutlineThickness = 1.3f;
                    hexLeft.OutlineThickness = 1.3f;
                    hexRight.OutlineThickness = 1.3f;
            }
        }

        public void TilePlusH()
        {
            if (H <= 70)
            {
                H = H + 10;

                TileType type = (TileType) M.GetValue((int) (H/10));

                UpdateTile(type);
            }
        }

        public void TileMinusH()
        {
            if (H >= 20)
            {
                H = H - 10;

                TileType type = (TileType)M.GetValue((int)(H / 10));

                UpdateTile(type);
            }
        }

        public void UpdateTile(TileType type)
        {
            hexLeft.SetPoint(0, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y));
            hexLeft.SetPoint(1, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y + H));
            hexLeft.SetPoint(2, new SFML.System.Vector2f(hexUp.GetPoint(1).X, hexUp.GetPoint(1).Y + H));
            hexLeft.SetPoint(3, new SFML.System.Vector2f(hexUp.GetPoint(1).X, hexUp.GetPoint(1).Y));

            hexRight.SetPoint(0, new SFML.System.Vector2f(hexUp.GetPoint(5).X, hexUp.GetPoint(5).Y));
            hexRight.SetPoint(1, new SFML.System.Vector2f(hexUp.GetPoint(5).X, hexUp.GetPoint(5).Y + H));
            hexRight.SetPoint(2, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y + H));
            hexRight.SetPoint(3, new SFML.System.Vector2f(hexUp.GetPoint(0).X, hexUp.GetPoint(0).Y));

            switch (type)
            {
                case TileType.Water:
                    hexUp.Texture = new Texture(contentDir + "TextureWater.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureWaterGran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureWaterGran.png");
                    break;
                case TileType.Sand:
                    hexUp.Texture = new Texture(contentDir + "TextureSand.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureSandGran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureSandGran.png");
                    break;
                case TileType.Ground1lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass1.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass1Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass1Gran.png");
                    break;
                case TileType.Ground2lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass2.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass2Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass2Gran.png");
                    break;
                case TileType.Ground3lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass3.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass3Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass3Gran.png");
                    break;
                case TileType.Ground4lvl:
                    hexUp.Texture = new Texture(contentDir + "TextureGrass4.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureGrass4Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureGrass4Gran.png");
                    break;
                case TileType.Rock1:
                    hexUp.Texture = new Texture(contentDir + "TextureRock1.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureRock1Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureRock1Gran.png");
                    break;
                case TileType.Rock2:
                    hexUp.Texture = new Texture(contentDir + "TextureRock2.png");
                    hexLeft.Texture = new Texture(contentDir + "TextureRock2Gran.png");
                    hexRight.Texture = new Texture(contentDir + "TextureRock2Gran.png");
                    break;
                case TileType.CHECK:
                    hexUp.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    hexLeft.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    hexRight.FillColor = new SFML.Graphics.Color(184, 10, 10);
                    break;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(hexUp, states);
            target.Draw(hexLeft, states);
            target.Draw(hexRight, states);
        }
    }
}

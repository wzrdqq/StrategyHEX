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
    class Chunk : Transformable, Drawable
    {
        public const int chunkSize = 8;

        Tile[][] tiles;
        TileType[] M = { TileType.None, TileType.Water, TileType.Sand, TileType.Ground1lvl, TileType.Ground2lvl, TileType.Ground3lvl, TileType.Ground4lvl, TileType.Rock1, TileType.Rock2, TileType.CHECK };

        Vector2i chunkPos;

        public Chunk(Vector2i chunkPos)
        {
            this.chunkPos = chunkPos;
            Position = new Vector2f(this.chunkPos.X * chunkSize * 2 * Tile.offsetX, this.chunkPos.Y * chunkSize * Tile.offsetY);
            
            tiles = new Tile[chunkSize][];
            for (int i = 0; i < chunkSize; i++)
            {
                tiles[i] = new Tile[chunkSize];
            }

        }

        public void SetTile(TileType type, int x, int y)
        {
            tiles[x][y] = new Tile(type);

            if (x % 2 == 0)
            {
                tiles[x][y].Position = new SFML.System.Vector2f(y * 2 * Tile.offsetX, x * Tile.offsetY - tiles[x][y].H);
            }
            else
            {
                tiles[x][y].Position = new SFML.System.Vector2f(y * 2 * Tile.offsetX - 1 * Tile.offsetX, x * Tile.offsetY - tiles[x][y].H);
            }

        }

        public void NewTilePosition(Tile tile, Vector2i posTile)
        {
            if (posTile.X % 2 == 0)
            {
                tile.Position = new SFML.System.Vector2f(posTile.Y * 2 * Tile.offsetX, posTile.X * Tile.offsetY - tile.H);
            }
            else
            {
                tile.Position = new SFML.System.Vector2f(posTile.Y * 2 * Tile.offsetX - 1 * Tile.offsetX, posTile.X * Tile.offsetY - tile.H);
            }
        }

        public Vector2f GetPosOfCenterChunk()
        {
            return new Vector2f((tiles[chunkSize/2][chunkSize / 2].Position.X + tiles[(chunkSize / 2) + 1][(chunkSize / 2) + 1].Position.X) / 2, (tiles[chunkSize / 2][chunkSize / 2].Position.Y + tiles[(chunkSize / 2) + 1][(chunkSize / 2) + 1].Position.Y) / 2);
        }

        public Vector2i TileClick(Vector2f pos, Vector2f chunkPos)
        {
            int x = -1;
            int y = -1;

            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    if ((pos.X > tiles[i][j].vecDot[1].X + tiles[i][j].Position.X + chunkPos.X) && (pos.X < tiles[i][j].vecDot[5].X + tiles[i][j].Position.X + chunkPos.X))
                    {
                        float y1 = ((pos.X - tiles[i][j].Position.X - tiles[i][j].vecDot[1].X - chunkPos.X) / tiles[i][j].vecDot[1].X) * (-tiles[i][j].vecDot[1].Y + tiles[i][j].vecDot[0].Y) + (tiles[i][j].Position.Y - tiles[i][j].vecDot[1].Y + chunkPos.Y);
                        float y2 = ((pos.X - tiles[i][j].Position.X - tiles[i][j].vecDot[5].X - chunkPos.X) / tiles[i][j].vecDot[5].X) * (-tiles[i][j].vecDot[5].Y + tiles[i][j].vecDot[0].Y) + (tiles[i][j].Position.Y - tiles[i][j].vecDot[5].Y + chunkPos.Y);
                        float y3 = ((pos.X - tiles[i][j].Position.X - tiles[i][j].vecDot[2].X - chunkPos.X) / tiles[i][j].vecDot[2].X) * (-tiles[i][j].vecDot[2].Y + tiles[i][j].vecDot[3].Y) + (tiles[i][j].Position.Y - tiles[i][j].vecDot[2].Y + chunkPos.Y);
                        float y4 = ((pos.X - tiles[i][j].Position.X - tiles[i][j].vecDot[4].X - chunkPos.X) / tiles[i][j].vecDot[4].X) * (-tiles[i][j].vecDot[4].Y + tiles[i][j].vecDot[3].Y) + (tiles[i][j].Position.Y - tiles[i][j].vecDot[4].Y + chunkPos.Y);

                        if ((pos.Y > y1) && (pos.Y > y2) && (pos.Y < y3) && (pos.Y < y4))
                        {
                            x = i;
                            y = j;
                        }
                    }
                }
            }
            
            if ((x > -1) && (y > -1))
            {
                return new Vector2i(x, y);
            }
            else
            {
                return new Vector2i(-1, -1);
            }

        }

        public Tile ThisTile(Vector2i tilePos)
        {
            if ((tilePos.X > -1) && (tilePos.Y > -1))
            {
                return tiles[tilePos.X][tilePos.Y];
            }
            else
            {
                return null;
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            for (int x = 0; x < chunkSize; x++)
            {
                for (int k = 1; k < 9; k++)
                {
                    for (int y = 0; y < chunkSize; y++)
                    {
                        if (tiles[x][y] == null)
                        {
                            continue;
                        }
                        else
                        {
                            if ((tiles[x][y].H == k * 10) && (TileMap.numberRow == x))
                            target.Draw(tiles[x][y], states);
                        }
                    }
                }
            }
        }
    }
}

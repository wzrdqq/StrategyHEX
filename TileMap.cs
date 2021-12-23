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
    class TileMap : Transformable, Drawable
    {
        public const int mapSize = 16;

        Chunk[][] chunks;

        public static int numberRow = 0;

        public static Vector2i centerChunk = new Vector2i();

        public static int radiusChunk = 3;

        public static float zoom = 27f;

        public TileMap()
        {
            chunks = new Chunk[mapSize][];

            for (int i = 0; i < mapSize; i++)
            {
                chunks[i] = new Chunk[mapSize];
            }

            centerChunk = new Vector2i(mapSize/2, mapSize / 2);

            Random rand = new Random();
            MyOpenSimplexNoise sN = new MyOpenSimplexNoise(rand.Next());
            for (int i = 0; i < Chunk.chunkSize * mapSize; i++)
            {
                for (int j = 0; j < Chunk.chunkSize * mapSize; j++)
                {
                    float k = (float) sN.Evaluate((double)(i * (Math.PI/2)) / zoom, (double)(j * (Math.PI / 2)) / zoom);
                    TileType type = TileType.None;

                    if (k < -0.217f)
                    {
                        type = TileType.Water;
                    }
                    else if (k < 0f)
                    {
                        type = TileType.Sand;
                    }
                    else if (k < 0.271f)
                    {
                        type = TileType.Ground1lvl;
                    }
                    else if (k < 0.423f)
                    {
                        type = TileType.Ground2lvl;
                    }
                    else if (k < 0.601f)
                    {
                        type = TileType.Ground3lvl;
                    }
                    else if (k < 0.691f)
                    {
                        type = TileType.Ground4lvl;
                    }
                    else if (k < 0.74f)
                    {
                        type = TileType.Rock1;
                    }
                    else if (k >= 0.74f)
                    {
                        type = TileType.Rock2;
                    }

                    SetTile(type, i, j);
                }
            }
        }

        public Chunk GetChunkByVector2i(Vector2i chunkNumber)
        {
            return chunks[chunkNumber.X][chunkNumber.Y];
        }

        public Vector2i GetVector2iChunk(Vector2f pos)
        {
            int x = -1;
            int y = -1;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Vector2i vec = chunks[i][j].TileClick(pos, chunks[i][j].Position);
                    if ((vec.X > -1)&&(vec.Y > -1))
                    {
                        x = i;
                        y = j;
                    }
                }
                if ((x > -1) && (y > -1))
                {
                    break;
                }
            }

            return new Vector2i(x, y);
        }

        public void SetTile(TileType type, int x, int y)
        {
            var chunk = GetChunk(x, y);
            var tilePos = GetTilePosFromChunk(x, y);

            chunk.SetTile(type, tilePos.Y, tilePos.X);
        }

        public Chunk GetChunk(int x, int y)
        {
            int X = x / Chunk.chunkSize;
            int Y = y / Chunk.chunkSize;

            if (chunks[X][Y] == null)
            {
                chunks[X][Y] = new Chunk(new Vector2i(X, Y));
            }

            return chunks[X][Y];
        }

        public Vector2i GetTilePosFromChunk(int x, int y)
        {
            int X = x / Chunk.chunkSize;
            int Y = y / Chunk.chunkSize;

            return new Vector2i(x - X * Chunk.chunkSize, y - Y * Chunk.chunkSize);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            for (int y = centerChunk.Y - radiusChunk; y < centerChunk.Y + radiusChunk + 1; y++)
            {
                for (int k = 0; k < Chunk.chunkSize; k++)
                {
                    numberRow = k;
                    for (int x = centerChunk.X - radiusChunk; x < centerChunk.X + radiusChunk + 1; x++)
                    {
                        if ((x > 0) && (x < mapSize) && (y > 0) && (y < mapSize))
                        {
                            if (chunks[x][y] == null)
                            {
                                continue;
                            }
                            else
                            {
                                target.Draw(chunks[x][y], states);
                            }
                        }
                    }
                }
                /*for (int x = 0; x < mapSize; x++)
                {
                    if (chunks[x][y] == null)
                    {
                        continue;
                    }
                    else
                    {
                        target.Draw(chunks[x][y], states);
                    }
                }*/
            }
        }
    }
}

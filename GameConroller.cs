using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoStepBack
{
    class GameConroller
    {
        TileMap map;
        Tile tileClick;
        Tile tileLastClick;
        Chunk ChunkClick;

        public Vector2i chunkNumber;
        public Vector2i tileNumber;

        public GameConroller()
        {
            map = new TileMap();
        }

        public void TileClick(Vector2f pos)
        {
            chunkNumber = new Vector2i(-1, -1);
            
            chunkNumber = map.GetVector2iChunk(pos);

            if ((chunkNumber.X > -1) && (chunkNumber.Y > -1))
            {
                Debug.WriteLine("Click chunk[" + chunkNumber.X + "][" + chunkNumber.Y + "]");

                if (tileClick != null)
                {
                    tileClick.TileCLick();
                }
                tileLastClick = tileClick;

                ChunkClick = map.GetChunkByVector2i(chunkNumber);
                tileNumber = ChunkClick.TileClick(pos, map.GetChunkByVector2i(chunkNumber).Position);
                Debug.WriteLine("Click ot tile[" + tileNumber.X + "][" + tileNumber.Y + "]");
                tileClick = ChunkClick.ThisTile(tileNumber);

                if (tileClick != null)
                {
                    tileClick.TileCLick();
                }

                if ((tileClick == tileLastClick) && (tileClick != null))
                {
                    tileClick.TileCLick();
                    tileClick = null;
                }
            }
            else
            {
                Debug.WriteLine("No click on any chunk");
            }
        }

        public void TilePlusH()
        {
            if (tileClick != null)
            {
                tileClick.TilePlusH();
                ChunkClick.NewTilePosition(tileClick, tileNumber);
            }
        }

        public void TileMinusH()
        {
            if (tileClick != null)
            {
                tileClick.TileMinusH();
                ChunkClick.NewTilePosition(tileClick, tileNumber);
            }
        }

        public void Update()
        {

        }

        public void IfNewChunk(Vector2f center)
        {
            Vector2f posCenterChunk = map.GetChunkByVector2i(TileMap.centerChunk).GetPosOfCenterChunk() + map.GetChunkByVector2i(TileMap.centerChunk).Position;

            if (center.X < posCenterChunk.X - 2 * Tile.offsetX * (Chunk.chunkSize / 2))
            {
                if (TileMap.centerChunk.X > 0)
                {
                    TileMap.centerChunk = new Vector2i(TileMap.centerChunk.X - 1, TileMap.centerChunk.Y);
                }
            }
            else if (center.X > posCenterChunk.X + 2 * Tile.offsetX * (Chunk.chunkSize / 2))
            {
                if (TileMap.centerChunk.X < TileMap.mapSize - 1)
                {
                    TileMap.centerChunk = new Vector2i(TileMap.centerChunk.X + 1, TileMap.centerChunk.Y);
                }
            }
            else if (center.Y < posCenterChunk.Y - Tile.offsetY * (Chunk.chunkSize / 2))
            {
                if (TileMap.centerChunk.Y > 0)
                {
                    TileMap.centerChunk = new Vector2i(TileMap.centerChunk.X, TileMap.centerChunk.Y - 1);
                }
            }
            else if (center.Y > posCenterChunk.Y + 2 * Tile.offsetX * (Chunk.chunkSize / 2))
            {
                if (TileMap.centerChunk.Y < TileMap.mapSize - 1)
                {
                    TileMap.centerChunk = new Vector2i(TileMap.centerChunk.X, TileMap.centerChunk.Y + 1);
                }
            }
        }

        public void Draw()
        {
            Program.Window.Draw(map);
        }
    }
}

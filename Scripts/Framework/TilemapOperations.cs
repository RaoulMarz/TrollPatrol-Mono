using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using TrollSmasher.DataTypes;

namespace TrollSmasher.Framework
{
    public class TilemapOperations
    {
        private TileMap _sourceMap = null;

        public TilemapOperations(TileMap sourceMap)
        {
        }

        /*
         * Array get_used_cells_by_id ( int id ) const
         */
        public List<MapTile> GetTilesOnFilter()
        {
            List<MapTile> res = null;
            return res;
        }

        public Tuple<int, int> GetVisibleTilesDimension()
        {
            Tuple<int, int> res = null;
            return res; 
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Map
    {
        private int level;
        private String nameLevel;

        private int maxMapX;
        private int maxMapY;

        private int[,] tiles;

        public Map(int level)
        {
            this.level = level;
            nameLevel = "map" + level.ToString("D2");
        }

        public void loadMap()
        {
            List<int[]> lignes = new List<int[]>();
            using (StreamReader reader = new StreamReader("Content/maps/"+ nameLevel + ".txt"))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null)
                {
                    string[] strings = ligne.Split(' ');
                    int[] nombres = new int[strings.Length];
                    maxMapY = strings.Length;
                    for (int i = 0; i < maxMapY; i++)
                    {
                        if (strings[i] != "")
                            nombres[i] = int.Parse(strings[i]);
                    }

                    lignes.Add(nombres);
                }
                maxMapX = lignes.Count;

                tiles = new int[maxMapX-1, maxMapY-1];

                for (int x = 0; x < maxMapX; x++)
                {
                    for (int y = 0; y < maxMapY; y++)
                    {
                        tiles[x, y] = lignes[x+1][y+1];
                    }
                }
            }
        }

        public void showMap()
        {

        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    //Classe contenant les méthodes afin de créer et d'afficher la MAP
    public class Map
    {
        //Tailles des tiles de la MAP
        const int tailleWidth = 32;
        const int tailleHeight = 32;

        private int level;
        private String nameLevel;

        //Tailles de la MAP
        private int maxMapX;
        private int maxMapY;

        //Données de la MAP
        private char[,] mapGame;

        public Map(int level)
        {
            this.level = level;
            nameLevel = "map" + level.ToString("D2");
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public String NameLevel
        {
            get { return nameLevel; }
            set { nameLevel = value; }
        }

        public int MaxMapX
        {
            get { return maxMapX; }
            set { maxMapX = value; }
        }

        public int MaxMapY
        {
            get { return maxMapY; }
            set { maxMapY = value; }
        }

        public char[,] MapGame
        {
            get { return mapGame; }
            set { mapGame = value; }
        }

        //Charger les données du fichier de la MAP dans le tableau mapGame
        public void loadMap()
        {
            List<char[]> lignes = new List<char[]>();
            //Lecture du fichier de la MAP et enregistrement des infos dans le tableau mapGame
            using (StreamReader reader = new StreamReader("Content/maps/"+ nameLevel + ".txt"))
            {
                string ligne;
                while ((ligne = reader.ReadLine()) != null)
                {
                    string[] strings = ligne.Split(' ');
                    char[] characters = new char[strings.Length];

                    //Taille (colonnes) de la MAP
                    maxMapY = strings.Length;
                    for (int i = 0; i < maxMapY; i++)
                    {
                        if (strings[i] != "")
                            characters[i] = char.Parse(strings[i]);
                    }

                    lignes.Add(characters);
                }

                //Taille (lignes) de la MAP
                maxMapX = lignes.Count;

                mapGame = new char[maxMapX, maxMapY];
                
                //Création du tableau mapGame
                for (int x = 0; x < maxMapX; x++)
                {
                    for (int y = 0; y < maxMapY; y++)
                    {
                        mapGame[x, y] = lignes[x][y];
                    }
                }
            }
        }

        //Afficher la MAP d'après le fichier .txt chargée précédemment
        public void showMap(SpriteBatch spriteBatch, Objet mur, Objet pacSamourai)
        {
            spriteBatch.Begin();

            //Chargement des textures de la MAP
            for (int x = 0; x < MaxMapX; x++)
            {
                for (int y = 0; y < MaxMapY; y++)
                {
                    Vector2 coord = new Vector2(y * tailleWidth, x * tailleHeight);

                    switch (MapGame[x, y])
                    {
                        case '0':
                            spriteBatch.Draw(mur.Texture, coord, Color.White);
                            break;
                        case '1':
                            spriteBatch.Draw(mur.Texture, coord, Color.Black);
                            break;
                        case 'P':
                            spriteBatch.Draw(pacSamourai.Texture, coord, Color.White);
                            break;
                    }
                }
            }

            spriteBatch.End();
        }
    }
}

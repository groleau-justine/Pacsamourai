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
        private int level;
        private String nameLevel;

        //Tailles de la MAP
        private int maxMapX;
        private int maxMapY;

        //Données de la MAP
        private char[,] mapGame;

        private Boolean bonus;
        private int xBonus;
        private int yBonus;

        public Map(int level)
        {
            this.level = level;
            nameLevel = "map" + level.ToString("D2");
            bonus = false;
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

        //Affichage des bonus
        public void drawBonus(Random aleatoire)
        {
            bonus = false;
            char bonusChoisi = GenerateurBonus.recupererBonus(aleatoire);

            if (!bonusChoisi.Equals('v'))
            {
                //On récupère les cases vides ou il est possible de poser un bonus
                List<Vector2> positionsPossiblesBonus = new List<Vector2>();

                for (int x = 0; x < MaxMapX; x++)
                {
                    for (int y = 0; y < MaxMapY; y++)
                    {
                        if ('1'.Equals(MapGame[x, y]))
                        {
                            positionsPossiblesBonus.Add(new Vector2(x, y));
                        }
                    }
                }

                if (positionsPossiblesBonus.Count != 0)
                {
                    int position = aleatoire.Next(0, positionsPossiblesBonus.Count);
                    List<Vector2> caseMap = positionsPossiblesBonus.GetRange(position, 1);
                    Vector2 caseBonus = caseMap[0];
                    xBonus = (int)caseBonus.X;
                    yBonus = (int)caseBonus.Y;

                    //On modifie la Map afin quelle prenne en compte le bonus
                    this.MapGame[xBonus, yBonus] = 'B';
                    bonus = true;
                }
            }    
        }

        //Remove des bonus
        public void removeBonus()
        {
            if (bonus)
            {
                this.MapGame[xBonus, yBonus] = '1';
            }
        }
    }
}

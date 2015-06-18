using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Fantome : ObjetAnime
    {
        private int numFantome;
        private char typeFantome;
        private Boolean isPeur;
        private Texture2D texture;
        private int positionXFantome;
        private int positionYFantome;
        private Point lastPositionFantome;
        private Point pointDep;
        private string lastMove;
        private Boolean goFantome;

        private int positionXPacSamurai;
        private int positionYPacSamurai;
        private int tempsVSPacSamurai;

        private Map useMap;
        private char lastBonus;
        private char bonus;

        public Fantome(List<Texture2D> listeTextures, List<Texture2D> listeTexturesPeur, int numFantome, char typeFantome, Map useMap)
            : base(listeTextures, listeTexturesPeur)
        {
            this.numFantome = numFantome;
            
            switch (numFantome)
            {
                case 1:
                    pointDep = new Point(3, 5);
                    break;
                case 2:
                    pointDep = new Point(3, 15);
                    break;
                case 3:
                    pointDep = new Point(15, 5);
                    break;
                case 4:
                    pointDep = new Point(15, 15);
                    break;
            }

            this.typeFantome = typeFantome;
            this.texture = base.ListeTextures[0];
            this.lastMove = null;
            this.goFantome = false;
            this.useMap = useMap;
            this.bonus = ' ';

            mAJPositions();
            lastPositionFantome = new Point(positionXFantome, positionYFantome);
        }

        public int NumFantome
        {
            get { return numFantome; }
            set { numFantome = value; }
        }

        public char TypeFantome
        {
            get { return typeFantome; }
            set { typeFantome = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public int PositionXFantome
        {
            get { return positionXFantome; }
            set { positionXFantome = value; }
        }

        public int PositionYFantome
        {
            get { return positionYFantome; }
            set { positionYFantome = value; }
        }

        public Point PointDep
        {
            get { return pointDep; }
            set { pointDep = value; }
        }

        public Boolean IsPeur
        {
            get { return isPeur; }
            set { isPeur = value; }
        }

        public Boolean GoFantome
        {
            get { return goFantome; }
            set { goFantome = value; }
        }

        //Fonction de modification du visuel des fantômes
        public Texture2D choiceOfTexture()
        {
            if (isPeur)
            {
                texture = base.ListeTexturesInvincible[0];
            }
            else
            {
                texture = base.ListeTextures[0];
            }
            return texture;
        }

        //Fonction de mise à jour des positions du PacSamurai et du Fantome
        private void mAJPositions() {

            for (int x = 0; x < useMap.MaxMapX; x++)
            {
                for (int y = 0; y < useMap.MaxMapY; y++)
                {
                    if (useMap.MapGame[x, y] == 'P')
                    {
                        positionXPacSamurai = x;
                        positionYPacSamurai = y;
                    }
                    else if (useMap.MapGame[x, y] == typeFantome)
                    {
                        positionXFantome = x;
                        positionYFantome = y;
                    }
                }
            }
        }

        //Fonction contenant l'algorithme de calcul du temps nécessaire avant de rejoindre le PacSamurai
        private void algoDistanceFVSP()
        {

            //Calcul de base sans prendre en compte les murs...
            tempsVSPacSamurai = Math.Abs(positionXPacSamurai - positionXFantome) + Math.Abs(positionYPacSamurai - positionYFantome);
        }
        
        //Fonction calculant le temps nécessaire avant de rejoindre le PacSamurai
        public void calculTempsVSPacSamurai() {
            mAJPositions();
            algoDistanceFVSP();
        }

        //Fonction permettant de modifier l'IA selon la situation...
        public Boolean choixFantomeIA()
        {
            Boolean mortDePac = false;

            //Calcul la distance entre le fantôme et le Pac
            calculTempsVSPacSamurai();

            //Si le fantôme attaque le pacman
            if (tempsVSPacSamurai < (useMap.MaxMapX - 1) / 2)
            {
                mortDePac = fantomeIAAttaque(1);
            }
            //Si le fantôme bouge aléatoirement
            else if (positionXFantome < pointDep.X + (useMap.MaxMapX - 1) / 4 && positionXFantome > pointDep.X - (useMap.MaxMapX - 1) / 4 && positionYFantome < pointDep.Y + (useMap.MaxMapY - 1) / 4 && positionYFantome > pointDep.Y - (useMap.MaxMapY - 1) / 4)
            {
                mortDePac = fantomeIAAleatoire();
            }
            //Si le fantôme rejoint sa zone
            else
            {
                mortDePac = fantomeIAAttaque(0);
            }

            return mortDePac;

        }

        //Fonction contenant l'IA attaquante
        private Boolean fantomeIAAttaque(int choice)
        {
            Boolean mortDePac = false;
            int positionX = 0, positionY = 0;
            Boolean move = false;

            //Utilisation d'une IA maison...
            //Calcul prenant en compte un algorithme maison...
            List<string> aleaMove = mAJDepAlea(positionXFantome, positionYFantome);

            List<char> notIn = new List<char>();
            switch (typeFantome)
            {
                case 'F':
                    notIn.Add('Q');
                    notIn.Add('R');
                    notIn.Add('V');
                    break;
                case 'Q':
                    notIn.Add('F');
                    notIn.Add('R');
                    notIn.Add('V');
                    break;
                case 'R':
                    notIn.Add('F');
                    notIn.Add('Q');
                    notIn.Add('V');
                    break;
                case 'V':
                    notIn.Add('F');
                    notIn.Add('Q');
                    notIn.Add('R');
                    break;
            }

            if (aleaMove.Count != 0)
            {
                //Remise en place du bonus
                if (lastBonus != ' ')
                {
                    useMap.MapGame[lastPositionFantome.X, lastPositionFantome.Y] = lastBonus;
                }

                //Remise en place du bonus
                if (bonus != ' ')
                {
                    if (notIn.Contains(useMap.MapGame[positionXFantome, positionYFantome]))
                    {
                        lastPositionFantome.X = positionXFantome;
                        lastPositionFantome.Y = positionYFantome;

                        lastBonus = bonus;
                    }
                    else
                    {
                        useMap.MapGame[positionXFantome, positionYFantome] = bonus;
                    }
                }
                else
                {
                    useMap.MapGame[positionXFantome, positionYFantome] = '1';
                }

                //Mise à jour de la position à atteindre...
                if (choice == 0)
                {
                    positionX = pointDep.X;
                    positionY = pointDep.Y;
                }
                else if (choice == 1)
                {
                    positionX = positionXPacSamurai;
                    positionY = positionYPacSamurai;
                }
                    
                //Modification de l'axe des X
                if (positionX - positionXFantome < 0 && aleaMove.Contains("v-"))
                {
                    positionXFantome--;
                    move = true;
                }
                else if (positionX - positionXFantome > 0 && aleaMove.Contains("v+"))
                {
                    positionXFantome++;
                    move = true;
                }
                //Modification de l'axe des Y
                else if (positionY - positionYFantome < 0 && aleaMove.Contains("h-"))
                {
                    positionYFantome--;
                    move = true;
                }
                else if (positionY - positionYFantome > 0 && aleaMove.Contains("h+"))
                {
                    positionYFantome++;
                    move = true;
                }

                if (!move)
                {
                    //Si le fantôme ne peut pas rejoindre le point indiqué, son IA devient aléatoire
                    fantomeIAAleatoire();
                }
                else
                {
                    //Enregistrement du prochain bonus et déplacement
                    if (useMap.MapGame[positionXFantome, positionYFantome] == 'S' || useMap.MapGame[positionXFantome, positionYFantome] == 'M' || useMap.MapGame[positionXFantome, positionYFantome] == 'B')
                    {
                        bonus = useMap.MapGame[positionXFantome, positionYFantome];
                    }
                    else
                    {
                        bonus = ' ';
                        if (useMap.MapGame[positionXFantome, positionYFantome] == 'P')
                        {
                            mortDePac = true;
                        }
                    }
                    
                    useMap.MapGame[positionXFantome, positionYFantome] = typeFantome;
                }
            }
            return mortDePac;
        }

        //Fonction contenant l'IA bloquante
        private Boolean fantomeIABlock()
        {
            Boolean mortDePac = false;

            return mortDePac;
        }

        //Fonction contenant l'IA aléatoire
        private Boolean fantomeIAAleatoire()
        {
            Boolean mortDePac = false;
            List<string> aleaMove = mAJDepAlea(positionXFantome, positionYFantome);

            List<char> notIn = new List<char>();
            switch (typeFantome)
            {
                case 'F':
                    notIn.Add('Q');
                    notIn.Add('R');
                    notIn.Add('V');
                    break;
                case 'Q':
                    notIn.Add('F');
                    notIn.Add('R');
                    notIn.Add('V');
                    break;
                case 'R':
                    notIn.Add('F');
                    notIn.Add('Q');
                    notIn.Add('V');
                    break;
                case 'V':
                    notIn.Add('F');
                    notIn.Add('Q');
                    notIn.Add('R');
                    break;
            }

            if (aleaMove.Count != 0)
            {
                Random rand = new Random(numFantome);
                string moveChoisi = aleaMove[rand.Next(0, aleaMove.Count - 1)];

                //Remise en place du bonus
                if (lastBonus != ' ')
                {
                    useMap.MapGame[lastPositionFantome.X, lastPositionFantome.Y] = lastBonus;
                }

                //Remise en place du bonus
                if (bonus != ' ')
                {
                    if (notIn.Contains(useMap.MapGame[positionXFantome, positionYFantome]))
                    {
                        lastPositionFantome.X = positionXFantome;
                        lastPositionFantome.Y = positionYFantome;

                        lastBonus = bonus;
                    }
                    else
                    {
                        useMap.MapGame[positionXFantome, positionYFantome] = bonus;
                    }
                }
                else
                {
                    useMap.MapGame[positionXFantome, positionYFantome] = '1';
                }

                //Déplacement des fantômes
                switch (moveChoisi)
                {
                    case "h+":
                        if (positionYFantome != useMap.MaxMapY - 1)
                            if (useMap.MapGame[positionXFantome, positionYFantome + 1] == 'S' || useMap.MapGame[positionXFantome, positionYFantome + 1] == 'M' || useMap.MapGame[positionXFantome, positionYFantome + 1] == 'B')
                            {
                                bonus = useMap.MapGame[positionXFantome, positionYFantome + 1];
                            }
                            else
                            {
                                bonus = ' ';
                                if (useMap.MapGame[positionXFantome, positionYFantome + 1] == 'P')
                                {
                                    mortDePac = true;
                                }
                            }
                        else
                        {
                            if (useMap.MapGame[positionXFantome, 0] == 'S' || useMap.MapGame[positionXFantome, 0] == 'M' || useMap.MapGame[positionXFantome, 0] == 'B')
                            {
                                bonus = useMap.MapGame[positionXFantome, 0];
                            }
                            else
                            {
                                bonus = ' ';
                                if (useMap.MapGame[positionXFantome, 0] == 'P')
                                {
                                    mortDePac = true;
                                }
                            }
                        }
                        // Quand le fantôme se déplace le plus à droite 
                        // et se retrouve à gauche de la map (passage de la postion y = 20 à y = 0)
                        if (positionYFantome == useMap.MaxMapY - 1)
                        {
                            useMap.MapGame[positionXFantome, 0] = typeFantome;
                            positionYFantome = 0;
                        }
                        else
                        {
                            useMap.MapGame[positionXFantome, positionYFantome + 1] = typeFantome;
                            positionYFantome++;
                        }
                        break;
                    case "h-":
                        if (positionYFantome != 0)
                        {
                            if (useMap.MapGame[positionXFantome, positionYFantome - 1] == 'S' || useMap.MapGame[positionXFantome, positionYFantome - 1] == 'M' || useMap.MapGame[positionXFantome, positionYFantome - 1] == 'B')
                            {
                                bonus = useMap.MapGame[positionXFantome, positionYFantome - 1];
                            }
                            else
                            {
                                bonus = ' ';
                                if (useMap.MapGame[positionXFantome, positionYFantome - 1] == 'P')
                                {
                                    mortDePac = true;
                                }
                            }
                        }
                        else
                        {
                            if (useMap.MapGame[positionXFantome, useMap.MaxMapY - 1] == 'S' || useMap.MapGame[positionXFantome, useMap.MaxMapY - 1] == 'M' || useMap.MapGame[positionXFantome, useMap.MaxMapY - 1] == 'B')
                            {
                                bonus = useMap.MapGame[positionXFantome, useMap.MaxMapY - 1];
                            }
                            else
                            {
                                bonus = ' ';
                                if (useMap.MapGame[positionXFantome, useMap.MaxMapY - 1] == 'P')
                                {
                                    mortDePac = true;
                                }
                            }
                        }
                        // Quand le fantôme se déplace le plus à gauche 
                        // et se retrouve à droite de la map (passage de la postion y = 0 à y = 20)
                        if (positionYFantome == 0)
                        {
                            useMap.MapGame[positionXFantome, useMap.MaxMapY - 1] = typeFantome;
                            positionYFantome = useMap.MaxMapY - 1;
                        }
                        else
                        {
                            useMap.MapGame[positionXFantome, positionYFantome - 1] = typeFantome;
                            positionYFantome--;
                        }
                        break;
                    case "v+":
                        if (useMap.MapGame[positionXFantome + 1, positionYFantome] == 'S' || useMap.MapGame[positionXFantome + 1, positionYFantome] == 'M' || useMap.MapGame[positionXFantome + 1, positionYFantome] == 'B')
                        {
                            bonus = useMap.MapGame[positionXFantome + 1, positionYFantome];
                        }
                        else
                        {
                            bonus = ' ';
                            if (useMap.MapGame[positionXFantome + 1, positionYFantome] == 'P')
                            {
                                mortDePac = true;
                            }
                        }
                        useMap.MapGame[positionXFantome + 1, positionYFantome] = typeFantome;
                        positionXFantome++;
                        break;
                    case "v-":
                        if (useMap.MapGame[positionXFantome - 1, positionYFantome] == 'S' || useMap.MapGame[positionXFantome - 1, positionYFantome] == 'M' || useMap.MapGame[positionXFantome - 1, positionYFantome] == 'B')
                        {
                            bonus = useMap.MapGame[positionXFantome - 1, positionYFantome];
                        }
                        else
                        {
                            bonus = ' ';
                            if (useMap.MapGame[positionXFantome - 1, positionYFantome] == 'P')
                            {
                                mortDePac = true;
                            }
                        }
                        useMap.MapGame[positionXFantome - 1, positionYFantome] = typeFantome;
                        positionXFantome--;
                        break;
                }

                lastMove = moveChoisi;
            }
            return mortDePac;
        }

        ////Fonction de mise à jour des possibilités de déplacement
        private List<string> mAJDepAlea(int positionXFantome, int positionYFantome)
        {
            Boolean continuerMove = true, exitChoice = false, secondeChoice = false;
            List<string> aleaMove = new List<string>();
            char[] notIn = { '0' };

            while (continuerMove)
            {
                if (positionYFantome <= useMap.MaxMapY - 1 && (lastMove != "h-" || exitChoice))
                {
                    if (positionYFantome < useMap.MaxMapY - 1)
                    {
                        if (!notIn.Contains(useMap.MapGame[positionXFantome, positionYFantome + 1]))
                        {
                            aleaMove.Add("h+");
                        }
                    }
                    else
                    {
                        aleaMove.Add("h+");
                    }
                }

                if (positionYFantome >= 0 && (lastMove != "h+" || exitChoice))
                {
                    if (positionYFantome > 0)
                    {
                        if (!notIn.Contains(useMap.MapGame[positionXFantome, positionYFantome - 1]))
                        {
                            aleaMove.Add("h-");
                        }
                    }
                    else
                    {
                        aleaMove.Add("h-");
                    }
                }

                if (positionXFantome < useMap.MaxMapX - 1 && (lastMove != "v-" || exitChoice))
                {
                    if (!notIn.Contains(useMap.MapGame[positionXFantome + 1, positionYFantome]))
                    {
                        aleaMove.Add("v+");
                    }
                }

                if (positionXFantome > 0 && (lastMove != "v+" || exitChoice))
                {
                    if (!notIn.Contains(useMap.MapGame[positionXFantome - 1, positionYFantome]))
                    {
                        aleaMove.Add("v-");
                    }
                }

                if (aleaMove.Count != 0 || secondeChoice)
                {
                    continuerMove = false;
                }
                else
                {
                    exitChoice = true;
                    secondeChoice = true;
                }
            }

            return aleaMove;
        }
    }
}

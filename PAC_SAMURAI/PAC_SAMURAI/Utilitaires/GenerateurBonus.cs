using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class GenerateurBonus
    {
        //Tableau contenant tous les bonus possibles
        // v : vide (pas de bonus)
        // c : cerise
        private static char[] bonus = {'v', 'c'};

        public static char recupererBonus(Random aleatoire)
        {
            int nbAlea = aleatoire.Next(0, bonus.Length);
            return bonus[nbAlea];
        }
    }
}

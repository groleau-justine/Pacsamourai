using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PAC_SAMURAI
{
    public class Root
    {
        private int num;
        private List<Point> listPoint = new List<Point>();
        private int duree;

        public Root(int num, Point point, int duree)
        {
            this.num = num;
            listPoint.Add(point);
            this.duree = duree;
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public List<Point> ListPoint
        {
            get { return listPoint; }
            set { listPoint = value; }
        }

        public int Duree
        {
            get { return duree; }
            set { duree = value; }
        }
        public void ajouterPoint(Point point)
        {
            listPoint.Add(point);
            duree++;
        }

    }
}

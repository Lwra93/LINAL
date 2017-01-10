using System;
using LINAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LINAL_Test
{
    [TestClass]
    public class VectorTest
    {

        private Point P, Q, S;
        private Vector PSupport, PQ, PS;

        [TestInitialize]
        public void init()
        {
            P = new Point(3, -1, 2);
            Q = new Point(1, 3, 4);
            S = new Point(2, 3, 2);

            PSupport = P.MakeVector();
            PQ = new Vector(P, Q);
            PS = new Vector(P, S);
        }

        [TestMethod]
        public void IsDependant()
        {

            Assert.AreEqual(false, PQ.IsDependantOf(PS));

        }

        [TestMethod]
        public void Inproduct()
        {

            Assert.AreEqual(18, PQ.GetInproduct(PS));

        }

        [TestMethod]
        public void CrossProduct()
        {

            Vector v = PQ.GetSimplified().GetCrossProduct(PS.GetSimplified());
            Assert.AreEqual(-4, v.GetX());
            Assert.AreEqual(-1, v.GetY());
            Assert.AreEqual(-2, v.GetZ());

        }

        [TestMethod]
        public void PointInPlane()
        {
            
            Plane p = new Plane();
            p.Add(P);
            p.Add(Q);
            p.Add(S);



            p.BuildFormula();

            Assert.AreEqual(true, p.IsInPlane(new Point(-2, 13, 5)));


        }
    }
}

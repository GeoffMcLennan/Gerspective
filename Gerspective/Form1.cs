using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerspective {
    public partial class Form1 : Form {
        private PerspectiveData data;
        private double[,] rTrans = new double[4, 4];
        private double[,] lTrans = new double[4, 4];

        private double[,] rScrnPts;
        private double[,] lScrnPts;
        double numPts;

        public Form1() {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pea) {
            Graphics g = pea.Graphics;

            double rTemp;
            double lTemp;

            if (data != null) {

                for (int i = 0; i < numPts; i++) {
                    for (int j = 0; j < 4; j++) {
                        rTemp = 0.0;
                        lTemp = 0.0;
                        for (int k = 0; k < 4; k++) {
                            rTemp += data.XRight[i, k] * rTrans[k, j];
                            lTemp += data.XLeft[i, k] * lTrans[k, j];
                        }
                        rScrnPts[i, j] = rTemp;
                        lScrnPts[i, j] = lTemp;
                    }
                }

                // Draw right eye image
                Pen pen = new Pen(Color.Cyan, 1);
                for (int i = 0; i < data.Lines.GetLength(0); i++) {
                    g.DrawLine(pen, (int)rScrnPts[data.Lines[i, 0], 0], (int)rScrnPts[data.Lines[i, 0], 1],
                        (int)rScrnPts[data.Lines[i, 1], 0], (int)rScrnPts[data.Lines[i, 1], 1]);
                }

                // Draw left eye image
                pen.Color = Color.Red;
                for (int i = 0; i < data.Lines.GetLength(0); i++) {
                    g.DrawLine(pen, (int)lScrnPts[data.Lines[i, 0], 0], (int)lScrnPts[data.Lines[i, 0], 1],
                        (int)lScrnPts[data.Lines[i, 1], 0], (int)lScrnPts[data.Lines[i, 1], 1]);
                }
            }
        }

        private void newDataToolStripMenuItem_Click(object sender, EventArgs e) {
            data = InputParser.OpenData();

            if (data != null) {
                numPts = data.XRight.GetLength(0);
                rScrnPts = new double[data.XRight.GetLength(0), data.XRight.GetLength(1)];
                lScrnPts = new double[data.XLeft.GetLength(0), data.XLeft.GetLength(1)];
                InitializeImage();
            }
        }

        void InitializeImage() {
            SetIdentity(rTrans, 4, 4);
            SetIdentity(lTrans, 4, 4);

            Translate(-data.XRight[0, 0], -data.XRight[0, 1], 0, ref rTrans);
            Translate(-data.XLeft[0, 0], -data.XLeft[0, 1], 0, ref lTrans);
            Reflect();
            Scale(3.79);
            Translate(Width / 2, Height / 2, 0, ref rTrans);
            Translate(Width / 2, Height / 2, 0, ref lTrans);
            Invalidate();
        }

        void SetIdentity(double[,] A, int nrow, int ncol) {
            for (int i = 0; i < nrow; i++) {
                for (int j = 0; j < ncol; j++) A[i, j] = 0.0d;
                A[i, i] = 1.0d;
            }
        }

        void AddToTNet(double[,] transform, ref double[,] ctrans) {
            double[,] tNet = new double[4, 4];

            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    for (int k = 0; k < 4; k++) {
                        tNet[i, j] += ctrans[i, k] * transform[k, j];
                    }
                }
            }
            ctrans = tNet;
        }

        void Translate(double x, double y, double z, ref double[,] ctrans) {
            double[,] transform = new double[4, 4];
            SetIdentity(transform, 4, 4);
            transform[3, 0] = x;
            transform[3, 1] = y;
            transform[3, 2] = z;
            AddToTNet(transform, ref ctrans);
        }

        void Reflect() {
            double[,] transform = new double[4, 4];
            SetIdentity(transform, 4, 4);
            transform[1, 1] = -1;
            AddToTNet(transform, ref rTrans);
            AddToTNet(transform, ref lTrans);
        }

        void Scale(double factor) {
            double[,] transform = new double[4, 4];
            SetIdentity(transform, 4, 4);
            transform[0, 0] = factor;
            transform[1, 1] = factor;
            transform[2, 2] = factor;
            AddToTNet(transform, ref rTrans);
            AddToTNet(transform, ref lTrans);
        }
    }
}

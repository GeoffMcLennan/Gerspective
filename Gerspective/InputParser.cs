using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerspective
{
    class InputParser
    {

        public static PerspectiveData OpenData()
        {
            PerspectiveData pd = new PerspectiveData();

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Data Files|*.dat";
            ofd.Title = "Open Points Data File";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    StreamReader stream = fi.OpenText();
                    String text;
                    List<Tuple<double, double, double>> input = new List<Tuple<double, double, double>>();
                    double[,] data;
                    do
                    {
                        text = stream.ReadLine();
                        if (text != null)
                        {
                            if (text == "")
                            {
                                data = new double[input.Count, 3];
                                for (int i = 0; i < input.Count; i++)
                                {
                                    data[i, 0] = input[i].Item1;
                                    data[i, 1] = input[i].Item2;
                                    data[i, 2] = input[i].Item3;
                                }
                                pd.XLeft = data;
                                input = new List<Tuple<double, double, double>>();
                            } else
                            {
                                String[] split = text.Split(null);
                                input.Add(Tuple.Create(Double.Parse(split[0]), Double.Parse(split[1]), Double.Parse(split[2])));
                            }
                        }
                    } while (text != null);
                    data = new double[input.Count, 4];
                    for (int i = 0; i < input.Count; i++)
                    {
                        data[i, 0] = input[i].Item1;
                        data[i, 1] = input[i].Item2;
                        data[i, 2] = input[i].Item3;
                        data[i, 3] = 1;
                    }
                    pd.XRight = data;
                } catch (Exception e)
                {
                    MessageBox.Show("Failed to parse points from file");
                    return null;
                }
            }

            ofd = new OpenFileDialog();
            ofd.Filter = "Data Files|*.dat";
            ofd.Title = "Open Lines Data File";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    FileInfo fi = new FileInfo(ofd.FileName);
                    StreamReader stream = fi.OpenText();
                    String text;
                    List<Tuple<int, int>> input = new List<Tuple<int, int>>();
                    do
                    {
                        text = stream.ReadLine();
                        if (text != null)
                        {
                            string[] split = text.Split(null);
                            input.Add(Tuple.Create(Int32.Parse(split[0]), Int32.Parse(split[1])));
                        }
                    } while (text != null);
                    int[,] data = new int[input.Count, 2];
                    for (int i = 0; i < input.Count; i++)
                    {
                        data[i, 0] = input[i].Item1;
                        data[i, 1] = input[i].Item2;
                    }
                    pd.Lines = data;
                } catch (Exception e)
                {
                    MessageBox.Show("Failed to parse lines from file");
                    return null;
                }
            }

            return pd;
        }
    }
}

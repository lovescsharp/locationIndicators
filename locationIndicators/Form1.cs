using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace locationIndicators
{
    public partial class Form1 : Form
    {
        private List<List<Rectangle>> li;
        private Size size;
        private Point initCoords;
        public Form1()
        {
            InitializeComponent();

            KeyPreview = true;

            KeyDown += Add;
            MouseDown += MouseButtonClick;

            size = new Size(30, 50);

            li = new List<List<Rectangle>>();
            li.Add(new List<Rectangle>());
            li[0].Add(new Rectangle(initCoords, size));
            Paint += new PaintEventHandler(Draw);

        }
        private void MouseButtonClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //adding indicator to row
            {
                for (int i = 0; i < li.Count; i++)
                {
                    for (int j = 0; j < li[i].Count; j++)
                    {
                        if (li[i][j].Contains(e.Location))
                        {
                            if (li[i].Count == 1)
                            {
                                AddRect(li[i], i, j);
                                li.Add(new List<Rectangle>());
                                return;
                            } 
                            else
                            {
                                AddRect(li[i], i, j);
                                return;
                            }
                        }
                        else if (li[i][j].Contains(e.Location))
                        {
                            AddRect(li[i], i, j);
                            return;
                        }
                    }
                }
            }
            else if (e.Button == MouseButtons.Right) //remove indicator from row
            {
                for (int i = 0; i < li.Count; i++)
                {
                    for (int j = 0; j < li[i].Count; j++)
                    {
                        if (li[i][j].Contains(e.Location))
                        {
                            if (i == 0 && j == 0) return;
                            else if (li[i].Count == 1)
                            {
                                if (i + 1 == li.Count) return;
                                else 
                                {
                                    for (int r = 0; r < li[i + 1].Count; r++)
                                    {
                                        Rectangle rect = li[i + 1][r];
                                        rect.Location = new Point(li[i][r].Location.X, li[i][r].Location.Y);
                                        li[i + 1][r] = rect;
                                    }
                                }
                                li.RemoveAt(i);
                                Invalidate();
                                return;
                            }
                            else
                            {
                                if (j + 1 == li[i].Count) return;
                                li[i].RemoveAt(j);
                                int count = li[i].Count;
                                li[i].Clear();
                                for (int r = 0; r < count; r++)
                                {
                                    AddRect(li[i], i, r);
                                }
                                Invalidate();

                                return;
                            }
                        }
                    }
                }
            }
        }
        private void AddRect(List<Rectangle> indicators, int row, int col)
        {
            indicators.Add(new Rectangle(
                    (row) * (10 + size.Width), col * (10 + size.Height), size.Width, size.Height
                ));
            Invalidate();
        }
        private void Add(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A) li.Add(new List<Rectangle>() { new Rectangle(initCoords.X, li.Count * (10 + size.Height), size.Width, size.Height) });//add new row 
            else if (e.KeyCode == Keys.D) UpdateLI();
            Invalidate();
        }
        private void Draw(object sender, PaintEventArgs e)
        {
            using (Pen pen = new Pen(Color.NavajoWhite, 6.0f))
            {
                foreach (List<Rectangle> chain in li)
                {
                    foreach (Rectangle r in chain) e.Graphics.DrawRectangle(pen, r);
                }
            }
        }
        private void UpdateLI()
        {
            li = new List<List<Rectangle>>() { new List<Rectangle>() };
            AddRect(li[0], 0, 0);
            Invalidate();
        }
    }
}

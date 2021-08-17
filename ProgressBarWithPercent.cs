using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;


namespace WebPageStuff
{

    public enum ProgressBrushes
    {
        SolidColorBrush,
        LinearGradientBrush,
        RadialGradientBrush,
        ImageBrush,
        DrawingBrush,
        VisualBrush

    };
    
    public enum ProgressTypes
    {
        Cover,
        Reveal
    }

    public enum ProgressOrientations
    {
        Landscape,
        Portrait
    }

    public partial class ProgressBarWithPercent : UserControl
    {
        public ProgressBarWithPercent()
        {
            InitializeComponent();
            label1.ForeColor = Color.Black;
            this.ForeColor = SystemColors.Highlight;
            ftTextFont = label1.Font;
            //clrProgressBackColor = this.BackColor;
            //clrProgressForeColor = this.ForeColor;
            clrTextForeColor = label1.ForeColor;
            caLabelTextAlign = label1.TextAlign;
            //this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected float percent = 0.0f;

        public float value
        {
            get
            {
                return percent;
            }
            set
            {
                if (value < 0) value = 0;
                else if (value > 100) value = 100;
                percent = value;
                label1.Text = value.ToString();
                if (bShowPercentSign == true) label1.Text += "%";
                this.Invalidate();
            }
        }

        private Color clrProgressBackColor;

        public Color ProgressBackColor
        {
            get { return clrProgressBackColor; }
            set { clrProgressBackColor = value; this.BackColor = value; }
        }

        //private Color clrProgressForeColor;

        //public Color ProgressForeColor
        //{
        //    get { return clrProgressForeColor; }
        //    set { clrProgressForeColor = value; this.ForeColor = value; }
        //}

        private Color clrTextForeColor;

        public Color TextForeColor
        {
            get { return clrTextForeColor; }
            set { clrTextForeColor = value; label1.ForeColor = value; }
        }

        private ProgressBrushes pbProgressBrush;

        public ProgressBrushes ProgressBrush
        {
            get { return pbProgressBrush; }
            set { pbProgressBrush = value; }
        }

        private LinearGradientMode lgmLinearGradientBrushMode;

        public LinearGradientMode LinearGradientBrushMode
        {
            get { return lgmLinearGradientBrushMode; }
            set { lgmLinearGradientBrushMode = value; }
        }

        private ProgressTypes ptProgressType;

        public ProgressTypes ProgressType
        {
            get { return ptProgressType; }
            set { ptProgressType = value; }
        }

        private ProgressOrientations poProgressOrientation;

        public ProgressOrientations ProgressOrientation
        {
            get { return poProgressOrientation; }
            set { poProgressOrientation = value; }
        }
        
        private ContentAlignment caLabelTextAlign;

        public ContentAlignment LabelTextAlign
        {
            get { return caLabelTextAlign; }
            set { caLabelTextAlign = value;
            PositionLabel();
             }
        }

        private Image imgImageBrushImage;

        public Image ImageBrushImage
        {
            get { return imgImageBrushImage; }
            set { imgImageBrushImage = value; }
        }
        
        private bool bShowPercentSign = false;

        public bool ShowPercentSign
        {
            get { return bShowPercentSign; }
            set { bShowPercentSign = value;
            if (bShowPercentSign == true)
            {
                label1.Text = percent.ToString() + "%";
            }
            else
            {
                label1.Text = percent.ToString();
            }
            }
        }

        private Font ftTextFont;

        public Font TextFont
        {
            get { return ftTextFont; }
            set { ftTextFont = value; label1.Font = value; }
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int ix = 0;
            int iy = 0;
            int iwidth = 0;
            int iheight = 0;

            switch (ptProgressType)
            {
            case ProgressTypes.Cover:
                {
                    switch (poProgressOrientation)
                    {
                    case ProgressOrientations.Landscape:
                        {
                            iwidth = (int)((percent / 100) * this.Width);
                            iheight = this.Height;
                            break;
                        }
                    case ProgressOrientations.Portrait:
                        {
                            iy = (int)(((100 - percent) / 100) * this.Height);
                            iheight = this.Height - iy;
                            iwidth = this.Width;
                            break;
                        }
                    }

                    break;
                }
            case ProgressTypes.Reveal:
                {
                    switch (poProgressOrientation)
                    {
                    case ProgressOrientations.Landscape:
                        {
                            iwidth = (int)(((100 - percent) / 100) * this.Width);
                            ix = this.Width - iwidth;
                            iheight = this.Height;
                            break;
                        }
                    case ProgressOrientations.Portrait:
                        {
                            iheight = (int)(((100 - percent) / 100) * this.Height);
                            iwidth = this.Width;
                            break;
                        }
                    }
                    
                    break;
                }
            }

            switch (pbProgressBrush)
            {
            case ProgressBrushes.SolidColorBrush:
                {
                    Brush b = new SolidBrush(this.ForeColor);
                    e.Graphics.FillRectangle(b, ix, iy, iwidth, iheight);
                    b.Dispose();
                    break;
                }
            case ProgressBrushes.LinearGradientBrush:
                {
                    LinearGradientBrush lb = new LinearGradientBrush(new Rectangle(0, 0, this.Width, this.Height), Color.FromArgb(255, this.ForeColor), Color.FromArgb(255, clrProgressBackColor), lgmLinearGradientBrushMode);
                    e.Graphics.FillRectangle(lb, ix, iy, iwidth, iheight );
                    lb.Dispose();
                    break;
                }
            case ProgressBrushes.ImageBrush:
                {
                    Graphics g = e.Graphics;
                    Rectangle r = this.ClientRectangle;
                    //ControlPaint.DrawBorder(g, r, Color.Black, ButtonBorderStyle.Solid);

                    r.Inflate(-1, -1);
                    r.Width = (int)((r.Width
                                    * (percent / this.Width))) - 1;
                    if ((r.Width < 1))
                    {
                        return;
                    }

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                   
                    g.DrawImage(imgImageBrushImage, r, r, GraphicsUnit.Pixel);
                    break;
                }
            case ProgressBrushes.DrawingBrush:
                {

                    break;
                }
            case ProgressBrushes.RadialGradientBrush:
                {
                    
                    break;
                }
            case ProgressBrushes.VisualBrush:
                {
                    
                    break;
                }
            }
        }

        private void Pbar_SizeChanged(object sender, EventArgs e)
        {
            PositionLabel();
        }

        private void label1_SizeChanged(object sender, EventArgs e)
        {
            PositionLabel();
        }

        private void PositionLabel()
        {
            switch (caLabelTextAlign)
            {
            case ContentAlignment.MiddleCenter:
                label1.Location = new Point(this.Width / 2 - 21 / 2 - 4, this.Height / 2 - 15 / 2);
                break;
            case ContentAlignment.MiddleLeft:
                label1.Location = new Point(0, this.Height / 2 - 15 / 2);
                break;
            case ContentAlignment.MiddleRight:
                label1.Location = new Point(this.Width - label1.Width, this.Height / 2 - 15 / 2);
                break;
            case ContentAlignment.TopCenter:
                label1.Location = new Point(this.Width / 2 - 21 / 2 - 4, 0);
                break;
            case ContentAlignment.TopLeft:
                label1.Location = new Point(0, 0);
                break;
            case ContentAlignment.TopRight:
                label1.Location = new Point(this.Width - label1.Width, 0);
                break;
            case ContentAlignment.BottomCenter:
                label1.Location = new Point(this.Width / 2 - 21 / 2 - 4, this.Height - label1.Height);
                break;
            case ContentAlignment.BottomLeft:
                label1.Location = new Point(0,  this.Height - label1.Height);
                break;
            case ContentAlignment.BottomRight:
                label1.Location = new Point(this.Width - label1.Width,  this.Height - label1.Height);
                break;

            default:
                break;
            }
        }

    }
}

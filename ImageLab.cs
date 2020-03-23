using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Iris
{
    public class ImageLab
    {
        static int Hgt, Wid;
        //byte[] Mask1;   
        //byte[] Mask2;

        static byte Gry;

        static double One;
        static byte Flg, G, C;
        static int Hm, HmN, I, Wm, WmN, X1, X2, Xx, Y1, Y2, Yy;
        static long Max, Min;
        static double Rat, Sm, Xc, Yc, Sm2, Rad, Ang, SmC2, SmS2, SmR, SmRC, SmRs, A0, A1, A2, dR;
        static int X, Y;
        static double[] Cs;
        static double[] Sn;
        static int Xm, Xp, Ym, Yp;
        static int[] Xb;
        static int[] Yb;
        static long J, N;
        static byte[] Flgg;
        static double[] Rho;



        //byte Tbl[];
        static double S, R1, R2, Cc;




        static byte[] Tbl;
        static long[] cl = new long[2];
        static long[] His = new long[256];

        static byte[,] gray;
        static byte[,] Mask1;
        static byte[,] Mask2;
        static byte[,] temp;
        static byte[,] Red;
        static byte[,] Grn;
        static byte[,] Blu;
        //
        public static Color[,] c;
        public static Color[,] c1;
        public static Color[,] origin;
        public static Color[,] rgb;
        public static int i, j, x, y, width, height, red, green, blue, brightness;

        public static int W { get; private set; }
        public static int Ww { get; private set; }

        public static int remain, stride;
        private static int F;

        public static void update()
        {
            for (i = 1; i < height - 1; i++)
            {
                for (j = 1; j < width - 1; j++)
                {
                    c[j, i] = c1[j, i];
                }
            }
        }

        public static Bitmap Load(Bitmap bmp)
        {
            height = bmp.Height;
            width = bmp.Width;

            c = new Color[bmp.Width, bmp.Height];
            gray = new byte[bmp.Width, bmp.Height];
            Red = new byte[width, height];
            Grn = new byte[width, height];
            Blu = new byte[width, height];

            var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                stride = bmpdata.Stride;
                remain = stride - bmp.Width * 3;
                var p = (byte*)(void*)scan0;
                for (i = 0; i < bmp.Height; i++)
                {
                    for (j = 0; j < bmp.Width; j++)
                    {
                        c[j, i] = Color.FromArgb(p[2], p[1], p[0]);
                        gray[j, i] = p[0];
                        //Convert.ToByte(p[2] * .299 + p[1] * .587 + p[0] * .114);
                        p += 3;
                    }
                    p += remain;
                }
                bmp.UnlockBits(bmpdata);
            }
            return bmp;
        }

        // first stage
        public static Bitmap FirstStage(Bitmap bmp)
        {

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    G = gray[j, i];
                    His[G] = His[G] + 1;
                }
            }

            Rat = 0.004 * (width) * (height);
            Min = 3;
            Sm = His[Min];

            while (Sm < Rat)
            {
                Min += 1;
                Sm += His[Min];
            }

            Max = 255;
            Sm = His[Max];

            while (Sm < Rat)
            {
                Max -= 1;
                Sm += His[Max];
            }

            Tbl = new byte[256];
            S = 1.0 / (Max - Min);

            for (I = 0; I < 256; I++)
            {
                if (I < Min)
                {
                    Tbl[I] = 0;
                }
                else if (I >= Max)
                {
                    Tbl[I] = 255;
                }
                else
                {
                    Tbl[I] = (byte)(255 * Math.Pow(S * (I - Min), 0.1));
                }
            }

            Mask1 = new byte[width, height];
            temp = new byte[width, height];
            cl[0] = 0;
            cl[1] = 255;

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    G = gray[j, i];
                    I = Tbl[G];
                    if (I > 128)
                    {
                        Mask1[j, i] = 1;
                        temp[j, i] = 255;
                    }
                    else
                    {
                        Mask1[j, i] = 0;
                        temp[j, i] = 0;
                    }

                }
            }

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {

                    if (Mask1[j, i] == 1)
                        c[j, i] = Color.FromArgb(255, 255, 255, 255);
                    else
                        c[j, i] = Color.FromArgb(0, 0, 0, 0);

                }
            }

            var bmpdata = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < bmp.Height; i++)
                {
                    for (j = 0; j < bmp.Width; j++)
                    {
                        p[2] = c[j, i].R;
                        p[1] = c[j, i].G;
                        p[0] = c[j, i].B;
                        // updating arrya of colors for future work
                        c[j, i] = Color.FromArgb(p[2], p[1], p[0]);
                        p += 3;
                    }
                    p += remain;
                }
            }
            bmp.UnlockBits(bmpdata);
            return bmp;
        }

        // second and third stage
        public static Bitmap SecondStage(Bitmap bitmap)
        {
            Flg = 0;
            X1 = 0;

            while (Flg == 0)
            {
                for (i = 0; i < height; i++)
                {
                    if (Mask1[X1, i] == 1)
                        Flg = 1;
                }
                if (Flg == 0)
                    X1 += 1;
            }

            Flg = 0;
            X2 = width - 1;

            while (Flg == 0)
            {
                for (i = 0; i < height; i++)
                {
                    if (Mask1[X2, i] == 1)
                        Flg = 1;
                }
                if (Flg == 0)
                    X2 -= 1;
            }

            Flg = 0;
            Y1 = 0;

            while (Flg == 0)
            {
                for (j = X1; j <= X2; j++)
                {
                    if (Mask1[j, Y1] == 1)
                        Flg = 1;
                }
                if (Flg == 0)
                    Y1 += 1;
            }

            Flg = 0;
            Y2 = height - 1;

            while (Flg == 0)
            {
                for (j = X1; j < X2; j++)
                {
                    if (Mask1[j, Y2] == 1)
                        Flg = 1;
                }
                if (Flg == 0)
                    Y2 -= 1;
            }

            WmN = X2 - X1;
            HmN = Y2 - Y1;

            byte[,] tRed = new byte[WmN + 1, HmN + 1];
            byte[,] tGrn = new byte[WmN + 1, HmN + 1];
            byte[,] tBlu = new byte[WmN + 1, HmN + 1];
            byte[,] tMask1 = new byte[WmN + 1, HmN + 1];

            for (Y = Y1; Y <= Y2; Y++)
            {
                Yy = Y - Y1;
                for (X = X1; X <= X2; X++)
                {
                    Xx = X - X1;
                    tMask1[Xx, Yy] = Mask1[X, Y];
                    tRed[Xx, Yy] = Red[X, Y];
                    tGrn[Xx, Yy] = Grn[X, Y];
                    tBlu[Xx, Yy] = Blu[X, Y];

                }
            }

            Array.Clear(Mask1, 0, Mask1.Length);
            Array.Clear(Red, 0, Red.Length);
            Array.Clear(Grn, 0, Grn.Length);
            Array.Clear(Blu, 0, Blu.Length);
            Wm = WmN;
            Hm = HmN;


            for (Y = 0; Y <= Hm; Y++)
                for (X = 0; X <= Wm; X++)
                {
                    Mask1[X, Y] = tMask1[X, Y];
                    Red[X, Y] = tRed[X, Y];
                    Grn[X, Y] = tGrn[X, Y];
                    Blu[X, Y] = tBlu[X, Y];
                }

            Array.Clear(tMask1, 0, tMask1.Length);
            Array.Clear(tRed, 0, tRed.Length);
            Array.Clear(tGrn, 0, tGrn.Length);
            Array.Clear(tBlu, 0, tBlu.Length);
            //
            Array.Clear(His, 0, His.Length);
            Array.Clear(Tbl, 0, Tbl.Length);


            His = new long[256];
            Tbl = new byte[256];
            N = 0;
            Sm = 0;
            Sm2 = 0;


            for (i = 0; i <= Hm; i++)
            {
                for (j = 0; j <= Wm; j++)
                {
                    if (Mask1[j, i] == 1)
                    {

                        G = gray[j, i];
                        N += 1;
                        Sm += G;
                        Sm2 += G * G;

                    }

                }
            }


            Sm /= N;
            Sm2 = (Sm2 / N) - (Sm * Sm);
            Sm2 = Math.Pow(Sm2, 2);
            Min = (long)(Sm - 1.2 * Sm2);

            if (Min < 20)
            {
                Min = 19;
                Max = (long)(Sm + 2 * Sm2);
            }

            if (Max > 200)
                Max = 200;

            S = 1.0 / (Max - Min);



            for (I = 0; I <= 255; I++)
            {
                if (I < Min)
                    Tbl[I] = 0;
                else if (I >= Max)
                    Tbl[I] = 255;
                else
                    Tbl[I] = (byte)(255 * Math.Pow((S * (I - Min)), 1.5));
            }


            Mask2 = new byte[Wm, Hm];




            for (i = 0; i < Hm; i++)
                for (j = 0; j < Wm; j++)
                {
                    Mask2[j, i] = 0;
                    if (Mask1[j, i] == 1)
                    {
                        G = gray[j, i];
                        I = Tbl[G];
                        if (I > 80)
                        {
                            Mask2[j, i] = 0;
                            c[j, i] = Color.FromArgb(0, 0, 0, 0);
                        }
                        else
                        {
                            Mask2[j, i] = 1;
                            c[j, i] = Color.FromArgb(255, 255, 255, 255);
                        }

                    }

                }

            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < bitmap.Height; i++)
                {
                    for (j = 0; j < bitmap.Width; j++)
                    {
                        p[2] = c[j, i].R;
                        p[1] = c[j, i].G;
                        p[0] = c[j, i].B;
                        // updating arrya of colors for future work
                        c[j, i] = Color.FromArgb(p[2], p[1], p[0]);
                        p += 3;
                    }
                    p += remain;
                }
            }
            bitmap.UnlockBits(bmpdata);

            return bitmap;
        }

        // fourth stage
        public static Bitmap FourthStage(Bitmap bitmap)
        {
            //SeedFilling(0, 1);
            //SeedFilling(1, 0);
            //
            Cs = new double[720];
            Sn = new double[720];
            One = Math.PI / 180.0;
            //
            for (I = 0; I < 720; I++)
            {
                Ang = One * I * .5;
                Cs[I] = Math.Cos(Ang);
                Sn[I] = Math.Sin(Ang);
            }
            //

            N = 0;
            Xc = 0;
            Yc = 0;
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == 1)
                    {
                        N++;
                        Xc += X;
                        Yc += Y;
                    }
                }
            }
            //
            Xc /= N;
            Yc /= N;
            Rad = Math.Pow(N / Math.PI, 2);
            R1 = (int)(.45 * Rad);
            R2 = (int)(1.55 * Rad);
            Xb = new int[719];
            Yb = new int[719];
            //

            for (I = 0; I < 719; I++)
            {
                for (J = (long)R1; J < R2; J++)
                {
                    X = (int)(Xc + J * Cs[I]);
                    Y = (int)(Yc + J * Sn[I]);
                    if (X >= 0 && X <= Wm && Y >= 0 && Y <= Hm)
                    {
                        if (Mask2[X, Y] == 0)
                        {
                            Mask2[X, Y] = 2;
                            J = (long)R2;
                            Xb[I] = X;
                            Yb[I] = Y;
                        }
                        else
                        {
                            J = (long)R2;
                        }
                    }
                }
            }
            //
            Flgg = new byte[719];
            Rho = new double[719];

            for (I = 0; I < 719; I++)
            {
                Flgg[I] = 1;
            }
            //
            for (int M = 1; M <= 3; M++)
            {
                SmC2 = SmS2 = SmR = SmRC = SmRs = 0;
                for (I = 0; I < 719; I++)
                {
                    if (Flgg[I] == 1)
                    {
                        SmC2 += Cs[I] * Cs[I];
                        SmS2 = +Sn[I] * Sn[I];
                        Rho[I] = Math.Pow(Math.Pow(Xb[I] - Xc, 2) + Math.Pow(Yb[I] - Yc, 2), 2);
                        SmR += Rho[I];
                        SmRC += Rho[I] * Cs[I];
                        SmRs += Rho[I] * Sn[I];
                    }
                }
                A0 = SmR / 720;
                A1 = SmRC / SmC2;
                A2 = SmRs / SmS2;
                J = 0;
                for (I = 0; I < 719; I++)
                {
                    dR = A0 + A1 * Cs[I] + A2 * Sn[I] - Rho[I];
                    if (dR > .03 * A0)
                    {
                        Flgg[I] = 0;
                    }
                    else
                    {
                        Flgg[I] = 1;
                    }
                    j++;
                }
            }
            //

            temp = new byte[Wm, Hm];
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] > 0)
                    {
                        temp[X, Y] = 254;
                        c[X, Y] = Color.FromArgb(128, 128, 128, 128);
                    }
                    else
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.FromArgb(0, 0, 0, 0);
                    }
                }
            }
            //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < bitmap.Height; i++)
                {
                    for (j = 0; j < bitmap.Width; j++)
                    {
                        p[2] = c[j, i].R;
                        p[1] = c[j, i].G;
                        p[0] = c[j, i].B;
                        // updating arrya of colors for future work
                        c[j, i] = Color.FromArgb(p[2], p[1], p[0]);
                        p += 3;
                    }
                    p += remain;
                }
            }
            bitmap.UnlockBits(bmpdata);

            return bitmap;
        }

        private static void SeedFilling(byte C, byte RC)
        {
            //replace small segments of C color by RC color

            //
            Wm = width;
            Hm = height;
            N = 0;

            Mask2 = new byte[Wm, Hm];

            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    N += Mask2[X, Y];

                }
            }
            //
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == C)
                    {
                        J = 0;
                        I = 0;
                        Xb = new int[300000];
                        Yb = new int[300000];
                        Xb[I] = X;
                        Yb[I] = Y;
                        Mask2[X, Y] = 2;
                        while (J <= I)
                        {
                            Xx = Xb[J];
                            Yy = Yb[J];
                            Xm = Xx - 1;
                            Xp = Xx + 1;
                            Ym = Yy - 1;
                            Yp = Yy + 1;
                            if (Xp <= Wm)
                            {
                                if (Mask2[Xp, Yy] == C)
                                {
                                    I++;
                                    Xb[I] = Xp;
                                    Yb[I] = Yy;
                                    Mask2[Xp, Yy] = 2;
                                }
                            }
                            //
                            if (Xm >= 0)
                            {
                                if (Mask2[Xm, Yy] == C)
                                {
                                    I++;
                                    Xb[I] = Xm;
                                    Yb[I] = Yy;
                                    Mask2[Xm, Yy] = 2;
                                }
                            }
                            //
                            if (Yp <= Hm)
                            {
                                if (Mask2[Xx, Yp] == C)
                                {
                                    I++;
                                    Xb[I] = Xx;
                                    Yb[I] = Yp;
                                    Mask2[Xx, Yp] = 2;
                                }
                            }
                            //
                            if (Ym >= 0)
                            {
                                if (Mask2[Xx, Ym] == C)
                                {
                                    I++;
                                    Xb[I] = Xx;
                                    Yb[I] = Ym;
                                    Mask2[Xx, Ym] = 2;
                                }
                            }
                            //
                            j++;
                        }// while end
                        //
                        if (J < 0.1 * N)
                        {
                            for (I = 0; I < J; I++)
                            {
                                Mask2[Xb[I], Yb[I]] = RC;
                            }
                        }
                    }
                }
            }
            //
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == 2)
                    {
                        Mask2[X, Y] = C;
                        c[X, Y] = Color.FromArgb(255 * Mask2[X, Y]);
                    }
                }
            }
        }
    }
}
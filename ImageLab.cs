using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Iris
{
    public class ImageLab
    {
        static int Hgt, Wid;
        //byte[] Mask1;   
        //byte[] Mask2;

        static byte Gry;
        static long L;
        static byte G, C;
        static int Hm, HmN, I, Wm, WmN, X1, X2, Xx, Y1, Y2, Yy, Nn;
        static int Max, Min, Flg;
        static float Sm, Sm2, Ang, One;
        static double Rat, Xc, Yc, Rad, SmC2, SmS2, SmR, SmRC, SmRS, A0, A1, A2, dR;
        static int X, Y;
        static float[] Cs;
        static float[] Sn;
        static int Xm, Xp, Ym, Yp;
        static int[] Xb;
        static int[] Yb;
        static long J, N;
        static byte[] Flgg;
        static float[] Rho;



        //byte Tbl[];
        static double R, R1, R2, Cc, Ss, T;
        static float S;




        static byte[] Tbl;
        static byte[] cl = new byte[2];
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
                        Red[j, i] = p[2];
                        Grn[j, i] = p[1];
                        Blu[j, i] = p[0];
                        c[j, i] = Color.FromArgb(p[2], p[1], p[0]);
                        gray[j, i] = c[j, i].B;
                        //Convert.ToByte(p[2] * .299 + p[1] * .587 + p[0] * .114);
                        // p[0];
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
            His = new long[256];
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
            S = 1.0f / (Max - Min);

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
                        c[j, i] = Color.White;
                    }
                    else
                    {
                        Mask1[j, i] = 0;
                        c[j, i] = Color.Black;
                    }
                    temp[j, i] = cl[Mask1[j, i]];
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
                for (j = X1; j <= X2; j++)
                {
                    if (Mask1[j, Y2] == 1)
                        Flg = 1;
                }
                if (Flg == 0)
                    Y2 -= 1;
            }

            WmN = X2 - X1;
            HmN = Y2 - Y1;
            var bitmap1 = CropImage(bitmap, WmN + 1, HmN + 1);


            byte[,] tRed = new byte[WmN + 1, HmN + 1];
            byte[,] tGrn = new byte[WmN + 1, HmN + 1];
            byte[,] tBlu = new byte[WmN + 1, HmN + 1];
            byte[,] tMask1 = new byte[WmN + 1, HmN + 1];

            for (Y = Y1; Y < Y2; Y++)
            {
                Yy = Y - Y1;
                for (X = X1; X < X2; X++)
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
            Wm = WmN + 1;
            Hm = HmN + 1;

            Red = new byte[WmN + 1, HmN + 1];
            Blu = new byte[WmN + 1, HmN + 1];
            Grn = new byte[WmN + 1, HmN + 1];
            Mask1 = new byte[WmN + 1, HmN + 1];


            for (Y = 0; Y < HmN + 1; Y++)
            {
                for (X = 0; X < WmN + 1; X++)
                {
                    Mask1[X, Y] = tMask1[X, Y];
                    Red[X, Y] = tRed[X, Y];
                    Grn[X, Y] = tGrn[X, Y];
                    Blu[X, Y] = tBlu[X, Y];
                }
            }

            Array.Clear(tMask1, 0, tMask1.Length);
            Array.Clear(tRed, 0, tRed.Length);
            Array.Clear(tGrn, 0, tGrn.Length);
            Array.Clear(tBlu, 0, tBlu.Length);
            //
            Array.Clear(His, 0, His.Length);
            Array.Clear(Tbl, 0, Tbl.Length);

            // Stage 3 Gamma = 1.5
            His = new long[256];
            Tbl = new byte[256];
            N = 0;
            Sm = 0;
            Sm2 = 0;


            for (i = 0; i < HmN + 1; i++)
            {
                for (j = 0; j < WmN + 1; j++)
                {
                    if (Mask1[j, i] == 1)
                    {
                        G = gray[j, i];
                    }
                    N += 1;
                    Sm += G;
                    Sm2 += Convert.ToInt32(G) * G;
                }
            }


            Sm /= N;
            Sm2 = Sm2 / N - Sm * Sm;
            Sm2 = (float)Math.Sqrt(Sm2);
            Min = (int)(Sm - (1.2 * Sm2));

            if (Min < 20)
            {
                Min = 19;
            }

            Max = (int)(Sm + 2 * Sm2);

            if (Max > 200)
                Max = 200;

            S = 1.0f / (Max - Min);

            double ff;

            for (I = 0; I < 256; I++)
            {
                if (I < Min)
                    Tbl[I] = 0;
                else if (I >= Max)
                    Tbl[I] = 255;
                else
                {
                    ff = (255 * Math.Pow((S * (I - Min)), 1.5f));
                    Tbl[I] = (byte)ff;
                }
            }


            Mask2 = new byte[WmN + 1, HmN + 1];

            for (i = 0; i < HmN; i++)
                for (j = 0; j < WmN; j++)
                {
                    Mask2[j, i] = 0;
                    c[j, i] = Color.FromArgb(0, 0, 0);
                    if (Mask1[j, i] == 1)
                    {
                        G = gray[j, i];
                        I = Tbl[G];
                        if (I > 80)
                        {
                            Mask2[j, i] = 0;
                            c[j, i] = Color.Black;
                        }
                        else
                        {
                            Mask2[j, i] = 1;
                            c[j, i] = Color.White;
                        }

                    }

                }


            var bmpdata = bitmap1.LockBits(new Rectangle(0, 0, WmN + 1, HmN + 1), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                stride = bmpdata.Stride;
                remain = stride - bitmap1.Width * 3;
                var p = (byte*)(void*)scan0;
                for (i = 0; i < HmN + 1; i++)
                {
                    for (j = 0; j < WmN + 1; j++)
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
            bitmap1.UnlockBits(bmpdata);

            return bitmap1;
        }

        // fourth stage
        public static Bitmap ThirdStage(Bitmap bitmap)
        {
            Cs = new float[720 + 1]; Sn = new float[720 + 1]; One = (float)(Math.PI / 180);
            for (I = 0; I <= 720; I++) { Ang = (float)(One * I * 0.5); Cs[I] = (float)Math.Cos(Ang); Sn[I] = (float)Math.Sin(Ang); } // I

            N = 0; Xc = 0; Yc = 0;
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == 1) { N += 1; Xc += X; Yc += Y; }
                } // X /*? : */			
            } // Y
            Xc /= N; Yc /= N; Rad = (float)Math.Sqrt(N / Math.PI);
            R1 = Convert.ToInt16(0.45 * Rad); R2 = Convert.ToInt16(1.55 * Rad);
            Xb = new int[719 + 1]; Yb = new int[719 + 1];
            for (I = 0; I <= 719; I++)
            {
                for (J = Convert.ToInt32(R1); J <= Convert.ToInt32(R2); J++)
                {
                    X = Convert.ToInt32(Xc + J * Cs[I]); Y = Convert.ToInt32(Yc + J * Sn[I]);
                    if (X >= 0 && X <= Wm && Y >= 0 && Y <= Hm)
                    {
                        if (Mask2[X, Y] == 0) { Mask2[X, Y] = 2; J = Convert.ToInt32(R2); Xb[I] = X; Yb[I] = Y; }
                    }
                    else
                    {
                        J = Convert.ToInt32(R2);
                    }
                } // J
            } // I

            Flgg = new byte[719 + 1]; Rho = new float[719 + 1]; for (I = 0; I <= 719; I++) { Flgg[I] = 1; } // I
            for (int M = 1; M <= 3; M++)
            {
                SmC2 = 0; SmS2 = 0; SmR = 0; SmRC = 0; SmRS = 0;
                for (I = 0; I <= 719; I++)
                {
                    if (Flgg[I] == 1)
                    {
                        SmC2 += Cs[I] * Cs[I]; SmS2 += Sn[I] * Sn[I];
                        Rho[I] = (float)Math.Sqrt(Convert.ToInt32(Math.Pow((Xb[I] - Xc), 2)) + Convert.ToInt32(Math.Pow((Yb[I] - Yc), 2)));
                        SmR += Rho[I]; SmRC += Rho[I] * Cs[I];
                        SmRS += Rho[I] * Sn[I];
                    }
                } // I
                A0 = SmR / 720; A1 = SmRC / SmC2; A2 = SmRS / SmS2;

                J = 0;
                for (I = 0; I <= 719; I++)
                {
                    dR = (float)(A0 + A1 * Cs[I] + A2 * Sn[I] - Rho[I]);
                    if (dR > 0.03 * A0) Flgg[I] = 0; else { Flgg[I] = 1; J += 1; }
                } // I
                  // Debug.Print J; A0; A1; A2
            } // M

            temp = new byte[Wm + 1, Hm + 1];
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    // If Mask2(X, Y) > 0 Then L = RGB(128, 128, 128) Else L = 0
                    // Form1.PSet (X, Y), L
                    if (Mask2[X, Y] > 0)
                    {
                        temp[X, Y] = 254;
                        c[X, Y] = Color.FromArgb(254);
                    }
                    else
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.FromArgb(0);
                    }
                } // X /*? : */			
            } // Y

            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
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

        public static Bitmap SeedFilling(Bitmap bitmap, byte C, byte RC)
        {
            byte Flg; int I, J, N;
            int X, Xm, Xp, Xx, Y;
            int Ym, Yp, Yy; int[] Xb = null; int[] Yb = null;

            N = 0;
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++) { N += Mask2[X, Y]; }
            }

            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == C)
                    {
                        J = 0; I = 0; Xb = new int[300000 + 1]; Yb = new int[300000 + 1];
                        Xb[I] = X; Yb[I] = Y; Mask2[X, Y] = 2;
                        while (J <= I)
                        {
                            Xx = Xb[J]; Yy = Yb[J]; Xm = Xx - 1; Xp = Xx + 1; Ym = Yy - 1; Yp = Yy + 1;
                            if (Xp < Wm)
                            {
                                if (Mask2[Xp, Yy] == C) { I += 1; Xb[I] = Xp; Yb[I] = Yy; Mask2[Xp, Yy] = 2; }
                            }
                            if (Xm >= 0)
                            {
                                if (Mask2[Xm, Yy] == C) { I += 1; Xb[I] = Xm; Yb[I] = Yy; Mask2[Xm, Yy] = 2; }
                            }
                            if (Yp < Hm)
                            {
                                if (Mask2[Xx, Yp] == C) { I += 1; Xb[I] = Xx; Yb[I] = Yp; Mask2[Xx, Yp] = 2; }
                            }
                            if (Ym >= 0)
                            {
                                if (Mask2[Xx, Ym] == C) { I += 1; Xb[I] = Xx; Yb[I] = Ym; Mask2[Xx, Ym] = 2; }
                            }
                            J += 1;
                        }

                        if (J < 0.1 * N)
                        {
                            for (I = 0; I <= J; I++) { Mask2[Xb[I], Yb[I]] = RC; }
                        }
                    }
                }
            }

            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Mask2[X, Y] == 2)
                    {
                        Mask2[X, Y] = C;
                    }

                    int cc = Mask2[X, Y] * 255;

                    if (cc >= 255)
                    {
                        cc = 255;
                    }

                    c[X, Y] = Color.FromArgb(cc, cc, cc);
                    // Form1.PSet (X, Y), 16777215 * Mask2(X, Y)
                } // X /*? : */			} // Y

            }

            //

            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
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

        public static Bitmap CropImage(Bitmap bitmap, int w, int h)
        {
            var bmp = bitmap;
            var rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            var rawOriginal = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int origByteCount = rawOriginal.Stride * rawOriginal.Height;
            byte[] origBytes = new byte[origByteCount];
            Marshal.Copy(rawOriginal.Scan0, origBytes, 0, origByteCount);

            //I want to crop a 100x100 section starting at 15, 15.
            int startX = 0;
            int startY = 0;
            int width = w;
            int height = h;
            int BPP = 3;        //4 Bpp = 32 bits, 3 = 24, etc.

            byte[] croppedBytes = new byte[width * height * BPP];

            //Iterate the selected area of the original image, and the full area of the new image
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width * BPP; j += BPP)
                {
                    int origIndex = (startX * rawOriginal.Stride) + (i * rawOriginal.Stride) + (startY * BPP) + (j);
                    int croppedIndex = (i * width * BPP) + (j);

                    //copy data: once for each channel
                    for (int k = 0; k < BPP; k++)
                    {
                        croppedBytes[croppedIndex + k] = origBytes[origIndex + k];
                    }
                }
            }

            //copy new data into a bitmap
            var croppedBitmap = new Bitmap(width, height);
            var croppedData = croppedBitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(croppedBytes, 0, croppedData.Scan0, croppedBytes.Length);

            bmp.UnlockBits(rawOriginal);
            croppedBitmap.UnlockBits(croppedData);
            return croppedBitmap;
        }

        public static Bitmap FourthStage(Bitmap bitmap)
        {
            //''''''Stage - 5: Determine the Outer Boundary
            Nn = 360 * 7; One = (float)(2 * Math.PI / Nn);
            for (I = 0; I <= Nn; I++)
            {
                T = One * I; Cc = (float)Math.Cos(T); Ss = (float)Math.Sin(T);
                R = (float)(A0 + A1 * Cc + A2 * Ss); Xx = Convert.ToInt32(Xc + R * Cc); Yy = Convert.ToInt32(Yc + R * Ss);
                if (I == 0)
                {
                    c[Xx, Yy] = Color.White;
                }
                else
                {
                    c[Xx, Yy] = Color.White;
                }
                // If I = 0 Then Form1.PSet (Xx, Yy), 255 Else Form1.Line -(Xx, Yy), 255
                temp[Xx, Yy] = 255; // : Debug.Print Cc, Ss;
            } // I
            Xx = Convert.ToInt32(Wm / 2.0); Yy = Convert.ToInt32(Hm / 2.0);
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X <= Xx; X++)
                {
                    if (temp[X, Y] != 255)
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.Black;
                    }
                    else X = Xx;
                } // X /*? : */			
            } // Y
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = Wm; X >= Xx; X--)
                {
                    if (temp[X, Y] != 255)
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.Black;
                    }
                    else X = Xx;
                } // X /*? : */			
            } // Y
            for (X = 0; X < Wm; X++)
            {
                for (Y = 0; Y <= Yy; Y++)
                {
                    if (temp[X, Y] != 255)
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.Black;
                    }
                    else Y = Yy;
                } // Y /*? : */			
            } // X
            for (X = 0; X < Wm; X++)
            {
                for (Y = Hm-1; Y > Yy; Y--)
                {
                    if (temp[X, Y] != 255)
                    {
                        temp[X, Y] = 0;
                        c[X, Y] = Color.Black;
                    }
                    else Y = Yy;
                } // Y /*? : */			
            } // X
              //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
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

        public static Bitmap FifthStage(Bitmap bitmap)
        {
            // Stage-6: Remove the Outer Part that Found Outside the Iris Region
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (temp[X, Y] != 254)
                    {
                        Blu[X, Y] = 0; Grn[X, Y] = 0; Red[X, Y] = 0;
                    }
                } // X /*? : */			
            } // Y
              //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
                    {
                        p[2] = Red[j, i]; //c[j, i].R;
                        p[1] = Grn[j, i]; //c[j, i].G;
                        p[0] = Blu[j, i]; //c[j, i].B;
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

        public static Bitmap SixthStage(Bitmap bitmap)
        {
            Sm = 0; N = 0; gray = new byte[Wm + 1, Hm + 1];
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    I = 0;
                    if (Mask2[X, Y] == 1)
                    {
                        if (Red[X, Y] > Grn[X, Y]) Max = Red[X, Y]; else Max = Grn[X, Y];
                        if (Red[X, Y] < Grn[X, Y]) Min = Red[X, Y]; else Min = Grn[X, Y];
                        if (Max < Blu[X, Y]) Max = Blu[X, Y];
                        if (Min > Blu[X, Y]) Min = Blu[X, Y];
                        I = (Max - Min) * 4; if (I > 255) I = 255;
                        gray[X, Y] = (byte)I; Sm += I; Sm2 += I * I; N += 1;
                        // Form1.PSet (X, Y), I
                    }
                } // X /*? : */			
            } // Y
            Sm /= N; Sm2 = Sm2 / N - Sm * Sm; Sm -= 0.75f * (float)Math.Sqrt(Sm2);
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    temp[X, Y] = 0;
                    if (Mask2[X, Y] == 1)
                    {
                        if (gray[X, Y] < Sm) temp[X, Y] = 253;
                    }
                    // Form1.PSet (X, Y), Tmp(X, Y)
                } // X /*? : */			
            } // Y

            //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
                    {
                        p[2] = 0;// temp[j, i]; //c[j, i].R;
                        p[1] = temp[j, i]; //c[j, i].G;
                        p[0] = 0;// temp[j, i]; //c[j, i].B;
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

        public static Bitmap SeventhStage(Bitmap bitmap)
        {
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (temp[X, Y] > 0) temp[X, Y] = 1;
                } // X /*? : */			
            } // Y
            for (J = 0; J <= 3; J++)
            {
                for (Y = 1; Y < Hm ; Y++)
                {
                    for (X = 1; X < Wm ; X++)
                    {
                        gray[X, Y] = 0;
                        if (temp[X, Y] == 1)
                        {
                            L = 0; X1 = X - 1; X2 = X + 1; Y1 = Y - 1; Y2 = Y + 1;
                            for (Yy = Y1; Yy <= Y2; Yy++)
                            {
                                for (Xx = X1; Xx <= X2; Xx++)
                                {
                                    L += temp[Xx, Yy];
                                } // Xx /*? : */							
                            } // Yy
                            if (L > 1) gray[X, Y] = 1;
                        }
                    } // X /*? : */				
                } // Y
                for (Y = 0; Y < Hm; Y++)
                {
                    for (X = 0; X < Wm; X++) { temp[X, Y] = gray[X, Y]; } // X /*? : */ 
                } // Y
            } // J
            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (temp[X, Y] == 1) temp[X, Y] = 255;
                    // Form1.PSet (X, Y), Tmp(X,Y)
                } // X /*? : */			
            } // Y

            //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
                    {
                        p[2] = temp[j, i]; //c[j, i].R;
                        p[1] = temp[j, i]; //c[j, i].G;
                        p[0] = temp[j, i]; //c[j, i].B;
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

        public static Bitmap EighthStage(Bitmap bitmap)
        {
            float Alpha = 1.2f;
            double Rr, dX, dY;
            for (I = 1; I <= 4; I++)
            {
                Xc = 0; Yc = 0; N = 0;
                for (Y = 0; Y < Hm; Y++)
                {
                    for (X = 0; X < Wm; X++)
                    {
                        if (temp[X, Y] > 0) { temp[X, Y] = 1; N += 1; Xc += X; Yc += Y; }
                    } // X /*? : */				
                } // Y
                Xc /= N; Yc /= N; R = (float)(Alpha * Math.Sqrt(N / Math.PI)); Rr = R * R;
                for (Y = 0; Y < Hm; Y++)
                {
                    for (X = 0; X < Wm; X++)
                    {
                        if (temp[X, Y] > 0)
                        {
                            dX = X - Xc; dY = Y - Yc; if (dX * dX + dY * dY > Rr) temp[X, Y] = 0;
                        }
                    } // X /*? : */				
                } // Y
                Alpha *= 0.9f; if (Alpha < 1) Alpha = 1;
            } // I
            for (Y = 0; Y <= Hm; Y++)
            {
                X1 = Wm; X2 = 0;
                for (X = 0; X < Wm; X++)
                {
                    if (temp[X, Y] > 0) { X1 = X; X = Wm; }
                } // X
                for (X = Wm; X >= 0; X--)
                {
                    if (temp[X, Y] > 0) { X2 = X; X = 0; }
                } // X
                for (X = X1; X <= X2; X++) { temp[X, Y] = 1; } // X
            } // Y
            for (X = 0; X <= Wm; X++)
            {
                Y1 = Hm; Y2 = 0;
                for (Y = 0; Y < Hm; Y++)
                {
                    if (temp[X, Y] > 0) { Y1 = Y; Y = Hm; }
                } // Y
                for (Y = Hm; Y >= 0; Y--)
                {
                    if (temp[X, Y] > 0) { Y2 = Y; Y = 0; }
                } // Y
                for (Y = Y1; Y <= Y2; Y++) { temp[X, Y] = 1; } // Y
            } // X

            for (Y = 0; Y < Hm; Y++)
            {
                for (X = 0; X < Wm; X++)
                {
                    if (Red[X, Y] > 0 || Grn[X, Y] > 0 || Blu[X, Y] > 0)
                    {
                        if (temp[X, Y] > 0) { Red[X, Y] = 0; Grn[X, Y] = 0; Blu[X, Y] = 0; }
                    }
                } // X /*? : */			
            } // Y

            //
            var bmpdata = bitmap.LockBits(new Rectangle(0, 0, Wm, Hm), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            var scan0 = bmpdata.Scan0;
            unsafe
            {
                var p = (byte*)(void*)scan0;
                for (i = 0; i < Hm; i++)
                {
                    for (j = 0; j < Wm; j++)
                    {
                        p[2] = Red[j, i]; //c[j, i].R;
                        p[1] = Grn[j, i]; //c[j, i].G;
                        p[0] = Blu[j, i]; //c[j, i].B;
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

    }
}
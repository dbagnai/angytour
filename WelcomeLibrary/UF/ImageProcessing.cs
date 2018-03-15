using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Data;
using AForge.Imaging;
using AForge;

namespace WelcomeLibrary.UF
{
    public class CSharpMask
    {
        public int TopLeft = 0, TopMiddle = 0, TopRight = 0;
        public int MiddleLeft = 0, Pixel = 1, MiddleRight = 0;
        public int BottomLeft = 0, BottomMiddle = 0, BottomRight = 0;
        public int Factor = 1;
        public int Offset = 0;

        public void setAll(int nVal)
        {
            TopLeft = TopMiddle = TopRight = MiddleLeft = Pixel = MiddleRight = BottomLeft = BottomMiddle = BottomRight = nVal;
        }
        public CSharpMask()
        {
            //
            // TODO: Add constructor logic here
            //
        }
    }


    public class ImageProcessing
    {


#if false

        public static bool Median(Bitmap b)
        {
            ///
            ///It removes any kind of any Noise present in the image
            ///
            Bitmap b2 = (Bitmap)b.Clone();

            // GDI+ still lies to us - the return format is BGR, NOT RGB.
            BitmapData bmData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;
            ArrayList list;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width * 3;

                p += stride;
                p2 += stride;

                for (int y = 1; y < b.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        list = new ArrayList();
                        list.Add(p2[0]);
                        list.Add((p2 - 3)[0]);
                        list.Add((p2 + 3)[0]);
                        list.Add((p2 - stride)[0]);
                        list.Add((p2 - stride + 3)[0]);
                        list.Add((p2 - stride - 3)[0]);

                        list.Add((p2 + stride)[0]);
                        list.Add((p2 + stride + 3)[0]);
                        list.Add((p2 + stride - 3)[0]);

                        list.Sort();
                        p[0] = (byte)list[4];

                        ++p;
                        ++p2;
                    }

                    p += nOffset + 3;
                    p2 += nOffset + 3;
                }
            }

            b.UnlockBits(bmData);
            b2.UnlockBits(bmData2);

            return true;
        }
        public static Bitmap MeanFilter(Bitmap b, int nWidth)
        {
            CSharpMask m = new CSharpMask();
            m.setAll(1);
            m.Pixel = nWidth;
            m.Factor = 9;
            m.Offset = 0;

            return simpleConv(b, m);
        }

        public static Bitmap GaussianSmoothing(Bitmap b, int nWidth)
        {
            CSharpMask m = new CSharpMask();
            m.setAll(2);
            m.Pixel = nWidth;
            m.Factor = 16;
            m.Offset = 0;
            m.BottomLeft = 1;
            m.BottomRight = 1;
            m.TopLeft = 1;
            m.TopRight = 1;

            return simpleConv(b, m);
        }

        public static Bitmap simpleConv(Bitmap b, CSharpMask m)
        {
            if (m.Factor == 0)
                return b;

            Bitmap OutPutImage;
            OutPutImage = (Bitmap)b.Clone();
            BitmapData bData = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            BitmapData copyData = b.LockBits(new Rectangle(0, 0, OutPutImage.Width, OutPutImage.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            int stride = bData.Stride;
            int stride2 = stride * 2;

            System.IntPtr ptr = bData.Scan0;
            System.IntPtr ptrOutPut = copyData.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)ptr;
                byte* pOutPut = (byte*)(void*)ptrOutPut;

                int nOffset = stride - b.Width * 3;
                int nWidth = b.Width - 2;
                int nHeight = b.Height - 2;
                int nPixel;

                for (int x = 0; x < nWidth; x++)
                    for (int y = 0; y < nHeight; y++)
                    {
                        /*
                        *Now we will process the three pixels R,G and B
                        *in a single Pass				 
                        * */
                        nPixel = (((pOutPut[2] * m.TopLeft) + (pOutPut[5] * m.TopMiddle) + (pOutPut[8] * m.TopRight) +
                            (pOutPut[stride + 2] * m.MiddleLeft) + (pOutPut[stride + 5] * m.Pixel) + (pOutPut[stride + 8] * m.MiddleRight)
                            + (pOutPut[stride2 + 2] * m.BottomLeft) + (pOutPut[stride2 + 5] * m.BottomMiddle) + (pOutPut[stride2 + 8] * m.BottomRight)) / m.Factor) + m.Offset;

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        pOutPut[5 + stride] = (byte)nPixel;
                        nPixel = ((((pOutPut[1] * m.TopLeft) + (pOutPut[4] * m.TopMiddle) +
                            (pOutPut[7] * m.TopRight) + (pOutPut[1 + stride] * m.MiddleLeft) +
                            (pOutPut[4 + stride] * m.Pixel) + (pOutPut[7 + stride] * m.MiddleRight) +
                            (pOutPut[1 + stride2] * m.BottomLeft) +
                            (pOutPut[4 + stride2] * m.BottomMiddle) +
                            (pOutPut[7 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[4 + stride] = (byte)nPixel;

                        nPixel = ((((pOutPut[0] * m.TopLeft) + (pOutPut[3] * m.TopMiddle) +
                            (pOutPut[6] * m.TopRight) + (pOutPut[0 + stride] * m.MiddleLeft) +
                            (pOutPut[3 + stride] * m.Pixel) +
                            (pOutPut[6 + stride] * m.MiddleRight) +
                            (pOutPut[0 + stride2] * m.BottomLeft) +
                            (pOutPut[3 + stride2] * m.BottomMiddle) +
                            (pOutPut[6 + stride2] * m.BottomRight))
                            / m.Factor) + m.Offset);

                        if (nPixel < 0) nPixel = 0;
                        if (nPixel > 255) nPixel = 255;
                        p[3 + stride] = (byte)nPixel;

                        p += 3;
                        pOutPut += 3;
                    }

                p += nOffset;
                pOutPut += nOffset;
            }

            b.UnlockBits(bData);
            OutPutImage.UnlockBits(copyData);

            return OutPutImage;
        }
        public static bool NoiseRemoval(Bitmap IntensityImage)
        {

            /*It removes the pixel
             * that is stood alone any where in the 
             * vicinity.
             *It is found to be accurate 4 our System.
             * */

            Bitmap b2 = (Bitmap)IntensityImage.Clone();
            byte val;
            // GDI+ still lies to us - the return format is BGR, NOT RGIntensityImage.
            BitmapData bmData = IntensityImage.LockBits(new Rectangle(0, 0, IntensityImage.Width, IntensityImage.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData bmData2 = b2.LockBits(new Rectangle(0, 0, IntensityImage.Width, IntensityImage.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            int stride = bmData.Stride;
            System.IntPtr Scan0 = bmData.Scan0;
            System.IntPtr Scan02 = bmData2.Scan0;

            unsafe
            {
                byte* p = (byte*)(void*)Scan0;
                byte* p2 = (byte*)(void*)Scan02;

                int nOffset = stride - IntensityImage.Width * 3;
                int nWidth = IntensityImage.Width * 3;

                //int nPixel=0;

                p += stride;
                p2 += stride;
                //int val;
                for (int y = 1; y < IntensityImage.Height - 1; ++y)
                {
                    p += 3;
                    p2 += 3;

                    for (int x = 3; x < nWidth - 3; ++x)
                    {
                        val = p2[0];
                        if (val == 0)
                            if ((p2 + 3)[0] == 0 || (p2 - 3)[0] == 0 || (p2 + stride)[0] == 0 || (p2 - stride)[0] == 0 || (p2 + stride + 3)[0] == val || (p2 + stride - 3)[0] == 0 || (p2 - stride - 3)[0] == 0 || (p2 + stride + 3)[0] == 0)
                                p[0] = val;
                            else
                                p[0] = 255;

                        ++p;
                        ++p2;
                    }

                    p += nOffset + 3;
                    p2 += nOffset + 3;
                }
            }

            IntensityImage.UnlockBits(bmData);
            b2.UnlockBits(bmData2);
            return true;
        }

#endif

        public static Bitmap applicaMediano(System.Drawing.Bitmap image, int size)
        {

            AForge.Imaging.Filters.Median filter = new AForge.Imaging.Filters.Median(size);
            // apply filter
            System.Drawing.Bitmap newImage = filter.Apply(image);
            return newImage;
        }

        public static Bitmap applicaMedio(System.Drawing.Bitmap image)
        {

            AForge.Imaging.Filters.Mean filter = new AForge.Imaging.Filters.Mean();
            // apply filter
            System.Drawing.Bitmap newImage = filter.Apply(image);
            return newImage;
        }

        public static Bitmap applicaBrightness(System.Drawing.Bitmap image, double br)
        {
            AForge.Imaging.Filters.BrightnessCorrection filter = new AForge.Imaging.Filters.BrightnessCorrection(br);
            //System.Drawing.Bitmap newImage = filter.Apply(image);
            //return newImage;
            // apply the filter
            filter.ApplyInPlace(image);
            return image;

        }

        public static Bitmap applicaContrast(System.Drawing.Bitmap image, double cr)
        {
            AForge.Imaging.Filters.ContrastCorrection filter = new AForge.Imaging.Filters.ContrastCorrection(cr);
            //System.Drawing.Bitmap newImage = filter.Apply(image);
            //return newImage;
            //// apply the filter
            filter.ApplyInPlace(image);
            return image;
        }

        public static Bitmap applicaContrastStretch(System.Drawing.Bitmap image)
        {
            AForge.Imaging.Filters.ContrastStretch filter = new AForge.Imaging.Filters.ContrastStretch();
            //System.Drawing.Bitmap newImage = filter.Apply(image);
            //return newImage;
            //// apply the filter
            filter.ApplyInPlace(image);
            return image;
        }

        public static Bitmap applicaAdaptiveSmoothing(System.Drawing.Bitmap image)
        {

            System.Drawing.Bitmap newImage = null;
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                newImage = AForge.Imaging.Image.Clone(image, PixelFormat.Format24bppRgb);
            }
            else
                newImage = image;
            AForge.Imaging.Filters.AdaptiveSmoothing f1 = new AForge.Imaging.Filters.AdaptiveSmoothing();
            newImage = f1.Apply(newImage);
            return newImage;
            //AForge.Imaging.Filters.AdaptiveSmoothing f1 = new AForge.Imaging.Filters.AdaptiveSmoothing();
            //f1.ApplyInPlace(image);
            //return image;
        }

        public static Bitmap applicaConservativeSmoothing(System.Drawing.Bitmap image)
        {
            AForge.Imaging.Filters.ConservativeSmoothing f1 = new AForge.Imaging.Filters.ConservativeSmoothing();
            f1.ApplyInPlace(image);
            return image;
        }

        public static Bitmap applicaGaussianBlur(System.Drawing.Bitmap image, double sigma, int size)
        {
            AForge.Imaging.Filters.GaussianBlur f1 = new AForge.Imaging.Filters.GaussianBlur(sigma, size);
            f1.ApplyInPlace(image);
            return image;
        }

        public static Bitmap applicaBlur(System.Drawing.Bitmap image)
        {
            AForge.Imaging.Filters.Blur f1 = new AForge.Imaging.Filters.Blur();
            f1.ApplyInPlace(image);
            return image;
        }
        public static Bitmap applicaSaturationCorrection(System.Drawing.Bitmap image, double cr)
        {
            // create filter
            AForge.Imaging.Filters.SaturationCorrection filter = new AForge.Imaging.Filters.SaturationCorrection(cr);
            //System.Drawing.Bitmap newImage = filter.Apply(image);
            //return newImage;
            // apply the filter
            filter.ApplyInPlace(image);
            return image;
        }
        public static Bitmap applicaHSLFilter(System.Drawing.Bitmap image, double lu, double sat)
        {
            // create filter
            AForge.Imaging.Filters.HSLLinear filter = new AForge.Imaging.Filters.HSLLinear();
            // configure the filter
            filter.InLuminance = new DoubleRange(0, lu);
            filter.OutSaturation = new DoubleRange(sat, 1);
            // apply the filter
            filter.ApplyInPlace(image);
            return image;
        }
        public static Bitmap applicaResizeBicubic(System.Drawing.Bitmap image, Int32 width, int height)
        {
            System.Drawing.Bitmap newImage = null;
            if (image.PixelFormat != PixelFormat.Format24bppRgb)
            {
                newImage = AForge.Imaging.Image.Clone(image, PixelFormat.Format24bppRgb);
            }
            else
                newImage = image;
            AForge.Imaging.Filters.ResizeBicubic f1 = new AForge.Imaging.Filters.ResizeBicubic(width, height);
            newImage = f1.Apply(newImage);
            return newImage;
        }
        public static Bitmap applicaResizeBilinear(System.Drawing.Bitmap image, Int32 width, int height)
        {
            AForge.Imaging.Filters.ResizeBilinear f1 = new AForge.Imaging.Filters.ResizeBilinear(width, height);
            System.Drawing.Bitmap newImage = f1.Apply(image);
            return newImage;
        }
        public static Bitmap applicaResizeNN(System.Drawing.Bitmap image, Int32 width, int height)
        {
            AForge.Imaging.Filters.ResizeNearestNeighbor f1 = new AForge.Imaging.Filters.ResizeNearestNeighbor(width, height);
            System.Drawing.Bitmap newImage = f1.Apply(image);
            return newImage;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace Fishbowl
{
    // Utility function class.
    class FishUtil
    {
        public static Random random = new Random();

        public static bool containsPoint(Bubble bubble, Point point)
        {
            Bubble.Point bubblepos = bubble.getPosition();
            return (bubblepos.x - point.X) * (bubblepos.x - point.X) + (bubblepos.y - point.Y) * (bubblepos.y - point.Y)
                < bubble.getRadius() * bubble.getRadius();
        }

        public static double ClampDouble(double n, double low, double high)
        {
            if (n < low) n = low;
            if (n > high) n = high;
            return n;
        }

        // Taken from http://www.splinter.com.au/converting-hsv-to-rgb-colour-using-c/

        /// <summary>
        /// Convert HSV to RGB
        /// h is from 0-360
        /// s,v values are 0-1
        /// r,g,b values are 0-255
        /// Based upon http://ilab.usc.edu/wiki/index.php/HSV_And_H2SV_Color_Space#HSV_Transformation_C_.2F_C.2B.2B_Code_2
        /// </summary>
        public static void HsvToRgb(double h, double S, double V, out byte r, out byte g, out byte b)
        {
            double H = h;
            while (H < 0) { H += 360; };
            while (H >= 360) { H -= 360; };
            double R, G, B;
            if (V <= 0)
            { R = G = B = 0; }
            else if (S <= 0)
            {
                R = G = B = V;
            }
            else
            {
                double hf = H / 60.0;
                int i = (int)Math.Floor(hf);
                double f = hf - i;
                double pv = V * (1 - S);
                double qv = V * (1 - S * f);
                double tv = V * (1 - S * (1 - f));
                switch (i)
                {

                    // Red is the dominant color

                    case 0:
                        R = V;
                        G = tv;
                        B = pv;
                        break;

                    // Green is the dominant color

                    case 1:
                        R = qv;
                        G = V;
                        B = pv;
                        break;
                    case 2:
                        R = pv;
                        G = V;
                        B = tv;
                        break;

                    // Blue is the dominant color

                    case 3:
                        R = pv;
                        G = qv;
                        B = V;
                        break;
                    case 4:
                        R = tv;
                        G = pv;
                        B = V;
                        break;

                    // Red is the dominant color

                    case 5:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                    case 6:
                        R = V;
                        G = tv;
                        B = pv;
                        break;
                    case -1:
                        R = V;
                        G = pv;
                        B = qv;
                        break;

                    // The color is not defined, we should throw an error.

                    default:
                        //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                        R = G = B = V; // Just pretend its black/white
                        break;
                }
            }
            r = (byte)(Clamp((int)(R * 255.0)));
            g = (byte)(Clamp((int)(G * 255.0)));
            b = (byte)(Clamp((int)(B * 255.0)));
        }

        private static int Clamp(int i)
        {
            if (i < 0) return 0;
            if (i > 255) return 255;
            return i;
        }

        // http://www.javascripter.net/faq/rgb2hsv.htm
        public static void RgbToHsv(int ri, int gi, int bi, out double computedH, out double computedS, out double computedV)
        {
            computedH = 0;
            computedS = 0;
            computedV = 0;

            ri = Clamp(ri);
            gi = Clamp(gi);
            bi = Clamp(bi);

            double r = (double)ri / 255;
            double g = (double)gi / 255;
            double b = (double)bi / 255;

            double minRGB = Math.Min(r, Math.Min(g, b));
            double maxRGB = Math.Max(r, Math.Max(g, b));

            // Black-gray-white
            if (minRGB == maxRGB)
            {
                computedV = minRGB;
                return;
            }

            // Colors other than black-gray-white
            double d = (r == minRGB) ? g - b : ((b == minRGB) ? r - g : b - r);
            double h = (r == minRGB) ? 3 : ((b == minRGB) ? 1 : 5);
            computedH = 60 * (h - d / (maxRGB - minRGB));
            computedS = (maxRGB - minRGB) / maxRGB;
            computedV = maxRGB;

        }

        // http://www.switchonthecode.com/tutorials/javascript-interactive-color-picker

        public static void PosToHsv(double x, double y, out double s, out double v)
        {
            s = x / 255;
            v = 1 - y / 255;
        }

        public static void HsvToPos(double s, double v, out double x, out double y)
        {
            x = s * 255;
            y = (1 - v) * 255;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using NeuralNetwork1;

namespace NeuralNetwork1
{
    public class SamplerConverter
    {
        public SamplerConverter()
        {

        }

        public SamplesSet ConvertSet(string path)
        {
            SamplesSet set = new SamplesSet();
            for(int i = 0; i < 300; i++)
            {
                for(int j = 0; j < 3000; j+=300)
                {
                    if (i == 0 && j == 0)
                        continue;
                    Bitmap cur_img = new Bitmap(path + (i + j).ToString() + ".jpg");
                    set.AddSample(Convert(cur_img,(FigureType)(j / 300)));
                }
            }
            return set;
        }

        public static Sample Convert(Bitmap bitmap, FigureType type = FigureType.Undef)
        {
            int square_count_x = 20;
            int square_count_y = 20;
            int square_width = 100 / square_count_x;
            int square_height = 100 / square_count_y;
            double[] input = new double[square_count_x * square_count_y];
            for (int i = 0; i < square_count_x; i++)
            {
                for (int j = 0; j < square_count_y; j++)
                {
                    for (int k = 0; k < square_width; k++)
                    {
                        for (int m = 0; m < square_height; m++)
                        {
                            Color cur_pix = bitmap.GetPixel(i * square_width + k, j * square_height + m);
                            if (cur_pix.R >= 127)
                            {
                                input[i * square_count_x + j] += 1;
                            }
                        }
                    }
                }
            }
            return new Sample(input, 10, type);
        }

        //public static Sample Convert(Bitmap bitmap, FigureType type = FigureType.Undef)
        //{


        //    //int square_count_x = 20;
        //    //int square_count_y = 20;
        //    //int square_width = 100 / square_count_x;
        //    //int square_height = 100 / square_count_y;
        //    double[] input = new double[200];
        //    bool col = false; // false - black, white - true
        //    for (int i = 0; i < 100; i++)
        //    {
        //        Color cur_pix = bitmap.GetPixel(i, 0);
        //        if (cur_pix.R > 127)
        //            col = true;
        //        for (int j = 0; j < 100; j++)
        //        {
        //            cur_pix = bitmap.GetPixel(i, j);
        //            if ((col && cur_pix.R <= 127) || (!col && cur_pix.R > 127))
        //            {
        //                input[i] += 1;
        //                col = !col;
        //            }
        //        }
        //    }
        //    for (int i = 0; i < 100; i++)
        //    {
        //        Color cur_pix = bitmap.GetPixel(0, i);
        //        if (cur_pix.R >= 127)
        //            col = true;
        //        for (int j = 0; j < 100; j++)
        //        {
        //            cur_pix = bitmap.GetPixel(j, i);
        //            if ((col && cur_pix.R <= 127) || (!col && cur_pix.R > 127))
        //            {
        //                input[i + 100] += 1;
        //                col = !col;
        //            }
        //        }
        //    }
        //    return new Sample(input, 10, type);
        //}

    }
}

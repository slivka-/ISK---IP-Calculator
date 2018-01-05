namespace IP_Calculator.ManualCalculations
{
    using IP_Calculator.DataVisualization;
    using IP_Calculator.ManualCalculations;
    using System;
    using System.Collections.ObjectModel;

    static class Draw
    {
        public static String createFile(ObservableCollection<ManualDataRow> a)
        {
            int iteration = 0;
            string text2 = "";
            string newLine = System.Environment.NewLine;
            string text1 = "object=Router;" + newLine + "type=Router;" + newLine + "x1=588;" + newLine + "y1=329;" + newLine + "lnk1=;" + newLine +
                    "x2=668;" + newLine + "y2=409;" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine;

            foreach (ManualDataRow item in a)
            {
                Char delimiter = '.';
                String[] minH = item.HostMin.Split(delimiter);
                String[] maxH = item.HostMax.Split(delimiter);

                int sx1 = 0;
                int sy1 = 0;
                int ex1 = 0;
                int ey1 = 0;
                int e1x1 = 0;
                int e1y1 = 0;

                if (iteration == 0)
                {
                    sx1 = 425;
                    sy1 = 358;

                    ex1 = 323;
                    ey1 = 234;

                    e1x1 = 323;
                    e1y1 = 424;
                }
                else if (iteration == 1)
                {
                    sx1 = 721;
                    sy1 = 350;

                    ex1 = 853;
                    ey1 = 234;

                    e1x1 = 853;
                    e1y1 = 424;
                }
                else if (iteration == 2)
                {
                    sx1 = 568;
                    sy1 = 492;

                    ex1 = 493;
                    ey1 = 594;

                    e1x1 = 683;
                    e1y1 = 594;
                }
                else if (iteration == 3)
                {
                    sx1 = 568;
                    sy1 = 196;

                    ex1 = 493;
                    ey1 = 64;

                    e1x1 = 683;
                    e1y1 = 64;
                }

                int Emitter1 = Int32.Parse(minH[3]) + 1;
                int Emitter2 = Int32.Parse(maxH[3]);

                text2 = text2 + "object=Switch_" + iteration + ";" + newLine + "type=Receiver;" + newLine + "x1=" + sx1.ToString() + ";" + newLine + "y1=" + sy1.ToString() + ";" + newLine +
                       "lnk1=;" + newLine + "x2=" + (sx1 + 120).ToString() + ";" + newLine + "y2=" + (sy1 + 30).ToString() + ";" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine +

                       "object=" + minH[2] + "." + Emitter1 + ";" + newLine + "type=Emitter;" + newLine + "x1=" + ex1.ToString() + ";" + newLine + "y1=" + ey1.ToString() + ";" + newLine +
                       "lnk1=;" + newLine + "x2=" + (ex1 + 80).ToString() + ";" + newLine + "y2=" + (ey1 + 80).ToString() + ";" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine +

                      "object=" + item.HostMin + "/" + item.NetAddressSize + ";" + newLine + "type=Line;" + newLine + "x1=932;" + newLine + "y1=279;" + newLine +
                       "lnk1=Router;" + newLine + "x2=490;" + newLine + "y2=205;" + newLine + "lnk2=Switch_" + iteration + ";" + newLine + "end object." + newLine + newLine +

                      "object=;" + newLine + "type=Line;" + newLine + "x1=436;" + newLine + "y1=349;" + newLine + "lnk1=Switch_" + iteration + ";" + newLine +
                       "x2=628;" + newLine + "y2=369;" + newLine + "lnk2=" + minH[2] + "." + Emitter1 + ";" + newLine + "end object." + newLine + newLine;

                if ((Int32.Parse(minH[3]) + 1 != Int32.Parse(maxH[3])))
                {
                    //Console.WriteLine(item.HostMax);

                    text2 = text2 + "object=" + minH[2] + "." + Emitter2 + ";" + newLine + "type=Emitter;" + newLine + "x1=" + e1x1.ToString() + ";" + newLine + "y1=" + e1y1.ToString() + ";" + newLine + "lnk1=;" + newLine +
                       "x2=" + (e1x1 + 80).ToString() + ";" + newLine + "y2=" + (e1y1 + 80).ToString() + ";" + newLine + "lnk2 =;" + newLine + "end object." + newLine + newLine +

                       "object=;" + newLine + "type=Line;" + newLine + "x1=436;" + newLine + "y1=349;" + newLine + "lnk1=Switch_" + iteration + ";" + newLine +
                       "x2=628;" + newLine + "y2=369;" + newLine + "lnk2=" + minH[2] + "." + Emitter2 + ";" + newLine + "end object." + newLine + newLine;
                }

                iteration++;

            }
            return text1+text2;
        }




        public static String createFile2(ObservableCollection<ManualDataRow> a)
        {
            int iteration = 0;
            string text2 = "";
            string newLine = System.Environment.NewLine;

            int sx1 = 80;
            int sy1 = 284;
            int ex1 = 30;
            int ey1 = 467;
            int e1x1 = 120;

            foreach (ManualDataRow item in a)
            {
                Char delimiter = '.';
                String[] minH = item.HostMin.Split(delimiter);
                String[] maxH = item.HostMax.Split(delimiter);

                int Emitter1 = Int32.Parse(minH[3]) + 1;
                int Emitter2 = Int32.Parse(maxH[3]);

                

                text2 = text2 + "object=Switch_" + iteration + ";" + newLine + "type=Receiver;" + newLine + "x1=" + sx1.ToString() + ";" + newLine + "y1=" + sy1.ToString() + ";" + newLine +
                       "lnk1=;" + newLine + "x2=" + (sx1 + 120).ToString() + ";" + newLine + "y2=" + (sy1 + 30).ToString() + ";" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine +

                       "object=" + minH[2] + "." + Emitter1 + ";" + newLine + "type=Emitter;" + newLine + "x1=" + ex1.ToString() + ";" + newLine + "y1=" + ey1.ToString() + ";" + newLine +
                       "lnk1=;" + newLine + "x2=" + (ex1 + 80).ToString() + ";" + newLine + "y2=" + (ey1 + 80).ToString() + ";" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine +

                      "object=" + item.HostMin + "/" + item.NetAddressSize + ";" + newLine + "type=Line;" + newLine + "x1=932;" + newLine + "y1=279;" + newLine +
                       "lnk1=Router;" + newLine + "x2=490;" + newLine + "y2=205;" + newLine + "lnk2=Switch_" + iteration + ";" + newLine + "end object." + newLine + newLine +

                      "object=;" + newLine + "type=Line;" + newLine + "x1=436;" + newLine + "y1=349;" + newLine + "lnk1=Switch_" + iteration + ";" + newLine +
                       "x2=628;" + newLine + "y2=369;" + newLine + "lnk2=" + minH[2] + "." + Emitter1 + ";" + newLine + "end object." + newLine + newLine;

                if ((Int32.Parse(minH[3]) + 1 != Int32.Parse(maxH[3])))
                {
                    //Console.WriteLine(item.HostMax);

                    text2 = text2 + "object=" + minH[2] + "." + Emitter2 + ";" + newLine + "type=Emitter;" + newLine + "x1=" + e1x1.ToString() + ";" + newLine + "y1=" + ey1.ToString() + ";" + newLine + "lnk1=;" + newLine +
                       "x2=" + (e1x1 + 80).ToString() + ";" + newLine + "y2=" + (ey1 + 80).ToString() + ";" + newLine + "lnk2 =;" + newLine + "end object." + newLine + newLine +

                       "object=;" + newLine + "type=Line;" + newLine + "x1=436;" + newLine + "y1=349;" + newLine + "lnk1=Switch_" + iteration + ";" + newLine +
                       "x2=628;" + newLine + "y2=369;" + newLine + "lnk2=" + minH[2] + "." + Emitter2 + ";" + newLine + "end object." + newLine + newLine;
                }
                sx1 = sx1 + 160;
                ex1 = ex1 + 160;
                e1x1 = ex1 + 80;
                iteration++;

                if(iteration%2 != 0)
                {
                    sy1 = sy1 + 30;
                }
                else
                {
                    sy1 = sy1 - 30;
                }
                
            }
            
            string text1 = "object=Router;" + newLine + "type=Router;" + newLine + "x1="+ ((sx1 + 80) / 2) + ";" + newLine + "y1=80;" + newLine + "lnk1=;" + newLine +
                    "x2="+ (((sx1 + 80) / 2)+80) + ";" + newLine + "y2=160;" + newLine + "lnk2=;" + newLine + "end object." + newLine + newLine;

            
            return text1 +text2;

        }
    }
}

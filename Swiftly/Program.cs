using System;

namespace Swiftly
{
    /***     Copyright 2020 MAJE Software. All Rights Reserved.                            ***/
    /***                                                                                   ***/
    /*** Permission is hereby granted, free of charge, to any person obtaining a copy of   ***/
    /*** this software and associated documentation files (the "Software"), to deal in the ***/
    /*** Software without restriction, including without limitation the rights to use,     ***/
    /*** copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the   ***/
    /*** Software, and to permit persons to whom the Software is furnished to do so,       ***/
    /*** subject to the following conditions:                                              ***/
    /***                                                                                   ***/
    /***                                                                                   ***/ 
    /*** The above copyright notice and this permission notice shall be included in all    ***/
    /*** copies or substantial portions of the Software.                                   ***/
    /***                                                                                   ***/
    /*** THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR        ***/
    /*** IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS  ***/
    /*** FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR    ***/
    /*** COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN ***/
    /*** AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION   ***/
    /*** WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.                   ***/

    class Program
    {
        /*********************************************************************************/
        /***     Parsing Constants                                                     ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/

        const int VALID_PARSE = 0;
        const int INVALID_PARSE = -1;

        /*********************************************************************************/
        /***     Parse Input String                                                    ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static int parse_input_string(string line)
        {
            try
            {
                decimal singularPrice = 0;
                decimal promoSingularPrice = 0;
                decimal splitPrice = 0;
                decimal promoSplitPrice = 0;

                string productId = line.Substring(0, 8);
                string productDescription = line.Substring(9, 58).Trim();

                decimal.TryParse(line.Substring(69, 8).ToString(), out singularPrice);
                singularPrice /= 100;
                decimal.TryParse(line.Substring(78, 8).ToString(), out promoSingularPrice);
                promoSingularPrice /= 100;
                decimal.TryParse(line.Substring(87, 8).ToString(), out splitPrice);
                splitPrice /= 100;
                decimal.TryParse(line.Substring(96, 8).ToString(), out promoSplitPrice);
                promoSplitPrice /= 100;

                string regular = line.Substring(105, 8);
                string promo = line.Substring(114, 8);
                string flags = line.Substring(123, 9);
                string productSize = line.Substring(133, 9).Trim();
            } catch
            {
                return INVALID_PARSE;
            }

            return VALID_PARSE;

        }

        /*********************************************************************************/
        /***     Load Input Data                                                       ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void load_input_file()
        {
            int intParse = -1;
            int intLineNum = 0;

            string[] lines = System.IO.File.ReadAllLines(@"C:\Users\marty\Source\Repos\Swiftly\Swiftly\input.txt");
         
            System.Console.WriteLine("Contents of input file = ");
            foreach (string line in lines)
            {
                intParse = parse_input_string(line);

                if (intParse == INVALID_PARSE)
                {
                    Console.WriteLine("Input line could not be parsed: " + line);
                } else
                {
                    Console.WriteLine("Input line parsed successfully!! " + line);
                }    
            }
        }

        /*********************************************************************************/
        /***     Main Routine                                                          ***/
        /***                                                                           ***/
        /***     Copyright 2020 MAJE Software. All Rights Reserved.                    ***/
        /*********************************************************************************/
        static void Main(string[] args)
        {
            load_input_file();
        }
    }
}

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading.Tasks;



namespace ExploringDataTypes

{

    class Program

    {

        static void Main(string[] args)

        {

            int integerTest = 0;

            decimal usedForCurrency = 0;

            float aDifferentNumberFormat = 0.01234567890F;

            double andYetAnotherOne = 0.01234567890D;

            bool boolTest = true;

            int aDifferentInteger = 1 + 3;

            bool aDifferentBool = (usedForCurrency == integerTest);

            string aTextTest = "Hi everyone!";

            int myStringLen = aTextTest.Length;

            string aDifferentString = "Learning C# is fun!";



            Console.WriteLine("integerTest's value is " + integerTest

                + ", usedForCurrency holds " + usedForCurrency

                + ", aDifferentNumberFormat's value is " + aDifferentNumberFormat

                + ", while andYetAnotherOne stores " + andYetAnotherOne);

            Console.WriteLine();



            Console.WriteLine(aTextTest);

            Console.WriteLine(aTextTest.ToUpper());

            Console.WriteLine(aTextTest.Substring(0, 2));

            Console.WriteLine("The first 'e' in '" + aTextTest + "' is in position " + aTextTest.IndexOf("e"));

            Console.WriteLine("The last 'e' in '" + aTextTest + "' is in position " + aTextTest.LastIndexOf("e"));

            Console.WriteLine("The total length of the string is " + myStringLen);

            // Inserting text on an existing string, just for output

            Console.WriteLine("Using the Insert method to add some text results in: " + aDifferentString.Insert(15, "easy and "));

            // But it doesn't affect the original string

            Console.WriteLine("But the original string remains untouched: " + aDifferentString);

            // Now let's change the value, by assigning the function output to the original variable

            aDifferentString = aDifferentString.Insert(15, "easy and ");

            Console.WriteLine("Now aDifferentString has a different value: " + aDifferentString);

            // Append some text

            aDifferentStringOnSteroids.Append(" But sometimes is quirky...");

            Console.WriteLine();

            Console.WriteLine("aDifferentStringOnSteroids after the Append:\n"

                               + aDifferentStringOnSteroids);

            // Insert some text

            aDifferentStringOnSteroids.Insert(46, "very ");

            Console.WriteLine();

            Console.WriteLine("aDifferentStringOnSteroids after the Insert:\n"

                               + aDifferentStringOnSteroids);

            // Replace some text

            aDifferentStringOnSteroids.Replace("very", "extremely");

            Console.WriteLine();

            Console.WriteLine("aDifferentStringOnSteroids after the Replace:\n"

                               + aDifferentStringOnSteroids);

            // Remove some text

            aDifferentStringOnSteroids.Remove(28, 37);

            Console.WriteLine();

            Console.WriteLine("aDifferentStringOnSteroids after the Remove:\n"

                               + aDifferentStringOnSteroids);




            Console.ReadKey();

        }

    }

}

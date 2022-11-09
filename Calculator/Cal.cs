using System;

namespace Calculator
{
    public class Cal
    {
        public int FirstNumber { get; set; }
        public int SecondNumber { get; set; }

        public int Add()
        {
            return FirstNumber + SecondNumber;
        }

        public int Substarct()
        {
            return FirstNumber - SecondNumber;
        }
    }
}

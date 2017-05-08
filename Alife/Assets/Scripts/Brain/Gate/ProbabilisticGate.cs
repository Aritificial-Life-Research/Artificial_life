using System.Collections.Generic;
using System.Linq;
using Common;
using System;


namespace Gate
{
    public  class ProbabilisticGate : BaseBehaviour
    {


        List<int> inputs;  
        List<int> outputs;

        private int _id;
        public List<List<double>> table;

        //default constructor
        public ProbabilisticGate(List<int> inputsAddress,List<int> outputsAddress, List<List<double>> rawTable,int id)
        {
            _id = id;
            int i, j;
            inputs = inputsAddress;
            outputs = outputsAddress;

            int numInputs = inputs.Count();
            int numOutputs = outputs.Count();

            ListExtras.Resize(table,2^numInputs);
            //normalize each row
            for (i = 0; i < (2^numInputs); i++)
            {  //for each row (each possible input bit string)
                ListExtras.Resize(table[i],(2^numOutputs));
                // first sum the row
                double S = 0;
                for (j = 0; j < (2^numOutputs); j++)
                {
                    S += rawTable[i][j];
                }
                // now normalize the row
                if (S == 0.0)
                {  //if all the inputs on this row are 0, then give them all a probability of 1/(2^(number of outputs))
                    for (j = 0; j < (2^numOutputs); j++)
                        table[i][j] = 1.0 / (2 ^ numOutputs);
                }
                else
                {  //otherwise divide all values in a row by the sum of the row
                    for (j = 0; j < (2^numOutputs); j++)
                        table[i][j] = rawTable[i][j] / S;
                }
            }
        }


        void update(ref List<double> nodes, ref List<double>  nextNodes)
        {  //this translates the input bits of the current states to the output bits of the next states
            int input = Utillity.vectorToBitToInt(ref nodes,inputs, true); // converts the input values into an index (true indicates to reverse order)
            int outputColumn = 0;
            Random random= new Random();
            double r = random.NextDouble();  // r will determine with set of outputs will be chosen
            while (r > table[input][outputColumn])
            {
                r -= table[input][outputColumn];  // this goes across the probability table in row for the given input and subtracts each
                                                  // value in the table from r until r is less than a value it reaches
                outputColumn++;  // we have not found the correct output so move to the next output
            }
            //convert the int outputcolumn to a binary (2->010)
            for (int i = 0; i < outputs.Count(); i++)  //for each output...
                nextNodes[outputs[i]] += 1.0 * ((outputColumn >> (outputs.Count() - 1 - i)) & 1);  // convert output (the column number) to bits and pack into next states
                                                                                                  // but always put the last bit in the first input (to maintain consistancy)
        }

    }

}

public static class ListExtras
{
    //    list: List<T> to resize
    //    size: desired new size
    // element: default value to insert

    public static void Resize<T>(List<T> list, int size, T element = default(T))
    {
        int count = list.Count;

        if (size < count)
        {
            list.RemoveRange(size, count - size);
        }
        else if (size > count)
        {
            if (size > list.Capacity)   // Optimization
                list.Capacity = size;

            list.AddRange(Enumerable.Repeat(element, size - count));
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Program
    {

                                             //0  1                  1 2
        public static int Dig(int[] arr, int selected_Cell, int temp_layer)
        {
            int layer= temp_layer, temp_lay;

            for (int j = selected_Cell+1; j < arr.Length; j++)
            {

                if (arr[selected_Cell] < arr[j])
                {
                    temp_lay = Dig(arr, j, temp_layer+1);
                    if (layer < temp_lay) { layer = temp_lay;}
                    //else {  layer = temp_layer; }
                }
                else { if (layer < temp_layer) { layer = temp_layer; } }
            }

            return layer;
        }


        static void Main(string[] args)
        {
            Console.Write("수열의 길이 : ");
            int length = int.Parse(Console.ReadLine());

            int[] arr = new int[length];
            for (int i = 0; i < length; i++) { Console.WriteLine((i+1)+"번 숫자 : "); arr[i] = int.Parse(Console.ReadLine()); }


            int layer = 1;
            int temp_layer;

            for (int i = 0;i < length; i++)
            {
                temp_layer = Dig(arr, i, 1);

                if(temp_layer > layer) { layer = temp_layer; }
            }

            Console.WriteLine($"가장 긴 오름차순 부분 수열의 길이 = {layer}");
        }
    }
}

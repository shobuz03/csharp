﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventDelegate
{
    /// <summary>
    /// This is subscriber class
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            AddTwoNumbers a=new AddTwoNumbers();
            //Event gets binded with delegates
            a.EventOddNumber+=new AddTwoNumbers.DelegateOddNumber(EventMessage);//(4)
            a.Add();
            Console.ReadKey();
        }
        /// <summary>
        /// Delegate calls this metheod when event raised.
        /// </summary>
        static void EventMessage()//(5)
        {
            Console.WriteLine("Event executed : This is odd number.");
        }
    }
}

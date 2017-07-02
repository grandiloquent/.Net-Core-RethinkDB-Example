using System;
using System.Collections;
using RethinkDb.Driver.Ast;

namespace EverStore.Libraries
{
    public class Logger
    {
        public bool Enable { get; set; }
        public string Namespace { get; set; }

        public Logger()
        {
            Enable = true;
        }
        public void L(string str)
        {
            if (Enable)
                Console.WriteLine($"{Namespace} => {str}");
        }

        public void L(object obj)
        {
            if (Enable)
                Console.WriteLine($"{Namespace} => {obj}");
        }

        public static Logger GetNewInstance()
        {
            return new Logger();
        }
    }
}
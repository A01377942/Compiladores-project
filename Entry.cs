/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/
using System;
using System.Collections.Generic;

namespace QuetzalDragon
{
    class Entry
    {

        readonly bool isPrimitive;

        readonly int arity;

        //private ISet<string> lst;


        public bool IsPrimitive
        {
            get { return isPrimitive; }
        }

        public int Arity
        {
            get { return arity; }

        }

        public ISet<string> Lst
        {
            get;
            set;
        }

        public Entry(bool isPrimitive, int arity, ISet<string> lst)
        {
            this.isPrimitive = isPrimitive;
            this.arity = arity;
            this.Lst = lst;

        }
        /**/
        private string lstToString()
        {
            var result = "";
            if (Lst != null)
            {

                foreach (string value in Lst)
                {
                    result += value;
                    result += ", ";
                }
            }
            return result;
        }
        public string[] arrayLst(){
            
            var result = new string[Lst.Count];
            var index=0;
            if (Lst != null)
            {

                foreach (string value in Lst)
                {
                   result[index]=value;
                   index++;
                }
            }
            return result;

        }
        

        public override string ToString()
        {
            //return $"{{{category}, \"{lexeme}\", @({row}, {column})}}";
            return $"isPrimitive: {isPrimitive}, arity: {arity}, lst: {lstToString()}";
        }
    }
}
/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

using System;
using System.Collections.Generic;

namespace QuetzalDragon
{

    class SemanticVisitor2
    {



        //-----------------------------------------------------------
        public IDictionary<string, Entry> Fgst
        {
            get;
            private set;
        }


        public ISet<string> Vgst
        {
            get;
            private set;
        }

        public ISet<string> local_Lst
        {
            get;
            private set;
        }
        //-----------------------------------------------------------
        public SemanticVisitor2(IDictionary<string, Entry> Fgst, ISet<string> Vgst)
        {
            this.Fgst = Fgst;
            this.Vgst = Vgst;
            local_Lst = new HashSet<string>();
        }


        //-----------------------------------------------------------
        public void Visit(Program node)
        {
            Visit((dynamic)node[0]);

        }

        //-----------------------------------------------------------
        public void Visit(Def_list node)
        {
            VisitChildrenInDefList(node);

        }

        //funciona tanto para argumentos de funcion como para declaracion de variables.
        public void Visit(VarDefList node)
        {

            foreach (var n in node)
            {

                var variableName = n.AnchorToken.Lexeme;

                if (local_Lst.Contains(variableName))
                {
                    throw new SemanticError(
                        "Duplicated variable in LST: " + variableName,
                        n.AnchorToken);

                }
                else
                {
                    local_Lst.Add(variableName);
                }

            }
        }
        public void Visit(Fun_Def node)
        {
            foreach (var n in node)
            {
                // //si resulta que la funcion si tiene parametros, entonces los lee.
                // if (n.AnchorToken == null)
                // {
                //     Visit((dynamic)node[0]);
                // }

                Visit((dynamic)n);

            }

        }
        public int VisitArgumentsDef(VarDefList node)
        {
            //si VarDefList en realidad esta definiendo variables, entonces
            //regresa 0 parametros
            if (node.AnchorToken != null)
            {
                return 0;
            }

            return node.NumberChildrens;

        }

        //-----------------------------------------------------------
        void VisitChildren(Node node)
        {
            foreach (var n in node)
            {
                Visit((dynamic)n);
            }
        }

        void VisitChildrenInDefList(Node node)
        {
            foreach (var n in node)
            {
                //Solo se lee Fun_Def porque las variables globales ya fueron leidas
                //en la primer pasada
                if (n.GetType().Name.Equals("Fun_Def"))
                {

                    Visit((dynamic)n);

                    //obtener nombre de la funcion
                    var nombreFun = n.AnchorToken.Lexeme;

                    //Asignar el LST local dentro del Fgst table
                    Fgst[nombreFun].Lst = local_Lst;
                    //limpiar el local_Lst
                    local_Lst = new HashSet<string>();

                    //Console.WriteLine(n.GetType().Name);
                }

            }
        }
        //-----------------------------------------------------------

    }
}

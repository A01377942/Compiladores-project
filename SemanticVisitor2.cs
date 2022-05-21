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

        //Variables globales
        public ISet<string> Vgst
        {
            get;
            private set;
        }
        //Variables locales
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

        public void Visit(SUBSTRACTION node)
        {

            Visit((dynamic)node[0]);

            Visit((dynamic)node[1]);
        }
        public void Visit(FUN_CALL node)
        {
            var nodoIdentificador = node[0];
            var nombreFuncion = nodoIdentificador.AnchorToken.Lexeme;

            //checar si existe funcion
            if (Fgst.ContainsKey(nombreFuncion))
            {

                //checar numero de aurgumentos permitidos
                int numeroParametros = Fgst[nombreFuncion].Arity;

                Visit_Expr_List((dynamic)node[1], numeroParametros);
            }
            else
            {

                throw new SemanticError("funcion doesnt exist: " + nombreFuncion,
                                        node[0].AnchorToken);
            }
        }

        public void Visit_Expr_List(Expr_List node, int numeroParametros)
        {
            if (node.NumberChildrens == numeroParametros)
            {
                VisitChildren(node);
            }
            else
            {
                throw new SemanticError(
                                        "function doesnt have correct number of arguments: ");
            }
        }

        public void Visit(Identifier node)
        {
            //Buscar si ya se definio de manera loca o global la varibable.

            var nombreVariable = node.AnchorToken.Lexeme;
            //se busca de manera local
            if (local_Lst.Contains(nombreVariable))
            {


            }
            //se busca de manera global
            else if (Vgst.Contains(nombreVariable))
            {

            }
            //Variable no existe
            else
            {
                throw new SemanticError(
                                     "Variable doesnt exist: " + nombreVariable,
                                     node.AnchorToken);
            }
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

                Visit((dynamic)n);

            }

        }
        public void Visit(Stmt_Loop node)
        {

            Visit((dynamic)node[0]);
        }

        public void Visit(Stmt_If node)
        {

            //Visit((dynamic)node[0]);
            VisitChildren(node);
        }
        public void Visit(Stmt_List node)
        {

            VisitChildren(node);
        }

        public void Visit(IntLiteral node)
        {
            int result;
            if (!Int32.TryParse(node.AnchorToken.Lexeme, out result))
            {
                throw new SemanticError(
                                  "int is not 32 bits: " + node.AnchorToken.Lexeme,
                                  node.AnchorToken);
            }
        }

        public void Visit(Assign node)
        {
            //Buscar si ya se definio de manera loca o global la varibable.
            var nodoIdentificador = node[0];
            var nombreVariable = nodoIdentificador.AnchorToken.Lexeme;

            //se busca de manera local
            if (local_Lst.Contains(nombreVariable))
            {

                Visit((dynamic)node[1]);
            }
            //se busca de manera global
            else if (Vgst.Contains(nombreVariable))
            {
                Visit((dynamic)node[1]);
            }
            //Variable no existe
            else
            {
                throw new SemanticError(
                                     "Variable doesnt exist: " + nombreVariable,
                                     nodoIdentificador.AnchorToken);
            }

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

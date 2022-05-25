/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

using System;
using System.Collections.Generic;

namespace QuetzalDragon
{

    class SemanticVisitor1
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
        //-----------------------------------------------------------
        public SemanticVisitor1()
        {
            Fgst = new Dictionary<string, Entry>();
            Vgst = new HashSet<string>();//new SortedDictionary<string, Entry>();
            createPrimitiveFunctions();
        }

        private void createPrimitiveFunctions()
        {
            Fgst["printi"] = new Entry(true, 1, null);
            Fgst["printc"] = new Entry(true, 1, null);
            Fgst["prints"] = new Entry(true, 1, null);
            Fgst["println"] = new Entry(true, 0, null);
            Fgst["readi"] = new Entry(true, 0, null);
            Fgst["reads"] = new Entry(true, 0, null);
            Fgst["new"] = new Entry(true, 1, null);
            Fgst["size"] = new Entry(true, 1, null);
            Fgst["add"] = new Entry(true, 2, null);
            Fgst["get"] = new Entry(true, 2, null);
            Fgst["set"] = new Entry(true, 3, null);
        }

        public void verificarMain(){

            if(!Fgst.ContainsKey("main")){
                throw new SemanticError(
                        "program doesnt have main function defined");
            }
            if(Fgst["main"].Arity>0){
  throw new SemanticError(
                        "main function has one o more parameters");
            }
        }

        //-----------------------------------------------------------
        public void Visit(Program node)
        {
            Visit((dynamic)node[0]);
            verificarMain();
        }

        //-----------------------------------------------------------
        public void Visit(Def_list node)
        {
            VisitChildren(node);

        }

        public void Visit(VarDefList node)
        {
            //VisitChildren(node);
            foreach (var n in node)
            {
                //Visit((dynamic)n);
                var variableName = n.AnchorToken.Lexeme;
                if (Vgst.Contains(variableName))
                {
                    throw new SemanticError(
                        "Duplicated variable: " + variableName,
                        n.AnchorToken);

                }
                else
                {
                    Vgst.Add(variableName);
                }

            }
        }
        public void Visit(Fun_Def node)
        {
            var variableName = node.AnchorToken.Lexeme;
            var arity = VisitArgumentsDef((dynamic)node[0]);
            // var variableName = node[0].AnchorToken.Lexeme;

            if (Fgst.ContainsKey(variableName))
            {
                throw new SemanticError(
                    "Duplicated function: " + variableName,
                    node.AnchorToken);

            }
            else
            {
                Fgst[variableName] = new Entry(false, arity, null);
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

        //-----------------------------------------------------------

    }
}

/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

using System;
using System.Text;
using System.Collections.Generic;

namespace QuetzalDragon
{

    class WatVisitor
    {

        //IDictionary<string, Type> table;
        //Variables globales
        public ISet<string> Vgst
        {
            get;
            private set;
        }
        public IDictionary<string, Entry> Fgst
        {
            get;
            private set;
        }

        public WatVisitor(IDictionary<string, Entry> Fgst, ISet<string> Vgst)
        {
            this.Fgst = Fgst;
            this.Vgst = Vgst;
        }

        public string GenerateLabel()
        {
            return string.Format("${0:00000}", labelCounter++);
        }

        private string currentFunction = "";
        private bool areVariablesSet = false;

        private bool isReturn = false;
        int labelCounter = 0;


        //ADD_Z
        public static IList<int> AsCodePoints(string str)
        {
            var result = new List<int>(str.Length);
            for (var i = 0; i < str.Length; i++)
            {
                result.Add(char.ConvertToUtf32(str, i));
                if (char.IsHighSurrogate(str, i))
                {
                    i++;
                }
            }
            return result;
        }

        public string Visit(Program node)
        {
            return ";; WebAssembly text format code generated by "
                + "the QuetzalDragon compiler.\n\n"
                + "(module\n"
                + "  (import \"quetzal\" \"printi\" (func $printi (param i32) (result i32)))\n"
                + "  (import \"quetzal\" \"printc\" (func $printc (param i32) (result i32)))\n"
                + "  (import \"quetzal\" \"prints\" (func $prints (param i32) (result i32)))\n"
                + "  (import \"quetzal\" \"println\" (func $println (result i32)))\n"
                + "  (import \"quetzal\" \"readi\" (func $readi (result i32)))\n"
                + "  (import \"quetzal\" \"reads\" (func $reads (result i32)))\n"
                + "  (import \"quetzal\" \"new\" (func $new (param i32) (result i32)))\n"
                + "  (import \"quetzal\" \"size\" (func $size (param i32) (result i32)))\n"
                + "  (import \"quetzal\" \"add\" (func $add (param i32 i32) (result i32)))\n"
                + "  (import \"quetzal\" \"get\" (func $get (param i32 i32) (result i32)))\n"
                + "  (import \"quetzal\" \"set\" (func $set (param i32 i32 i32) (result i32)))\n"
                + declareGlobalVariables()
                + Visit((dynamic)node[0])
                + "  )\n"
                + ")\n";
        }

        public string Visit(Def_list node)
        {

            return VisitChildrenInDefList(node);

        }

        public string declareGlobalVariables()
        {

            //si hay variables globales, las declaras en wat
            if (Vgst.Count > 0)
            {
                var sb = new StringBuilder();
                foreach (var entry in Vgst)
                {
                    sb.Append($"  (global ${entry} (mut i32) (i32.const 0))\n");

                }
                return sb.ToString();
            }
            return "\n";
        }


        public string VisitChildrenInDefList(Node node)
        {

            var sb = new StringBuilder();
            foreach (var n in node)
            {
                if (n.GetType().Name.Equals("Fun_Def"))
                {

                    currentFunction = n.AnchorToken.Lexeme;
                    sb.Append(Visit((dynamic)n));
                    areVariablesSet = false;
                    isReturn = false;
                }

            }
            return sb.ToString();
        }

        public string Visit(Fun_Def node)
        {
            var sb = new StringBuilder();
            var funtionName = node.AnchorToken.Lexeme;
            //si la funcion no es main, la define
            if (!funtionName.Equals("main"))
            {
                sb.Append($"  (func ${funtionName}\n");

                //Agrega parametros
                var parametrosArray = Fgst[funtionName].arrayLst();
                for (int i = 0; i < Fgst[funtionName].Arity; i++)
                {
                    sb.Append($"  (param ${parametrosArray[i]} i32)\n");
                }
                sb.Append($"  (result i32)\n");
                sb.Append($"   (local $_temp i32)\n");

                //Agregar cuerpo funcion
                foreach (var n in node)
                {

                    sb.Append(Visit((dynamic)n));

                }
                if (isReturn == false)
                {
                    sb.Append("   i32.const 0\n");
                    sb.Append("   return\n");
                }

                sb.Append("  )\n");
            }
            //si es  main
            else
            {
                sb.Append("  (func\n");
                sb.Append("    (export \"main\")\n");
                sb.Append("    (result i32)\n");
                sb.Append($"   (local $_temp i32)\n");
                //Agregar cuerpo funcion
                foreach (var n in node)
                {

                    sb.Append(Visit((dynamic)n));

                }
                if (isReturn == false)
                {
                    sb.Append("  i32.const 0\n");
                }

            }




            return sb.ToString();

        }

        public string Visit(Stmt_List node)
        {

            return VisitChildren(node);
        }

        public string Visit(Stmt_If node)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            //Se evalua el boleano
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("   if\n");
            //sb.Append(VisitChildren(node));


            //statement list
            sb.Append(Visit((dynamic)node[1]));

            var Else_if_nodo = (Else_If_List)(dynamic)node[2];
            var elseChindrens = (Else)(dynamic)node[3];

            if (Else_if_nodo.NumberChildrens >= 1)
            {
                //Console.WriteLine(Else_if_nodo.NumberChildrens);

                var index = 0;
                string[] arrayElifs = new string[Else_if_nodo.NumberChildrens];
                foreach (var n in Else_if_nodo)
                {
                    arrayElifs[index] = Visit((dynamic)n);
                    index++;
                }

                // //Else_If_List
                var actualStr = "";
                sb.Append("   else\n");
                for (int i = arrayElifs.Length - 1; i >= 0; i--)
                {

                    if (arrayElifs.Length - 1 == i)
                    {

                        sb2.Append(arrayElifs[i]);
                        if (elseChindrens.NumberChildrens >= 1)
                        {
                            sb2.Append("else\n");
                            sb2.Append(Visit((dynamic)node[3]));
                        }
                        sb2.Append("end\n");
                        actualStr = sb2.ToString();
                        sb2.Clear();
                    }
                    else
                    {

                        sb2.Append(arrayElifs[i]);
                        sb2.Append("else\n");
                        sb2.Append(actualStr);
                        sb2.Append("end\n");
                        actualStr = sb2.ToString();
                        sb2.Clear();
                    }
                }
                sb.Append(actualStr);




            }
            else
            {
                //var elseChindrens = (Else)(dynamic)node[3];
                if (elseChindrens.NumberChildrens >= 1)
                {
                    sb.Append("else\n");
                    sb.Append(Visit((dynamic)node[3]));
                }

            }






            sb.Append("   end\n");
            return sb.ToString();
        }

        public string Visit(Array node)
        {
            var sb = new StringBuilder();
            //Evaluar los argumentos
            var nodoExpr_List = (Expr_List)(dynamic)node[0];

            sb.Append("  ;; Arreglo\n");
            sb.Append("    i32.const 0\n");
            sb.Append("    call $new\n");
            sb.Append("    local.set $_temp\n");

            for (int i = 0; i < nodoExpr_List.NumberChildrens + 1; i++)
            {
                sb.Append("   local.get $_temp\n");
            }

            foreach (var n in nodoExpr_List)
            {
                sb.Append(Visit((dynamic)n));
                sb.Append("   call $add\n");
                sb.Append("   drop\n");
            }

            return sb.ToString();
        }

        string breakLabel = null;


        public string Visit(Stmt_Loop node)
        {
            var et1 = GenerateLabel();
            var et2 = GenerateLabel();
            var oldLabel = breakLabel;
            breakLabel = et1;
            var result =
               ";;loop \n" +
               "    block  " + et1 + "\n"
               + "      loop  " + et2 + "\n"
               + Visit((dynamic)node[0])
               + "    br  " + et2 + "\n"
               + "      end\n"
               + "    end\n";
            breakLabel = oldLabel;
            return result;

        }

        public string Visit(Stmt_Break node)
        {
            return "    br  " + breakLabel + "\n";
        }


        public string Visit(Else_If_List node)
        {
            var sb = new StringBuilder();

            return sb.ToString();
        }

        public string Visit(Elif node)
        {
            var sb = new StringBuilder();
            //Se evalua el boleano
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("   if\n");

            //statement list
            sb.Append(Visit((dynamic)node[1]));
            return sb.ToString();
        }


        public string Visit(Else node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            return sb.ToString();
        }

        public string Visit(Assign node)
        {
            var nombreVariable = obtenerNombreIdentifier((dynamic)node[0]);
            var result = "";
            //Analizar si mi variable es local o global
            //Si es local
            if (Fgst[currentFunction].Lst.Contains(nombreVariable))
            {
                result = $"    local.set ${nombreVariable}\n";
            }
            //Si es global
            else
            {
                result = $"    global.set ${nombreVariable}\n";
            }

            return Visit((dynamic)node[1])
                    + result;
        }

        public string obtenerNombreIdentifier(Identifier node)
        {
            return $"{node.AnchorToken.Lexeme}";
        }

        //
        public string Visit(Identifier node)
        {
            //Checar si es variable local
            if (Fgst[currentFunction].Lst.Contains(node.AnchorToken.Lexeme))
            {
                return $"    local.get ${node.AnchorToken.Lexeme}\n";
            }

            //Checar si es variable global
            if (Vgst.Contains(node.AnchorToken.Lexeme))
            {
                return $"    global.get ${node.AnchorToken.Lexeme}\n";
            }

            return $"    local.get ${node.AnchorToken.Lexeme}\n";
        }

        public string Visit(IntLiteral node)
        {
            return $"    i32.const {node.AnchorToken.Lexeme}\n";
        }


        public string Visit(FUN_CALL node)
        {
            var sb = new StringBuilder();

            //Funcion
            var nodoIdentificador = node[0];
            var nombreFuncion = nodoIdentificador.AnchorToken.Lexeme;

            //Agregar parametros
            sb.Append(Visit((dynamic)node[1]));

            //llamar funcion
            sb.Append($"   call ${nombreFuncion}\n");
            if (node.isStatement)
            {
                sb.Append("   drop\n");
            }


            return sb.ToString();

        }

        public string Visit(Expr_List node)
        {
            return VisitChildren(node);
        }

        public string VisitChildren(Node node)
        {
            var sb = new StringBuilder();
            foreach (var n in node)
            {
                sb.Append(Visit((dynamic)n));
            }
            return sb.ToString();
        }

        //Zab
        public string Visit(Stmt_Return node)
        {
            isReturn = true;
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("   return\n");

            return sb.ToString();
        }

        //Zab
        public string Visit(NOT node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("   i32.eqz\n");
            return sb.ToString();
        }
        //Zab
        public string Visit(Empty node)
        {
            var sb = new StringBuilder();
            return sb.ToString();
        }

        //Zab
        public string Visit(String node)
        {
            var sb = new StringBuilder();
            var stringLexeme = node.AnchorToken.Lexeme;
            var newString = stringLexeme.Substring(1, stringLexeme.Length - 2);
            var charArray = newString.ToCharArray();
            var index = 0;
            var stringSize = 0;

            //Recorrer el String que esta en un arreglo de characters
            while (index < charArray.Length)
            {
                //Console.WriteLine("char " + charArray[index] + " en index " + index);

                //Si solamente es un character normal, ex 'A', agregar su codigo
                if (charArray[index].ToString() != @"\")
                {

                    sb.Append("i32.const " + AsCodePoints(charArray[index].ToString())[0] + ";; " + charArray[index].ToString() + "\n");
                    sb.Append("call $add\n");
                    sb.Append("drop\n");
                    sb.Append("\n");
                    index++;
                    stringSize++;
                }
                //Si contiene el character "\", entonces validar si ...
                else
                {
                    //validar si se puede saber su valor completo, ex "\n", "\r".
                    if (index + 1 < charArray.Length)
                    {
                        switch (charArray[index + 1].ToString())
                        {
                            //Si en realidad es "/n", entonces agrega su codigo
                            case "n":
                                sb.Append("i32.const 10" + ";; " + @"\n" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            //falta agregar mas casos
                            case "r":
                                sb.Append("   i32.const 13" + ";; " + @"\r" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            case "t":
                                sb.Append("   i32.const 9" + ";; " + @"\t" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            case @"\":
                                sb.Append("   i32.const 92" + ";; " + @"\\" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            case @"'":
                                sb.Append("   i32.const 39" + ";; " + @"\'" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            case @"""":
                                sb.Append("   i32.const 34" + ";; " + @"\""" + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index += 2;
                                stringSize++;
                                break;
                            case @"u":
                                var inicio = index + 3;
                                var numberString = stringLexeme.Substring(inicio, 6);


                                int decValue = Convert.ToInt32(numberString, 16);

                                sb.Append("   i32.const " + decValue + ";; " + numberString + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");

                                index += 8;
                                stringSize++;
                                break;
                            //Si en realidad solo es el character "\" , entonces agregar su codigo
                            default:
                                sb.Append("i32.const " + AsCodePoints(charArray[index].ToString())[0] + ";; " + charArray[index].ToString() + "\n");
                                sb.Append("call $add\n");
                                sb.Append("drop\n");
                                sb.Append("\n");
                                index++;
                                stringSize++;
                                break;
                        }
                    }

                    //Si en realidad tienes un caso como "\",entonces
                    else
                    {

                        sb.Append("i32.const " + AsCodePoints(charArray[index].ToString())[0] + ";; " + charArray[index].ToString() + "\n");
                        sb.Append("call $add\n");
                        sb.Append("drop\n");
                        sb.Append("\n");
                        index++;
                        stringSize++;
                    }
                }


            }
            var sb2 = new StringBuilder();
            sb2.Append(" i32.const 0\n");
            sb2.Append(" call $new\n");
            sb2.Append(" local.set $_temp\n");
            for (int i = 0; i < stringSize + 1; i++)
            {
                sb2.Append(" local.get $_temp\n");
            }

            sb2.Append("\n");
            sb2.Append(sb.ToString());
            return sb2.ToString();
        }

        //Jonathan Implementations

        public string Visit(MULTIPLICATION node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.mul\n");
            return sb.ToString();
        }

        public string Visit(Expr_And node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("   i32.and\n");
            return sb.ToString();
        }

        public string Visit(Expr_Or node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.or\n");
            return sb.ToString();
        }

        public string Visit(PLUS node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.add\n");
            return sb.ToString();
        }

        public string Visit(DIVISION node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.div_s\n");
            return sb.ToString();
        }

        public string Visit(REMINDER node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.rem_s\n");
            return sb.ToString();
        }

        public string Visit(EQUAL_TO node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.eq\n");
            return sb.ToString();
        }

        public string Visit(NOT_EQUAL_TO node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.ne\n");
            return sb.ToString();
        }
        public string Visit(LESS_THAN node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.lt_s\n");
            return sb.ToString();
        }
        public string Visit(LESS_EQUAL_THAN node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.le_s\n");
            return sb.ToString();
        }

        public string Visit(GREATHER_THAN node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.gt_s\n");
            return sb.ToString();
        }

        public string Visit(GREATHER_EQUAL_THAN node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append(Visit((dynamic)node[1]));

            sb.Append("     i32.ge_s\n");
            return sb.ToString();
        }

        public string Visit(Stmt_Incr node)
        {
            var variable = (dynamic)node[0].AnchorToken.Lexeme;

            var result = "";
            //Analizar si mi variable es local o global
            //Si es local
            if (Fgst[currentFunction].Lst.Contains(variable))
            {
                result = $"    local.set ${variable}\n";
            }
            //Si es global
            else
            {
                result = $"    global.set ${variable}\n";
            }

            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("     i32.const 1\n");
            sb.Append("     i32.add\n");
            sb.Append(result);
            return sb.ToString();
        }

        public string Visit(Stmt_Decr node)
        {

            var variable = (dynamic)node[0].AnchorToken.Lexeme;

            var result = "";
            //Analizar si mi variable es local o global
            //Si es local
            if (Fgst[currentFunction].Lst.Contains(variable))
            {
                result = $"    local.set ${variable}\n";
            }
            //Si es global
            else
            {
                result = $"    global.set ${variable}\n";
            }


            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            sb.Append("     i32.const 1\n");
            sb.Append("     i32.sub\n");
            sb.Append(result);
            return sb.ToString();

        }

        public string Visit(Op_Unary node)
        {
            var sb = new StringBuilder();
            sb.Append(Visit((dynamic)node[0]));
            return sb.ToString();
        }

        public string Visit(Boolean node)
        {
            var sb = new StringBuilder();
            if (node.AnchorToken.Lexeme.Equals("true"))
            {
                sb.Append("     i32.const 1\n");
            }
            else
            {
                sb.Append("     i32.const 0\n");
            }
            return sb.ToString();
        }

        public string Visit(Character node)
        {
            var sb = new StringBuilder();
            var stringLexeme = node.AnchorToken.Lexeme;
            var newString = stringLexeme.Substring(1, stringLexeme.Length - 2);

            if (newString.Length == 1)
            {
                sb.Append("   i32.const " + AsCodePoints(newString)[0] + ";; " + newString + "\n");
            }
            else
            {
                switch (newString[1].ToString())
                {
                    //Si en realidad es "/n", entonces agrega su codigo
                    case "n":
                        sb.Append("   i32.const 10" + ";; " + @"\n" + "\n");
                        break;

                    case "r":
                        sb.Append("   i32.const 13" + ";; " + @"\r" + "\n");
                        break;
                    case "t":
                        sb.Append("   i32.const 9" + ";; " + @"\t" + "\n");
                        break;
                    case @"\":
                        sb.Append("   i32.const 92" + ";; " + @"\\" + "\n");
                        break;
                    case @"'":
                        sb.Append("   i32.const 39" + ";; " + @"\'" + "\n");
                        break;
                    case @"""":
                        sb.Append("   i32.const 34" + ";; " + @"\""" + "\n");
                        break;
                    case @"u":
                        var numberString = stringLexeme.Substring(3, 6);
                        int decValue = Convert.ToInt32(numberString, 16);
                        sb.Append("   i32.const " + decValue + ";; " + numberString + "\n");
                        break;
                    default:
                        break;
                }
            }

            return sb.ToString();

        }

        public string Visit(SUBSTRACTION node)
        {
            var sb = new StringBuilder();

            if (node.NumberChildrens == 1)
            {

                sb.Append(Visit((dynamic)node[0]));

            }
            else
            {
                sb.Append(Visit((dynamic)node[0]));
                sb.Append(Visit((dynamic)node[1]));
            }


            sb.Append("     i32.sub\n");
            return sb.ToString();
        }



        //Declara variables
        //funciona tanto para argumentos de funcion como para declaracion de variables.
        public string Visit(VarDefList node)
        {
            var sb = new StringBuilder();
            // foreach (var n in node) {
            //     sb.Append(declareLocalVariable((dynamic) n));
            // }
            var arguments = Fgst[currentFunction].arrayLst();

            if (areVariablesSet != true)
            {
                for (int i = Fgst[currentFunction].Arity; i < arguments.Length; i++)
                {
                    sb.Append($"    (local ${arguments[i]} i32)\n");
                }
                areVariablesSet = true;
            }
            return sb.ToString();

        }

        public string declareLocalVariable(Identifier node)
        {

            //return $"    local.set ${node.AnchorToken.Lexeme}\n";
            return $"    (local ${node.AnchorToken.Lexeme} i32)\n";
        }

        string VisitBinaryOperator(string op, Node node)
        {
            return Visit((dynamic)node[0])
                + Visit((dynamic)node[1])
                + $"    {op}\n";
        }
    }
}

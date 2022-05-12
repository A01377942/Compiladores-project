/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

/*
 * QuetzalDragon LL(1) Grammar:
 *

 Zabdiel:
‹program› → ‹def-list› “EOF”
‹def-list› →‹def›*
‹def›→‹var-def› | ‹fun-def›
‹var-def›→var ‹var-list› ;
‹var-list›→‹id-list›
‹id-list›→<id>(,<id>)*
‹fun-def›→‹id› ( ‹id-list›? ) { ‹var-def›* ‹stmt-list›* }
<stmt-list>›→<stmt>*
‹stmt›→<id>(=<expr>; | (<expr-list>);) |<stmt-inc>|<stmt-dec>|<stmt-if>|‹stmt-loop›|‹stmt-break›|‹stmt-return›|‹stmt-empty›
‹expr-list› →(‹expr› ‹expr-list-cont›)?
‹expr-list-cont›→ (,<expr> <expr-list-cont>)?
‹stmt-incr›→    inc ‹id› ;
‹stmt-decr›→   dec ‹id› ; 
<stmt-if>→if ( ‹expr› ) { ‹stmt-list› } ‹else-if-list› ‹else›


Jonathan
‹else-if-list›→    ( elif ( ‹expr› ) { ‹stmt-list› })*
‹else›→    ( else{ ‹stmt-list› })*
‹stmt-loop›→    loop { ‹stmt-list› }
‹stmt-break›→    break ;
‹stmt-return›→    return ‹expr› ;
‹stmt-empty›→    ;
‹expr›→           ‹expr-or›
‹expr-or›→       ‹expr-and› (or ‹expr-and›)*
‹expr-and›→    ‹expr-comp› (and ‹expr-comp›)*
‹expr-comp›→‹expr-rel›(‹op-comp› ‹expr-rel›)*
‹op-comp›→    ==|!=


Emiliano:
‹expr-rel›→      ‹expr-add› (‹op-rel› ‹expr-add›)*
‹op-rel›→         <|<=|>|>=
‹expr-add›→    ‹expr-mul›(‹op-add› ‹expr-mul›)*
‹op-add›→       +|−
‹expr-mul›→    ‹expr-unary›(‹op-mul› ‹expr-unary›)*
‹op-mul›→       *|/|%
‹expr-unary›→ ‹op-unary›‹expr-unary› |‹expr-primary›
‹op-unary›→    +|−|not
‹expr-primary›→          <id> (  (‹expr-list› ) )?|‹array›|‹lit›|( ‹expr› )
‹array›→          [ ‹expr-list› ]
‹lit›→   ‹lit-bool›|‹lit-int›|‹lit-char›|‹lit-str›

 */

using System;
using System.Collections.Generic;

namespace QuetzalDragon
{

    class Parser
    {


        static readonly ISet<TokenCategory> def_Values =
            new HashSet<TokenCategory>() {
                TokenCategory.VAR,
                TokenCategory.IDENTIFIER
            };

        static readonly ISet<TokenCategory> stmt_Values =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.INC,
                TokenCategory.DEC,
                TokenCategory.IF,
                TokenCategory.LOOP,
                TokenCategory.BREAK,
                TokenCategory.RETURN,
                TokenCategory.END_OF_LINE
            };

        static readonly ISet<TokenCategory> expr_Values =
            new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.SUBSTRACTION,
                TokenCategory.NOT,
                TokenCategory.IDENTIFIER,
                TokenCategory.LEFT_SQUARE_BRACKET,
                         TokenCategory.TRUE,
             TokenCategory.FALSE,
             TokenCategory.INT_LITERAL,
             TokenCategory.CHARACTER,
             TokenCategory.STRING

            };

        static readonly ISet<TokenCategory> expr_Primary_Values =
            new HashSet<TokenCategory>() {
                TokenCategory.IDENTIFIER,
                TokenCategory.LEFT_SQUARE_BRACKET,
                         TokenCategory.TRUE,
             TokenCategory.FALSE,
             TokenCategory.INT_LITERAL,
             TokenCategory.CHARACTER,
             TokenCategory.STRING,
         TokenCategory.PLUS,
                TokenCategory.SUBSTRACTION,
                TokenCategory.NOT
            };

        static readonly ISet<TokenCategory> Unary_Values =
              new HashSet<TokenCategory>() {
                TokenCategory.PLUS,
                TokenCategory.SUBSTRACTION,
                TokenCategory.NOT

              };

        static readonly ISet<TokenCategory> Lit_Values =
              new HashSet<TokenCategory>() {
             TokenCategory.TRUE,
             TokenCategory.FALSE,
             TokenCategory.INT_LITERAL,
             TokenCategory.CHARACTER,
             TokenCategory.STRING

              };

        IEnumerator<Token> tokenStream;

        public Parser(IEnumerator<Token> tokenStream)
        {
            this.tokenStream = tokenStream;
            this.tokenStream.MoveNext();
        }

        public TokenCategory CurrentToken
        {
            get { return tokenStream.Current.Category; }
        }

        public Token Expect(TokenCategory category)
        {
            if (CurrentToken == category)
            {
                Token current = tokenStream.Current;
                tokenStream.MoveNext();
                return current;
            }
            else
            {
                throw new SyntaxError(category, tokenStream.Current);
            }
        }

        public Node Program()
        {
            //Def_list();
            //Expect(TokenCategory.EOF);

            var def_list = new Def_list();


            while (def_Values.Contains(CurrentToken))
            {
                //Def();
                def_list.Add(Def());
            }

            Expect(TokenCategory.EOF);

            return new Program() {
                def_list
            };
        }

        //Se elimino
        // public void Def_list()
        // {
        //     while (def_Values.Contains(CurrentToken))
        //     {
        //         Def();
        //     }
        // }

        public Node Def()
        {
            switch (CurrentToken)
            {

                case TokenCategory.VAR:
                    return Var_Def();
                    break;

                case TokenCategory.IDENTIFIER:
                    return Fun_Def();
                    break;

                default:
                    throw new SyntaxError(def_Values,
                                        tokenStream.Current);

            }

        }

        public Node Var_Def()
        {
            var token = Expect(TokenCategory.VAR);
            //Var_List();
            var var_list = Var_List();
            var_list.AnchorToken = token;

            Expect(TokenCategory.END_OF_LINE);
            return var_list;
        }


        public Node Var_List()
        {
            return ID_List();
        }

        public Node ID_List()
        {

            // Expect(TokenCategory.IDENTIFIER);
            // while (CurrentToken == TokenCategory.COMA)
            // {
            //     Expect(TokenCategory.COMA);
            //     Expect(TokenCategory.IDENTIFIER);
            // }
            var result = new VarDefList();
            result.Add(new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });

            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                result.Add(new Identifier()
                {
                    AnchorToken = Expect(TokenCategory.IDENTIFIER)
                });

            }
            return result;
        }

        public Node Fun_Def()
        {
            /*       Expect(TokenCategory.IDENTIFIER);

                  Expect(TokenCategory.PARENTHESIS_OPEN);
                  if (CurrentToken == TokenCategory.IDENTIFIER)
                  {
                      ID_List();
                  }
                  Expect(TokenCategory.PARENTHESIS_CLOSE);
                  Expect(TokenCategory.LEFT_CURLY_BRACE);
                  while (CurrentToken == TokenCategory.VAR)
                  {
                      Var_Def();
                  }

                  while (stmt_Values.Contains(CurrentToken))
                  {
                      Stmt_List();
                  }
                  Expect(TokenCategory.RIGHT_CURLY_BRACE); */

            var result = new Fun_Def();
            result.AnchorToken = Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            if (CurrentToken == TokenCategory.IDENTIFIER)
            {
                result.Add(ID_List());
            }
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.LEFT_CURLY_BRACE);
            while (CurrentToken == TokenCategory.VAR)
            {
                result.Add(Var_Def());
            }

            while (stmt_Values.Contains(CurrentToken))
            {
                result.Add(Stmt_List());

            }

            Expect(TokenCategory.RIGHT_CURLY_BRACE);

            return result;
        }


        public Node Stmt_List()
        {
            var result = new Stmt_List();

            while (stmt_Values.Contains(CurrentToken))
            {
                result.Add(Stmt());

            }
            return result;
        }
        /*
                case TokenCategory.IDENTIFIER:
                            var result = new Assign();
                            result.Add(new Identifier()
                            {
                                AnchorToken = Expect(TokenCategory.IDENTIFIER)
                            });
                            //Expect(TokenCategory.IDENTIFIER);
                            //result.AnchorToken = Expect(TokenCategory.IDENTIFIER);
                            switch (CurrentToken)
                            {
                                case TokenCategory.ASSIGN:
                                    result.AnchorToken = Expect(TokenCategory.ASSIGN);
                                    result.Add(Expr());
                                    Expect(TokenCategory.END_OF_LINE);
                                    return result;
                                    break;
                                case TokenCategory.PARENTHESIS_OPEN:
                                    Expect(TokenCategory.PARENTHESIS_OPEN);
                                    result.Add(Expr_List());
                                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                                    Expect(TokenCategory.END_OF_LINE);
                                    return result;
                                    break;
                                default:
                                    throw new SyntaxError(def_Values,
                                                        tokenStream.Current);
                            }
                            break;
        */
        public Node Stmt()
        {
            //var result = new Stmt();
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    var result = new Assign();
                    var token = Expect(TokenCategory.IDENTIFIER);

                    //Expect(TokenCategory.IDENTIFIER);
                    //result.AnchorToken = Expect(TokenCategory.IDENTIFIER);
                    switch (CurrentToken)
                    {
                        case TokenCategory.ASSIGN:
                            result.Add(new Identifier()
                            {
                                AnchorToken = token
                            });
                            result.AnchorToken = Expect(TokenCategory.ASSIGN);
                            result.Add(Expr());
                            Expect(TokenCategory.END_OF_LINE);
                            return result;
                            break;
                        case TokenCategory.PARENTHESIS_OPEN:
                            //
                            var result3 = new FUN_CALL();
                            result3.Add(new Identifier()
                            {
                                AnchorToken = token
                            });
                            Expect(TokenCategory.PARENTHESIS_OPEN);
                            result3.Add(Expr_List());
                            Expect(TokenCategory.PARENTHESIS_CLOSE);
                            Expect(TokenCategory.END_OF_LINE);
                            return result3;
                            break;
                        default:
                            throw new SyntaxError(def_Values,
                                                tokenStream.Current);
                    }
                    break;

                case TokenCategory.INC:
                    return Stmt_Incr();
                    break;
                case TokenCategory.DEC:
                    return Stmt_Decr();
                    break;
                case TokenCategory.IF:
                    return Stmt_If();
                    break;
                case TokenCategory.LOOP:
                    return Stmp_Loop();
                    break;
                case TokenCategory.BREAK:
                    return Stmp_Break();
                    break;
                case TokenCategory.RETURN:
                    return Stmp_Return();
                    break;
                case TokenCategory.END_OF_LINE:
                    var result2 = new Empty();
                    result2.AnchorToken = Expect(TokenCategory.END_OF_LINE);
                    return result2;
                    break;

                default:
                    throw new SyntaxError(def_Values,
                                        tokenStream.Current);

            }

        }

        //‹expr-list› →(‹expr› ‹expr-list-cont›)?
        public Node Expr_List()
        {
            var result = new Expr_List();

            if (expr_Values.Contains(CurrentToken))
            {
                result.Add(Expr());
                result.Add(Expr_List_Cont());

            }
            return result;

        }

        public Node Stmt_Incr()
        {
            var tokenId = Expect(TokenCategory.INC);
            var result = new Stmt_Incr();

            result.Add(new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });

            result.AnchorToken = tokenId;
            Expect(TokenCategory.END_OF_LINE);

            return result;
        }

        public Node Stmt_Decr()
        {
            // Expect(TokenCategory.DEC);
            // Expect(TokenCategory.IDENTIFIER);
            // Expect(TokenCategory.END_OF_LINE);
            var tokenId = Expect(TokenCategory.DEC);
            var result = new Stmt_Decr();
            result.Add(new Identifier()
            {
                AnchorToken = Expect(TokenCategory.IDENTIFIER)
            });
            result.AnchorToken = tokenId;
            Expect(TokenCategory.END_OF_LINE);

            return result;

        }

        public Node Stmt_If()
        {
            // Expect(TokenCategory.IF);
            // Expect(TokenCategory.PARENTHESIS_OPEN);
            // Expr();
            // Expect(TokenCategory.PARENTHESIS_CLOSE);
            // Expect(TokenCategory.LEFT_CURLY_BRACE);
            // Stmt_List();
            // Expect(TokenCategory.RIGHT_CURLY_BRACE);
            // Else_If_List();
            // Else();
            var result = new Stmt_If();
            result.AnchorToken = Expect(TokenCategory.IF);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            result.Add(Expr());
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.LEFT_CURLY_BRACE);
            result.Add(Stmt_List());
            Expect(TokenCategory.RIGHT_CURLY_BRACE);
            result.Add(Else_If_List());
            result.Add(Else());

            return result;
        }
        public Node Else_If_List()
        {
            var result = new Else_If_List();

            while (CurrentToken == TokenCategory.ELIF)
            {
                var elif = new Elif();
                var token = Expect(TokenCategory.ELIF);
                elif.AnchorToken = token;
                Expect(TokenCategory.PARENTHESIS_OPEN);
                elif.Add(Expr());

                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.LEFT_CURLY_BRACE);
                elif.Add(Stmt_List());
                Expect(TokenCategory.RIGHT_CURLY_BRACE);
                result.Add(elif);
            }
            return result;
        }

        public Node Else()
        {
            var result = new Else();

            while (CurrentToken == TokenCategory.ELSE)
            {
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.LEFT_CURLY_BRACE);
                result.Add(Stmt_List());
                Expect(TokenCategory.RIGHT_CURLY_BRACE);
            }
            return result;
        }

        public Node Stmp_Loop()
        {
            // Expect(TokenCategory.LOOP);
            // Expect(TokenCategory.LEFT_CURLY_BRACE);
            // var var_list = Stmt_List();
            // Expect(TokenCategory.RIGHT_CURLY_BRACE);
            // return var_list;

            var result = new Stmt_Loop();
            result.AnchorToken = Expect(TokenCategory.LOOP);
            Expect(TokenCategory.LEFT_CURLY_BRACE);
            result.Add(Stmt_List());
            Expect(TokenCategory.RIGHT_CURLY_BRACE);
            return result;
        }
        public Node Stmp_Break()
        {
            var result = new Stmt_Break();
            result.AnchorToken = Expect(TokenCategory.BREAK);
            Expect(TokenCategory.END_OF_LINE);
            return result;
        }

        public Node Stmp_Return()
        {
            // Expect(TokenCategory.RETURN);
            // Expr();
            // Expect(TokenCategory.END_OF_LINE);

            var result = new Stmt_Return();
            result.AnchorToken = Expect(TokenCategory.RETURN);
            result.Add(Expr());
            Expect(TokenCategory.END_OF_LINE);
            return result;
        }

        //Ahora: ‹expr-list-cont›→ (,<expr> <expr-list-cont>)?
        public Node Expr_List_Cont()
        {
            var result = new Expr_List_Cont();

            if (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                result.Add(Expr());
                result.Add(Expr_List_Cont());
            }

            return result;
        }
        public Node Expr()
        {
            return Expr_Or();
        }

        public Node Expr_Or()
        {
            var result = Expr_And();
            while (CurrentToken == TokenCategory.OR)
            {
                var token = Expect(TokenCategory.OR);
                var node = new Expr_Or();
                node.AnchorToken = token;
                node.Add(result);
                node.Add(Expr_And());
                result = node;
            }
            return result;
        }

        public Node Expr_And()
        {
            var result = Expr_Comp();
            //result.Add(Expr_Comp());
            while (CurrentToken == TokenCategory.AND)
            {
                var token = Expect(TokenCategory.AND);
                var node = new Expr_And();
                node.AnchorToken = token;
                node.Add(result);
                node.Add(Expr_Comp());
                result = node;
            }
            return result;
        }

        //‹expr-comp›→‹expr-rel›(‹op-comp› ‹expr-rel›)*
        public Node Expr_Comp()
        {
            var result = Expr_rel();


            while (CurrentToken == TokenCategory.EQUAL_TO || CurrentToken == TokenCategory.NOT_EQUAL_TO)
            {
                //result.Add(Op_Comp());
                //result.Add(Expr_rel());
                var node = Op_Comp();
                node.Add(result);
                node.Add(Expr_rel());
                result = node;
            }
            return result;
        }

        public Node Op_Comp()
        {
            //var result = new Op_Comp();
            switch (CurrentToken)
            {
                case TokenCategory.EQUAL_TO:
                    //result.AnchorToken = Expect(TokenCategory.EQUAL_TO);
                    //EQUAL_TO
                    return new EQUAL_TO()
                    {
                        AnchorToken = Expect(TokenCategory.EQUAL_TO)
                    };
                    break;
                case TokenCategory.NOT_EQUAL_TO:
                    //result.AnchorToken = Expect(TokenCategory.NOT_EQUAL_TO);
                    return new NOT_EQUAL_TO()
                    {
                        AnchorToken = Expect(TokenCategory.NOT_EQUAL_TO)
                    };
                    break;
                default:
                    throw new SyntaxError(TokenCategory.EQUAL_TO, tokenStream.Current);
            }
            //return result;
        }


        public Node Expr_rel()
        {

            //var result = new Expr_Rel();
            //result.Add(Expr_add());
            var result = Expr_add();
            while (CurrentToken == TokenCategory.LESS_THAN || CurrentToken == TokenCategory.LESS_EQUAL_THAN
            || CurrentToken == TokenCategory.GREATHER_THAN || CurrentToken == TokenCategory.GREATHER_EQUAL_THAN)
            {
                /*
                switch (CurrentToken)
                {
                    case TokenCategory.LESS_THAN:
                        Expect(TokenCategory.LESS_THAN);
                        Expr_add();
                        break;
                    case TokenCategory.LESS_EQUAL_THAN:
                        Expect(TokenCategory.LESS_EQUAL_THAN);
                        Expr_add();
                        break;
                    case TokenCategory.GREATHER_THAN:
                        Expect(TokenCategory.GREATHER_THAN);
                        Expr_add();
                        break;
                    case TokenCategory.GREATHER_EQUAL_THAN:
                        Expect(TokenCategory.GREATHER_EQUAL_THAN);
                        Expr_add();
                        break;
                    default:
                        throw new SyntaxError(TokenCategory.LESS_THAN, tokenStream.Current);
                }
                */
                // result.Add(Op_rel());
                var node = Op_rel();
                node.Add(result);
                node.Add(Expr_add());
                result = node;
            }
            return result;
        }

        public Node Op_rel()
        {
            //var result = new Op_Rel();
            switch (CurrentToken)
            {
                case TokenCategory.LESS_THAN:
                    //result.AnchorToken = Expect(TokenCategory.LESS_THAN);
                    // result.Add(Expr_add());
                    return new LESS_THAN()
                    {
                        AnchorToken = Expect(TokenCategory.LESS_THAN)
                    };
                    break;
                case TokenCategory.LESS_EQUAL_THAN:

                    //result.AnchorToken = Expect(TokenCategory.LESS_EQUAL_THAN);
                    // result.Add(Expr_add());
                    return new LESS_EQUAL_THAN()
                    {
                        AnchorToken = Expect(TokenCategory.LESS_EQUAL_THAN)
                    };
                    break;
                case TokenCategory.GREATHER_THAN:

                    //result.AnchorToken = Expect(TokenCategory.GREATHER_THAN);
                    //result.Add(Expr_add());
                    return new GREATHER_THAN()
                    {
                        AnchorToken = Expect(TokenCategory.GREATHER_THAN)
                    };
                    break;
                case TokenCategory.GREATHER_EQUAL_THAN:
                    //result.AnchorToken = Expect(TokenCategory.GREATHER_EQUAL_THAN);
                    //result.Add(Expr_add());
                    return new GREATHER_EQUAL_THAN()
                    {
                        AnchorToken = Expect(TokenCategory.GREATHER_EQUAL_THAN)
                    };
                    break;
                default:
                    throw new SyntaxError(TokenCategory.LESS_THAN, tokenStream.Current);
            }
            //return result;
        }

        public Node Expr_add()
        {
            // var result = new Expr_Add();
            //result.Add(Expr_mul());
            var result = Expr_mul();
            while (CurrentToken == TokenCategory.PLUS || CurrentToken == TokenCategory.SUBSTRACTION)
            {
                /*
                switch (CurrentToken)
                {
                    case TokenCategory.PLUS:
                        Expect(TokenCategory.PLUS);
                        Expr_mul();
                        break;
                    case TokenCategory.SUBSTRACTION:
                        Expect(TokenCategory.SUBSTRACTION);
                        Expr_mul();
                        break;
                    default:
                        throw new SyntaxError(TokenCategory.PLUS, tokenStream.Current);
                }
                */
                // result.Add(Op_add());
                var node = Op_add();
                node.Add(result);
                node.Add(Expr_mul());
                result = node;
            }
            return result;
        }

        public Node Op_add()
        {
            // var result = new Op_Add();
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    //result.AnchorToken = Expect(TokenCategory.PLUS);
                    //result.Add(Expr_mul());
                    return new PLUS()
                    {
                        AnchorToken = Expect(TokenCategory.PLUS)
                    };
                    break;
                case TokenCategory.SUBSTRACTION:

                    //result.AnchorToken = Expect(TokenCategory.SUBSTRACTION);
                    //result.Add(Expr_mul());
                    return new SUBSTRACTION()
                    {
                        AnchorToken = Expect(TokenCategory.SUBSTRACTION)
                    };
                    break;
                default:
                    throw new SyntaxError(TokenCategory.PLUS, tokenStream.Current);
            }
            //return result;
        }


        public Node Expr_mul()
        {
            //var result = new Expr_Mul();
            //result.Add(Expr_unary());
            var result = Expr_unary();
            while (CurrentToken == TokenCategory.MULTIPLICATION || CurrentToken == TokenCategory.DIVISION || CurrentToken == TokenCategory.REMINDER)
            {
                /*
                switch (CurrentToken)
                {
                    case TokenCategory.MULTIPLICATION:
                        Expect(TokenCategory.MULTIPLICATION);
                        Expr_unary();
                        break;
                    case TokenCategory.DIVISION:
                        Expect(TokenCategory.DIVISION);
                        Expr_unary();
                        break;
                    case TokenCategory.REMINDER:
                        Expect(TokenCategory.REMINDER);
                        Expr_unary();
                        break;
                    default:
                        throw new SyntaxError(TokenCategory.MULTIPLICATION, tokenStream.Current);
                }
                */
                //result.Add(Op_mul());
                var node = Op_mul();
                node.Add(result);
                node.Add(Expr_unary());
                result = node;
            }
            return result;
        }

        public Node Op_mul()
        {
            //var result = new Op_Mul();
            switch (CurrentToken)
            {
                case TokenCategory.MULTIPLICATION:
                    return new MULTIPLICATION()
                    {
                        AnchorToken = Expect(TokenCategory.MULTIPLICATION)
                    };
                    //  result.AnchorToken = Expect(TokenCategory.MULTIPLICATION);
                    // result.Add(Expr_unary());

                    break;
                case TokenCategory.DIVISION:
                    return new DIVISION()
                    {
                        AnchorToken = Expect(TokenCategory.DIVISION)
                    };
                    //result.AnchorToken = Expect(TokenCategory.DIVISION);
                    //result.Add(Expr_unary());

                    break;
                case TokenCategory.REMINDER:
                    return new REMINDER()
                    {
                        AnchorToken = Expect(TokenCategory.REMINDER)
                    };
                    //result.AnchorToken = Expect(TokenCategory.REMINDER);
                    //result.Add(Expr_unary());

                    break;
                default:
                    throw new SyntaxError(TokenCategory.MULTIPLICATION, tokenStream.Current);
            }

            //return result;
        }

        /*
        Antes:
        ‹expr-unary›→ (‹op-unary›)*‹expr-primary›
      public void Expr_unary()
        {
            while (Unary_Values.Contains(CurrentToken))
            {
                Op_unary();
            }
            Expr_Primary();
        }
        */
        //Ahora:
        //‹expr-unary›→ ‹op-unary›‹expr-unary› |‹expr-primary›
        public Node Expr_unary()
        {
            //var result = new Expr_Unary();
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    // result.Add(Op_unary());
                    // result.Add(Expr_unary());
                    var token1 = Expect(TokenCategory.PLUS);
                    var expr1 = Expr_unary();
                    var result1 = new PLUS() { expr1 };
                    result1.AnchorToken = token1;
                    return result1;
                    break;
                case TokenCategory.SUBSTRACTION:
                    //result.Add(Op_unary());
                    // result.Add(Expr_unary());
                    var token2 = Expect(TokenCategory.SUBSTRACTION);
                    var expr2 = Expr_unary();
                    var result2 = new SUBSTRACTION() { expr2 };
                    result2.AnchorToken = token2;
                    return result2;
                    break;
                case TokenCategory.NOT:
                    //result.Add(Op_unary());
                    // result.Add(Expr_unary());
                    var token3 = Expect(TokenCategory.NOT);
                    var expr3 = Expr_unary();
                    var result3 = new NOT() { expr3 };
                    result3.AnchorToken = token3;
                    return result3;
                    break;
                case TokenCategory.IDENTIFIER:
                    //result.Add(Expr_primary());
                    return Expr_primary();
                    break;
                case TokenCategory.LEFT_SQUARE_BRACKET:
                    return Expr_primary();
                    break;
                case TokenCategory.TRUE:
                    return Expr_primary();
                    break;
                case TokenCategory.FALSE:
                    return Expr_primary();
                    break;
                case TokenCategory.INT_LITERAL:
                    return Expr_primary();
                    break;
                case TokenCategory.CHARACTER:
                    return Expr_primary();
                    break;
                case TokenCategory.STRING:
                    return Expr_primary();
                    break;
                default:
                    throw new SyntaxError(expr_Primary_Values, tokenStream.Current);
            }
            //return result;
        }

        public Node Op_unary()
        {
            var result = new Op_Unary();
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    result.AnchorToken = Expect(TokenCategory.PLUS);
                    break;
                case TokenCategory.SUBSTRACTION:
                    result.AnchorToken = Expect(TokenCategory.SUBSTRACTION);
                    break;
                case TokenCategory.NOT:
                    result.AnchorToken = Expect(TokenCategory.NOT);
                    break;
                default:
                    throw new SyntaxError(Unary_Values, tokenStream.Current);
            }
            return result;
        }

        public Node Expr_primary()
        {
            //var result = new Expr_Primary();
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    var token = Expect(TokenCategory.IDENTIFIER);

                    //Expect(TokenCategory.IDENTIFIER);
                    // var result = new FUN_CALL();
                    // result.Add(new Identifier()
                    // {
                    //     AnchorToken = Expect(TokenCategory.IDENTIFIER)
                    // });

                    if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                    {
                        var result = new FUN_CALL();
                        result.Add(new Identifier()
                        {
                            AnchorToken = token
                        });
                        Expect(TokenCategory.PARENTHESIS_OPEN);

                        result.Add(Expr_List());

                        Expect(TokenCategory.PARENTHESIS_CLOSE);
                        return result;
                    }
                    var result2 = new Identifier();
                    result2.AnchorToken = token;
                    return result2;
                    break;
                case TokenCategory.LEFT_SQUARE_BRACKET:
                    //result.Add(Array());
                    return Array();
                    break;
                case TokenCategory.TRUE:
                    //Expect(TokenCategory.TRUE);
                    //result.Add(Lit());
                    return Lit();
                    break;
                case TokenCategory.FALSE:
                    //Expect(TokenCategory.FALSE);
                    //result.Add(Lit());
                    return Lit();
                    break;
                case TokenCategory.INT_LITERAL:
                    //Expect(TokenCategory.INT_LITERAL);
                    //result.Add(Lit());
                    return Lit();
                    break;
                case TokenCategory.CHARACTER:
                    //Expect(TokenCategory.CHARACTER);
                    //result.Add(Lit());
                    return Lit();
                    break;
                case TokenCategory.STRING:
                    // Expect(TokenCategory.STRING);
                    //result.Add(Lit());
                    return Lit();
                    break;
                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    // result.Add(Expr());
                    return Expr();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    break;
                default:
                    throw new SyntaxError(Lit_Values, tokenStream.Current);
            }
            //return result;
        }
        public Node Array()
        {
            var result = new Array();
            Expect(TokenCategory.LEFT_SQUARE_BRACKET);
            result.Add(Expr_List());
            Expect(TokenCategory.RIGHT_SQUARE_BRACKET);
            return result;
        }

        public Node Lit()
        {
            //var result = new Lit();
            switch (CurrentToken)
            {
                case TokenCategory.TRUE:
                    //result.AnchorToken = Expect(TokenCategory.TRUE);
                    return new Boolean()
                    {
                        AnchorToken = Expect(TokenCategory.TRUE)
                    };
                    break;
                case TokenCategory.FALSE:
                    //result.AnchorToken = Expect(TokenCategory.FALSE);
                    return new Boolean()
                    {
                        AnchorToken = Expect(TokenCategory.FALSE)
                    };
                    break;
                case TokenCategory.INT_LITERAL:
                    //result.AnchorToken = Expect(TokenCategory.INT_LITERAL);
                    return new IntLiteral()
                    {
                        AnchorToken = Expect(TokenCategory.INT_LITERAL)
                    };
                    break;
                case TokenCategory.CHARACTER:
                    //result.AnchorToken = Expect(TokenCategory.CHARACTER);
                    return new Character()
                    {
                        AnchorToken = Expect(TokenCategory.CHARACTER)
                    };

                    break;
                case TokenCategory.STRING:
                    // result.AnchorToken = Expect(TokenCategory.STRING);
                    return new String()
                    {
                        AnchorToken = Expect(TokenCategory.STRING)
                    };
                    break;
                default:
                    throw new SyntaxError(Lit_Values, tokenStream.Current);
            }
            //return result;
        }



    }
}

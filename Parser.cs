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
            Expect(TokenCategory.VAR);
            //Var_List();
            var var_list = Var_List();

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

        public Node Stmt()
        {
            var result = new Stmt();
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    //Expect(TokenCategory.IDENTIFIER);
                    result.AnchorToken = Expect(TokenCategory.IDENTIFIER);
                    switch (CurrentToken)
                    {
                        case TokenCategory.ASSIGN:

                            Expect(TokenCategory.ASSIGN);
                            result.Add(Expr());
                            Expect(TokenCategory.END_OF_LINE);
                            break;
                        case TokenCategory.PARENTHESIS_OPEN:
                            Expect(TokenCategory.PARENTHESIS_OPEN);
                            result.Add(Expr_List());
                            Expect(TokenCategory.PARENTHESIS_CLOSE);
                            Expect(TokenCategory.END_OF_LINE);
                            break;
                        default:
                            throw new SyntaxError(def_Values,
                                                tokenStream.Current);
                    }
                    break;

                case TokenCategory.INC:
                    result.Add(Stmt_Incr());
                    break;
                case TokenCategory.DEC:
                    result.Add(Stmt_Decr());
                    break;
                case TokenCategory.IF:
                    result.Add(Stmt_If());
                    break;
                case TokenCategory.LOOP:
                    result.Add(Stmp_Loop());
                    break;
                case TokenCategory.BREAK:
                    result.Add(Stmp_Break());
                    break;
                case TokenCategory.RETURN:
                    result.Add(Stmp_Return());
                    break;
                case TokenCategory.END_OF_LINE:
                    Expect(TokenCategory.END_OF_LINE);
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

        }
        public Node Else_If_List()
        {

            while (CurrentToken == TokenCategory.ELIF)
            {
                Expect(TokenCategory.ELIF);
                Expect(TokenCategory.PARENTHESIS_OPEN);
                Expr();
                Expect(TokenCategory.PARENTHESIS_CLOSE);
                Expect(TokenCategory.LEFT_CURLY_BRACE);
                Stmt_List();
                Expect(TokenCategory.RIGHT_CURLY_BRACE);
            }
        }

        public void Else()
        {
            while (CurrentToken == TokenCategory.ELSE)
            {
                Expect(TokenCategory.ELSE);
                Expect(TokenCategory.LEFT_CURLY_BRACE);
                Stmt_List();
                Expect(TokenCategory.RIGHT_CURLY_BRACE);
            }
        }

        public void Stmp_Loop()
        {
            Expect(TokenCategory.LOOP);
            Expect(TokenCategory.LEFT_CURLY_BRACE);
            Stmt_List();
            Expect(TokenCategory.RIGHT_CURLY_BRACE);
        }
        public void Stmp_Break()
        {
            Expect(TokenCategory.BREAK);
            Expect(TokenCategory.END_OF_LINE);
        }

        public void Stmp_Return()
        {
            Expect(TokenCategory.RETURN);
            Expr();
            Expect(TokenCategory.END_OF_LINE);
        }

        /*
        Anterior:
        ‹expr-list-cont›→(, ‹expr›)*
                public void Expr_List_Cont()
        {
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                Expr();
            }
        }

        */
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
        }
        public void Expr()
        {

            Expr_Or();
        }

        public void Expr_Or()
        {

            Expr_And();
            while (CurrentToken == TokenCategory.OR)
            {
                Expect(TokenCategory.OR);
                Expr_And();
            }
        }

        public void Expr_And()
        {
            Expr_Comp();
            while (CurrentToken == TokenCategory.AND)
            {
                Expect(TokenCategory.AND);
                Expr_Comp();
            }
        }

        //‹expr-comp›→‹expr-rel›(‹op-comp› ‹expr-rel›)*
        public void Expr_Comp()
        {
            Expr_rel();
            while (CurrentToken == TokenCategory.EQUAL_TO || CurrentToken == TokenCategory.NOT_EQUAL_TO)
            {
                Op_Comp();
                Expr_rel();
            }

        }

        public void Op_Comp()
        {

            switch (CurrentToken)
            {
                case TokenCategory.EQUAL_TO:
                    Expect(TokenCategory.EQUAL_TO);
                    break;
                case TokenCategory.NOT_EQUAL_TO:
                    Expect(TokenCategory.NOT_EQUAL_TO);
                    break;
                default:
                    throw new SyntaxError(TokenCategory.EQUAL_TO, tokenStream.Current);
            }
        }


        public void Expr_rel()
        {

            Expr_add();
            while (CurrentToken == TokenCategory.LESS_THAN || CurrentToken == TokenCategory.LESS_EQUAL_THAN
            || CurrentToken == TokenCategory.GREATHER_THAN || CurrentToken == TokenCategory.GREATHER_EQUAL_THAN)
            {
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
            }
        }

        public void Expr_add()
        {
            Expr_mul();
            while (CurrentToken == TokenCategory.PLUS || CurrentToken == TokenCategory.SUBSTRACTION)
            {
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
            }
        }

        public void Expr_mul()
        {
            Expr_unary();
            while (CurrentToken == TokenCategory.MULTIPLICATION || CurrentToken == TokenCategory.DIVISION || CurrentToken == TokenCategory.REMINDER)
            {
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
            }
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
        public void Expr_unary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    Op_unary();
                    Expr_unary();
                    break;
                case TokenCategory.SUBSTRACTION:
                    Op_unary();
                    Expr_unary();
                    break;
                case TokenCategory.NOT:
                    Op_unary();
                    Expr_unary();
                    break;
                case TokenCategory.IDENTIFIER:
                    Expr_Primary();
                    break;
                case TokenCategory.LEFT_SQUARE_BRACKET:
                    Expr_Primary();
                    break;
                case TokenCategory.TRUE:
                    Expr_Primary();
                    break;
                case TokenCategory.FALSE:
                    Expr_Primary();
                    break;
                case TokenCategory.INT_LITERAL:
                    Expr_Primary();
                    break;
                case TokenCategory.CHARACTER:
                    Expr_Primary();
                    break;
                case TokenCategory.STRING:
                    Expr_Primary();
                    break;
                default:
                    throw new SyntaxError(expr_Primary_Values, tokenStream.Current);
            }
        }

        public void Op_unary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.PLUS:
                    Expect(TokenCategory.PLUS);
                    break;
                case TokenCategory.SUBSTRACTION:
                    Expect(TokenCategory.SUBSTRACTION);
                    break;
                case TokenCategory.NOT:
                    Expect(TokenCategory.NOT);
                    break;
                default:
                    throw new SyntaxError(Unary_Values, tokenStream.Current);
            }
        }

        public void Expr_Primary()
        {
            switch (CurrentToken)
            {
                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    if (CurrentToken == TokenCategory.PARENTHESIS_OPEN)
                    {

                        Expect(TokenCategory.PARENTHESIS_OPEN);

                        Expr_List();

                        Expect(TokenCategory.PARENTHESIS_CLOSE);

                    }
                    break;
                case TokenCategory.LEFT_SQUARE_BRACKET:
                    Array();
                    break;
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;
                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                case TokenCategory.CHARACTER:
                    Expect(TokenCategory.CHARACTER);
                    break;
                case TokenCategory.STRING:
                    Expect(TokenCategory.STRING);
                    break;
                case TokenCategory.PARENTHESIS_OPEN:
                    Expect(TokenCategory.PARENTHESIS_OPEN);
                    Expr();
                    Expect(TokenCategory.PARENTHESIS_CLOSE);
                    break;
                default:
                    throw new SyntaxError(Lit_Values, tokenStream.Current);
            }
        }
        public void Array()
        {
            Expect(TokenCategory.LEFT_SQUARE_BRACKET);
            Expr_List();
            Expect(TokenCategory.RIGHT_SQUARE_BRACKET);
        }

        public void Lit()
        {
            switch (CurrentToken)
            {
                case TokenCategory.TRUE:
                    Expect(TokenCategory.TRUE);
                    break;
                case TokenCategory.FALSE:
                    Expect(TokenCategory.FALSE);
                    break;
                case TokenCategory.INT_LITERAL:
                    Expect(TokenCategory.INT_LITERAL);
                    break;
                case TokenCategory.CHARACTER:
                    Expect(TokenCategory.CHARACTER);
                    break;
                case TokenCategory.STRING:
                    Expect(TokenCategory.STRING);
                    break;
                default:
                    throw new SyntaxError(Lit_Values, tokenStream.Current);
            }
        }



    }
}

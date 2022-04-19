/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

/*
 * QuetzalDragon LL(1) Grammar:
 *

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

        public void Program()
        {
            Def_list();
            Expect(TokenCategory.EOF);
        }


        public void Def_list()
        {
            while (def_Values.Contains(CurrentToken))
            {
                Def();
            }

        }
        public void Def()
        {
            switch (CurrentToken)
            {

                case TokenCategory.VAR:
                    Var_Def();
                    break;

                case TokenCategory.IDENTIFIER:
                    Fun_Def();
                    break;

                default:
                    throw new SyntaxError(def_Values,
                                        tokenStream.Current);

            }

        }

        public void Var_Def()
        {
            Expect(TokenCategory.VAR);
            Var_List();
            Expect(TokenCategory.END_OF_LINE);
        }

        public void Fun_Def()
        {
            Expect(TokenCategory.IDENTIFIER);
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
            Expect(TokenCategory.RIGHT_CURLY_BRACE);

        }
        public void Var_List()
        {
            ID_List();
        }
        public void ID_List()
        {
            Expect(TokenCategory.IDENTIFIER);
            while (CurrentToken == TokenCategory.COMA)
            {
                Expect(TokenCategory.COMA);
                Expect(TokenCategory.IDENTIFIER);
            }
        }

        public void Stmt_List()
        {
            while (stmt_Values.Contains(CurrentToken))
            {
                Stmt();
            }
        }

        public void Stmt()
        {
            switch (CurrentToken)
            {

                case TokenCategory.IDENTIFIER:
                    Expect(TokenCategory.IDENTIFIER);
                    switch (CurrentToken)
                    {
                        case TokenCategory.ASSIGN:
                            Expect(TokenCategory.ASSIGN);
                            Expr();
                            Expect(TokenCategory.END_OF_LINE);
                            break;
                        case TokenCategory.PARENTHESIS_OPEN:
                            Expect(TokenCategory.PARENTHESIS_OPEN);
                            Expr_List();
                            Expect(TokenCategory.PARENTHESIS_CLOSE);
                            Expect(TokenCategory.END_OF_LINE);
                            break;
                        default:
                            throw new SyntaxError(def_Values,
                                                tokenStream.Current);

                    }
                    break;

                case TokenCategory.INC:
                    Stmt_Incr();
                    break;
                case TokenCategory.DEC:
                    Stmt_Decr();
                    break;
                case TokenCategory.IF:
                    Stmt_If();
                    break;
                case TokenCategory.LOOP:
                    Stmp_Loop();
                    break;
                case TokenCategory.BREAK:
                    Stmp_Break();
                    break;
                case TokenCategory.RETURN:
                    Stmp_Return();
                    break;
                case TokenCategory.END_OF_LINE:
                    Expect(TokenCategory.END_OF_LINE);
                    break;

                default:
                    throw new SyntaxError(def_Values,
                                        tokenStream.Current);

            }
        }


        public void Stmt_Incr()
        {
            Expect(TokenCategory.INC);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.END_OF_LINE);
        }

        public void Stmt_Decr()
        {
            Expect(TokenCategory.DEC);
            Expect(TokenCategory.IDENTIFIER);
            Expect(TokenCategory.END_OF_LINE);
        }

        public void Stmt_If()
        {
            Expect(TokenCategory.IF);
            Expect(TokenCategory.PARENTHESIS_OPEN);
            Expr();
            Expect(TokenCategory.PARENTHESIS_CLOSE);
            Expect(TokenCategory.LEFT_CURLY_BRACE);
            Stmt_List();
            Expect(TokenCategory.RIGHT_CURLY_BRACE);
            Else_If_List();
            Else();
        }
        public void Else_If_List()
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

        public void Expr_List()
        {
            
        }
        public void Expr_List_Cont()
        {

        }
        public void Expr()
        {

        }

    }
}

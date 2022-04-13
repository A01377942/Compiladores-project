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

        }
    }
}

/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace QuetzalDragon
{

    class SyntaxError : Exception
    {

        public SyntaxError(TokenCategory expectedCategory,
                           Token token) :
            base($"Syntax Error: Expecting {expectedCategory} \n"
                 + $"but found {token.Category} (\"{token.Lexeme}\") at "
                 + $"row {token.Row}, column {token.Column}.")
        {
        }

        public SyntaxError(ISet<TokenCategory> expectedCategories,
                           Token token) :
            base($"Syntax Error: Expecting one of {Elements(expectedCategories)}\n"
                 + $"but found {token.Category} (\"{token.Lexeme}\") at "
                 + $"row {token.Row}, column {token.Column}.")
        {
        }

        static string Elements(ISet<TokenCategory> expectedCategories)
        {
            var sb = new StringBuilder("{");
            var first = true;
            foreach (var elem in expectedCategories)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(", ");
                }
                sb.Append(elem);
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}

/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace QuetzalDragon
{

    class Scanner
    {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"
                (?<MultiLineComment>    [/][*](.|\n)*?[*][/] )
              | (?<Comment>    [/][/].*   )   
              | (?<String>      [""]([^""\n\\]|(\\([nrt\\'""]|u[0-9a-fA-F]{6})))*[""] )
              | (?<Character>      [']([^'\n\\]|(\\([nrt\\'""]|u[0-9a-fA-F]{6})))['] )
              | (?<Newline>    \n        )
              | (?<WhiteSpace> \s        ) 
              | (?<Coma>        [,]       )
              | (?<LeftSquareBracket>  [[] )
              | (?<RightSquareBracket>  ] )
              | (?<LessEqualThan>   [<][=]  )
              | (?<NotEqual>   [!][=]    )
              | (?<GreaEqualThan>   [>][=]  )
              | (?<GreaterThan> [>]      )
              | (?<Less>       [<]       )
              | (?<Plus>       [+]       )
              | (?<Substraction>[-]       )
              | (?<Division>   [/]       )
              | (?<Reminder>   [%]       )
              | (?<Mul>        [*]       )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<CurlyLeft>  [{]       )
              | (?<CurlyRight> [}]       )
              | (?<EqualTo>     [=][=]    )
              | (?<Assign>     [=]       )
              | (?<True>       true\b      )
              | (?<False>      false\b      )   
              | (?<IntLiteral> [-]?\d+   )
              | (?<Var>        var\b     )
              | (?<Dec>        dec\b     )
              | (?<Inc>        inc\b     )
              | (?<And>        and\b     )
              | (?<Or>         or\b      )
              | (?<Not>        not\b     ) 
              | (?<If>         if\b      )
              | (?<Elif>       elif\b    )
              | (?<Else>       else\b    )
              | (?<Print>      print\b   )
              | (?<Loop>       loop\b    )
              | (?<Break>      break\b   )
              | (?<Return>      return\b   )
              | (?<EndOfLine>      ;   )
              | (?<Identifier> [a-zA-Z_]\w* )   
              | (?<Other>      .         ) 
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> tokenMap =
            new Dictionary<string, TokenCategory>() {
                {"String", TokenCategory.STRING},
                {"Character", TokenCategory.CHARACTER},
                {"Coma", TokenCategory.COMA},
                {"LeftSquareBracket", TokenCategory.LEFT_SQUARE_BRACKET},
                {"RightSquareBracket", TokenCategory.RIGHT_SQUARE_BRACKET},
                {"And", TokenCategory.AND},
                {"EqualTo", TokenCategory.EQUAL_TO},
                {"NotEqual", TokenCategory.NOT_EQUAL_TO},
                {"LessEqualThan", TokenCategory.LESS_EQUAL_THAN},
                {"GreaEqualThan", TokenCategory.GREATHER_EQUAL_THAN},
                {"Less", TokenCategory.LESS_THAN},
                {"GreaterThan", TokenCategory.GREATHER_THAN},
                {"Plus", TokenCategory.PLUS},
                {"Substraction", TokenCategory.SUBSTRACTION},
                {"Division", TokenCategory.DIVISION},
                {"Reminder", TokenCategory.REMINDER},
                {"Mul", TokenCategory.MULTIPLICATION},
                {"CurlyLeft", TokenCategory.LEFT_CURLY_BRACE},
                {"CurlyRight", TokenCategory.RIGHT_CURLY_BRACE},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"Assign", TokenCategory.ASSIGN},
                {"True", TokenCategory.TRUE},
                {"False", TokenCategory.FALSE},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Var", TokenCategory.VAR},
                {"Dec", TokenCategory.DEC},
                {"Inc", TokenCategory.INC},
                {"Or", TokenCategory.OR},
                {"Not", TokenCategory.NOT},
                {"If", TokenCategory.IF},
                {"Elif", TokenCategory.ELIF},
                {"Else", TokenCategory.ELSE},
                {"Print", TokenCategory.PRINT},
                {"Loop", TokenCategory.LOOP},
                {"Break", TokenCategory.BREAK},
                {"Return", TokenCategory.RETURN},
                {"EndOfLine", TokenCategory.END_OF_LINE},
                {"Identifier", TokenCategory.IDENTIFIER}
            };

        public Scanner(string input)
        {
            this.input = input;
        }

        public IEnumerable<Token> Scan()
        {

            var result = new LinkedList<Token>();
            var row = 1;
            var columnStart = 0;

            foreach (Match m in regex.Matches(input))
            {

                if (m.Groups["Newline"].Success)
                {

                    row++;
                    columnStart = m.Index + m.Length;

                }
                else if (m.Groups["MultiLineComment"].Success)
                {
                    // Skip white space and comments.
                    //match.Value.Split('\n').Length
                    row += (m.Value.Split('\n').Length - 1);
                }


                else if (m.Groups["WhiteSpace"].Success
                  || m.Groups["Comment"].Success)
                {

                    // Skip white space and comments.

                }
                else if (m.Groups["Other"].Success)
                {

                    // Found an illegal character.
                    result.AddLast(
                        new Token(m.Value,
                            TokenCategory.ILLEGAL_CHAR,
                            row,
                            m.Index - columnStart + 1));

                }
                else
                {

                    // Must be any of the other tokens.
                    result.AddLast(FindToken(m, row, columnStart));
                }
            }

            result.AddLast(
                new Token(null,
                    TokenCategory.EOF,
                    row,
                    input.Length - columnStart + 1));

            return result;
        }

        Token FindToken(Match m, int row, int columnStart)
        {
            foreach (var name in tokenMap.Keys)
            {
                if (m.Groups[name].Success)
                {
                    return new Token(m.Value,
                        tokenMap[name],
                        row,
                        m.Index - columnStart + 1);
                }
            }
            throw new InvalidOperationException(
                "regex and tokenMap are inconsistent: " + m.Value);
        }
    }

}
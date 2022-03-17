/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
*/

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Buttercup
{

    class Scanner
    {

        readonly string input;

        static readonly Regex regex = new Regex(
            @"
                (?<MultiLineComment>    [/][*](.|\n)*?[*][/] )
              | (?<Comment>    [/][/].*   )
              | (?<Newline>    \n        )
              | (?<WhiteSpace> \s        )     # Must go after Newline.
              | (?<And>        [&]       )
              | (?<Less>       [<]       )
              | (?<Plus>       [+]       )
              | (?<Mul>        [*]       )
              | (?<Neg>        [-]       )
              | (?<ParLeft>    [(]       )
              | (?<ParRight>   [)]       )
              | (?<Assign>     [=]       )
              | (?<True>       [#]t      )
              | (?<False>      [#]f      )
              | (?<IntLiteral> \d+       )
              | (?<Bool>       bool\b    )
              | (?<End>        end\b     )
              | (?<If>         if\b      )
              | (?<Int>        int\b     )
              | (?<Print>      print\b   )
              | (?<Then>       then\b    )
              | (?<Identifier> [a-zA-Z]+ )     # Must go after all keywords
              | (?<Other>      .         )     # Must be last: match any other character.
            ",
            RegexOptions.IgnorePatternWhitespace
                | RegexOptions.Compiled
                | RegexOptions.Multiline
            );

        static readonly IDictionary<string, TokenCategory> tokenMap =
            new Dictionary<string, TokenCategory>() {
                {"And", TokenCategory.AND},
                {"Less", TokenCategory.LESS},
                {"Plus", TokenCategory.PLUS},
                {"Mul", TokenCategory.MUL},
                {"Neg", TokenCategory.NEG},
                {"ParLeft", TokenCategory.PARENTHESIS_OPEN},
                {"ParRight", TokenCategory.PARENTHESIS_CLOSE},
                {"Assign", TokenCategory.ASSIGN},
                {"True", TokenCategory.TRUE},
                {"False", TokenCategory.FALSE},
                {"IntLiteral", TokenCategory.INT_LITERAL},
                {"Bool", TokenCategory.BOOL},
                {"End", TokenCategory.END},
                {"If", TokenCategory.IF},
                {"Int", TokenCategory.INT},
                {"Print", TokenCategory.PRINT},
                {"Then", TokenCategory.THEN},
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

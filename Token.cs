/*
Zabdiel Valentin A01377950
Emiliano Javier Gómez Jiménez A01377235
Luis Jonathan Rosas Ramos A01377942
*/

namespace QuetzalDragon{
    class Token
    {

        readonly string lexeme;

        readonly TokenCategory category;

        readonly int row;

        readonly int column;

        public string Lexeme
        {
            get { return lexeme; }
        }

        public TokenCategory Category
        {
            get { return category; }
        }

        public int Row
        {
            get { return row; }
        }

        public int Column
        {
            get { return column; }
        }

        public Token(string lexeme,
                     TokenCategory category,
                     int row,
                     int column)
        {
            this.lexeme = lexeme;
            this.category = category;
            this.row = row;
            this.column = column;
        }

        public override string ToString()
        {
            return $"{{{category}, \"{lexeme}\", @({row}, {column})}}";
        }
    }
}